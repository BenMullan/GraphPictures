Namespace Library

	Namespace CompilerExtentions

		Public Module StringExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Chops off Excess Characters from the end of the String if it's too long
			''' </summary>
			<Runtime.CompilerServices.Extension()>
			Public Function MakeNoLongerThan(ByVal _InputString$, ByVal _MaxLength As UInt32) As String
				Return If(_InputString.Length > _MaxLength, _InputString.Substring(0, CInt(_MaxLength)), _InputString)
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Path is Valid, before returning a version of it with a backslach if it dosen't have one already.
			''' </summary>
			<Runtime.CompilerServices.Extension()>
			Public Function Clense(ByVal _InputString$, ByVal _AllowedChars As Char()) As String
				Return (New String(_InputString.ToCharArray().Where(AddressOf _AllowedChars.Contains).ToArray()))
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Path is Valid, before returning a version of it with a backslach if it dosen't have one already.
			''' </summary>
			<Runtime.CompilerServices.Extension()>
			Public Function MakeValidFolderPath(ByVal _PotentialFolderPath$) As String
				If (IO.Path.GetInvalidPathChars().Any(AddressOf _PotentialFolderPath.Contains)) Then Throw (New ArgumentException("The Folder Path could not be made valid because it contained at least one invalid character. It was: {" & _PotentialFolderPath & "}"c, "_PotentialFolderPath"))
				Return If(_PotentialFolderPath.LastCharacter() = "\"c, _PotentialFolderPath, _PotentialFolderPath & "\"c)
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Splits the String whenever one of the _CharsAtWhichToSplitString occours, but the _CharAtWhichToSplitString stays in each New String
			''' <example>
			''' "Hello, World; Today" and {","c, ";"c} Becomes {"Hello", ", World", "; Today"}
			''' </example>
			''' </summary>
			''' <param name="_InputString"></param>
			''' <param name="_CharsAtWhichToSplitString"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			<System.Runtime.CompilerServices.Extension()>
			Public Function SplitAtWithoutRemoving(ByVal _InputString$, ByVal _CharsAtWhichToSplitString As Char()) As String()

				Dim _StringsToReturn As New List(Of String)()
				Dim _StringToAddAccumulator$ = String.Empty

				For Each _Char As Char In _InputString.ToCharArray()

					If _CharsAtWhichToSplitString.Contains(_Char) Then

						REM Only add the String to be returned, if it is not empty
						If (Not String.IsNullOrEmpty(_StringToAddAccumulator)) Then _StringsToReturn.Add(_StringToAddAccumulator)
						_StringToAddAccumulator = String.Empty

					Else
						REM The _Char isn't one to split the String at
					End If

					_StringToAddAccumulator &= _Char.ToString()

				Next

				REM Add the Remaining contents of the Accumulator to the output
				_StringsToReturn.Add(_StringToAddAccumulator)

				Return _StringsToReturn.ToArray()

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Modifies the Existing String, as well as returning it.
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function RemoveLineBreaksAndTabs$(ByRef _InputString$)

				REM 12	=	Form Feed
				REM 13	=	Carridge Return

				_InputString = _InputString.Replace(vbCr, "")
				_InputString = _InputString.Replace(vbLf, "")
				_InputString = _InputString.Replace(vbTab, "")

				Return _InputString

			End Function

			''' <summary>
			''' (Custom Extention) Modifies the Existing String, as well as returning it.
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ReplaceLineBreaksAndTabsWithSpaces$(ByRef _InputString$)

				REM 12	=	Form Feed
				REM 13	=	Carridge Return

				_InputString = _InputString.Replace(vbCr, " ")
				_InputString = _InputString.Replace(vbLf, " ")
				_InputString = _InputString.Replace(vbTab, " ")

				Return _InputString

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Returns a new version of the string, which dosen't contain any of the disallowed Characters
			''' </summary>
			''' <param name="_InputString"></param>
			''' <param name="_DisallowedChars">The characters to remove from the String</param>
			''' <returns>The String, without these Characters in it</returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function RemoveAllOccourancesOf(ByVal _InputString$, ByVal ParamArray _DisallowedChars As Char()) As Global.System.String

				Dim _CleanedStringToReturn$ = String.Empty

				For Each _Char As Char In _InputString.ToCharArray()
					If (Not _DisallowedChars.Contains(_Char)) Then _CleanedStringToReturn &= _Char.ToString()
				Next

				Return _InputString

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Removes any Spaces, Tabs, or LineBreak Characters which appear at the start of the String
			''' </summary>
			''' <param name="_InputString">The String from which to Remove WhiteSpace</param>
			''' <returns>The String which has been modified</returns>
			''' <remarks>Not only modifies this method the Input String, but it also returns the resultant output. Chars removed (ASCII): 09, 10, 11, 12, 13, 32</remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function RemoveLeadingWhiteSpace$(ByRef _InputString$)

				REM 09	=	Horisontal Tab
				REM 10	=	Line Feed
				REM	11	=	Vertical Tab
				REM 12	=	Form Feed
				REM 13	=	Carridge Return
				REM 32	=	Space

				Dim _DisallowedChars As Char() = {Chr(9I), Chr(10I), Chr(11I), Chr(12I), Chr(13I), Chr(32I)}

				For Each _Char As Char In _InputString$.ToCharArray()

					'If the _Char is one of the ones we want to Remove...
					If _DisallowedChars.Contains(_Char) Then
						'...Then Remove it.
						_InputString.RemoveFirstCharacter()
					Else
						'Otherwise, We must have hit the start of meaningful Characters in the String, so stop checking.
						'Simply return the String as it is, now that WhiteSpace has been removed.
						Return _InputString
					End If

				Next

				'We could end up here, if:
				'	the Entire _InputString$ was all WhiteSpace,
				'	Or, the _InputString was a String::Empty when passed to the Function,
				'So retrun it, even though it's empty.
				Return _InputString

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Removes any Spaces, Tabs, or LineBreak Characters which appear at the end of the String
			''' </summary>
			''' <param name="_InputString">The String from which to Remove WhiteSpace</param>
			''' <returns>The String which has been modified</returns>
			''' <remarks>Not only modifies this method the Input String, but it also returns the resultant output. Chars removed (ASCII): 09, 10, 11, 12, 13, 32</remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function RemoveTrailingWhiteSpace$(ByRef _InputString$)

				REM 09	=	Horisontal Tab
				REM 10	=	Line Feed
				REM	11	=	Vertical Tab
				REM 12	=	Form Feed
				REM 13	=	Carridge Return
				REM 32	=	Space

				Dim _DisallowedChars As Char() = {Chr(9I), Chr(10I), Chr(11I), Chr(12I), Chr(13I), Chr(32I)}

				For Each _Char As Char In _InputString$.ToCharArray().Reverse()

					'If the _Char is one of the ones we want to Remove...
					If _DisallowedChars.Contains(_Char) Then
						'...Then Remove it.
						_InputString.RemoveLastCharacter()
					Else
						'(REMEMBER: this is going from the last Char in the String to the first one)
						'Otherwise, We must have hit the start of meaningful Characters in the String, so stop checking.
						'Simply return the String as it is, now that WhiteSpace has been removed.
						Return _InputString
					End If

				Next

				'We could end up here, if:
				'	the Entire _InputString$ was all WhiteSpace,
				'	Or, the _InputString was a String::Empty when passed to the Function,
				'So retrun it, even though it's empty.
				Return _InputString

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Removes the First Character from the String
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns></returns>
			''' <remarks>Both Modifies the Parameter, and returns the post-computation Result</remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function RemoveFirstCharacter$(ByRef _InputString$)

				'If the String is Empty, we wouldn't be able to Remove the FirstChar
				If _InputString.IsEmpty() Then Throw New Exception("The First Character could not be removed from the String, because the String is empty.")

				'If we've gotten to here, the String must have at least 1 Character, so Remove the FirstChar
				_InputString = _InputString.Remove(0I, 1I)
				Return _InputString

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Removes the Last Character from the String
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns></returns>
			''' <remarks>Both Modifies the Parameter, and returns the post-computation Result</remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function RemoveLastCharacter$(ByRef _InputString$)

				'If the String is Empty, we wouldn't be able to Remove the LastChar
				If _InputString.IsEmpty() Then Throw New Exception("The Last Character could not be removed from the String, because the String is empty.")

				'If we've gotten to here, the String must have at least 1 Character, so Remove the LastChar
				_InputString = _InputString.Remove(_InputString.Length - 1, 1I)
				Return _InputString

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Removes the Last two Characters from the String
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns></returns>
			''' <remarks>Both Modifies the Parameter, and returns the post-computation Result</remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function RemoveLastTwoCharacters$(ByRef _InputString$)

				'If the String is Empty, we wouldn't be able to Remove the LastChar
				If _InputString.IsEmpty() Then Throw New Exception("The Last Character could not be removed from the String, because the String is empty.")

				'If we've gotten to here, the String must have at least 1 Character, so Remove the LastChar
				_InputString = _InputString.Remove(_InputString.Length - 1, 1I)

				REM Check if the input is now empty wegen the last removal
				If _InputString.IsEmpty() Then Throw New Exception("The Second To Last Character could not be removed from the String, because the String was empty after the removal of it's first character.")

				_InputString = _InputString.Remove(_InputString.Length - 1, 1I)

				Return _InputString

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Evaluates weather or not the String is Empty. (WhiteSpace does not count as Empty)
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns>Retruns True if the String is Empty</returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function IsEmpty(ByRef _InputString$) As Boolean
				Return _InputString = Global.System.String.Empty
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Returns the First Character in the String
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns>Returns the First Character in the String</returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function FirstCharacter(ByRef _InputString$) As Char

				If _InputString.IsEmpty() Then Throw New Exception("The First Character of the String could not be found, because the String is empty.")
				Return _InputString.ToCharArray()(0)

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Returns the Last Character in the String
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns>Returns the Last Character in the String</returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function LastCharacter(ByRef _InputString$) As Char

				If _InputString.IsEmpty() Then Throw New Exception("The Last Character of the String could not be found, because the String is empty.")
				Return _InputString.ToCharArray()(_InputString.Length - 1)

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Encapsulates the current System.String Object with the _StringWithWhichToWrapInput$. This modifies the String Object from which this method is called, and returns the result too.
			''' </summary>
			''' <param name="_InputString">The String to Wrap</param>
			''' <param name="_StringWithWhichToWrapInput">The String to prepend and append the _InputString$ with</param>
			''' <returns>The Encapsulated String</returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function EncapsulateIn(ByRef _InputString$, ByVal _StringWithWhichToWrapInput$) As String
				_InputString = _StringWithWhichToWrapInput & _InputString & _StringWithWhichToWrapInput
				Return _InputString
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Matches a String against a Regular Expression
			''' </summary>
			''' <param name="_StringToValidate"></param>
			''' <param name="_RegExPattern">The pattern to satisfy</param>
			''' <returns>Returns True if the RegExp is satisfied by the Input String.</returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function MatchesRegEx(ByVal _StringToValidate$, ByVal _RegExPattern$) As Boolean
				Return (New Global.System.Text.RegularExpressions.Regex([pattern]:=_RegExPattern).IsMatch(_StringToValidate))
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Converts a Mathematical Expression E.g. 5 + (2 - 6) into a Latex one, replacing Brackets and MultiplyBy Symbols
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function MathsExpressionToLatex(ByVal _MathsExpression$) As System.String

				Return _MathsExpression.Replace("(", "\left(").Replace(")", "\right)").Replace("*", "\cdot ")

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Converts a LaTeX Expression, to a Mathematical Expression E.g. 5 + (2 - 6).Brackets and MultiplyBy Symbols are replaced
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function LatexToMathsExpression(ByVal _Latex$) As System.String

				Return _Latex$.Replace("\left(", "(").Replace("\right)", ")").Replace("\cdot ", "*")

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Attempts to convert the String to a Decimal. Throws an Exception on failure to do so.
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ToDecimal(ByVal _InputString$) As System.Decimal

				Try

					If _InputString.IsEmpty() Then Throw (New Exception("The String was empty."))
					If Not _InputString.MatchesRegEx("^\d+(\.\d+)?$") Then Throw (New Exception("The String did not match the Regular Expression for a Decimal"))

					Return CDec(_InputString)

				Catch _Ex As Exception When True
					Throw (New Exception("The following Exception was thrown upon attempting to convert the string """ & _InputString & """, into a Decimal: " & _Ex.Message, _Ex))
				End Try

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Returns the String encoded into Base64.
			''' </summary>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ToASCIIBase64(ByVal _InputString$) As String

				Try

					If _InputString.IsEmpty() Then Throw (New Exception("The String was empty."))

					Return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(_InputString))

				Catch _Ex As Exception When True
					Throw (New Exception("The following Exception was thrown upon attempting to convert get the ASCII Base64 for a String: " & _Ex.Message, _Ex))
				End Try

			End Function

		End Module

		Public Module NumericExtentions

#Region "Numeric Conversion Extention Methods"

			Const BytesInKB% = 1024I

			''' <summary>
			''' (MullNet CompilerExtention) Converts [A Number of Bytes] into [How many KB that would be]
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function BytesToIntegralKB(ByVal _NumberOfBytes As UInt64) As UInt64

				Return CULng(CLng(_NumberOfBytes) \ BytesInKB%)

			End Function

#End Region

			''' <summary>
			''' Evaluates weather or not the _Integer is Even, by BitWise ANDing it with 1
			''' </summary>
			''' <param name="_Integer"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			<System.Runtime.CompilerServices.Extension()>
			Public Function IsEven(ByVal _Integer%) As Boolean
				Return (_Integer And 1) = 0
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Returns 105% of the existing Integer Value, Rounded to the nearest Integer if a Decimal would be produced.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function Plus5Percent(ByRef _InputInteger%) As Int32

				Return CInt(_InputInteger * 1.05)

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Evaluates weather or not the _InputInteger is [Higher than the _LowerValue] AND [Lower than the _HigherValue]
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function IsBetween(ByRef _InputNumber&, ByVal _LowerValue&, ByVal _HigherValue&) As Boolean

				Return ((_InputNumber > _LowerValue) AndAlso (_InputNumber < _HigherValue))

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Evaluates weather or not the _InputInteger is [Higher than OR EQUAL TO the _LowerValue] AND [Lower than OR EQUAL TO the _HigherValue]
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function IsBetweenInclusive(ByRef _InputInt64&, ByVal _LowerValue&, ByVal _HigherValue&) As Boolean

				Return ((_InputInt64 >= _LowerValue) AndAlso (_InputInt64 <= _HigherValue))

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Evaluates weather or not the _Integer is at least _Amount.
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function IsAtLeast(ByVal _Integer%, ByVal _Amount%) As Boolean

				Return (_Integer >= _Amount)

			End Function

		End Module

		Public Module CollectionTypeExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Returns the Maximun possible Index one could specify for a non-[dynamically-allocated], zero-indexed Array
			''' </summary>
			''' <param name="_Array"></param>
			''' <returns>Returns the Maximun possible Index one could specify for a non-[dynamically-allocated], zero-indexed Array</returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function MaxIndex%(ByRef _Array As Array)
				Return (_Array.Length - 1)
			End Function

			''' <summary>
			''' (MullNet CompilerExtention)
			''' </summary>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function PenultimateIndex%(ByRef _Array As Array)
				Return (_Array.Length - 2)
			End Function

			''' <summary>
			''' (MullNet CompilerExtention)
			''' </summary>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ListValues(ByVal _List As List(Of UInt64)) As String
				Dim _ValuesString$ = "{"
				_List.ForEach(Sub(_Value As UInt64) _ValuesString &= (_Value & ", "))
				If Not _List.Count = 0 Then _ValuesString.RemoveLastTwoCharacters()
				Return _ValuesString & "}"
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Repeats the _Object _Times times, returning the resultans Array in which every item is a copy of (or Referance to) _Object
			''' </summary>
			<Runtime.CompilerServices.Extension()>
			Public Function Repeat_TimesNotImplementedCorrectly(Of T)(ByRef _Object As T, ByVal _Times As UInt32) As T()

				'This does it one too many or one too few times, but the SubBezier Logic already relies on it, so it hasn't been changed yet...

				Dim _TsToReturn As New List(Of T)()

				For _Time As UInt32 = 0 To _Times
					_TsToReturn.Add(_Object)
				Next

				Return _TsToReturn.ToArray()

			End Function

		End Module

		Public Module GraphPictureObjectExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Attempts to convert the String into a ColourResources.ColourRange.ColourGenerationMode
			''' </summary>
			''' <param name="_InputString"></param>
			''' <returns></returns>
			''' <remarks></remarks>
			<System.Runtime.CompilerServices.Extension()>
			Public Function ToColourGenerationMode(ByVal _InputString$) As ColourResources.ColourRange.ColourGenerationMode
				Try
					Return CType([Enum].Parse(GetType(ColourResources.ColourRange.ColourGenerationMode), _InputString$), ColourResources.ColourRange.ColourGenerationMode)
				Catch _Ex As Exception When True
					Throw New Exception("The following Exception was thrown upon attempting to Convert a String to a ColourResources.ColourRange.ColourGenerationMode: " & _Ex.Message, _Ex)
				End Try
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Attempts to convert the String into a DesmosStateCreationOptions.DesmosLineStyle
			''' </summary>
			<System.Runtime.CompilerServices.Extension()>
			Public Function ToDesmosLineStyle(ByVal _InputString$) As DesmosStateCreationOptions.DesmosLineStyle
				Try
					Return CType([Enum].Parse(GetType(DesmosStateCreationOptions.DesmosLineStyle), _InputString$), DesmosStateCreationOptions.DesmosLineStyle)
				Catch _Ex As Exception When True
					Throw New Exception("The following Exception was thrown upon attempting to Convert a String to a DesmosStateCreationOptions.DesmosLineStyle: " & _Ex.Message, _Ex)
				End Try
			End Function

			<System.Runtime.CompilerServices.Extension()>
			Public Function MovedBy(ByVal _OrigionalPoint As Drawing.Point, ByVal _AmountByWhichToChange As Drawing.Point) As Drawing.Point
				Return (New Drawing.Point(
				 x:=(_OrigionalPoint.X + _AmountByWhichToChange.X),
				 y:=(_OrigionalPoint.Y + _AmountByWhichToChange.Y)
				))
			End Function

			<System.Runtime.CompilerServices.Extension()>
			Public Function GoBackBy(ByVal _OrigionalPoint As Drawing.Point, ByVal _AmountByWhichToGoBack As Drawing.Point) As Drawing.Point
				Return (New Drawing.Point(
				 x:=(_OrigionalPoint.X - _AmountByWhichToGoBack.X),
				 y:=(_OrigionalPoint.Y - _AmountByWhichToGoBack.Y)
				))
			End Function

			<System.Runtime.CompilerServices.Extension()>
			Public Function LUPAsItWouldHaveBeenAfterIndex(ByRef _PointsArray As Drawing.Point(), ByVal _Index%, ByVal _WhatLUPWasAtPointsArray0 As Drawing.Point) As Drawing.Point

				Dim _LUPToReturn As Drawing.Point = _WhatLUPWasAtPointsArray0

				For _PointNumber% = 0I To _Index Step +1
					_LUPToReturn = _LUPToReturn.MovedBy(_PointsArray(_PointNumber))
				Next

				Return _LUPToReturn

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Produces a JSON String which a Desmos Instance can be set to, in order to draw all the DesmosExpressions in the Array
			''' </summary>
			''' <param name="_DesmosExpressions">An object of Type [IDesmosExpression()]</param>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ToDesmosState(ByRef _DesmosExpressions As IDesmosExpression(), ByVal _Options As DesmosStateCreationOptions) As String

				REM Scale the Beziers to a consistent Graph-Size (so that e.g. consecutive Video Frames aren't shake-y)
				_DesmosExpressions = _DesmosExpressions.ScaledTo(Library.Resources.BezierScaling_TargetDesmosGraphWidth)

				Dim _GraphZoomSize@ = _Options.GraphZoomSize

				'If there are no Beziers (E.g. if it's an enpty VideoFrame), then return a GraphSize of the Standard Viewport
				Dim _BeziersGraphSize As DesmosGraphSize = _
				 If(_DesmosExpressions.Length = 0,
				  [DesmosGraphSize].StandardViewport,
				  _DesmosExpressions.GetGraphSize()
				 )

				Dim _GraphMidPoint As New Drawing.Point((_BeziersGraphSize.MostNegativeX + _BeziersGraphSize.MostPositiveX) \ 2, (_BeziersGraphSize.MostNegativeY + _BeziersGraphSize.MostPositiveY) \ 2)

				REM GraphBounds via MIDPOINT
				'Calcuate the Positioning by taking the _GraphMidPoint, and making the Adding [half the graph Height and width, times the _GraphZoomSize] to the MidPoint, to get the xmin, xmax, ymin, and ymax
				'Dim _DesmosStateJSON$ = "{""version"":8,""graph"":{" & _
				' IIf(_Options.ShowGrid, String.Empty, """showGrid"":false,""showXAxis"":false,""showYAxis"":false,""xAxisNumbers"":false,""yAxisNumbers"":false,""polarNumbers"":false,").ToString() & _
				' """viewport"":{""xmin"":" & CInt(_GraphMidPoint.X - ((_BeziersGraphSize.Width \ 2) * _GraphZoomSize)).ToString() & _
				' ",""ymin"":" & CInt(_GraphMidPoint.Y - ((_BeziersGraphSize.Height \ 2) * _GraphZoomSize)).ToString() & _
				' ",""xmax"":" & CInt(_GraphMidPoint.X + ((_BeziersGraphSize.Width \ 2) * _GraphZoomSize)).ToString() & _
				' ",""ymax"":" & CInt(_GraphMidPoint.Y + ((_BeziersGraphSize.Height \ 2) * _GraphZoomSize)).ToString() & _
				' "}},""randomSeed"":""32678c750ac8c06911bb2d61bca23ab9"",""expressions"":{""list"":[" & vbCrLf & vbCrLf

				REM GraphBounds via CONSISTENT BOUNDRIES
				'Calcuate the Positioning by taking the _GraphMidPoint, and making the Adding [half the graph Height and width, times the _GraphZoomSize] to the MidPoint, to get the xmin, xmax, ymin, and ymax
				Dim _DesmosStateJSON$ = "{""version"":8,""graph"":{" & _
				 IIf(_Options.ShowGrid, String.Empty, """showGrid"":false,""showXAxis"":false,""showYAxis"":false,""xAxisNumbers"":false,""yAxisNumbers"":false,""polarNumbers"":false,").ToString() & _
				 """viewport"":{""xmin"":" & Library.Resources.GraphViewport_MinX.ToString() & _
				 ",""ymin"":" & Library.Resources.GraphViewport_MinY.ToString() & _
				 ",""xmax"":" & Library.Resources.GraphViewport_MaxX.ToString() & _
				 ",""ymax"":" & Library.Resources.GraphViewport_MaxY.ToString() & _
				 "}},""randomSeed"":""32678c750ac8c06911bb2d61bca23ab9"",""expressions"":{""list"":[" & vbCrLf & vbCrLf

				Dim _ExpressionID% = 1
				Dim _ExpressionColour As Drawing.Color
				Dim _ExpressionLineOpacity@ = _Options.LinesOpacity
				Dim _ExpressionLineWidth@ = _Options.LinesWidth
				Dim _ExpressionLineStyle As DesmosStateCreationOptions.DesmosLineStyle = _Options.LinesStyle
				Dim _ExpressionFill As [Boolean] = _Options.Fill
				Dim _ExpressionFillOpacity@ = _Options.FillOpacity

				Dim _Colours As New Library.ColourResources.ColourRange(_Options.ColourAllocationMode)

				For _ExpressionIndex% = 0 To (_DesmosExpressions.Count - 1) Step +1

					'Get the proportion through all the Expressions in the IDesmosExpression(), by doing (_ExpressionIndex / (_DesmosExpressions.Count - 1))
					_ExpressionColour = If(_Options.ColourAllocationMode = ColourRange.ColourGenerationMode.UseExpressionColour, _DesmosExpressions(_ExpressionIndex).Colour, _Colours(CDec(_ExpressionIndex / (_DesmosExpressions.Count - 1))))
					_DesmosStateJSON &= "{""type"":""expression"",""id"":""" & _ExpressionID.ToString() & """,""color"":""" & String.Format("#{0:X2}{1:X2}{2:X2}", _ExpressionColour.R, _ExpressionColour.G, _ExpressionColour.B) & """,""latex"":""" & _DesmosExpressions(_ExpressionIndex).ToLatexExpression() & """,""lineOpacity"":""" & _ExpressionLineOpacity.ToString() & """,""lineWidth"":""" & _ExpressionLineWidth.ToString() & """,""lineStyle"":""" & _ExpressionLineStyle.ToString() & """,""fill"":" & _Options.Fill.ToString().ToLower() & IIf(_Options.Fill, ",""fillOpacity"":""" & _Options.FillOpacity.ToString() & """", String.Empty).ToString() & "}," & vbCrLf & vbCrLf

					_ExpressionID += 1
				Next

				REM If the ForLoop did add some {expression}s to the String, then there would be an extra [,] on the end
				If (Not _DesmosExpressions.Length = 0) Then
					_DesmosStateJSON.RemoveTrailingWhiteSpace()
					_DesmosStateJSON.RemoveLastCharacter()
				End If

				_DesmosStateJSON &= "]}}"

				Return _DesmosStateJSON

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Returns a LineBreak-Deliminated List of all the Expressions as Latex, ready for pasting into Desmos
			''' </summary>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ToDesmosPastable(ByRef _DesmosExprs As IDesmosExpression()) As String

				Return String.Join(
				 separator:=vbCrLf,
				 values:=(From _DesmosExpr As IDesmosExpression In _DesmosExprs Select _DesmosExpr.ToLatexExpression())
				)

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Gets the MostPositive and MostNegative X and Y Values in the Array of all Beziers
			''' </summary>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function GetGraphSize(ByRef _DesmosExpressions As IDesmosExpression()) As DesmosGraphSize

				If _DesmosExpressions.Length = 0 Then Throw New Exception("The GraphSize of the array of DesmosExpressions on a Graph can not be calcuated, because there are no [DesmosExpression]'s in the Array.")

				Dim _MostPositiveX% = _DesmosExpressions.First().GraphBoundries.MostPositiveX
				Dim _MostPositiveY% = _DesmosExpressions.First().GraphBoundries.MostPositiveY
				Dim _MostNegativeX% = _DesmosExpressions.First().GraphBoundries.MostNegativeX
				Dim _MostNegativeY% = _DesmosExpressions.First().GraphBoundries.MostNegativeY

				Dim _CurrentExpressionBoundries As DesmosGraphSize

				For Each _DesmosExpression As IDesmosExpression In _DesmosExpressions

					_CurrentExpressionBoundries = _DesmosExpression.GraphBoundries

					If _CurrentExpressionBoundries.MostPositiveX > _MostPositiveX Then _MostPositiveX = _CurrentExpressionBoundries.MostPositiveX
					If _CurrentExpressionBoundries.MostPositiveY > _MostPositiveY Then _MostPositiveY = _CurrentExpressionBoundries.MostPositiveY
					If _CurrentExpressionBoundries.MostNegativeX < _MostNegativeX Then _MostNegativeX = _CurrentExpressionBoundries.MostNegativeX
					If _CurrentExpressionBoundries.MostNegativeY < _MostNegativeY Then _MostNegativeY = _CurrentExpressionBoundries.MostNegativeY

				Next

				MsgDebug("Returning Entire SVG GraphSize:" & vbCrLf & vbCrLf & "xmin: " & _MostNegativeX & vbCrLf & "ymin: " & _MostNegativeY & vbCrLf & "xmax: " & _MostPositiveX & vbCrLf & "ymax: " & _MostPositiveY)

				Return New DesmosGraphSize() With {
				.MostPositiveX = _MostPositiveX,
				.MostPositiveY = _MostPositiveY,
				.MostNegativeX = _MostNegativeX,
				.MostNegativeY = _MostNegativeY
				}

			End Function

			''' <summary>(MullNet CompilerExtention) Gets the MostPositive and MostNegative X and Y Values in the Array of all Beziers</summary>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ScaledTo(ByVal _DesmosExpressions As IDesmosExpression(), ByVal _NewWidth As UInt32) As IDesmosExpression()

				REM We need to make the MostPositiveX equal to 1000.
				'	For any X point, it becomes [the % it is of the input's width (MostPositiveX - MostNegativeX)] of [_NewWidth]

				REM All the _DesmosExpressions need to be either [CubicBezier]s or [QuadraticBezier]s
				If Not _DesmosExpressions.Select(Of Type)(Function(_IDE As IDesmosExpression) _IDE.GetType()) _
				 .All(AddressOf {GetType(QuadraticBezier), GetType(CubicBezier)}.Contains) _
				 Then Throw New GraphPictures.Library.Exceptions.GraphPicturesException("The DesmosExpressions cannot be scaled, because they are not all Quadratic or Cubic Bezier Objects.")

				Dim _InputGraphSize As DesmosGraphSize = _DesmosExpressions.GetGraphSize()
				Dim _NewHeight As UInt32 = CUInt((_InputGraphSize.Height / _InputGraphSize.Width) * _NewWidth)

				Dim _ScaledDesmosExpressions As New List(Of IDesmosExpression)()

				'Iteratively scale each of the Beziers from the Input
				For Each _InputDesmosExpression As IDesmosExpression In _DesmosExpressions

					Select Case _InputDesmosExpression.GetType()

						Case GetType(QuadraticBezier)

							'Create a version of the Bezier, scaled to the new dimensions
							Dim _QuadraticBezier As QuadraticBezier = CType(_InputDesmosExpression, QuadraticBezier)

							_QuadraticBezier.StartPoint = New Drawing.Point(
							 x:=CInt((_QuadraticBezier.StartPoint.X / _InputGraphSize.Width) * _NewWidth),
							 y:=CInt((_QuadraticBezier.StartPoint.Y / _InputGraphSize.Height) * _NewHeight)
							)

							_QuadraticBezier.ControlPoint = New Drawing.Point(
							 x:=CInt((_QuadraticBezier.ControlPoint.X / _InputGraphSize.Width) * _NewWidth),
							 y:=CInt((_QuadraticBezier.ControlPoint.Y / _InputGraphSize.Height) * _NewHeight)
							)

							_QuadraticBezier.EndPoint = New Drawing.Point(
							 x:=CInt((_QuadraticBezier.EndPoint.X / _InputGraphSize.Width) * _NewWidth),
							 y:=CInt((_QuadraticBezier.EndPoint.Y / _InputGraphSize.Height) * _NewHeight)
							)

							_ScaledDesmosExpressions.Add(_QuadraticBezier)

						Case GetType(CubicBezier)

							'Create a version of the Bezier, scaled to the new dimensions
							Dim _CubicBezier As CubicBezier = CType(_InputDesmosExpression, CubicBezier)

							_CubicBezier.StartPoint = New Drawing.Point(
							 x:=CInt((_CubicBezier.StartPoint.X / _InputGraphSize.Width) * _NewWidth),
							 y:=CInt((_CubicBezier.StartPoint.Y / _InputGraphSize.Height) * _NewHeight)
							)

							_CubicBezier.ControlPointOne = New Drawing.Point(
							 x:=CInt((_CubicBezier.ControlPointOne.X / _InputGraphSize.Width) * _NewWidth),
							 y:=CInt((_CubicBezier.ControlPointOne.Y / _InputGraphSize.Height) * _NewHeight)
							)

							_CubicBezier.ControlPointTwo = New Drawing.Point(
							 x:=CInt((_CubicBezier.ControlPointTwo.X / _InputGraphSize.Width) * _NewWidth),
							 y:=CInt((_CubicBezier.ControlPointTwo.Y / _InputGraphSize.Height) * _NewHeight)
							)

							_CubicBezier.EndPoint = New Drawing.Point(
							 x:=CInt((_CubicBezier.EndPoint.X / _InputGraphSize.Width) * _NewWidth),
							 y:=CInt((_CubicBezier.EndPoint.Y / _InputGraphSize.Height) * _NewHeight)
							)

							_ScaledDesmosExpressions.Add(_CubicBezier)

						Case Else : Throw New Exception("Unaccounted-for DesmosExpression Type: " & _InputDesmosExpression.GetType().FullName)
					End Select

				Next

				Return _ScaledDesmosExpressions.ToArray()

			End Function

		End Module

		Public Module IOObjectExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Gets the Size of the File in KB, to the nearest Integer
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function SizeInIntegralKB(ByRef _File As IO.FileInfo) As UInt64

				Return CULng(_File.Length).BytesToIntegralKB()

			End Function

		End Module

		Public Module ImageObjectExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Modifies (and Returns) the _InputBitmap, resized to the Specified Width and Height
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ResizeTo(ByRef _InputBitmap As Drawing.Bitmap, ByVal _TargetWidth%, ByVal _TargetHeight%) As Drawing.Bitmap

				Dim _OutputBitmapRectangle As Drawing.Rectangle = (New Drawing.Rectangle(0, 0, _TargetWidth, _TargetHeight))
				Dim _OutputBitmap As Drawing.Bitmap = (New Drawing.Bitmap(_TargetWidth, _TargetHeight))

				_OutputBitmap.SetResolution(_InputBitmap.HorizontalResolution, _InputBitmap.VerticalResolution)

				Using _Graphics As Drawing.Graphics = Drawing.Graphics.FromImage(_OutputBitmap)

					With _Graphics
						.CompositingMode = Drawing.Drawing2D.CompositingMode.SourceCopy
						.CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
						.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
						.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality
						.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
					End With

					Using _WrapMode As Drawing.Imaging.ImageAttributes = (New Drawing.Imaging.ImageAttributes())

						_WrapMode.SetWrapMode(Drawing.Drawing2D.WrapMode.TileFlipXY)
						_Graphics.DrawImage(_InputBitmap, _OutputBitmapRectangle, 0, 0, _InputBitmap.Width, _InputBitmap.Height, Drawing.GraphicsUnit.Pixel, _WrapMode)

					End Using

				End Using

				_InputBitmap = _OutputBitmap

				Return _OutputBitmap

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Modifies (and Returns) the _InputBitmap, resizing both the Width and Height by the Specified _PercentageOfCurrentDimensions
			''' </summary>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function ResizeToPercentage(ByRef _InputBitmap As Drawing.Bitmap, ByVal _PercentageOfCurrentDimensions As UInt16) As Drawing.Bitmap

				If Not CLng(_PercentageOfCurrentDimensions).IsBetweenInclusive(1, 10000) Then Throw New GraphPicturesException("The _PercentageOfCurrentDimensions for the Bitmap Resize was out of the permitted range (1 to 10000 Inclusive) at: " & _PercentageOfCurrentDimensions.ToString())

				Return _InputBitmap.ResizeTo(
				 _TargetWidth:=CInt(_InputBitmap.Width * (_PercentageOfCurrentDimensions / 100)),
				 _TargetHeight:=CInt(_InputBitmap.Height * (_PercentageOfCurrentDimensions / 100))
				)

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Evaluates what the Dimensions of a Bitmap become, when it is 
			''' </summary>
			''' <returns></returns>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function GetConstrictedSize(ByRef _Bitmap As Drawing.Bitmap, ByVal _MaxDimension As UInt32) As Drawing.Size

				Try

					If _MaxDimension < 1 Then Throw New Exception("The _MaxDimension cannot be less than 1")

					If (_Bitmap.Height > _MaxDimension) OrElse (_Bitmap.Width > _MaxDimension) Then

						'Multiply both of the Dimensions by [Max/LargestDimension]

						Return New Drawing.Size( _
						   CInt(_Bitmap.Width * (_MaxDimension / If(_Bitmap.Height > _Bitmap.Width, _Bitmap.Height, _Bitmap.Width))), _
						   CInt(_Bitmap.Height * (_MaxDimension / If(_Bitmap.Height > _Bitmap.Width, _Bitmap.Height, _Bitmap.Width)))
						)

					Else
						REM If we're here, then the _Bitmap is already within the specified DimensionalBounds
						Return _Bitmap.Size
					End If
				Catch _Ex As Exception When True
					Throw New Exception("The following Exception was thrown upon attempting to evaluate the Size of a Constricted Bitmap: " & _Ex.Message & ". The _MaxDimension was: " & _MaxDimension.ToString(), _Ex)
				End Try
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Makes sure that the Largest Dimension of the _Bitmap is no larger than the _MaxDimension, resizing the _Bitmap is needed.
			''' </summary>
			''' <returns></returns>
			''' <remarks>Modifies and Returns the _Bitmap</remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function MakeNoLargerThan(ByRef _Bitmap As Drawing.Bitmap, ByVal _MaxDimension As UInt32) As Drawing.Bitmap

				Try

					If _MaxDimension < 1 Then Throw New Exception("The _MaxDimension cannot be less than 1")

					If (_Bitmap.Height > _MaxDimension) OrElse (_Bitmap.Width > _MaxDimension) Then

						'Multiply both of the Dimensions by [Max/LargestDimension]

						Return _Bitmap.ResizeTo( _
						 CInt(_Bitmap.Width * (_MaxDimension / If(_Bitmap.Height > _Bitmap.Width, _Bitmap.Height, _Bitmap.Width))), _
						 CInt(_Bitmap.Height * (_MaxDimension / If(_Bitmap.Height > _Bitmap.Width, _Bitmap.Height, _Bitmap.Width)))
						)

					Else
						REM If we're here, then the _Bitmap is already within the specified DimensionalBounds
						Return _Bitmap
					End If
				Catch _Ex As Exception When True
					Throw New Exception("The following Exception was thrown upon attempting to Make a Bitmap no Larger than a MaxDimension: " & _Ex.Message & ". The _MaxDimension was: " & _MaxDimension.ToString(), _Ex)
				End Try
			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Returns a copy of the _Bitmap, zooming in by the the _ZoomFactor
			''' </summary>
			''' <param name="_Bitmap"></param>
			''' <param name="_ZoomFactor">
			''' E.g. [0.8] would cause 10% of the width to be removed from both sides, and 10% of the height to be removed from both the top and bottom, going inwards towards the center of the image.
			''' In other words, a _ZoomFactor of 0.8 causes the Returned copy of the Image to be 80% of the height of the origional, and 80% of the width of the origional
			''' </param>
			''' <returns></returns>
			''' <remarks>
			''' A _ZoomFactor of [0.8] would cause 10% of the width to be removed from both sides, and 10% of the height to be removed from both the top and bottom (going inwards towards the center of the image).
			''' In other words, a _ZoomFactor of 0.8 causes the Returned copy of the Image to be 80% of the height of the origional, and 80% of the width of the origional
			''' 0.8 would therefore actually cause 64% of the pixels of the origional image to be kept, because 10*10 = 100, but 8*8 only = 64.
			''' </remarks>
			<Runtime.CompilerServices.Extension()>
			Public Function GetZoomedInCopy(ByRef _Bitmap As Drawing.Bitmap, Optional ByVal _ZoomFactor As Decimal = 0.8@) As Drawing.Bitmap

				Try

					REM The _ZoomFactor must be [lessthan or equal to 1], and [greater than 0]; We cannot Zoom Out
					If Not ((_ZoomFactor <= 1) AndAlso (_ZoomFactor > 0)) Then Throw (New GraphPicturesConversionException("The _ZoomFactor must be [lessthan or equal to 1], and [greater than 0]. It was, however: " & _ZoomFactor.ToString()))


					Dim _RectangleOfOrigionalToKeep As New Drawing.RectangleF( _
					 x:=((_Bitmap.Size.Width * (1 - _ZoomFactor)) / 2), _
					 y:=((_Bitmap.Size.Height * (1 - _ZoomFactor)) / 2), _
					 width:=(_Bitmap.Size.Width * _ZoomFactor), _
					 height:=(_Bitmap.Size.Height * _ZoomFactor) _
					)

					Return _Bitmap.Clone(_RectangleOfOrigionalToKeep, _Bitmap.PixelFormat)










					REM --------------------------------------------------- Old Version with Manual Pixel Copying ---------------------------------------------------

					'REM The _ZoomFactor must be [lessthan or equal to 1], and [greater than 0]; We cannot Zoom Out
					'If Not ((_ZoomFactor <= 1) AndAlso (_ZoomFactor > 0)) Then Throw (New GraphPicturesConversionException("The _ZoomFactor must be [lessthan or equal to 1], and [greater than 0]. It was, however: " & _ZoomFactor.ToString()))

					'Dim _ZoomedInBitmap As New Drawing.Bitmap(CInt(_Bitmap.Width * _ZoomFactor), CInt(_Bitmap.Height * _ZoomFactor))

					''Keep track of where we got up to with Copying the PixEls from the _Bitmap, onto the _ZoomedInBitmap
					'Dim _CopiedX% = 0, _CopiedY% = 0

					'For _SourceXPixel% = CInt(((1 - _ZoomFactor) / 2) * _Bitmap.Width) To CInt(_Bitmap.Width - (((1 - _ZoomFactor) / 2) * _Bitmap.Width)) - 2 Step +1

					'	For _SourceYPixel% = CInt(((1 - _ZoomFactor) / 2) * _Bitmap.Height) To CInt(_Bitmap.Height - (((1 - _ZoomFactor) / 2) * _Bitmap.Height)) - 2 Step +1

					'		Try
					'			'MsgBox("Setting X @ " & (_CopiedX - 1).ToString() & vbCrLf & "Setting Y @ " & (_CopiedY - 1).ToString())
					'			_ZoomedInBitmap.SetPixel(_CopiedX, _CopiedY, _Bitmap.GetPixel(_SourceXPixel, _SourceYPixel))
					'			'MsgBox("_SourceXPixel = " & _SourceXPixel.ToString() & "; _SourceYPixel = " & _SourceYPixel.ToString() & vbCrLf & vbCrLf & "_CopiedX=" & _CopiedX.ToString() & ";_CopiedY=" & _CopiedY.ToString() & vbCrLf & vbCrLf & ";Exception=" & _Ex.Message & vbCrLf & vbCrLf & "Width=" & _ZoomedInBitmap.Width.ToString() & ";Height=" & _ZoomedInBitmap.Height.ToString())
					'		Catch [_Ex] As Exception When True
					'			Throw New Exception("The following Exception was thrown upon attempting to Get a PixEl from the Source Bitmap, and copy it onto a PixEl on the _ZoomedInBitmap: " & _Ex.Message, _Ex)
					'		End Try

					'		'Increment the Row we're writing to on the _ZoomedInBitmap
					'		_CopiedY += 1

					'	Next

					'	'Reset the Row we're writing to on the _ZoomedInBitmap; we're about to move onto the next Column
					'	_CopiedY = 0

					'	'Increment the Colunm we're writing to on the _ZoomedInBitmap
					'	_CopiedX += 1

					'Next

					'Return _ZoomedInBitmap

				Catch _Ex As Exception When True
					Throw New GraphPicturesConversionException("The following Exception was Thrown upon attempting to Generate a Zoomed-In Copy of a System.Drawing.Bitmap: " & _Ex.Message, _Ex)
				End Try

			End Function

		End Module

		REM There exists also the HTTPObjectExtentions Module in WebParts
		Public Module WebObjectExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Writes to, and then Ends the Response Stream
			''' </summary>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Sub WriteAndEnd(ByRef _ResponseObject As Web.HttpResponse, ByVal _String$)
				Try
					_ResponseObject.Write(_String) : _ResponseObject.SafelyEnd()
				Catch _Ex As Exception When True
					_ResponseObject.SafelyEnd()
				End Try

				Return

			End Sub

			''' <summary>
			''' (MullNet CompilerExtention) Clears, Writes to, and then Ends the Response Stream
			''' </summary>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Sub ClearAndWriteAndEnd(ByRef _ResponseObject As Web.HttpResponse, ByVal _String$)

				Try
					_ResponseObject.Clear() : _ResponseObject.Write(_String) : _ResponseObject.SafelyEnd()
				Catch _Ex As Exception When True
					_ResponseObject.SafelyEnd()
				End Try

				Return

			End Sub

			''' <summary>
			''' (MullNet CompilerExtention) Ends the Response Stream, without Throwing the "Thread was being aborted" Exception
			''' </summary>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Sub [SafelyEnd](ByRef _ResponseObject As Web.HttpResponse)
				Web.HttpContext.Current.Response.Flush() 'Sends all currently buffered output to the client.
				Web.HttpContext.Current.Response.SuppressContent = True	'Gets or sets a value indicating whether to send HTTP content to the client.
				Web.HttpContext.Current.ApplicationInstance.CompleteRequest() 'Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
				Return
			End Sub

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Specified QueryString is contained in the current Request Object.
			''' </summary>
			''' <param name="_RequestObject"></param>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function QueryStringIsSpecified(ByRef _RequestObject As Web.HttpRequest, ByVal _QueryStringKey$) As [Boolean]

				'Return _RequestObject.QueryString.AllKeys.Contains(_QueryStringKey)

				'A case-insensitive version:

				Try

					If (_QueryStringKey = Nothing) OrElse (_QueryStringKey Is Nothing) Then Throw New Web.HttpException("The specified _QueryStringKey Argument ""Is Nothing"".")

					_QueryStringKey = _QueryStringKey.[ToUpper]()

					For Each _Key$ In _RequestObject.QueryString.AllKeys
						If (_Key = Nothing) OrElse (_Key Is Nothing) Then Throw New Exception("At least one Key within the QueryStrings was [Nothing]. The QueryStrings may have ended with a ""&"".")
						If _Key.ToUpper() = _QueryStringKey Then Return True
					Next

					Return False

				Catch _Ex As Exception When True
					Throw New GraphPicturesException("Whilst resolving weather or not a QueryString was Specified: " & _Ex.Message)
				End Try

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Specified PostData Item is contained in the current Request Object.
			''' </summary>
			''' <param name="_RequestObject"></param>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function PostDataItemIsSpecified(ByRef _RequestObject As Web.HttpRequest, ByVal _PostDataItemKey$) As [Boolean]

				'Return _RequestObject.Form.AllKeys.Contains(_PostDataItemKey$)

				'A case-insensitive version:

				_PostDataItemKey = _PostDataItemKey.[ToUpper]()

				For Each _Key$ In _RequestObject.Form.AllKeys
					If _Key.ToUpper() = _PostDataItemKey Then Return True
				Next

				Return False

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Specified QueryString is contained in the current Request Object, and that it has a Value.
			''' </summary>
			''' <param name="_RequestObject"></param>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function QueryStringIsSpecifiedAndHasValue(ByRef _RequestObject As Web.HttpRequest, ByVal _QueryStringKey$) As [Boolean]

				Return (_RequestObject.QueryStringIsSpecified(_QueryStringKey) AndAlso (Not String.IsNullOrEmpty(_RequestObject.QueryString(_QueryStringKey))))

			End Function

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Specified PostData Item is contained in the current Request Object, and that it has a Value.
			''' </summary>
			''' <param name="_RequestObject"></param>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Function PostDataItemIsSpecifiedAndHasValue(ByRef _RequestObject As Web.HttpRequest, ByVal _PostDataItemKey$) As [Boolean]

				Return (_RequestObject.PostDataItemIsSpecified(_PostDataItemKey) AndAlso (Not String.IsNullOrEmpty(_RequestObject.Form(_PostDataItemKey$))))

			End Function

		End Module

		Public Module ControlExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Invokes the Control if required, and runs the specified _Action
			''' </summary>
			<System.Runtime.CompilerServices.Extension()> _
			Public Sub InvokeIfRequired(ByVal _Control As Windows.Forms.Control, ByVal _Action As Action)
				If _Control.InvokeRequired Then
					_Control.Invoke(_Action)
				Else
					_Action.Invoke()
				End If
			End Sub

		End Module

	End Namespace

End Namespace


#If False Then

Template for CompilerExtentions:

''' <summary>
''' (MullNet CompilerExtention) 
''' </summary>
''' <param name="_InputString"></param>
''' <returns></returns>
''' <remarks></remarks>
<Global.System.Runtime.CompilerServices.Extension()>
Public Function FirstCharacter(ByRef _InputString$) As Char

	If _InputString.IsEmpty() Then Throw New Exception("The First Character of the String could not be found, because the String is empty.")
	Return _InputString.ToCharArray()(0)

End Function

#End If