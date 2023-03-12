Imports GraphPictures.Library.CompilerExtentions

Public Class UploadRasterImage : Inherits System.Web.UI.Page

	Protected Sub Page_Load() Handles Me.Load
		StatusLabel.Text = "Status: File not yet uploaded"
	End Sub

	Protected Sub PerformUploadButton_Click() Handles PerformUploadButton.Click

		Try

			If RasterImageUploader.HasFile Then

				REM Perform some Initial Validation Checks
				'If (RasterImageUploader.FileName.Split("."c).Length = 0) OrElse Not (RasterImageUploader.FileName.MatchesRegEx("^[\w \-_]+\.\w+$")) Then Throw (New Exception("The [Uploaded File]'s Name was not formatted correctly. The name was: " & RasterImageUploader.FileName))
				If RasterImageUploader.PostedFile.ContentLength > 3072000 Then Throw (New Exception("The File Size is too large. The maximun allowed size is 1 Million Bytes (1MB)."))

				Dim _AllowedChars As Char() = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-.".ToCharArray()
				Dim _SaveAsFileName$ = String.Empty
				Dim _IdealFileName$ = (New IO.FileInfo(RasterImageUploader.FileName)).Name.Split("."c)(0).Clense(_AllowedChars).MakeNoLongerThan(25) & (New IO.FileInfo(RasterImageUploader.FileName)).Extension.Clense(_AllowedChars).MakeNoLongerThan(5)

				REM We want to keep the existing FileExtention, and Ideally Name, of the File
				If IO.File.Exists(GraphPictures.WebParts.Resources.WWWUploadedRasterImagesFolderPath & _IdealFileName) Then	'Find a Random FileName
					_SaveAsFileName = GraphPictures.Library.FileResources.FindAvaliableFileNameInFolder(GraphPictures.WebParts.Resources.WWWUploadedRasterImagesFolderPath, _IdealFileName)
					'Do : _SaveAsFileName$ = (Library.Resources.GetRandomString(10) & "."c & (New IO.FileInfo(RasterImageUploader.FileName)).Extension).ToUpper()
					'Loop Until (Not IO.File.Exists(GraphPictures.WebParts.Resources.WWWUploadedRasterImagesFolderPath & _SaveAsFileName$))
				Else
					_SaveAsFileName = _IdealFileName
				End If

				RasterImageUploader.SaveAs(Global.[GraphPictures].WebParts.Resources.WWWUploadedRasterImagesFolderPath & _SaveAsFileName)

				StatusLabel.Text = "Status: File Uploaded and saved successfully on the Server as " & _SaveAsFileName

			Else
				Throw New Exception("No File was chosen to be uploaded.")
			End If

		Catch _Ex As Exception When True
			StatusLabel.Text = "Status: The following Exception was thrown upon attempting to upload and save the File: " & _Ex.Message
		End Try

	End Sub

End Class