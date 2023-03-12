Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Public Module BezierObjects

		REM This is implemented by CubicBezier and QuadraticBezier
		Public Interface IDesmosExpression
			Function ToLatexExpression() As [String]
			ReadOnly Property GraphBoundries As DesmosGraphSize
			Property Colour As Drawing.Color
		End Interface

		Public Class NOrderBezier : Implements IDesmosExpression

			Friend Const NotEncapsulatedPlaceHolderRegExp$ = "¬(X|Y)\d{1,6}¬"

			Public ReadOnly Property Points As ObjectModel.ReadOnlyCollection(Of Drawing.Point)
				Get
					Return (New ObjectModel.ReadOnlyCollection(Of Drawing.Point)(Me.Internal_Points))
				End Get
			End Property

			Private Internal_Points As New List(Of Drawing.Point)()

			Public Sub New(ByVal ParamArray _Points As Int32()())

				REM We should have input such as:
				'{{14, 5}, {13, 5}, {56, -9}}

				REM That needs to become:
				'{New Drawing.Point(14, 5), New Drawing.Point(13, 5), New Drawing.Point(56, -9)}

				'There must be at least one point in the Array, to form a Point
				If Not _Points.Length.IsAtLeast(1) Then Throw New Exception("An attempt was made to instanciate an N-Order Bezier, however fewer than one Points were specified in the Constructor. The Number of Points specified was: " & _Points.Length.ToString())

				If _Points.Any(Function(_PointXY As Int32()) Not (_PointXY.Length = 2)) Then Throw New Exception("At least one of the Points in the Input of Points to the NOrderBezier Constructor did not have contain exactly two Numbers (An X and Y Value). The Input to this constructor must be in the form: E.g. {14, 5}, {13, 5}, {56, -9}. This is another constructor to this Type which accepts an Array of Points, E.g. {New Drawing.Point(14, 5), New Drawing.Point(13, 5), New Drawing.Point(56, -9)}.")

				Dim _MePoints As New List(Of System.Drawing.Point)()

				For Each _PointXY As Int32() In _Points
					_MePoints.Add(New Drawing.Point(_PointXY(0), _PointXY(1)))
				Next

				Me.Internal_Points = _MePoints

			End Sub

			Public Sub New(ByVal _Points As Drawing.Point())

				'There must be at least one point in the Array, to form a Point
				If Not _Points.Length.IsAtLeast(1) Then Throw New Exception("An attempt was made to instanciate an N-Order Bezier, however fewer than one Points were specified in the Constructor. The Number of Points specified was: " & _Points.Length.ToString())

				Me.Internal_Points = _Points.ToList()

			End Sub

			Public ReadOnly Property GraphBoundries As DesmosGraphSize Implements IDesmosExpression.GraphBoundries
				Get

					REM At a later date, perhaps rewrite this to return the actual Min&Max X&Y Values for the LINE itself,
					REM ...And not just it's Start, Control, and End Points

					Dim _MostPositiveX% = Me.Internal_Points(0).X
					Dim _MostPositiveY% = Me.Internal_Points(0).Y
					Dim _MostNegativeX% = Me.Internal_Points(0).X
					Dim _MostNegativeY% = Me.Internal_Points(0).Y

					For Each _Point As Drawing.Point In Me.Internal_Points

						If _Point.X > _MostPositiveX Then _MostPositiveX = _Point.X
						If _Point.Y > _MostPositiveY Then _MostPositiveY = _Point.Y
						If _Point.X < _MostNegativeX Then _MostNegativeX = _Point.X
						If _Point.Y < _MostNegativeY Then _MostNegativeY = _Point.Y

					Next

					Return New DesmosGraphSize() With {
					 .MostPositiveX = _MostPositiveX,
					 .MostPositiveY = _MostPositiveY,
					 .MostNegativeX = _MostNegativeX,
					 .MostNegativeY = _MostNegativeY
					}

				End Get
			End Property

			''' <summary>
			''' Returns the Bezier's Order, E.g. "Linear", or "9-th Order"
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property BezierOrderDescription As String
				Get

					If Me.Internal_Points.Count < 1 Then
						Throw New Exception("This Bezier Object is invalid!!! Fewer than one Points exist within it. (This was discovered whilst attempting to Generate a Description of the Bezier's Order...)")
					ElseIf Me.Internal_Points.Count = 1 Then
						Return "Point"
					ElseIf Me.Internal_Points.Count = 2 Then
						Return "Linear"
					ElseIf Me.Internal_Points.Count = 3 Then
						Return "Quadratic"
					ElseIf Me.Internal_Points.Count = 4 Then
						Return "Cubic"
					Else
						Return (Me.Internal_Points.Count - 1).ToString() & "-th Order"
					End If

				End Get
			End Property

			Public Function ToLatexExpression() As String Implements IDesmosExpression.ToLatexExpression

				REM If there is only one Point in Me.Points, then just return it's coordinates, to form a single Point
				If Me.Internal_Points.Count = 1 Then Return String.Format("({0},{1})", Me.Internal_Points(0).X.ToString(), Me.Internal_Points(0).Y.ToString()).MathsExpressionToLatex()

				Dim _XLatexToReturn$ = String.Empty
				Dim _YLatexToReturn$ = String.Empty

				Dim _HighestPUsed% = 0
				Dim _XA$, _XB$, _YA$, _YB$ 'Result = A + ((B - A) * t)

				_XA = "¬X0¬" : _XB = "¬X1¬"
				_YA = "¬Y0¬" : _YB = "¬Y1¬"

				Do Until _HighestPUsed = (Me.Internal_Points.Count - 1)

					_XLatexToReturn = "(" & _XA & "+((" & _XB & "-" & _XA & ")*t))"
					_YLatexToReturn = "(" & _YA & "+((" & _YB & "-" & _YA & ")*t))"

					_XA = _XLatexToReturn					'(¬X0¬ + (¬X1¬ - ¬X0¬) * t)
					_XB = _XA.WithAllPointsIncremented()	'(¬X1¬ + (¬X2¬ - ¬X1¬) * t)

					_YA = _YLatexToReturn					'(¬Y0¬ + (¬Y1¬ - ¬Y0¬) * t)
					_YB = _YA.WithAllPointsIncremented()	'(¬Y1¬ + (¬Y2¬ - ¬Y1¬) * t)

					_HighestPUsed += 1%

					MsgBox("(" & _XLatexToReturn & ", " & _YLatexToReturn & ")")

				Loop

				REM Now replace all the ¬X|Y*¬ Expressions with their actual values

				'Instance Members of the Class cannot be used in LambdaExpressions, so a Local Copy of Me.Points is made
				Dim _LocalCopyOfMePoints As Drawing.Point() = Me.Internal_Points.ToArray()

				_XLatexToReturn = System.Text.RegularExpressions.Regex.Replace(_XLatexToReturn, NotEncapsulatedPlaceHolderRegExp, evaluator:=New System.Text.RegularExpressions.MatchEvaluator( _
				 Function(_Match As System.Text.RegularExpressions.Match)

					 'The _Match.Value should look like:	¬X0¬
					 'We need to replace that with:			14

					 Return _LocalCopyOfMePoints(CInt(_Match.Value.Substring(2, _Match.Value.Length - 3))).X.ToString()

				 End Function _
				 ))

				_YLatexToReturn = System.Text.RegularExpressions.Regex.Replace(_YLatexToReturn, NotEncapsulatedPlaceHolderRegExp, evaluator:=New System.Text.RegularExpressions.MatchEvaluator( _
				 Function(_Match As System.Text.RegularExpressions.Match)

					 'The _Match.Value should look like:	¬Y0¬
					 'We need to replace that with:		19

					 Return _LocalCopyOfMePoints(CInt(_Match.Value.Substring(2, _Match.Value.Length - 3))).Y.ToString()

				 End Function _
				 ))

				Return [String].Format("({0}, {1})", _XLatexToReturn, _YLatexToReturn).MathsExpressionToLatex()

			End Function

			''' <summary>
			''' Retunrs the IDesmosExpressions, for the current nOrder Bezier, but with all of it's points as draggable elements in Desmos.
			''' Run these through the ToDesmosPastable() Compiler Extention Method for a String you can Paste into Desmos
			''' </summary>
			Public Function GetExpressionsForGraphWithDragablePoints(Optional ByVal _XYVariableNumberOffSet As UInt16 = 0) As IDesmosExpression()

				REM If there is only one Point in Me.Points, then just return it's coordinates, to form a single Point
				If Me.Internal_Points.Count = 1 Then Return CType({New GraphPictures.Library.PointPlottingObjects.PlottablePoint(Me.Internal_Points(0).X, Me.Internal_Points(0).Y, Drawing.Color.Black)}, IDesmosExpression())

				Dim _XLatexToReturn$ = String.Empty
				Dim _YLatexToReturn$ = String.Empty

				Dim _HighestPUsed% = 0
				Dim _XA$, _XB$, _YA$, _YB$ 'Result = A + ((B - A) * t)

				_XA = "¬X0¬" : _XB = "¬X1¬"
				_YA = "¬Y0¬" : _YB = "¬Y1¬"

				Do Until _HighestPUsed = (Me.Internal_Points.Count - 1)

					_XLatexToReturn = "(" & _XA & "+((" & _XB & "-" & _XA & ")*t))"
					_YLatexToReturn = "(" & _YA & "+((" & _YB & "-" & _YA & ")*t))"

					_XA = _XLatexToReturn					'(¬X0¬ + (¬X1¬ - ¬X0¬) * t)
					_XB = _XA.WithAllPointsIncremented()	'(¬X1¬ + (¬X2¬ - ¬X1¬) * t)

					_YA = _YLatexToReturn					'(¬Y0¬ + (¬Y1¬ - ¬Y0¬) * t)
					_YB = _YA.WithAllPointsIncremented()	'(¬Y1¬ + (¬Y2¬ - ¬Y1¬) * t)

					_HighestPUsed += 1%

				Loop

				Dim _DesmosExprsToReturn As New List(Of IDesmosExpression)()

				REM Now - DONT replace all the ¬X|Y*¬ Expressions with their actual values
				REM Instead, Replace ¬X0¬ with x_{0} etc...

				REM Add the DesmosExprs for the Variables like this: x_{0}=4.6		and		y_{0}=55
				REM Add the DesmosExpr for the DraggablePoint like this: (x_{0},y_{0})

				For _XYVariableNumber As UInt16 = _XYVariableNumberOffSet To CUShort(_XYVariableNumberOffSet + (Me.Points.Count - 1)) Step +1

					_DesmosExprsToReturn.Add(New DesmosVariable("x", _XYVariableNumber, Me.Points(_XYVariableNumber - _XYVariableNumberOffSet).X))
					_DesmosExprsToReturn.Add(New DesmosVariable("y", _XYVariableNumber, Me.Points(_XYVariableNumber - _XYVariableNumberOffSet).Y))

					_DesmosExprsToReturn.Add(New DraggablePoint(_XYVariableNumber))

				Next

				'Instance Members of the Class cannot be used in LambdaExpressions, so a Local Copy of Me.Points is made
				Dim _LocalCopyOfMePoints As Drawing.Point() = Me.Internal_Points.ToArray()

				_XLatexToReturn = System.Text.RegularExpressions.Regex.Replace(_XLatexToReturn, NotEncapsulatedPlaceHolderRegExp, evaluator:=New System.Text.RegularExpressions.MatchEvaluator( _
				 Function(_Match As System.Text.RegularExpressions.Match)

					 'The _Match.Value should look like:	¬X0¬
					 'We need to replace that with:			x_{0}

					 Return ("x_{" & (_XYVariableNumberOffSet + CInt(_Match.Value.Substring(2, _Match.Value.Length - 3))).ToString() & "}")

				 End Function _
				 ))

				_YLatexToReturn = System.Text.RegularExpressions.Regex.Replace(_YLatexToReturn, NotEncapsulatedPlaceHolderRegExp, evaluator:=New System.Text.RegularExpressions.MatchEvaluator( _
				 Function(_Match As System.Text.RegularExpressions.Match)

					 'The _Match.Value should look like:	¬Y0¬
					 'We need to replace that with:			y_{0}

					 Return ("y_{" & (_XYVariableNumberOffSet + CInt(_Match.Value.Substring(2, _Match.Value.Length - 3))).ToString() & "}")

				 End Function _
				 ))

				REM Now add the BezierExpr which referances all of those Variables
				_DesmosExprsToReturn.Add(New GenericDesmosLatexExpression(("(" & _XLatexToReturn & "," & _YLatexToReturn & ")").MathsExpressionToLatex()))

				Return _DesmosExprsToReturn.ToArray()

			End Function

			''' <summary>
			''' Retunrs the IDesmosExpressions, for the current nOrder Bezier, but with all of it's points as draggable elements in Desmos.
			''' Also returns [IDesmosExpression]'s for all the SubBeziers within the current NOrderBezier. These SubBeziers also referance the XYVariables, so response when the Points are dragged.
			''' Run these through the ToDesmosPastable() Compiler Extention Method for a String you can paste into Desmos
			''' </summary>
			Public Function GetExpressionsForGraphWithDragablePointsAndShowAllSubBeziers(Optional ByVal _XYVariableNumberOffSet As UInt16 = 0) As IDesmosExpression()

				REM If there is only one Point in Me.Points, then just return it's coordinates, to form a single Point. There are NoSubBeziers
				If Me.Internal_Points.Count = 1 Then Return CType({New GraphPictures.Library.PointPlottingObjects.PlottablePoint(Me.Internal_Points(0).X, Me.Internal_Points(0).Y, Drawing.Color.Black)}, IDesmosExpression())

				Dim _XLatexToReturn$ = String.Empty
				Dim _YLatexToReturn$ = String.Empty

				Dim _HighestPUsed% = 0
				Dim _XA$, _XB$, _YA$, _YB$ 'Result = A + ((B - A) * t)

				_XA = "¬X0¬" : _XB = "¬X1¬"
				_YA = "¬Y0¬" : _YB = "¬Y1¬"

				Do Until _HighestPUsed = (Me.Internal_Points.Count - 1)

					_XLatexToReturn = "(" & _XA & "+((" & _XB & "-" & _XA & ")*t))"
					_YLatexToReturn = "(" & _YA & "+((" & _YB & "-" & _YA & ")*t))"

					_XA = _XLatexToReturn					'(¬X0¬ + (¬X1¬ - ¬X0¬) * t)
					_XB = _XA.WithAllPointsIncremented()	'(¬X1¬ + (¬X2¬ - ¬X1¬) * t)

					_YA = _YLatexToReturn					'(¬Y0¬ + (¬Y1¬ - ¬Y0¬) * t)
					_YB = _YA.WithAllPointsIncremented()	'(¬Y1¬ + (¬Y2¬ - ¬Y1¬) * t)

					_HighestPUsed += 1%

				Loop

				Dim _DesmosExprsToReturn As New List(Of IDesmosExpression)()

				Try

					'SubBeziers are required for each order up to the Terminal Order of the current N-Order Bezier
					'These can be NOrder Beziers, but need to use the /x|y_\{\d+\}/ variables and not hard-coded constants
					'Null Points can be passed to the NOrderBezier Constructors, because they will referance the XYVariables

					For _SubBezierOrder As UInt16 = 1US To CUShort(Me.Points.Count - 2) Step +1

						REM Now Generate all of the Construction Bezier's of the current Order
						For _IndexOfCurrentSubBezierWithinCurrentOrder As UInt16 = 0 To CUShort((Me.Points.Count - _SubBezierOrder) - 1)

							_DesmosExprsToReturn.Add( _
							 (New NOrderBezier((New Drawing.Point(0, 0)).Repeat_TimesNotImplementedCorrectly(CUInt(_SubBezierOrder)))).GetExpressionsForGraphWithDragablePoints(_XYVariableNumberOffSet:=_IndexOfCurrentSubBezierWithinCurrentOrder).Where(Function(_IDesmosExpr As IDesmosExpression) TypeOf _IDesmosExpr Is GenericDesmosLatexExpression).First()
							 )

						Next

					Next

				Catch _Ex As Exception When True
					Throw New GraphPicturesException("During the Evaluation of the SubBeziers: " & _Ex.Message)
				End Try

				REM Now - DONT replace all the ¬X|Y*¬ Expressions with their actual values
				REM Instead, Replace ¬X0¬ with x_{0} etc...

				REM Add the DesmosExprs for the Variables like this: x_{0}=4.6		and		y_{0}=55
				REM Add the DesmosExpr for the DraggablePoint like this: (x_{0},y_{0})

				For _XYVariableNumber As UInt16 = _XYVariableNumberOffSet To CUShort(_XYVariableNumberOffSet + (Me.Points.Count - 1)) Step +1

					_DesmosExprsToReturn.Add(New DesmosVariable("x", _XYVariableNumber, Me.Points(_XYVariableNumber - _XYVariableNumberOffSet).X))
					_DesmosExprsToReturn.Add(New DesmosVariable("y", _XYVariableNumber, Me.Points(_XYVariableNumber - _XYVariableNumberOffSet).Y))

					_DesmosExprsToReturn.Add(New DraggablePoint(_XYVariableNumber))

				Next

				'Instance Members of the Class cannot be used in LambdaExpressions, so a Local Copy of Me.Points is made
				Dim _LocalCopyOfMePoints As Drawing.Point() = Me.Internal_Points.ToArray()

				_XLatexToReturn = System.Text.RegularExpressions.Regex.Replace(_XLatexToReturn, NotEncapsulatedPlaceHolderRegExp, evaluator:=New System.Text.RegularExpressions.MatchEvaluator( _
				 Function(_Match As System.Text.RegularExpressions.Match)

					 'The _Match.Value should look like:	¬X0¬
					 'We need to replace that with:			x_{0}

					 Return ("x_{" & (_XYVariableNumberOffSet + CInt(_Match.Value.Substring(2, _Match.Value.Length - 3))).ToString() & "}")

				 End Function _
				 ))

				_YLatexToReturn = System.Text.RegularExpressions.Regex.Replace(_YLatexToReturn, NotEncapsulatedPlaceHolderRegExp, evaluator:=New System.Text.RegularExpressions.MatchEvaluator( _
				 Function(_Match As System.Text.RegularExpressions.Match)

					 'The _Match.Value should look like:	¬Y0¬
					 'We need to replace that with:			y_{0}

					 Return ("y_{" & (_XYVariableNumberOffSet + CInt(_Match.Value.Substring(2, _Match.Value.Length - 3))).ToString() & "}")

				 End Function _
				 ))

				REM Now add the BezierExpr which referances all of those Variables
				_DesmosExprsToReturn.Add(New GenericDesmosLatexExpression(("(" & _XLatexToReturn & "," & _YLatexToReturn & ")").MathsExpressionToLatex()))

				Return _DesmosExprsToReturn.ToArray()

			End Function

			Public Property Colour As System.Drawing.Color Implements IDesmosExpression.Colour
				Get
					Return Drawing.Color.Black
				End Get
				Set(ByVal _NewValue As System.Drawing.Color)
					Throw New NotImplementedException("The ability to set the Colour of this Object Type has not been implemented yet.")
				End Set
			End Property

		End Class

		REM This is for NOrderBezier.ToLatexExpression()
		<Global.System.Runtime.CompilerServices.Extension()>
		Private Function WithAllPointsIncremented(ByVal _Expression$) As String

			REM Input looks like:			(¬X0¬ + (¬X1¬ - ¬X0¬) * t)
			REM Output should look like:	(¬X1¬ + (¬X2¬ - ¬X1¬) * t)

			Return System.Text.RegularExpressions.Regex.Replace(_Expression, NOrderBezier.NotEncapsulatedPlaceHolderRegExp, evaluator:=New System.Text.RegularExpressions.MatchEvaluator( _
			 Function(_Match As System.Text.RegularExpressions.Match)

				 'The _Match.Value should look like:	¬X0¬
				 'We need to replace that with:			¬X1¬

				 '		 ¬			The Second Char (X or Y)						The Number Incremented by 1							   ¬
				 Return "¬" & _Match.Value.ToCharArray()(1).ToString() & (CInt(_Match.Value.Substring(2, _Match.Value.Length - 3)) + 1) & "¬"

			 End Function _
			))

		End Function

		''' <summary>
		''' Represents a Bezier with a StartPoint, Two ControlPoints, and an EndPoint
		''' </summary>
		''' <remarks></remarks>
		Public Structure CubicBezier : Implements BezierObjects.IDesmosExpression

			Public Property StartPoint As System.Drawing.Point
			Public Property ControlPointOne As System.Drawing.Point
			Public Property ControlPointTwo As System.Drawing.Point
			Public Property EndPoint As System.Drawing.Point

			Public Sub New(ByVal _StartPoint As Drawing.Point, ByVal _ControlPointOne As Drawing.Point, ByVal _ControlPointTwo As Drawing.Point, ByVal _EndPoint As Drawing.Point)
				With Me
					.StartPoint = _StartPoint
					.ControlPointOne = _ControlPointOne
					.ControlPointTwo = _ControlPointTwo
					.EndPoint = _EndPoint
				End With
			End Sub

			''' <summary>
			''' Formats the Bezier as a LaTeX Expression, ready to paste into Graphing Calcuators such as Desmos
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function ToLatexExpression$() Implements IDesmosExpression.ToLatexExpression
				Return "\left(\left(1-t\right)^{3}\left(" & Me.StartPoint.X.ToString() & _
				 "\right)+3t\left(1-t\right)^{2}\left(" & Me.ControlPointOne.X.ToString() & _
				 "\right)+3t^{2}\left(1-t\right)\left(" & Me.ControlPointTwo.X.ToString() & _
				 "\right)+t^{3}\left(" & Me.EndPoint.X.ToString() & _
				 "\right),\left(1-t\right)^{3}\left(" & Me.StartPoint.Y.ToString() & _
				 "\right)+3t\left(1-t\right)^{2}\left(" & Me.ControlPointOne.Y.ToString() & _
				 "\right)+3t^{2}\left(1-t\right)\left(" & Me.ControlPointTwo.Y.ToString() & _
				 "\right)+t^{3}\left(" & Me.EndPoint.Y.ToString() & "\right)\right)"
			End Function

			''' <summary>
			''' Returns a String representing the Bezier's Start, Control and End Points
			''' </summary>
			''' <returns>Returns a String representing the Bezier's Start, Control and End Points</returns>
			''' <remarks></remarks>
			Public Overrides Function ToString() As String
				Return "{ StartPoint=[" & Me.StartPoint.ToString() & "], ControlPointOne=[" & Me.ControlPointOne.ToString() & "], ControlPointTwo=[" & Me.ControlPointTwo.ToString() & "], EndPoint=[" & Me.EndPoint.ToString() & "] }"
			End Function

#Region "Bezier Operators"
			Public Shared Operator =(ByVal _BezierOne As CubicBezier, ByVal _BezierTwo As CubicBezier) As Boolean
				Return ((_BezierOne.StartPoint = _BezierTwo.StartPoint) AndAlso (_BezierOne.ControlPointOne = _BezierTwo.ControlPointOne) AndAlso (_BezierOne.ControlPointTwo = _BezierTwo.ControlPointTwo) AndAlso (_BezierOne.EndPoint = _BezierTwo.EndPoint))
			End Operator

			Public Shared Operator <>(ByVal _BezierOne As CubicBezier, ByVal _BezierTwo As CubicBezier) As Boolean
				Return (Not (_BezierOne = _BezierTwo))
			End Operator

			Public Shared Operator =(ByVal _Bezier As CubicBezier, ByVal _DesmosExpression As IDesmosExpression) As Boolean
				Return (_Bezier.ToLatexExpression() = _DesmosExpression.ToLatexExpression())
			End Operator

			Public Shared Operator <>(ByVal _Bezier As CubicBezier, ByVal _DesmosExpression As IDesmosExpression) As Boolean
				Return (Not (_Bezier = _DesmosExpression))
			End Operator
#End Region

			''' <summary>
			''' Gets the Max and Min X and Y values in the Points of the Cubic Bezier
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property GraphBoundries As DesmosGraphSize Implements IDesmosExpression.GraphBoundries
				Get
					REM At a later date, perhaps rewrite this to return the actual Min&Max X&Y Values for the LINE itself,
					REM ...And not just it's Start, Control, and End Points

					MsgDebug("SubicBezier. Me.StartPoint.X == " & Me.StartPoint.X)
					MsgDebug("SubicBezier. Me.StartPoint.Y == " & Me.StartPoint.Y)

					Dim _MostPositiveX% = Me.StartPoint.X
					Dim _MostPositiveY% = Me.StartPoint.Y
					Dim _MostNegativeX% = Me.StartPoint.X
					Dim _MostNegativeY% = Me.StartPoint.Y

					MsgDebug("SubicBezier. After Setting, _MostPositiveX == " & _MostPositiveX)
					MsgDebug("SubicBezier. After Setting, _MostNegativeY == " & _MostNegativeY)

					If Me.StartPoint.X > _MostPositiveX Then _MostPositiveX = Me.StartPoint.X
					If Me.StartPoint.Y > _MostPositiveY Then _MostPositiveY = Me.StartPoint.Y
					If Me.StartPoint.X < _MostNegativeX Then _MostNegativeX = Me.StartPoint.X
					If Me.StartPoint.Y < _MostNegativeY Then _MostNegativeY = Me.StartPoint.Y

					If Me.ControlPointOne.X > _MostPositiveX Then _MostPositiveX = Me.ControlPointOne.X
					If Me.ControlPointOne.Y > _MostPositiveY Then _MostPositiveY = Me.ControlPointOne.Y
					If Me.ControlPointOne.X < _MostNegativeX Then _MostNegativeX = Me.ControlPointOne.X
					If Me.ControlPointOne.Y < _MostNegativeY Then _MostNegativeY = Me.ControlPointOne.Y

					If Me.ControlPointTwo.X > _MostPositiveX Then _MostPositiveX = Me.ControlPointTwo.X
					If Me.ControlPointTwo.Y > _MostPositiveY Then _MostPositiveY = Me.ControlPointTwo.Y
					If Me.ControlPointTwo.X < _MostNegativeX Then _MostNegativeX = Me.ControlPointTwo.X
					If Me.ControlPointTwo.Y < _MostNegativeY Then _MostNegativeY = Me.ControlPointTwo.Y

					If Me.EndPoint.X > _MostPositiveX Then _MostPositiveX = Me.EndPoint.X
					If Me.EndPoint.Y > _MostPositiveY Then _MostPositiveY = Me.EndPoint.Y
					If Me.EndPoint.X < _MostNegativeX Then _MostNegativeX = Me.EndPoint.X
					If Me.EndPoint.Y < _MostNegativeY Then _MostNegativeY = Me.EndPoint.Y

					MsgDebug("Returning Single CubicBezier GraphSize:" & vbCrLf & vbCrLf & "xmin: " & _MostNegativeX & vbCrLf & "ymin: " & _MostNegativeY & vbCrLf & "xmax: " & _MostPositiveX & vbCrLf & "ymax: " & _MostPositiveY)

					Return New DesmosGraphSize() With {
					 .MostPositiveX = _MostPositiveX,
					 .MostPositiveY = _MostPositiveY,
					 .MostNegativeX = _MostNegativeX,
					 .MostNegativeY = _MostNegativeY
					}

				End Get
			End Property

			Public Property Colour As System.Drawing.Color Implements IDesmosExpression.Colour
				Get
					Return Drawing.Color.Black
				End Get
				Set(ByVal value As System.Drawing.Color)
					Throw New NotImplementedException("The ability to set the Colour of this Object Type has not been implemented yet.")
				End Set
			End Property

		End Structure

		''' <summary>
		''' Represents a Bezier with a StartPoint, ControlPoint, and EndPoint
		''' </summary>
		''' <remarks></remarks>
		Public Structure QuadraticBezier : Implements BezierObjects.IDesmosExpression

			Public Property StartPoint As System.Drawing.Point
			Public Property ControlPoint As System.Drawing.Point
			Public Property EndPoint As System.Drawing.Point

			Public Sub New(ByVal _StartPoint As Drawing.Point, ByVal _PassThroughPoint As Drawing.Point, ByVal _EndPoint As Drawing.Point)
				With Me
					.StartPoint = _StartPoint
					.EndPoint = _EndPoint
					.MakeBezierPassThrough(_PassThroughPoint)
				End With
			End Sub

			''' <summary>
			''' Positions the ControlPoint, so that the Quadratic Bezier passes through the _DesiredPoint, Based off of the current Values of [the StartPoint and EndPoint, which must be set first for this to work].
			''' </summary>
			''' <param name="_DesiredPoint"></param>
			''' <remarks></remarks>
			Public Sub MakeBezierPassThrough(ByVal _DesiredPoint As System.Drawing.Point)

				Me.ControlPoint = New System.Drawing.Point(
				 x:=CInt(_DesiredPoint.X * 2 - (Me.StartPoint.X + Me.EndPoint.X) / 2),
				 y:=CInt(_DesiredPoint.Y * 2 - (Me.StartPoint.Y + Me.EndPoint.Y) / 2)
				)

			End Sub

			Public Shared Function StraightLineFromPoints(ByVal _LineStartingPoint As Drawing.Point, ByVal _LineEndingPoint As Drawing.Point) As QuadraticBezier
				REM Makes the New Bezier() Pass through the MidPoint of the Line from _LineStartingPoint to _LineEndingPoint
				Return (New QuadraticBezier(_LineStartingPoint, (New Drawing.Point(x:=CInt((_LineStartingPoint.X + _LineEndingPoint.X) / 2), y:=CInt((_LineStartingPoint.Y + _LineEndingPoint.Y) / 2))), _LineEndingPoint))
			End Function

			''' <summary>
			''' Formats the Bezier as a LaTeX Expression, ready to paste into Graphing Calcuators such as Desmos
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Function ToLatexExpression$() Implements IDesmosExpression.ToLatexExpression
				Return "\left(\left(1-t\right)\left(\left(1-t\right)\left(" & Me.StartPoint.X.ToString() & _
				 "\right)+t\left(" & Me.ControlPoint.X.ToString() & "\right)\right)+t\left(\left(1-t\right)\left(" & Me.ControlPoint.X.ToString() & _
				 "\right)+t\left(" & Me.EndPoint.X.ToString() & _
				 "\right)\right),\left(1-t\right)\left(\left(1-t\right)\left(" & Me.StartPoint.Y.ToString() & _
				 "\right)+t\left(" & Me.ControlPoint.Y.ToString() & "\right)\right)+t\left(\left(1-t\right)\left(" & Me.ControlPoint.Y.ToString() & _
				 "\right)+t\left(" & Me.EndPoint.Y.ToString() & "\right)\right)\right)"
			End Function

			''' <summary>
			''' Gets the Max and Min X and Y values in the Points of the Cubic Bezier
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property GraphBoundries As DesmosGraphSize Implements IDesmosExpression.GraphBoundries
				Get
					REM At a later date, perhaps rewrite this to return the actual Min&Max X&Y Values for the LINE itself,
					REM ...And not just it's Start, Control, and End Points

					Dim _MostPositiveX% = Me.StartPoint.X
					Dim _MostPositiveY% = Me.StartPoint.Y
					Dim _MostNegativeX% = Me.StartPoint.X
					Dim _MostNegativeY% = Me.StartPoint.Y

					If Me.StartPoint.X > _MostPositiveX Then _MostPositiveX = Me.StartPoint.X
					If Me.StartPoint.Y > _MostPositiveY Then _MostPositiveY = Me.StartPoint.Y
					If Me.StartPoint.X < _MostNegativeX Then _MostNegativeX = Me.StartPoint.X
					If Me.StartPoint.Y < _MostNegativeY Then _MostNegativeY = Me.StartPoint.Y

					If Me.ControlPoint.X > _MostPositiveX Then _MostPositiveX = Me.ControlPoint.X
					If Me.ControlPoint.Y > _MostPositiveY Then _MostPositiveY = Me.ControlPoint.Y
					If Me.ControlPoint.X < _MostNegativeX Then _MostNegativeX = Me.ControlPoint.X
					If Me.ControlPoint.Y < _MostNegativeY Then _MostNegativeY = Me.ControlPoint.Y

					If Me.EndPoint.X > _MostPositiveX Then _MostPositiveX = Me.EndPoint.X
					If Me.EndPoint.Y > _MostPositiveY Then _MostPositiveY = Me.EndPoint.Y
					If Me.EndPoint.X < _MostNegativeX Then _MostNegativeX = Me.EndPoint.X
					If Me.EndPoint.Y < _MostNegativeY Then _MostNegativeY = Me.EndPoint.Y

					MsgDebug("Returning Single QuadraticBezier GraphSize:" & vbCrLf & vbCrLf & "xmin: " & _MostNegativeX & vbCrLf & "ymin: " & _MostNegativeY & vbCrLf & "xmax: " & _MostPositiveX & vbCrLf & "ymax: " & _MostPositiveY)

					Return New DesmosGraphSize() With {
					 .MostPositiveX = _MostPositiveX,
					 .MostPositiveY = _MostPositiveY,
					 .MostNegativeX = _MostNegativeX,
					 .MostNegativeY = _MostNegativeY
					}

				End Get
			End Property

			''' <summary>
			''' Returns a String representing the Bezier's Start, Control and End Points
			''' </summary>
			''' <returns>Returns a String representing the Bezier's Start, Control and End Points</returns>
			''' <remarks></remarks>
			Public Overrides Function ToString() As String
				Return "{ StartPoint=[" & Me.StartPoint.ToString() & "], ControlPoint=[" & Me.ControlPoint.ToString() & "], EndPoint=[" & Me.EndPoint.ToString() & "] }"
			End Function

			Public Property Colour As System.Drawing.Color Implements IDesmosExpression.Colour
				Get
					Return Drawing.Color.Black
				End Get
				Set(ByVal value As System.Drawing.Color)
					Throw New NotImplementedException("The ability to set the Colour of this Object Type has not been implemented yet.")
				End Set
			End Property

#Region "Bezier Operators"
			Public Shared Operator =(ByVal _BezierOne As QuadraticBezier, ByVal _BezierTwo As QuadraticBezier) As Boolean
				Return ((_BezierOne.StartPoint = _BezierTwo.StartPoint) AndAlso (_BezierOne.ControlPoint = _BezierTwo.ControlPoint) AndAlso (_BezierOne.EndPoint = _BezierTwo.EndPoint))
			End Operator

			Public Shared Operator <>(ByVal _BezierOne As QuadraticBezier, ByVal _BezierTwo As QuadraticBezier) As Boolean
				Return (Not (_BezierOne = _BezierTwo))
			End Operator
#End Region

		End Structure

		Public Structure DraggablePoint : Implements IDesmosExpression

			''' <summary>
			''' The SubScript X and Y number to use for the corrosponding Desmos Variables for this DraggablePoint.
			''' E.g. were it 5, then this Point's LatexExpr would be (x_{5},y_{5})
			''' </summary>
			Public XYVariablesNumber As UInt16
			Private ExpressionColour_ As Drawing.Color

			Public Sub New(ByVal _XYVariablesNumber As UInt16)
				With Me
					.XYVariablesNumber = _XYVariablesNumber
					.Colour = Drawing.Color.Blue
				End With
			End Sub

			Public Property Colour As System.Drawing.Color Implements IDesmosExpression.Colour
				Get
					Return Me.ExpressionColour_
				End Get
				Set(ByVal _NewValue As System.Drawing.Color)
					Me.ExpressionColour_ = _NewValue
				End Set
			End Property

			''' <summary>
			''' !!! There are no GraphBoundries for a DraggablePoint; it's position is determined by it's X and Y Variables
			''' </summary>
			Public ReadOnly Property GraphBoundries As DesmosGraphSize Implements IDesmosExpression.GraphBoundries
				Get
					Return New DesmosGraphSize() With {.MostNegativeX = 0, .MostNegativeY = 0, .MostPositiveX = 0, .MostPositiveY = 0}
				End Get
			End Property

			Public Function ToLatexExpression() As String Implements IDesmosExpression.ToLatexExpression
				Return ("(x_{" & Me.XYVariablesNumber.ToString() & "},y_{" & Me.XYVariablesNumber.ToString() & "})").MathsExpressionToLatex()
			End Function

		End Structure

		Public Structure DesmosVariable : Implements IDesmosExpression

			Public Name$
			''' <summary>
			''' I.E. Name_{SubScriptNumber} for the LaTeX Expr
			''' </summary>
			Public SubScriptNumber As UInt16
			Public Value As Decimal

			Public Sub New(ByVal _Name$, ByVal _SubScriptNumber As UInt16, ByVal _Value As Decimal)
				With Me
					.Name = _Name
					.SubScriptNumber = _SubScriptNumber
					.Value = _Value
				End With
			End Sub

			''' <summary>
			''' This Type of DesmosExpr isn't actually seen on the Graph, so the Colour dosen't matter
			''' </summary>
			Public Property Colour As System.Drawing.Color Implements IDesmosExpression.Colour
				Get
					Return Drawing.Color.Black
				End Get
				Set(ByVal _NewValue As System.Drawing.Color)
					Throw New GraphPicturesException("The Colour of a DesmosVariable cannot be set because the Variable dosen't actually appear on the Graph")
				End Set
			End Property

			''' <summary>
			''' Variables do not appear on the Graph themselves, so do not have boundries
			''' </summary>
			Public ReadOnly Property GraphBoundries As DesmosGraphSize Implements IDesmosExpression.GraphBoundries
				Get
					Return DesmosGraphSize.NoGraphSize
				End Get
			End Property

			Public Function ToLatexExpression() As String Implements IDesmosExpression.ToLatexExpression
				Return (Me.Name & "_{" & Me.SubScriptNumber.ToString() & "}=" & Me.Value.ToString())
			End Function

		End Structure

		''' <summary>
		''' Represents a Generic Desmos Latex Expression
		''' </summary>
		Public Structure GenericDesmosLatexExpression : Implements IDesmosExpression

			Public LatexExpression$
			Private Colour_ As Drawing.Color

			Public Sub New(ByVal _LatexExpression$)
				Me.LatexExpression = _LatexExpression
			End Sub

			Public Property Colour As System.Drawing.Color Implements IDesmosExpression.Colour
				Get
					Return Me.Colour_
				End Get
				Set(ByVal _NewValue As System.Drawing.Color)
					Me.Colour_ = _NewValue
				End Set
			End Property

			Public ReadOnly Property GraphBoundries As DesmosGraphSize Implements IDesmosExpression.GraphBoundries
				Get
					Return DesmosGraphSize.NoGraphSize
				End Get
			End Property

			Public Function ToLatexExpression() As String Implements IDesmosExpression.ToLatexExpression
				Return Me.LatexExpression
			End Function

		End Structure

		REM This couldn't go inside the Structre Below because Struct's don't support Instance Member Initialisers
		Public ReadOnly PathInstructionChars As Char() = "MmLlCcZz".ToCharArray()

		''' <summary>
		''' Represents an SVG path element Instruction, such as "M 50 50"
		''' </summary>
		''' <remarks></remarks>
		Public Structure SVGPathInstruction

			Public Const PathInstructionRegExp$ = "^(((m|M|l|L|c|C) *(\-?\d{1,8})( \-?\d{1,8})+)|(z|Z))$"

			Public ReadOnly CommandLetter As Char
			Public ReadOnly Points As Drawing.Point()

			Public Sub New(ByVal _CommandLetter As Char, ByVal _Points As Drawing.Point())
				With Me
					.CommandLetter = _CommandLetter
					.Points = _Points
				End With
			End Sub

			''' <summary>
			''' Takes in a Singular Instruction which appeared in a Path's d="" Attribute. E.g. C 60 90
			''' </summary>
			''' <param name="_DAttributeInstruction"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Shared Function FromDAttributeInstruction(ByVal _DAttributeInstruction$) As SVGPathInstruction

				Try

					'The _DAttrInstr should not have any leading or Trailing Whitespace, but make sure anyway
					_DAttributeInstruction.RemoveLeadingWhiteSpace()
					_DAttributeInstruction.RemoveTrailingWhiteSpace()
					_DAttributeInstruction.RemoveLineBreaksAndTabs()

					REM Perform some checks on the _DAttrInstr we have been given:
					If String.IsNullOrEmpty(_DAttributeInstruction) Then Throw New Exception("An attempt was made to construct an SVGPathInstruction Object, however an empty string was passed to the function SVGPathInstruction.FromDAttributeInstruction(ByVal _DAttributeInstruction$).The string was found to be empty after removing all Tabs and LineBreaks. The Object cannot be constructed from an Empty String. Make sure that there are no Tabs and LineBreaks in the wrong places in the DAttribute in the SVG File.")
					If Not _DAttributeInstruction.MatchesRegEx(SVGPathInstruction.PathInstructionRegExp) Then Throw (New Exception("An attempt was made to create an SVGPathInstruction Object from a d-Attribute Instruction, however the instruction did not match the Regular Expression. The Instruction was: " & _DAttributeInstruction))

					Dim _InstructionLetter As Char = _DAttributeInstruction.FirstCharacter()

					REM At this point, we have already all the data we need, if the Instruction was a [z or Z]; No Points needed for [z]
					'The Z|z is made Uppercase at this point, because they are the same, and there is no point doing multiple checks
					'...For both the UPPER and lower case Versions later on in the Program.
					If Char.ToUpper(_InstructionLetter) = "Z"c Then Return (New SVGPathInstruction(Char.ToUpper(_InstructionLetter), {}))

					'Remove the actual Instruction which is the FirstChar
					_DAttributeInstruction.RemoveFirstCharacter()

					'REMOVE LEADING WHITESPACE, now that the CommandLetter has been removed. (There could have been some E.g. "l  504 345 6 33").
					'If you don't do this, the WhiteSpace could count as an extra _NumberFollowingInstruction, causing the checks to fail
					_DAttributeInstruction.RemoveLeadingWhiteSpace()

					'We should now be left with a series of Space-Deliminated Signed Integers, which we need to make into coordinates
					Dim _NumbersFollowingInstruction$() = _DAttributeInstruction.Split(" "c)

					'If the CommandLetter is an [M], then there must be exactly two numbers in the list.
					If (Char.ToUpper(_InstructionLetter) = "M"c) AndAlso (Not (_NumbersFollowingInstruction.Length = 2)) Then Throw (New Exception("The CommandLetter [M] or [m] was detected, however there were not exactly two numbers following the CommandLetter in the String. There must be exactly two; an X and a Y."))

					'There should be an even number of Numbers in the List; X and Y Coordinate Pairs
					If (Not _NumbersFollowingInstruction.Length.IsEven()) Then Throw (New Exception("There were not an even number of numbers after the [SVGPath Instruction]'s Letter. Bear in mind that any stray [Space Character] within the Coordinate Numbers would be counted as an additional Element.The Full Instruction was: " & _DAttributeInstruction))

					REM Now make the Numbers into [System.Drawing.Point]'s
					Dim _InstructionPoints As New List(Of Drawing.Point)()

					For _NumberIndex% = 0 To (_NumbersFollowingInstruction.Length - 1) Step +2
						_InstructionPoints.Add(New Drawing.Point(
						  x:=CInt(_NumbersFollowingInstruction(_NumberIndex)),
						  y:=CInt(_NumbersFollowingInstruction(_NumberIndex + 1))
						))
					Next

					REM Now build the SVGPathInstruction Object
					Return (New [SVGPathInstruction](_InstructionLetter, _InstructionPoints.ToArray()))

				Catch _Ex As System.Exception When True
					Throw New Exception("An attempt was made to instanciate an SVGPathInstruction from a d-Attribute, however the following Exception was throw during the process: " & _Ex.Message, _Ex)
				End Try

			End Function

			REM For the whole d="" Attr., of the <path>
			Public Shared Function FromDAttributeInstructions(ByVal _DAttributeString$) As SVGPathInstruction()

				Dim _SVGPathInstructionsToReturn As New List(Of SVGPathInstruction)()

				REM This makes sure that if a LineBreak|Tab was used instead of a Space, then the Input still matches the RegExp
				_DAttributeString.ReplaceLineBreaksAndTabsWithSpaces()

				For Each _InstructionString$ In _DAttributeString.RemoveLineBreaksAndTabs().SplitAtWithoutRemoving([BezierObjects].PathInstructionChars)
					_SVGPathInstructionsToReturn.Add(SVGPathInstruction.FromDAttributeInstruction(_InstructionString))
				Next

				Return _SVGPathInstructionsToReturn.ToArray()

			End Function

			''' <summary>
			''' Produces a String representing the CommandLetter and Points of the current SVGPathInstruction Instance.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Overrides Function ToString() As String

				Dim _PointsString$ = String.Empty

				For Each _Point As Drawing.Point In Me.Points
					_PointsString &= "{ X=" & _Point.X.ToString() & ", Y=" & _Point.Y.ToString() & " }, "
				Next

				REM Remove the ", " from the End... Unless there were no _Points because the CommandLetter was a z.
				If Not String.IsNullOrEmpty(_PointsString) Then
					_PointsString.RemoveLastCharacter() : _PointsString.RemoveLastCharacter()
				End If

				Return "{ CommandLetter=" & Me.CommandLetter.ToString() & ", Points=[" & _PointsString & "] }"

			End Function

		End Structure

		''' <summary>
		''' An structre to represent the Size of a BezierGraph
		''' </summary>
		''' <remarks></remarks>
		Public Structure DesmosGraphSize
			Public MostPositiveX% : Public MostPositiveY%
			Public MostNegativeX% : Public MostNegativeY%
			Public ReadOnly Property Height As [UInt32]
				Get
					Return CUInt(Me.MostPositiveY - Me.MostNegativeY)
				End Get
			End Property
			Public ReadOnly Property Width As [UInt32]
				Get
					Return CUInt(Me.MostPositiveX - Me.MostNegativeX)
				End Get
			End Property

			''' <summary>
			''' Used for Expressions which don't appear on the Graph themselves, such as Variables
			''' </summary>
			Public Shared ReadOnly Property NoGraphSize As DesmosGraphSize
				Get
					Return (New DesmosGraphSize() With {.MostNegativeX = 0, .MostNegativeY = 0, .MostPositiveX = 0, .MostPositiveY = 0})
				End Get
			End Property

			''' <summary>
			''' Used for Blank VideoFrames
			''' </summary>
			Public Shared ReadOnly Property StandardViewport As DesmosGraphSize
				Get
					Return (New DesmosGraphSize() With {.MostNegativeX = -10, .MostNegativeY = -7, .MostPositiveX = 10, .MostPositiveY = 7})
				End Get
			End Property
		End Structure

	End Module

End Namespace