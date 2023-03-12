Imports GraphPictures.Library.CompilerExtentions, Debugging = GraphPictures.Library.[Resources]

Namespace WebParts

	Public Module Resources

		REM The Folders which the WWW can access, containing the Images being Processed
		Public ReadOnly WWWSVGsFolderPath$ = (HttpContext.Current.Server.MapPath("~") & "ClientData\SVGFiles\")
		Public ReadOnly WWWUploadedRasterImagesFolderPath$ = (HttpContext.Current.Server.MapPath("~") & "ClientData\UploadedRasterImages\")
		Public ReadOnly WWWUploadedRasterImageBitmapConversionFolderPath$ = (HttpContext.Current.Server.MapPath("~") & "ClientData\UploadedRasterImages\ConvertedToBMP\")
		Public ReadOnly WWWCachedSVGBeziersFolderPath$ = (HttpContext.Current.Server.MapPath("~") & "ClientData\SVGFiles\CachedSVGBeziers\")
		Public ReadOnly WWWPotraceEXEPath$ = (HttpContext.Current.Server.MapPath("~") & "ClientData\Potrace.EXE")
		Public ReadOnly WWWVideosFolderPath$ = (HttpContext.Current.Server.MapPath("~") & "ClientData\Videos\")
		Public ReadOnly WWWFFMPEGEXEPath$ = (HttpContext.Current.Server.MapPath("~") & "ClientData\Videos\FFMPEG.EXE")

		Public ReadOnly WebPartsSVGGenerationOptions As New GraphPictures.Library.ImagingResources.SVGGenerator.SVGGenerationOptions() With {
		 .BitmapConversionFolder = New IO.DirectoryInfo(WWWUploadedRasterImageBitmapConversionFolderPath),
		 .PotraceEXE = New IO.FileInfo(WWWPotraceEXEPath),
		 .SVGSizeModerationMode = Library.SVGGenerator.SVGSizeModerationAlgorythm.AdjustInputQuality,
		 .TargetOrMaximun_SVGFileSizeKB = 40US
		}

		Public Const VideoFrameRate% = 1 'Frames Per Second
		Public Const MaxAllowedSVGFileSizeKB As UInt64 = 80	'The API will refuse to derive Beziers from SVG's over this Size
		Public Const DefaultVideoFrameTargetSVGFileSize As UInt16 = 60US
		Public ReadOnly DefaultVideoDesmosGraphOptions As New Library.Resources.DesmosStateCreationOptions() With {.ShowGrid = True, .ColourAllocationMode = Library.ColourRange.ColourGenerationMode.AllBlack, .GraphZoomSize = 1.05@}

		''' <summary>
		''' .APIR - WITH the dot (.)
		''' </summary>
		Public Const FileExtentionForCachedAPIResponses$ = ".APIR" 'The Extention used by cached API Responses saved to disk

		''' <summary>
		''' Returns the FileName of the new SVG, in the [ClientData\SVGFiles] Folder
		''' </summary>
		''' <param name="_UploadedImageFileName">JUST THE File Name of the SVG, it is assumed that it's in the [ClientData\UploadedRasterImages] Folder</param>
		''' <returns>Returns the FileName of the new SVG, in the [ClientData\SVGFiles] Folder</returns>
		''' <remarks></remarks>
		Public Function GenerateSVGFromUploadedImage(ByVal _UploadedImageFileName$, Optional ByVal _TargetSVGFileSizeKB As UInt16 = 30) As String

			Try

				With (New Library.ImagingResources.SVGGenerator(New IO.FileInfo(IO.Path.Combine(WWWUploadedRasterImagesFolderPath, _UploadedImageFileName)), WebParts.Resources.WebPartsSVGGenerationOptions))
					.SVGGenerationParameters.TargetOrMaximun_SVGFileSizeKB = _TargetSVGFileSizeKB
					Return .GenerateSVGInFolder(New IO.DirectoryInfo(WWWSVGsFolderPath)).LastSVGGenerated.Name
				End With

			Catch _Ex As Exception When True
				Throw (New GraphPictures.Library.GraphPicturesConversionException("WebParts: The following Exception was thrown upon attempting to Generate an SVG of desired FileSize " & _TargetSVGFileSizeKB & "KB: " & _Ex.Message & ". The File may well have been too large, and as such, may have generated too many Beziers. The SVGFile may, however, have still been generated...", _Ex))
			End Try

		End Function

#Region "VideoTranscoding Resources"

		REM VideoFile Location Convention:
		REM \ClientData\Videos\Input.MP4
		REM \ClientData\Videos\Input.MP4_Frames\Frame00001.BMP
		REM \ClientData\Videos\Input.MP4_Frames\SVGs\Frame00001.SVG
		REM \ClientData\Videos\Input.MP4_Frames\DesmosFrames\Frame00001.PNG
		REM \ClientData\Videos\DesmosRendered_Input.MP4
		REM \ClientData\Videos\AudioFor_Input.MP4
		REM \ClientData\Videos\WithAudio_DesmosRendered_Input.MP4

		''' <summary>
		''' Generates Frames from _VideoFileName$
		''' </summary>
		''' <param name="_VideoFileName"></param>
		''' <returns>The Number of Frames Generated</returns>
		''' <remarks></remarks>
		Public Function GenerateFramesFromVideo(ByVal _VideoFileName$) As UInt32

			Try

				'Check that the Specified File actually exists...
				If Not System.IO.File.Exists(WWWVideosFolderPath & _VideoFileName) Then Throw (New GraphPictures.Library.GraphPicturesConversionException("An attempt was made to Generate Frames from a VideoFile, however the File does not exist. The path was: " & (WWWVideosFolderPath & _VideoFileName)))

				REM Create the Directory into which the Frames for this Video will be Rendered
				IO.Directory.CreateDirectory(WWWVideosFolderPath & _VideoFileName & "_Frames\")

				REM Specify the CommandLine Arguments for FFMPEG
				Dim _FFMPEGCLAs$ = "-i """ & (WWWVideosFolderPath & _VideoFileName) & """ -r " & Resources.VideoFrameRate.ToString() & " """ & WWWVideosFolderPath & _VideoFileName & "_Frames\Frame%05d.BMP"""

				With (New Diagnostics.Process())
					.StartInfo = (New Diagnostics.ProcessStartInfo(WWWFFMPEGEXEPath, _FFMPEGCLAs))
					.Start()
					Try
						.WaitForExit()
						If Not (.ExitCode = 0) Then Throw New [Library].GraphPicturesConversionException("The FFMPEG Process returned a non-zero ExitCode. The ExitCode was: " & .ExitCode.ToString() & ". The Command-Line Arguments were: " & _FFMPEGCLAs & ". If FFMPEG complains about the Width or height not being an even number, then make -vf command look like this: [-vf ""pad=ceil(iw/2)*2:ceil(ih/2)*2,fps=25,format=yuv420p""] (Without the square brackets...)")
					Catch _Ex As Library.GraphPicturesConversionException When True
						Throw (New Library.GraphPicturesConversionException("The FFMPEG Process could not be waited upon to exit, or it returned an Error. The Exception Thrown was: " & _Ex.Message))
					End Try
				End With

				REM This last part of the function Works out how many Frames were generated...
				Dim _FrameFileNames As New List(Of String)()
				For Each _FrameFile As IO.FileInfo In (New IO.DirectoryInfo(WWWVideosFolderPath & _VideoFileName & "_Frames\").GetFiles())
					_FrameFileNames.Add(_FrameFile.Name)
				Next

				_FrameFileNames.Sort()

				REM Windows NT will generate a Thumbnail Image DataBase if the Thumbnails are used in Explorer.EXE
				If _FrameFileNames.Last().ToUpper() = "THUMBS.DB" Then _FrameFileNames.RemoveAt(_FrameFileNames.Count - 1)

				'Make sure that the Last FileName we're left with, is actually a Frame:
				If Not _FrameFileNames.Item(_FrameFileNames.Count - 1).MatchesRegEx(Library.Resources.VideoFrame_FileName_RegExp$) Then Throw (New Library.GraphPicturesConversionException("The Last FileName in the Video's Frames Directory did not match the Regular Expression for a Frame. The FileName was: " & _FrameFileNames.Item(_FrameFileNames.Count - 1)))

				REM E.g. Frame00152.BMP
				Return CUInt(_FrameFileNames.Item(_FrameFileNames.Count - 1).Substring(5, 5))

			Catch _Ex As System.Exception When True
				Throw (New Library.GraphPicturesConversionException("The following Exception was thrown upon attempting to Generate Frames from a Video: " & _Ex.Message, _Ex))
			End Try

		End Function

		''' <summary>
		''' Generated the output as DesmosRendered_{_VideoName}
		''' </summary>
		''' <remarks></remarks>
		Public Sub GenerateVideoFromDesmosFrames(ByVal _VideoName$)

			Try

				'Check that the Specified File actually exists...
				If Not Global.System.IO.Directory.Exists(WWWVideosFolderPath & _VideoName & "_Frames\DesmosFrames\") Then Throw (New GraphPictures.Library.GraphPicturesConversionException("An attempt was made to Generate A VideoFile From some DesmosFrames, however the DesmosFrames do not exist. The VideoFile was: " & _VideoName))

				REM Specify the CommandLine Arguments for FFMPEG
				Dim _FFMPEGCLAs$ = "-r " & Resources.VideoFrameRate.ToString() & " -start_number 0 -i """ & WWWVideosFolderPath & _VideoName & "_Frames\DesmosFrames\Frame%5d.PNG"" -c:v libx264 -vf ""fps=25,format=yuv420p"" """ & WWWVideosFolderPath & "DesmosRendered_" & _VideoName & """"

				With (New Diagnostics.Process())
					.StartInfo = (New Diagnostics.ProcessStartInfo(WWWFFMPEGEXEPath, _FFMPEGCLAs))
					.Start()
					Try
						.WaitForExit()
						If Not (.ExitCode = 0) Then Throw New [Library].GraphPicturesConversionException("The FFMPEG Process returned a non-zero ExitCode. The ExitCode was: " & .ExitCode.ToString() & ". The Command-Line Arguments were: " & _FFMPEGCLAs)
					Catch _Ex As Library.GraphPicturesConversionException When True
						Throw (New Library.GraphPicturesConversionException("The FFMPEG Process could not be waited upon to exit, or it returned an Error. The Exception Thrown was: " & _Ex.Message))
					End Try
				End With

				Return

			Catch _Ex As System.Exception When True
				Throw (New Library.GraphPicturesConversionException("The following Exception was thrown upon attempting to Generate A Video, from DesmosFrames: " & _Ex.Message, _Ex))
			End Try

		End Sub

		''' <summary>
		''' Produces an SVG for the Frame, assuming that the Frame is in the location [ClientData\Videos\VideoName.MP4_Frames\Frame00001.BMP].
		''' The SVG is then saved to [ClientData\Videos\VideoName.MP4_Frames\SVGs\Frame00001.SVG].
		''' </summary>
		''' <returns>All SVGGenerationResultData. To get the SVGFileName, use *.LastSVGFileGenerated.Name - This is just the Name of the File, NOT the FullPath</returns>
		Public Function GenerateSVGForVideoFrame(ByVal _VideoName$, ByVal _FrameNumber$, Optional ByVal _TargetSVGFileSizeKB As UInt16 = DefaultVideoFrameTargetSVGFileSize) As Library.ImagingResources.SVGGenerator.SVGGenerationResultData

			'From		\ClientData\Videos\{_VideoName}_Frames\Frame{_FrameNumber}.BMP
			'Generate	\ClientData\Videos\{_VideoName}_Frames\SVGs\Frame_{_FrameNumber}.SVG

			Try
				REM Check that the Output Dir for the SVG is generated already
				If Not IO.Directory.Exists(IO.Path.Combine(WWWVideosFolderPath, _VideoName & "_Frames\SVGs\")) Then
					Debugging.MsgDebug("The Output Folder for SVG's for the Video """ & _VideoName & """ does not exist; it will be created.")
					IO.Directory.CreateDirectory(WWWVideosFolderPath & _VideoName & "_Frames\SVGs\")
				End If

				With (New Library.ImagingResources.SVGGenerator(New IO.FileInfo(IO.Path.Combine(WWWVideosFolderPath, (_VideoName & "_Frames\Frame" & _FrameNumber & ".BMP"))), WebParts.Resources.WebPartsSVGGenerationOptions))
					.SVGGenerationParameters.TargetOrMaximun_SVGFileSizeKB = _TargetSVGFileSizeKB
					Return .GenerateSVG(IO.Path.Combine(WWWVideosFolderPath, (_VideoName & "_Frames\SVGs\Frame_" & _FrameNumber & ".SVG")))
				End With

			Catch _EmptySVGEx As GraphPictures.Library.GeneratedEmptySVGException
				REM We want to pass this Exception to the [Case "DesmosStateForVideoFrame"] API EndPoint,
				REM	...Who will send back the EmptyFrameDesmosGraphState
				Throw _EmptySVGEx
			Catch _Ex As Exception When True
				Throw (New GraphPictures.Library.GraphPicturesConversionException("VideoFrame to SVG: The following Exception was thrown upon attempting to Generate an SVG of desired FileSize " & _TargetSVGFileSizeKB & "KB: " & _Ex.Message, _Ex))
			End Try

		End Function

		''' <summary>
		''' Generates a new version of a DesmosRendered_Input.MP4, with the AudioTrack from the Input.MP4
		''' </summary>
		''' <remarks>Uses FFMPEG</remarks>
		Public Sub GenerateVersionOfVideoWithAudio(ByVal _VideoName$)

			REM The _VideoName ALREADY HAS THE DesmosRendered_ Part in it's name, from the JavaScript

			Try

				'Check that the Specified DesmosRendered File actually exists...

				If Not Global.System.IO.File.Exists(WWWVideosFolderPath & "DesmosRendered_" & _VideoName) Then Throw (New GraphPictures.Library.GraphPicturesConversionException("An attempt was made to Generate A VideoFile with Audio, From the DesmosRendered_VideoName.MP4, however the DesmosRendered File does not exist. The VideoFile Name was: " & _VideoName))
				If Not Global.System.IO.File.Exists(WWWVideosFolderPath & _VideoName) Then Throw (New GraphPictures.Library.GraphPicturesConversionException("An attempt was made to Generate A VideoFile with Audio, From the DesmosRendered_VideoName.MP4, however the Origional VideoFile does not exist. The VideoFile was: " & _VideoName))

				'Firstly, extract the Audio from the Input.MP4
				'ffmpeg -i sample.avi -q:a 0 -map a sample.mp3

				REM Specify the CommandLine Arguments for FFMPEG
				Dim _FFMPEGCLAs$ = "-i """ & WWWVideosFolderPath & _VideoName & """ -q:a 0 -map a """ & WWWVideosFolderPath & "AudioFor_" & _VideoName & ".MP3"""

				With (New Diagnostics.Process())
					.StartInfo = (New Diagnostics.ProcessStartInfo(WWWFFMPEGEXEPath, _FFMPEGCLAs))
					.Start()
					Try
						.WaitForExit()
						If Not (.ExitCode = 0) Then Throw New [Library].GraphPicturesConversionException("During AudioExtraction: The FFMPEG Process returned a non-zero ExitCode. The ExitCode was: " & .ExitCode.ToString() & ". The Command-Line Arguments were: " & _FFMPEGCLAs)
					Catch _Ex As Library.GraphPicturesConversionException When True
						Throw (New Library.GraphPicturesConversionException("During AudioExtraction: The FFMPEG Process could not be waited upon to exit, or it returned an Error. The Exception Thrown was: " & _Ex.Message))
					End Try
				End With


				'Now, Apply the Extracted AudioTrack to the DesmosRendered_VideoName.MP4
				'ffmpeg -i video.mp4 -i audio.wav -c:v copy -c:a aac output.mp4

				REM Specify the CommandLine Arguments for FFMPEG
				_FFMPEGCLAs$ = "-i """ & WWWVideosFolderPath & "DesmosRendered_" & _VideoName & """ -i """ & WWWVideosFolderPath & "AudioFor_" & _VideoName & ".MP3"" -c:v copy -c:a aac """ & WWWVideosFolderPath & "WithAudio_DesmosRendered_" & _VideoName & """"

				With (New Diagnostics.Process())
					.StartInfo = (New Diagnostics.ProcessStartInfo(WWWFFMPEGEXEPath, _FFMPEGCLAs))
					.Start()
					Try
						.WaitForExit()
						If Not (.ExitCode = 0) Then Throw New [Library].GraphPicturesConversionException("During AudioCopying: The FFMPEG Process returned a non-zero ExitCode. The ExitCode was: " & .ExitCode.ToString() & ". The Command-Line Arguments were: " & _FFMPEGCLAs)
					Catch _Ex As Library.GraphPicturesConversionException When True
						Throw (New Library.GraphPicturesConversionException("During AudioCopying: The FFMPEG Process could not be waited upon to exit, or it returned an Error. The Exception Thrown was: " & _Ex.Message))
					End Try
				End With

				Return

			Catch _Ex As System.Exception When True
				Throw (New Library.GraphPicturesConversionException("The following Exception was thrown upon attempting to Generate an Audio-Including Video, from The Input.MP4 Video: " & _Ex.Message, _Ex))
			End Try

		End Sub

#End Region

	End Module

End Namespace