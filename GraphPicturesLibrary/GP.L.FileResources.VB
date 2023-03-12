Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Public Module FileResources

		''' <summary>
		''' Removes the first N bytes from the start of the File's :Data$ Stream
		''' </summary>
		''' <param name="_FilePath"></param>
		''' <param name="_NumberOfBytesToRemove"></param>
		''' <remarks></remarks>
		Public Sub RemoveFirstNBytesFromStartOfFile(ByVal _FilePath$, ByVal _NumberOfBytesToRemove As UInt16)

			Try

				If Not IO.File.Exists(_FilePath) Then Throw New IO.FileNotFoundException("The File was not found on disk...", _FilePath)

				Dim _UntouchedFileBytes As Byte() = IO.File.ReadAllBytes(_FilePath)
				Dim _NewFileBytes As Byte() = New Byte(_UntouchedFileBytes.Length - _NumberOfBytesToRemove) {}

				Dim _NewBytes_Counter As Long = 0L

				For _ByteIndex& = _NumberOfBytesToRemove To _UntouchedFileBytes.LongLength - 1 Step +1
					_NewFileBytes(CInt(_NewBytes_Counter)) = _UntouchedFileBytes(CInt(_ByteIndex)) : _NewBytes_Counter += 1
				Next

				IO.File.WriteAllBytes(_FilePath, _NewFileBytes)

			Catch _Ex As Exception When True
				Throw New Exception("The following Exception was Thrown upon attempting to Remove the first " & _NumberOfBytesToRemove & " Bytes from the File """ & _FilePath & """: " & _Ex.Message, _Ex)
			End Try
		End Sub

		''' <summary>
		''' Returns a FileName which does not yet exist in the folder. An attempt is made to use the Seed as it is, but that FileName is already taken, then a random String will be prepended.
		''' </summary>
		Public Function FindAvaliableFileNameInFolder(ByVal _FolderPath$, Optional ByVal _FileNameSeed$ = "{Use Random String}") As String

			Try

				If Not IO.Directory.Exists(_FolderPath) Then Throw New ArgumentException("The specified _FolderPath does not exists en-disk", "_FolderPath")
				If Not _FolderPath.EndsWith("\") Then Throw New ArgumentException("The Specified _FolderPath did not end in a ""\""")

				If _FileNameSeed.Contains("\"c) Then Throw New ArgumentException("The _FileNameSeed contained a \. It may have been the full path to a file, instead of just the File's name.")

				If _FileNameSeed = "{Use Random String}" Then _FileNameSeed = GetRandomString(&O5US)
				Dim _FileNameToUse$ = _FileNameSeed

				'If that Seed is already non-existant in the _FolderPath, then this loop won't run.
				Do While IO.File.Exists(_FolderPath & _FileNameToUse)
					'If we're here, then the _FileNameToUse already exists in the Folder, so change it, and the loop will check it again...
					_FileNameToUse = GetRandomString(&H5US) & "_"c & _FileNameSeed
				Loop

				Return _FileNameToUse

			Catch _Ex As Exception When True
				Throw (New GraphPicturesException("The following Exception was thrown upon attempting to Get an Avaliable FileName in a Folder: " & _Ex.Message & ". The specified _FolderPath was: " & _FolderPath & ". The specified _FileNameSeed was: " & _FileNameSeed))
			End Try

		End Function

		Public Sub CheckFilePathsExist(ByVal ParamArray _FilePaths$())
			For Each _FilePath$ In _FilePaths
				If Not IO.File.Exists(_FilePath) Then Throw New IO.FileNotFoundException("The following File was not found: " & _FilePath)
			Next
		End Sub

		Public Sub CheckFolderPathsExistAndAreValid(ByVal ParamArray _FolderPaths$())
			For Each _FolderPath$ In _FolderPaths
				If Not IO.Directory.Exists(_FolderPath) Then Throw New IO.DirectoryNotFoundException("The following Directory was not found: " & _FolderPath)
				If Not _FolderPath.EndsWith("\") Then Throw New ArgumentException("A specified FolderPath did not end in a BackSlash (\). It was: " & _FolderPath)
			Next
		End Sub

	End Module

End Namespace