#Const VerboseDebugging = False
Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Public Module Resources

		Public Const _
		 GraphViewport_MinX% = -50,
		 GraphViewport_MaxX% = 1100,
		 GraphViewport_MinY% = -100,
		 GraphViewport_MaxY% = 700

		Public Const BezierScaling_TargetDesmosGraphWidth As UInt32 = 1000UI 'Passed into GraphPictureObjectExtentions.ScaledTo()
		Public Const VideoFrame_FileName_RegExp$ = "^Frame\d{5}\.BMP$"

		Public Sub MsgDebug(ByVal _Message$)
#If VerboseDebugging Then
			Global.System.Windows.Forms.MessageBox.Show(_Message, "GraphPictures Debug Message", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information, Windows.Forms.MessageBoxDefaultButton.Button1)
#End If
		End Sub

		''' <summary>
		''' Used for the _Options Argument to ToDesmosState() CompilerExtention
		''' </summary>
		Public Class DesmosStateCreationOptions

			Public ShowGrid As [Boolean] = True
			Public ColourAllocationMode As ColourRange.ColourGenerationMode = ColourRange.ColourGenerationMode.AllBlack
			Public GraphZoomSize@ = 1.05D
			Private LinesOpacity_@ = 1D	'Range of Valid Values: 0 To 1
			Public LinesWidth@ = 3D
			Public LinesStyle As DesmosLineStyle = DesmosLineStyle.Normal

			'If a FillOpacity is specified at all to Desmos, then Fill is automatically turned on!
			Public Fill As [Boolean] = False
			Private FillOpacity_@ = 0.41D	'Range: 0 To 1

			REM The Opacities need to be between 1 and 0, so PropertyMethods are used:
			Public Property LinesOpacity@
				Get
					Return Me.LinesOpacity_
				End Get
				Set(ByVal _NewValue@)
					If Not ((_NewValue >= 0) AndAlso (_NewValue <= 1)) Then Throw (New ArgumentOutOfRangeException("_NewValue", "An attempt was made to Assign [a Value not inclusivly between 0 and 1] to the LinesOpacity. The Value was: " & _NewValue))
					Me.LinesOpacity_ = _NewValue
				End Set
			End Property
			Public Property FillOpacity@
				Get
					Return Me.FillOpacity_
				End Get
				Set(ByVal _NewValue@)
					If Not ((_NewValue >= 0) AndAlso (_NewValue <= 1)) Then Throw (New ArgumentOutOfRangeException("_NewValue", "An attempt was made to Assign [a Value not inclusivly between 0 and 1] to the FillOpacity. The Value was: " & _NewValue))
					Me.FillOpacity_ = _NewValue
				End Set
			End Property

			Public Enum DesmosLineStyle
				Normal
				DOTTED
				DASHED
			End Enum

			''' <summary>
			''' Scans the QueryStrings in the _Request, and changes the propities of the current DesmosStateCreationOptions Object, based off of the QueryString Values if they are there...
			''' Returns the Current Instance in it's new State...
			''' </summary>
			Public Function ModifyByQueryStrings(ByRef _Request As System.Web.HttpRequest) As DesmosStateCreationOptions
				Try

					With Me
						If _Request.QueryStringIsSpecified("ShowGrid") Then .ShowGrid = (_Request.QueryString("ShowGrid").ToUpper() = "TRUE")
						If _Request.QueryStringIsSpecified("ColourAllocationMode") Then .ColourAllocationMode = _Request.QueryString("ColourAllocationMode").ToColourGenerationMode()
						If _Request.QueryStringIsSpecified("GraphZoomSize") Then .GraphZoomSize = _Request.QueryString("GraphZoomSize").ToDecimal()
						If _Request.QueryStringIsSpecified("LinesOpacity") Then .LinesOpacity = _Request.QueryString("LinesOpacity").ToDecimal()
						If _Request.QueryStringIsSpecified("LinesWidth") Then .LinesWidth = _Request.QueryString("LinesWidth").ToDecimal()
						If _Request.QueryStringIsSpecified("LinesStyle") Then .LinesStyle = _Request.QueryString("LinesStyle").ToDesmosLineStyle()
						If _Request.QueryStringIsSpecified("Fill") Then .Fill = (_Request.QueryString("Fill").ToUpper() = "TRUE")
						If _Request.QueryStringIsSpecified("FillOpacity") Then .FillOpacity = _Request.QueryString("FillOpacity").ToDecimal()
					End With

					'Return the current instance in it's new state.
					Return (Me)

				Catch _Ex As Exception When True
					Throw New Exception("The following Exception was Thrown upon attempting to Modify a DesmosStateCreationOptions Object with a HTTP Request: " & _Ex.Message, _Ex)
				End Try
			End Function

			''' <summary>
			''' Scans the QueryStrings in the _Request, creating a Perameter Object From them.
			''' </summary>
			Public Shared Function FromQueryStrings(ByRef _Request As System.Web.HttpRequest) As DesmosStateCreationOptions
				Try

					Dim _DesmosGraphOptions As Library.Resources.DesmosStateCreationOptions = (New Library.Resources.DesmosStateCreationOptions())

					With _DesmosGraphOptions
						If _Request.QueryStringIsSpecified("ShowGrid") Then .ShowGrid = (_Request.QueryString("ShowGrid").ToUpper() = "TRUE")
						If _Request.QueryStringIsSpecified("ColourAllocationMode") Then .ColourAllocationMode = _Request.QueryString("ColourAllocationMode").ToColourGenerationMode()
						If _Request.QueryStringIsSpecified("GraphZoomSize") Then .GraphZoomSize = _Request.QueryString("GraphZoomSize").ToDecimal()
						If _Request.QueryStringIsSpecified("LinesOpacity") Then .LinesOpacity = _Request.QueryString("LinesOpacity").ToDecimal()
						If _Request.QueryStringIsSpecified("LinesWidth") Then .LinesWidth = _Request.QueryString("LinesWidth").ToDecimal()
						If _Request.QueryStringIsSpecified("LinesStyle") Then .LinesStyle = _Request.QueryString("LinesStyle").ToDesmosLineStyle()
						If _Request.QueryStringIsSpecified("Fill") Then .Fill = (_Request.QueryString("Fill").ToUpper() = "TRUE")
						If _Request.QueryStringIsSpecified("FillOpacity") Then .FillOpacity = _Request.QueryString("FillOpacity").ToDecimal()
					End With

					Return _DesmosGraphOptions

				Catch _Ex As Exception When True
					Throw New Exception("The following Exception was Thrown upon attempting to Instanciate a DesmosStateCreationOptions Object from a HTTP Request: " & _Ex.Message, _Ex)
				End Try
			End Function

			''' <summary>
			''' Generates a String representing the current Instance of the CreationOptions
			''' </summary>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Overrides Function ToString() As String
				With Me
					Return String.Format("([ShowGrid]={0}{1}[ColourAllocationMode]={2}{1}[GraphZoomSize]={3}{1}[LinesOpacity]={4}{1}[LinesWidth]={5}{1}[LinesStyle]={6}{1}[Fill]={7}{1}[FillOpacity]={8})", .ShowGrid.ToString(), vbCrLf, .ColourAllocationMode.ToString(), .GraphZoomSize.ToString(), .LinesOpacity.ToString(), .LinesWidth.ToString(), .LinesStyle.ToString(), .Fill.ToString(), .FillOpacity.ToString())
				End With
			End Function

		End Class

		''' <summary>
		''' Generates a Random String of the Specified Length. The Characters used are specified in the Array inside the Function
		''' </summary>
		''' <param name="_WantedLength"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetRandomString(Optional ByVal _WantedLength As UInt16 = 5US) As String

			Dim _StringToReturn$ = String.Empty
			Dim _PossibleCharacters As Char() = "0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray()
			Dim _RandomNumber As New Random()

			For _CurrentCharacter% = 1 To _WantedLength Step +1
				_StringToReturn &= _PossibleCharacters(_RandomNumber.Next(0, _PossibleCharacters.Length - 1))
			Next

			Return _StringToReturn

		End Function

	End Module

End Namespace