Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Public Module Logging

		Public Sub Log(ByVal _LogText$)
#If DEBUG Then
			MsgBox("LOG:" & vbCrLf & _LogText, MsgBoxStyle.MsgBoxHelp, Nothing)
#End If
		End Sub

	End Module

End Namespace