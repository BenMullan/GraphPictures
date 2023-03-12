Imports GraphPictures.Library.CompilerExtentions, System.Drawing

Namespace Library

	Public Module ColourResources

		''' <summary>
		''' Represents a Range of Colours.
		''' </summary>
		''' <remarks></remarks>
		Public Class ColourRange : Inherits [Object]

			Public Property CurrentColourGenerationMode As ColourGenerationMode
			Public Property Colours As New List(Of Color)()

			Public Enum ColourGenerationMode
				UseExpressionColour
				RandomSolidColour
				BlackToWhite
				AllBlack
				BlueAndGreen
				OrangeAndPink
				RGBToBlack
				BlueCaves
				AquaSea
				Rainbow
			End Enum

			Public Sub New(ByVal _ColourGenerationMode As ColourRange.ColourGenerationMode)

				With Me
					.CurrentColourGenerationMode = _ColourGenerationMode
					.FillColoursList()
				End With

			End Sub

			Public Sub FillColoursList()

				Me.Colours.Clear()

				Select Case Me.CurrentColourGenerationMode

					Case ColourGenerationMode.UseExpressionColour

						REM No Colours should need to be added here, as this ColourGenerationMode means use the .Colour Property of an IDesmosExpression

					Case ColourGenerationMode.RandomSolidColour

						Dim _Random = New Random(DateTime.Now.Millisecond Mod 46)

						Me.Colours.Add(Color.FromArgb(_Random.Next(255), _Random.Next(255), _Random.Next(255)))

					Case ColourGenerationMode.BlackToWhite

						For _ShadeOfGray% = 0 To 255 Step +1
							Me.Colours.Add(Color.FromArgb(_ShadeOfGray, _ShadeOfGray, _ShadeOfGray))
						Next

					Case ColourGenerationMode.AllBlack

						Me.Colours.Add(Color.Black)

					Case ColourGenerationMode.AquaSea
						For _Red% = 0 To 255 Step +1
							For _Green = 0 To 255 Step +1
								For _Blue% = 0 To 255 Step +1
									Try
										Me.Colours.Add(Color.FromArgb(_Red, 255 - _Green, 255 - _Blue))
									Catch _Ex As Exception
										MsgBox("Skipping a Colour being added to the List. Mode: [Aqua Sea].")
									End Try
								Next
							Next
						Next

					Case ColourGenerationMode.BlueAndGreen
						For _Red% = 0 To 255 Step +1
							For _Green = 0 To 255 Step +1
								For _Blue% = 0 To 255 Step +1
									Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
								Next
							Next
						Next

					Case ColourGenerationMode.BlueCaves
						For _Red% = 0 To 255 Step +1
							Me.Colours.Add(Color.FromArgb(_Red, 0, 0))
							For _Green = 0 To 255 Step +1
								Me.Colours.Add(Color.FromArgb(0, _Green, 0))
								For _Blue% = 0 To 255 Step +1
									Me.Colours.Add(Color.FromArgb(0, 0, _Blue))
								Next
							Next
						Next

					Case ColourGenerationMode.OrangeAndPink
						For _Red% = 255 To 0 Step -1
							For _Green = 255 To 0 Step -1
								For _Blue% = 255 To 0 Step -1
									Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
								Next
							Next
						Next

					Case ColourGenerationMode.RGBToBlack
						For _Red% = 255 To 0 Step -1
							Me.Colours.Add(Color.FromArgb(_Red, 0, 0))
						Next
						For _Green = 255 To 0 Step -1
							Me.Colours.Add(Color.FromArgb(0, _Green, 0))
						Next
						For _Blue% = 255 To 0 Step -1
							Me.Colours.Add(Color.FromArgb(0, 0, _Blue))
						Next

					Case ColourGenerationMode.Rainbow
						Dim _Red, _Green, _Blue As Global.System.Int32 : _Red = 0 : _Green = 0 : _Blue = 0

						_Red = 255

						For _Green = 0 To 255 Step +1
							Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
						Next

						_Green = 255

						For _Red = 255 To 0 Step -1
							Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
						Next

						_Red = 0

						For _Blue = 0 To 255 Step +1
							Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
						Next

						_Blue = 255

						For _Green = 255 To 0 Step -1
							Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
						Next

						_Green = 0

						For _Red = 0 To 255 Step +1
							Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
						Next

						_Red = 255

						For _Blue = 255 To 0 Step -1
							Me.Colours.Add(Color.FromArgb(_Red, _Green, _Blue))
						Next

					Case Else : Throw New Exception("The following ColourGenerationMode was Unaccounted for in the FillColoursList() Sub: " & Me.CurrentColourGenerationMode.ToString())
				End Select
			End Sub

			''' <summary>
			''' Gets the Colour x% of the way through the Range
			''' </summary>
			''' <param name="_ProportionOfRangeLength">E.g. 0.52 Would be 52% of the way through the Colours Array of the current ColourRange Object</param>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Default Public ReadOnly Property ColourAtPercentageThrough(ByVal _ProportionOfRangeLength@) As Color
				Get
					Return Me.Colours.Item(CInt(_ProportionOfRangeLength * (Me.Colours.Count - 1)))
				End Get
			End Property

		End Class

	End Module

End Namespace