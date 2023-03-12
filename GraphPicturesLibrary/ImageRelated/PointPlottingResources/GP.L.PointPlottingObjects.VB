Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Public Module PointPlottingObjects

		Public Structure PlottablePoint : Implements IDesmosExpression

			Public X%, Y%
			Private ExpressionColour_ As Drawing.Color

			Public Sub New(ByVal _X%, ByVal _Y%, ByVal _Colour As Drawing.Color)
				With Me
					.X = _X
					.Y = _Y
					.Colour = _Colour
				End With
			End Sub

			Public ReadOnly Property GraphBoundries As BezierObjects.DesmosGraphSize Implements BezierObjects.IDesmosExpression.GraphBoundries
				Get
					Return New DesmosGraphSize() With {.MostNegativeX = Me.X, .MostNegativeY = Me.Y, .MostPositiveX = Me.X, .MostPositiveY = Me.Y}
				End Get
			End Property

			Public Function ToLatexExpression() As String Implements BezierObjects.IDesmosExpression.ToLatexExpression
				Return String.Format("({0},{1})", Me.X.ToString, Me.Y.ToString()).MathsExpressionToLatex()
			End Function

			Public Property Colour As System.Drawing.Color Implements IDesmosExpression.Colour
				Get
					Return Me.ExpressionColour_
				End Get
				Set(ByVal _NewValue As System.Drawing.Color)
					Me.ExpressionColour_ = _NewValue
				End Set
			End Property

		End Structure

		''' <summary>
		''' Represents a Graphic which can be plotted onto a DesmosGraph, using a Point for each Pixel
		''' </summary>
		''' <remarks></remarks>
		Public Class PointPlottedRasterGraphic : Inherits [Object]

			Public Const ResizeRasterGraphicsToMaxBounds As Boolean = True
			Public Const MaxRasterGraphicDimension% = 120

			Public RasterGraphic As Global.System.Drawing.Bitmap

			Public Sub New(ByVal _RasterGraphicFilePath$)

				If Not IO.File.Exists(_RasterGraphicFilePath) Then Throw New IO.FileNotFoundException("An attempt was made to Instanciate a PointPlottedRasterGraphic, however the Specified _RasterGraphicFilePath does not exist. That path was: " & _RasterGraphicFilePath, _RasterGraphicFilePath)
				Me.RasterGraphic = (New Drawing.Bitmap(_RasterGraphicFilePath))

				REM This makes the DesmosGraph's Rendering of the Image not flipped horozontally
				Me.RasterGraphic.RotateFlip(Drawing.RotateFlipType.RotateNoneFlipY)

				REM Make sure that the Input RasterImage isn't to large
				If ResizeRasterGraphicsToMaxBounds Then Me.RasterGraphic.MakeNoLargerThan(MaxRasterGraphicDimension)

			End Sub

			Public Function GetPoints() As IDesmosExpression()

				Dim _PointsToReturn As New List(Of IDesmosExpression)()

				For _XPixelNumber% = 0 To (Me.RasterGraphic.Width - 1) Step +1
					For _YPixelNumber% = (Me.RasterGraphic.Height - 1) To 0 Step -1

						_PointsToReturn.Add(New PlottablePoint(_XPixelNumber, _YPixelNumber, Me.RasterGraphic.GetPixel(_XPixelNumber, _YPixelNumber)))

					Next
				Next

				Return _PointsToReturn.ToArray()

			End Function

		End Class

	End Module

End Namespace