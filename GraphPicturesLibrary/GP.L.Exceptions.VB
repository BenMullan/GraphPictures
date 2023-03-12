Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Public Module Exceptions

		Public Class GraphPicturesException : Inherits Exception

			Public Sub New(ByVal _Message$)
				MyBase.New(_Message)
			End Sub

			Public Sub New(ByVal _Message$, ByVal _InnerException As System.Exception)
				MyBase.New(_Message, _InnerException)
			End Sub

		End Class

		Public Class GraphPicturesConversionException : Inherits GraphPicturesException

			Public Sub New(ByVal _Message$)
				MyBase.New(_Message)
			End Sub

			Public Sub New(ByVal _Message$, ByVal _InnerException As System.Exception)
				MyBase.New(_Message, _InnerException)
			End Sub

		End Class

		Public Class GeneratedEmptySVGException : Inherits GraphPicturesConversionException

			Public Sub New(ByVal _Message$, ByVal _SVGFilePath$)
				MyBase.New("An Empty SVGFile was generated from the RasterGraphic: {" & _Message & "}. The Path to the Empty SVGFile is apparently: {" & _SVGFilePath & "}"c)
			End Sub

		End Class

	End Module

End Namespace