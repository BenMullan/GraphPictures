for (var i = 0; i != _JSON.Content.length; ++i) {
    console.log("Item: " + _JSON.Content[i].Item);
}











'Public Const DefaultTargetSVGFileSizeKB As UInt16 = 40US






' ''' <summary>
' ''' Generates an SVG from a RasterGraphic, cropping the input RasterGraphic if it produces too many beziers in the output SVG
' ''' </summary>
' ''' <param name="_IdealMaximumSVGFileSizeKB">SVGs above this size will actually make it through if MaxAttempts to crop the image is reached, before an SVG below this size is generated...</param>
' ''' <returns>The Number of times the Input Image had to be cropped, before an SVG of acceptable size was produced.</returns>
'Public Function GenerateSVG(ByVal _InputRasterGraphic As IO.FileInfo, ByVal _FinalSVGFile_OutputPath$, ByVal _BMPConversionFolder_Path$, ByVal _PotraceEXE_FilePath$, Optional ByVal _IdealMaximumSVGFileSizeKB As UInt16 = ImagingResources.DefaultTargetSVGFileSizeKB) As Byte

'	REM Generate an SVG
'	REM See if it's FileSize is above the _MaximumSVGFileSizeKB. If it is:
'	REM		Generate a smaller version of the RasterGraphic and try again, until _MaxRasterGraphicPixelCountReductionAttempts is reached
'	REM If it isn't:
'	REM		Check to see that the Generated SVG contains some Beziers. If it does:
'	REM			Log() and Return
'	REM		If it dosen't:
'	REM			Throw a GeneratedEmptySVGException

'	Const _Max_RasterGraphicPixelCountReductionAttempts As [Byte] = 8US		'I.e. Proportion of Origional Image used for each iteration becomes: 1, .8, .64, .512, .4096, .32768, ...
'	Const _RasterGraphic_PixelCountReductionProportion As [Decimal] = 0.8@	'I.e. Keep the centeral 80% of the image if it produced too many Beziers...
'	Const _PotraceEXE_AllowedLifetimeMS As UInt32 = 12000UI					'Wait for a Maximun of 12 Seconds before Killing the process if it's still running, and returning

'	Dim _GeneratedSVGFileSizes As New List(Of UInt64)()						'Keep track of the SVGFileSizes for Debugging
'	Dim _CurrentRasterGraphic_FilePath$ = ""								'This points to the current RasterGraphic being used. It changes every time [a RasterGraphic produces too many Beziers and so 20% (Actually 36%) of it's pixels are removed]. When this occours, this Variable is updated to point to the new, samller, RasterGraphic, and then we try another 7 times to vary the TSize of that new RG, in order to produce an SVGFile of the sought size.
'	Dim _LastSVGFileGenerated As IO.FileInfo = Nothing						'Each time an SVGFile is generated, this is set to represent it. This allows it's FileSize to be resolved.
'	Dim _TempFiles_ToDeleteAtEnd As New List(Of IO.FileInfo)()				'Any tempoarary Files which need to be deleted once we have the SVG are added to this list, and are deleted at the end.

'	Try

'		'Check if the Specified Files and Folders actually exist...
'		FileResources.CheckFilePathsExist(_InputRasterGraphic.FullName, _PotraceEXE_FilePath)
'		FileResources.CheckFolderPathsExistAndAreValid(_BMPConversionFolder_Path)

'		REM Make sure that we are dealing with a .BMP file
'		If Not (_InputRasterGraphic.Extension.ToUpper() = ".BMP") Then
'			Try
'				'Get [an avaliable FileName in the Bitmap Conversion Folder], under which we can save a .BMP copy of the _InputRasterGraphic
'				Dim _NewRasterGraphic_FilePath$ = _BMPConversionFolder_Path & FileResources.FindAvaliableFileNameInFolder(_BMPConversionFolder_Path, "GPConverted_" & _InputRasterGraphic.Name & ".BMP")

'				With (New Drawing.Bitmap(_InputRasterGraphic.FullName))
'					'MsgDebug("About to save to: " & _NewRasterGraphic_FilePath)
'					.Save(_NewRasterGraphic_FilePath, Drawing.Imaging.ImageFormat.Bmp)
'					.Dispose()
'				End With

'				'Set the Path of the ImageFile potrace will use, to this new BMP
'				_CurrentRasterGraphic_FilePath = _NewRasterGraphic_FilePath

'				'We want to delete this BMP File at the end:
'				_TempFiles_ToDeleteAtEnd.Add(New IO.FileInfo(_CurrentRasterGraphic_FilePath))

'			Catch _BMPConvExcp As Exception
'				Throw New IO.IOException("Upon attempting to save the Input RasterGraphic as a BMP: " & _BMPConvExcp.Message)
'			End Try
'		Else
'			'If we're here, then the _InputRasterGraphic was already a .BMP
'			_CurrentRasterGraphic_FilePath = _InputRasterGraphic.FullName
'		End If

'		REM The _CurrentRasterGraphic_FilePath Now points to a .BMP File, ready for Potrace.

'		'This is used in the ForLoop, but is declared at this scope, so that it can be read for Returning the Number of attempts it took to produce the SVG
'		Dim _Current_RasterGraphicPixelCountReductionAttempt As [Byte] = 1US

'		For _Current_RasterGraphicPixelCountReductionAttempt = 1US To _Max_RasterGraphicPixelCountReductionAttempts Step +1

'			Dim _PotraceCLAs$ = "--output """ & _FinalSVGFile_OutputPath & """ --svg --progress --turdsize 5 """ & _CurrentRasterGraphic_FilePath & """"
'			'MsgDebug("Running Potrace with these CLAs:" & vbCrLf & _PotraceCLAs) : My.Computer.Clipboard.SetText(_PotraceCLAs)

'			REM Generate an SVGFile using the _CurrentTSize and _CurrentRasterGraphic_FilePath
'			ImagingResources.RunPotraceEXE(_PotraceEXE_FilePath, _PotraceCLAs, _PotraceEXE_AllowedLifetimeMS)

'			REM Now check that the File Generated is Below the maximun allowed Size
'			_LastSVGFileGenerated = (New IO.FileInfo(_FinalSVGFile_OutputPath))
'			_GeneratedSVGFileSizes.Add(_LastSVGFileGenerated.SizeInIntegralKB())

'			If _LastSVGFileGenerated.SizeInIntegralKB() > _IdealMaximumSVGFileSizeKB Then
'				REM We need to now generate a smaller version of the RasterGraphic and try again.

'				Dim _LastRasterGraphicUsed As New IO.FileInfo(_CurrentRasterGraphic_FilePath)

'				Try

'					Dim _ZoomedInRasterGraphic As New Drawing.Bitmap(_LastRasterGraphicUsed.FullName)

'					_CurrentRasterGraphic_FilePath = _BMPConversionFolder_Path & Library.FileResources.FindAvaliableFileNameInFolder(_BMPConversionFolder_Path, "Crop" & _Current_RasterGraphicPixelCountReductionAttempt & "_" & If(_Current_RasterGraphicPixelCountReductionAttempt = 1, _LastRasterGraphicUsed.Name, _LastRasterGraphicUsed.Name.Substring(6)))
'					_ZoomedInRasterGraphic.GetZoomedInCopy(_RasterGraphic_PixelCountReductionProportion).Save(_CurrentRasterGraphic_FilePath, Drawing.Imaging.ImageFormat.Bmp)

'					_ZoomedInRasterGraphic.Dispose()

'				Catch _Ex As Exception
'					Throw (New GraphPicturesConversionException("Upon attempting to Save a smaller version of the RasterGraphic: {" & _Ex.Message & "}. _Current_RasterGraphicPixelCountReductionAttempt = " & _Current_RasterGraphicPixelCountReductionAttempt.ToString(), _Ex))
'				End Try

'				REM Now that the New smaller RasterGraphic is being used, the old one can be deleted:
'				REM ...UNLESS: We converted the origional input to a .BMP, in which case we would be using it only on the first iteration,
'				REM ...And we have already added that .BMP to the list of files to delete at the end.
'				If Not (_Current_RasterGraphicPixelCountReductionAttempt = 1 AndAlso (Not _InputRasterGraphic.Name.EndsWith(".BMP", StringComparison.CurrentCultureIgnoreCase))) Then _TempFiles_ToDeleteAtEnd.Add(_LastRasterGraphicUsed)

'			Else REM The SVGFile is of an acceptable size, but is it Empty...?
'				If SVGFileContainsNoBeziers(_LastSVGFileGenerated.FullName) Then
'					Throw New GeneratedEmptySVGException("The RasterGraphic being used was: " & _CurrentRasterGraphic_FilePath & ".The _Current_RasterGraphicPixelCountReductionAttempt was: " & _Current_RasterGraphicPixelCountReductionAttempt.ToString(), _LastSVGFileGenerated.FullName)
'				Else REM The Generated SVG isn't empty, and is of an acceptable size...
'					Exit For
'				End If
'			End If

'		Next

'		REM If we've just left that loop because we used a cropped image, then the last Cropped BMP would not have been added to the list of them to delete:
'		If Not (_Current_RasterGraphicPixelCountReductionAttempt = 1) Then _TempFiles_ToDeleteAtEnd.Add(New IO.FileInfo(_CurrentRasterGraphic_FilePath))

'		MsgDebug("About to Return and perform Finally statment. Generated SVG's Sizes: " & String.Join(", ", _GeneratedSVGFileSizes.ToArray()) & vbCrLf & "Files to Delete: " & (_TempFiles_ToDeleteAtEnd.Aggregate(Of String, String)(String.Empty, Function(_TotalString$, _File As IO.FileInfo) _TotalString & vbCrLf & _File.FullName, Function(_String$) _String)))

'		REM If we're here, it must either be because [we hit the [Exit For], because the SVGFile produced was below the limit, AND the SVGFile wasn't empty], OR because [we reduced the size of the File the max. allowed number of times, and the SVGFile was still too large...]
'		REM Therefore it's okay to return the Number of times the InputImage had to be cropped in order to produce an acceptably-sized SVG
'		Return CByte(_Current_RasterGraphicPixelCountReductionAttempt - 1)

'	Catch _SVGEmptyExcp As GeneratedEmptySVGException : Throw
'	Catch _Ex As Exception
'		Throw (New GraphPicturesConversionException("Upon attempting to generate an SVG from a RasterGraphic: " & _Ex.Message, _Ex))
'	Finally	REM Delete any Tempoary Files Accumulated during SVGGeneration attempts...
'		Try
'			Dim _FilesWhichCouldNotBeDeleted As New List(Of String)()

'			For Each _File As IO.FileInfo In _TempFiles_ToDeleteAtEnd
'				Try
'					_File.Delete()
'				Catch _Ex As Exception
'					_FilesWhichCouldNotBeDeleted.Add(vbCrLf & "{ File = """ & _File.FullName & """; Reason = """ & _Ex.Message & """ }" & vbCrLf)
'				End Try
'			Next

'			If Not _FilesWhichCouldNotBeDeleted.Count = 0 Then Throw New IO.IOException("The Following Files could not be deleted: " & String.Join(", ", _FilesWhichCouldNotBeDeleted.ToArray()))

'		Catch _FilesDelEx As Exception
'			Throw New IO.IOException("One or more of the Tempoary Files could not be deleted during the generation of an SVG. The Exception was: " & _FilesDelEx.Message, _FilesDelEx)
'		End Try
'	End Try

'End Function


'IMPLEMENTED INTO NEW OBJECT
Public Sub RunPotraceEXE(ByVal _PotraceEXEPath$, ByVal _CLAs$, ByVal _AllowedLifetimeMS As UInt32)

	Try

		With (New Diagnostics.Process())
			.StartInfo = (New Diagnostics.ProcessStartInfo(_PotraceEXEPath$, _CLAs$))
			.Start()

			If Not .WaitForExit(CInt(_AllowedLifetimeMS)) Then	'Wait for a Maximun of 12 Seconds before Killing the process if it's still running, and returning
				If Not .HasExited Then .Kill()
				.Kill()
			End If

			MsgDebug("Potrace Exit Code: " & .ExitCode.ToString())
			If Not (.ExitCode = 0) Then Throw (New GraphPicturesConversionException("The Potrace Process returned a non-zero ExitCode. The ExitCode was: " & .ExitCode.ToString() & ". The Command-Line Arguments were: " & _CLAs))

		End With

		Return

	Catch [_Ex] As [Exception] When True
		Throw (New GraphPicturesConversionException("The Potrace Process could not be killed, or waited upon to exit, or it returned an Error. The Exception Thrown was: " & _Ex.Message, _Ex))
	End Try

End Sub

'IMPLEMENTED INTO NEW OBJECT
Public Function SVGFileContainsNoBeziers(ByVal _SVGFile_Path$) As Boolean
	Try

		'Check if the Specified File actually exists...
		If Not System.IO.File.Exists(_SVGFile_Path$) Then Throw (New System.Exception("The SVG File does not exist. The path was: " & _SVGFile_Path$))

		Dim _SVGFileText$ = My.Computer.FileSystem.ReadAllText(_SVGFile_Path$)

		REM If the File happerens to be empty, then throw an Exception. Sometimes Potrace will create empty SVGFiles if it's input image was invalid in some way.
		If String.IsNullOrEmpty(_SVGFileText) Then Throw New Exception("The specified SVGFile contained no text at all: " & _SVGFile_Path)


		'The number of <path>'s in the SVG == {that split} - 1

		'No <path>'s == No Beziers...
		Return (_SVGFileText.Split({"<path d="""}, StringSplitOptions.None).Length = 1)



	Catch _Ex As Exception REM When True
		Throw New Exception("Upon attempting to Resolve the Number of Beziers in an SVGFile: {" & _Ex.Message & "}", _Ex)
	End Try

End Function























'#Region "New RasterGraphic to SVG Implementation"

''Public Structure SVGGenerationOptions

''	Private PotraceEXE_FilePath_$
''	Public Property PotraceEXE_FilePath$
''		Get
''			If String.IsNullOrEmpty(Me.PotraceEXE_FilePath_) Then Throw New NullReferenceException("An attempt was made to get the PotraceEXE_FilePath of an SVGGenerationOptions Object, however the field was a NullOrEmpty String.")
''			Return Me.PotraceEXE_FilePath_
''		End Get
''		Set(ByVal _NewValue$)
''			If Not [IO].[File].[Exists](_NewValue) Then Throw New ArgumentException("An attempt was made to set the PotraceEXE_FilePath$ Property of an SVGGenerationOptions Object, but the specified _NewValue did not exist en-disk. It was: " & _NewValue, "_NewValue$")
''			Me.PotraceEXE_FilePath_ = _NewValue
''		End Set
''	End Property

''	Private BitmapConversionFolder_Path_$
''	Public Property BitmapConversionFolder_Path$
''		Get
''			If String.IsNullOrEmpty(Me.BitmapConversionFolder_Path_) Then Throw New NullReferenceException("An attempt was made to get the BitmapConversionFolder_Path of an SVGGenerationOptions Object, however the field was a NullOrEmpty String.")
''			Return Me.BitmapConversionFolder_Path_
''		End Get
''		Set(ByVal _NewValue$)
''			If Not [IO].[File].[Exists](_NewValue) Then Throw New ArgumentException("An attempt was made to set the BitmapConversionFolder_Path$ Property of an SVGGenerationOptions Object, but the specified _NewValue did not exist en-disk. It was: " & _NewValue, "_NewValue$")
''			Me.BitmapConversionFolder_Path_ = _NewValue
''		End Set
''	End Property

''	Private TargetSVGFile_SizeInKB_ As UInt16
''	Public Property TargetSVGFile_SizeInKB As UInt16
''		Get
''			If TargetSVGFile_SizeInKB = 0 Then Throw New NullReferenceException("An attempt was made to get the TargetSVGFile_SizeInKB of an SVGGenerationOptions Object, however the field was 0.")
''			Return Me.BitmapConversionFolder_Path_
''		End Get
''		Set(ByVal _NewValue$)
''			If Not [IO].[File].[Exists](_NewValue) Then Throw New ArgumentException("An attempt was made to set the BitmapConversionFolder_Path$ Property of an SVGGenerationOptions Object, but the specified _NewValue did not exist en-disk. It was: " & _NewValue, "_NewValue$")
''			Me.BitmapConversionFolder_Path_ = _NewValue
''		End Set
''	End Property

''End Structure

' ''' <summary>
' ''' This Function will attempt to name the SVGFile {RasterGraphicName.Ext}.SVG. If this name is already taken, then 5 random Chars are added to the front
' ''' </summary>
' ''' <returns>The FileName under which the SVGFile actually ended up being saved</returns>
'Public Function DontUse_GenerateSVGInFolder(ByVal _RasterGraphic As IO.FileInfo, ByVal _SVGOutputFolder_Path$, ByVal _BMPConversionFolder_Path$, ByVal _PotraceEXE_FilePath$, Optional ByVal _TargetSVGFileSizeKB As UInt16 = ImagingResources.DefaultTargetSVGFileSizeKB) As String

'	'Possible Problem: Although very unlikely, two instances of this Function could be running at the same time, and generate the same "random" SVGOutputName

'	Try

'		If Not _RasterGraphic.Exists Then Throw New IO.FileNotFoundException("The Specified _RasterGraphic File does not exist en-disk", _RasterGraphic.FullName)
'		FileResources.CheckFolderPathsExistAndAreValid(_SVGOutputFolder_Path)

'		REM Find an Avaliable FileName in the Folder
'		Dim _SVGFile_OutputPath$ = FileResources.FindAvaliableFileNameInFolder(_SVGOutputFolder_Path, _FileNameSeed:=_RasterGraphic.Name)

'		REM Generate the SVG with that name
'		GenerateSVG(_RasterGraphic, _SVGFile_OutputPath, _BMPConversionFolder_Path, _PotraceEXE_FilePath, _TargetSVGFileSizeKB)

'		REM Return that name, now that the SVGFile has been saved under it
'		Return _SVGFile_OutputPath

'	Catch _Ex As Exception When True
'		Throw New GraphPicturesConversionException("An attempt was made to Generate an SVG in a Specified Folder (" & _SVGOutputFolder_Path & "), however the following Exception was Thrown: " & _Ex.Message, _Ex)
'	End Try

'End Function

' ''' <summary>
' ''' Generates an SVG from the specified _RasterGraphic_FilePath, saving it under the name of the _FinalSVG_OutputPath, even if that file already exists.
' ''' </summary>
' ''' <param name="_InputRasterGraphic"></param>
' ''' <param name="_FinalSVG_OutputPath"></param>
' ''' <param name="_BMPConversionFolder_Path">If the _RasterGraphic_FilePath isn't a .BMP, then it will be converted to one, in this Folder</param>
' ''' <param name="_PotraceEXE_FilePath"></param>
' ''' <param name="_TargetSVGFileSizeKB"></param>
' ''' <remarks></remarks>
'Public Sub DontUse_GenerateSVG(ByVal _InputRasterGraphic As IO.FileInfo, ByVal _FinalSVG_OutputPath$, ByVal _BMPConversionFolder_Path$, ByVal _PotraceEXE_FilePath$, Optional ByVal _TargetSVGFileSizeKB As UInt16 = ImagingResources.DefaultTargetSVGFileSizeKB)

'	REM	Over a Maximun of 7 attempts:
'	REM		Over a Maximun of 8 Attempts:
'	REM			Generate SVG with TSize, called {_FinalSVGOutputPath}
'	REM		Look at SVGFile_Size and either Adjust and repeat, or Return because the Size is within the acceptable range
'	REM	Try again with 80% of the origional picture

'	REM Whenever a Return is about to be made, we need to check weather there are no Beziers in the SVGFile (via XML Parsing, not GetBeziersFromPotraceSVG()).
'	REM If there aren't any Beziers in the File, then Throw a [New GeneratedEmptySVGException()]

'	'Keep track of the SVGFileSizes and TSizes for Debugging
'	Dim _GeneratedSVGFileSizes As New List(Of UInt64)()
'	Dim _UsedTSizes As New List(Of UInt64)()

'	Try

'		'Check if the Specified Files and Folders actually exist...
'		FileResources.CheckFilePathsExist(_InputRasterGraphic.FullName, _PotraceEXE_FilePath)
'		FileResources.CheckFolderPathsExistAndAreValid(_BMPConversionFolder_Path)

'		Const _MaxRasterGraphic_PixelCountReductionAttempts As [Byte] = 7US	'I.e. Proportion of Origional Image used for each iteration becomes: 1, .8, .64, .512, .4096, .32768, ...
'		Const _RasterGraphic_PixelCountReductionProportion As Decimal = 0.8@ 'I.e. Keep the centeral 80% of the image if it produced too many Beziers...

'		Const _MaxTSizeModerationAttempts As [Byte] = 8US 'After 8 Attempts to adjust the TSize of the _CurrentRasterGraphic
'		Const _PotraceEXE_AllowedLifetimeMS As UInt32 = 12000UI	'Wait for a Maximun of 12 Seconds before Killing the process if it's still running, and returning
'		Const _FileSizeTolerancePercentage As [Byte] = 10US	'%. I.e. Allow the file through, as long as it is within {this}% Of the _TargetSVGFileSizeKB

'		Dim _LastSVGFileGenerated As IO.FileInfo = Nothing

'		Dim _CurrentTSize As UInt64 = 2UL
'		Dim _CurrentRasterGraphicPixelCountReductionAttempt As [Byte] = 1US
'		Dim _CurrentRasterGraphic_FilePath$ = "" 'This points to the current RasterGraphic being used. It changes every time [a RasterGraphic produces too many Beziers and so 20% (Actually 36%) of it's pixels are removed]. When this occours, this Variable is updated to point to the new, samller, RasterGraphic, and then we try another 7 times to vary the TSize of that new RG, in order to produce an SVGFile of the sought size.

'		REM Make sure that we are dealing with a .BMP file
'		If (Not _InputRasterGraphic.Name.EndsWith(".BMP", StringComparison.CurrentCultureIgnoreCase)) Then

'			'Get [an avaliable FileName in the Bitmap Conversion Folder], under which we can save a .BMP copy of the _InputRasterGraphic
'			Dim _NewRasterGraphic_FilePath$ = FileResources.FindAvaliableFileNameInFolder(_BMPConversionFolder_Path, _InputRasterGraphic.Name)

'			With (New Drawing.Bitmap(_InputRasterGraphic.FullName))
'				.Save(_NewRasterGraphic_FilePath, Drawing.Imaging.ImageFormat.Bmp)
'			End With
'			_CurrentRasterGraphic_FilePath = _NewRasterGraphic_FilePath
'		Else
'			'If we're here, then the _InputRasterGraphic was already a .BMP
'			_CurrentRasterGraphic_FilePath = _InputRasterGraphic.FullName
'		End If

'		REM The _CurrentRasterGraphic_FilePath Now points to a .BMP File, ready for Potrace.

'		Do
'			For _CurrentTSizeModerationAttempt% = 1 To _MaxTSizeModerationAttempts Step +1

'				Dim _PotraceCLAs$ = "--output """ & _FinalSVG_OutputPath & """ --svg --progress --turdsize " & _CurrentTSize.ToString() & " """ & _CurrentRasterGraphic_FilePath & """"

'				REM Generate an SVGFile using the _CurrentTSize and _CurrentRasterGraphic_FilePath
'				RunPotraceEXE(_PotraceEXE_FilePath, _PotraceCLAs, _PotraceEXE_AllowedLifetimeMS)

'				REM Now check that the File Generated is of the Desired Size

'				_LastSVGFileGenerated = (New IO.FileInfo(_FinalSVG_OutputPath))
'				_GeneratedSVGFileSizes.Add(_LastSVGFileGenerated.SizeInIntegralKB())

'				REM Calcuate the Percentage of the Target, that the FileSize is currently (i.e. 100% == [FileSize is perfect]; 50% == [FileSize is half of what it should be])
'				Dim _PercentageOfTargetFileSize As UInt64 = CULng((_LastSVGFileGenerated.SizeInIntegralKB() \ _TargetSVGFileSizeKB) * 100)

'				'Now actually make the Check, to see if the FileSize is acceptable
'				If CLng(_LastSVGFileGenerated.SizeInIntegralKB()).IsBetween(CInt(_TargetSVGFileSizeKB * ((100 - _FileSizeTolerancePercentage) / 100)), CInt(_TargetSVGFileSizeKB * ((100 + _FileSizeTolerancePercentage) / 100))) Then
'					Return
'				Else 'I.e. the FileSize was not acceptable; adjust the TSize...

'					Try
'						'Since the following operation (setting the new TSize) could result in an arithmetic overflow, we need to check for this
'						If (_CurrentTSize * (_PercentageOfTargetFileSize / 100)) >= UInt64.MaxValue Then Throw (New Exception("The new TSize would be greater than the max Value supported by a UInt64 (" & UInt64.MaxValue.ToString() & "). It would be: " & (_CurrentTSize * (_PercentageOfTargetFileSize / 100)).ToString()))

'						REM If we've gotten here, the range wasn't met (so we didn't return), so adjust the _CurrentTSize
'						'FileSize Too Small	->	Decrease TSize
'						'FileSize Too Large	->	Increase TSize
'						_CurrentTSize = CULng(_CurrentTSize * (_PercentageOfTargetFileSize / 100))
'						'E.g.						2	    *				50%						== 1

'					Catch _Ex As Exception
'						Throw New Exception("This Exception was thrown upon attempting to Assign the new TSize for Potrace whilst Generating an SVG for RasterGraphic """ & _CurrentRasterGraphic_FilePath & """: " & _Ex.Message)
'					End Try

'					_UsedTSizes.Add(_CurrentTSize)

'				End If

'				REM Unless this is the last iteration of this ForLoop, delete the SVGFile we just generated, because it's not of the correct size:
'				If Not _CurrentTSizeModerationAttempt = _MaxTSizeModerationAttempts Then IO.File.Delete(_FinalSVG_OutputPath)

'				REM On hitting the following "Next", we'll generate another SVGFile using the new TSize, OR, we'll move onto the next size of RasterGraphic

'			Next

'			REM If this is the 1st _CurrentRasterGraphicPixelCountReductionAttempt, AND the last SVG we just generated was empty,
'			REM ...Then reducing the RasterGraphic Size and trying again won't help us anyway. The RasterGraphic is probably just empty.
'			REM ...Therefore, we will Throw an Exception
'			If _CurrentRasterGraphicPixelCountReductionAttempt = 1 AndAlso SVGFileContainsNoBeziers(_FinalSVG_OutputPath) _
'			 Then Throw New GeneratedEmptySVGException("An Empty SVGFile was generated whilst using the entirety of the _InputRasterGraphic, over " & _MaxTSizeModerationAttempts & " attempts to moderate the TSize. The SVG FileSizes were: " & String.Join(", ", _GeneratedSVGFileSizes.ToArray()), _FinalSVG_OutputPath)

'			'This is needed so that we know which file to delete in a moment
'			Dim _LastRasterGraphic As IO.FileInfo = New IO.FileInfo(_CurrentRasterGraphic_FilePath)

'			REM If we're here, then we didn't Return, and therefore need to Generate [a version of the Current RasterGraphic, which is 80% of the size of the last one], and try again.
'			Try
'				Dim _ZoomedInRasterGraphic As New Drawing.Bitmap(_LastRasterGraphic.FullName)

'				MsgBox("Instanciated the Bitmap from: " & _LastRasterGraphic.FullName)

'				_CurrentRasterGraphic_FilePath = _BMPConversionFolder_Path & Library.FileResources.FindAvaliableFileNameInFolder(_BMPConversionFolder_Path, _LastRasterGraphic.Name)

'				MsgBox("Found this Free Name in the Folder: " & _CurrentRasterGraphic_FilePath)

'				_ZoomedInRasterGraphic.GetZoomedInCopy(_RasterGraphic_PixelCountReductionProportion).Save(_CurrentRasterGraphic_FilePath)

'			Catch _Ex As Exception
'				Throw New GraphPicturesConversionException("Upon attempting to Save a smaller version of the RasterGraphic: {" & _Ex.Message & "}", _Ex)
'			End Try

'			REM Now that the New smaller RasterGraphic is being used, delete the old one:
'			_LastRasterGraphic.Delete()

'			_CurrentRasterGraphicPixelCountReductionAttempt += CByte(1)

'		Loop Until _CurrentRasterGraphicPixelCountReductionAttempt = _MaxRasterGraphic_PixelCountReductionAttempts

'		REM Coming out of that Loop, it's because _MaxRasterGraphicPixelCountReductionAttempts was reached, before we could generate an SVG of the desired FileSize.
'		REM Therefore, we need to check weather the very last SVG was too small (but NOT empty) in size:
'		REM		SVGFile Empty		->	Throw GeneratedEmptySVGException
'		REM		SVGFile too Small	->	Log() and Return
'		REM		SVGFile too Large	->	Log() and Return

'		If SVGFileContainsNoBeziers(_LastSVGFileGenerated.FullName) Then
'			Throw New GeneratedEmptySVGException("After all attempts to regulate the size of the SVGFile, it still produced an empty SVGFile with no Beziers. The RasterGraphic was: " & _InputRasterGraphic.FullName, _LastSVGFileGenerated.FullName)
'		ElseIf _LastSVGFileGenerated.SizeInIntegralKB() > CInt(_TargetSVGFileSizeKB * ((100 + _FileSizeTolerancePercentage) / 100)) Then
'			Throw New GraphPicturesConversionException("After all attempts to produce an SVG of the Desired FileSize, if was still above + or - " & _FileSizeTolerancePercentage & "% of " & _TargetSVGFileSizeKB & "KB. It's size was: " & _LastSVGFileGenerated.SizeInIntegralKB() & "KB.")
'		ElseIf _LastSVGFileGenerated.SizeInIntegralKB() < CInt(_TargetSVGFileSizeKB * ((100 - _FileSizeTolerancePercentage) / 100)) Then
'			Throw New GraphPicturesConversionException("After all attempts to produce an SVG of the Desired FileSize, if was still below + or - " & _FileSizeTolerancePercentage & "% of " & _TargetSVGFileSizeKB & "KB. It's size was: " & _LastSVGFileGenerated.SizeInIntegralKB() & "KB.")
'		Else
'			Throw New Exception("This Exception should never be able to be seen. It's been Thrown because all attempts to regulate an SVG's FileSize have failed, and the final SVG wasn't empty, too large, or too small. The RasterGraphic was: " & _InputRasterGraphic.FullName & ".")
'		End If

'	Catch _Ex As Exception When True
'		Throw New GraphPicturesConversionException("The following Exception was thrown upon attempting to Generate an SVGFile for the RasterGraphic """ & _InputRasterGraphic.FullName & """: " & _Ex.Message & ". The SVGs generated so far produced these File Sizes: " & String.Join(", ", _GeneratedSVGFileSizes.ToArray()) & ". The TSizes used were: " & String.Join(", ", _UsedTSizes.ToArray()) & ". The Final Output Path specified for the SVGFile to be generated was: " & _FinalSVG_OutputPath, _Ex)
'	End Try

'End Sub

'#End Region

'#Region "Old SVGGeneration Logic which was actually in use"
' ''' <summary>
' ''' If the RasterGraphic is not already a .BMP file, then it is converted. Only the common Raster Formats are supported (BMP, JFIF, PNG etc...)
' ''' </summary>
' ''' <param name="_TempFolderInWhichToStoreBMP">The Folder in which to store the RasterImage as a .BMP file, in case it is not already a .BMP File</param>
' ''' <remarks></remarks>
'Public Sub GenerateSVGFromRasterGraphic(ByVal _RasterGraphicPath$, ByVal _DesiredOutputSVGPath$, ByVal _TempFolderInWhichToStoreBMP$, ByVal _PotraceExeFilePath$, Optional ByVal _TSize As UInt32 = &H2UI)

'	Try

'		'Check if the Specified Files and Folders actually exist...
'		If Not System.IO.File.Exists(_RasterGraphicPath) Then Throw (New System.Exception("An attempt was made to Generate an SVGFile from a RasterGraphic File, however the File does not exist. The path was: " & _RasterGraphicPath))
'		If Not System.IO.File.Exists(_PotraceExeFilePath) Then Throw (New System.Exception("An attempt was made to Generate an SVGFile from a RasterGraphic File, however the Potrace .EXE File does not exist. The path was: " & _PotraceExeFilePath))
'		If Not System.IO.Directory.Exists(_TempFolderInWhichToStoreBMP) Then Throw (New System.Exception("An attempt was made to Generate an SVGFile from a RasterGraphic File, however the specified path for [a temp folder to store a .bmp version of the RasterGraphic in case it isn't a .BMP] does not exist. The path was: " & _TempFolderInWhichToStoreBMP))

'		REM Make sure that we are dealing with a .BMP file
'		If (Not _RasterGraphicPath.EndsWith(".BMP", StringComparison.CurrentCultureIgnoreCase)) Then
'			Dim _RasterImage As New Drawing.Bitmap(_RasterGraphicPath)
'			_RasterGraphicPath = CStr(IIf(_TempFolderInWhichToStoreBMP.EndsWith("\"), _TempFolderInWhichToStoreBMP, [_TempFolderInWhichToStoreBMP] & "\"c)) & GetRandomString(&HAUS) & ".BMP"
'			_RasterImage.Save(_RasterGraphicPath, Drawing.Imaging.ImageFormat.Bmp)
'		End If

'		REM Coning out of that IFSTATMENT, the _RasterGraphicPath$ should now point to a .BMP; ready for Potrace

'		Dim _PotraceCLAs$ = "--output """ & _DesiredOutputSVGPath & """ --svg --progress --turdsize " & _TSize.ToString() & " """ & _RasterGraphicPath & """"

'		With (New Diagnostics.Process())
'			.StartInfo = (New Diagnostics.ProcessStartInfo(_PotraceExeFilePath, _PotraceCLAs))
'			.Start()
'			Try
'				If Not .WaitForExit(10000I) Then 'Wait for a Maximun of 10 Seconds before Killing the process if it's still running, and returning
'					If Not (.ExitCode = 0) Then Throw New GraphPicturesConversionException("The Potrace Process returned a non-zero ExitCode. The ExitCode was: " & .ExitCode.ToString() & ". The Command-Line Arguments were: " & _PotraceCLAs)
'					If Not .HasExited Then .Kill()
'					.Kill()
'				End If
'			Catch _Ex As [Exception] When True
'				Throw (New GraphPicturesConversionException("The Potrace Process could not be killed, or waited upon to exit, or it returned an Error. The Exception Thrown was: " & _Ex.Message))
'			End Try
'		End With

'		Return

'	Catch _Ex As System.Exception When True
'		Throw (New GraphPicturesConversionException("The following Exception was thrown upon attempting to convert a RasterGraphic to an SVGFile: " & _Ex.Message, _Ex))
'	End Try

'End Sub
'#End Region