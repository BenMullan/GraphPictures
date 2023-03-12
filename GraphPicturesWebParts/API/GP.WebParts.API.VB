Imports GraphPictures.Library.CompilerExtentions

Namespace WebParts

	Public Module API

#Region "Get.ASPX"

		Public Sub GetASPX(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			_Response.ContentType = "application/json"

			'We need a ?Item, to know what they want to "Get"...
			_Request.CheckRequiredQueryStringsAreSpecified(_Response, "Item")

			Select Case _Request.QueryString("Item")

				'Case "Debug"

				'	_Response.ClearAndWriteAndEnd("?Test IsSpecified: " & _Request.QueryStringIsSpecified("Test").ToString() & vbCrLf & "?Test HasValue: " & _Request.QueryStringIsSpecifiedAndHasValue("Test").ToString())

				Case "DesmosStateForSVG"

					REM QueryStrings:	[SVGFileName], [?BypassSVGFileSizeCheck], [?ShowGrid], [?ColourAllocationMode], [?GraphZoomSize], [?LinesOpacity], [?LinesWidth], [?LinesStyle], [?Fill], [?FillOpacity]
					REM Returns:		[DesmosState], [ExpressionCount]

					Try
						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "SVGFileName")

						'This file may or may not exist (we check in the following IF Statment) but this path is stored as a variable so that it need only be resolved once (It is referanced in both parts of the following If Statment)
						Dim _CachedSVGBeziers_FilePath$ = WebParts.WWWCachedSVGBeziersFolderPath & _Request.QueryString!SVGFileName & WebParts.FileExtentionForCachedAPIResponses

						' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
						' Because of the Boolean Expr below, if artistic options are specified,
						' a new copy of the Beziers will be cached with those options,
						' such that if a normal request is later made (without artistic options)
						' then the cached version WITH the artistic options will actually be served.
						' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

						REM Check if a Cached copy of this SVG's Beziers already exist
						REM If any of the artistic options are specified, then we don't want to used the cached copy (which is for without any artistic options)
						If _
						 (Not {"ShowGrid", "ColourAllocationMode", "GraphZoomSize", "LinesOpacity", "LinesWidth", "LinesStyle", "Fill", "FillOpacity"}.Any(AddressOf _Request.QueryStringIsSpecified)) _
						 AndAlso [IO].File.Exists(_CachedSVGBeziers_FilePath$) _
						Then 'Just serve the Cached copy of this [?SVGFileName]'s Beziers which have already been derived

							_Response.Clear()
							_Response.WriteFile(_CachedSVGBeziers_FilePath$)
							_Response.SafelyEnd()

						Else 'There is no Cached Copy; proceed to derive the Beziers from the specified ?SVGFileName, and then cache them

							'Refuse to Resolve BEZIERS from any SVGFile over the Maximun allowed Size, as set in GP.WebParts.Resources.VB
							'The ?BypassSVGFileSizeCheck being "True" Bypasses this check (for use manually on the initial generation of an SVG only by an administrator, whereafter cached copies can be served.)
							If Not (_Request.QueryStringIsSpecifiedAndHasValue("BypassSVGFileSizeCheck") AndAlso _Request.QueryString!BypassSVGFileSizeCheck.ToUpper() = "TRUE") Then _
							 If (IO.File.Exists(Global.GraphPictures.WebParts.WWWSVGsFolderPath & _Request.QueryString("SVGFileName"))) AndAlso ((New IO.FileInfo(Global.GraphPictures.WebParts.WWWSVGsFolderPath & _Request.QueryString("SVGFileName"))).SizeInIntegralKB() > WebParts.Resources.MaxAllowedSVGFileSizeKB) Then Throw New GraphPictures.Library.Exceptions.GraphPicturesException("The Specified SVGFile has a FileSize of " & (New IO.FileInfo(Global.GraphPictures.WebParts.WWWSVGsFolderPath & _Request.QueryString("SVGFileName"))).SizeInIntegralKB().ToString() & "KB, which is greater than the largest allowed SVGFileSize of " & WebParts.Resources.MaxAllowedSVGFileSizeKB & "KB")

							'Generate the Beziers for the ?SVGFileName
							Dim _Beziers As Global.GraphPictures.Library.BezierObjects.IDesmosExpression() _
							 = Global.GraphPictures.Library.BezierResources.GetBeziersFromPotraceSVG(Global.GraphPictures.WebParts.WWWSVGsFolderPath & _Request.QueryString("SVGFileName"))

							'Resolve the Options for how A DesmosGraph should be made, from those Beziers
							Dim _DesmosGraphOptions As Library.Resources.DesmosStateCreationOptions = (New Library.Resources.DesmosStateCreationOptions()).ModifyByQueryStrings(_Request)

							Dim _SVGBeziersAPIResponse$ = "{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""DesmosState"" : " & _Beziers.ToDesmosState(_DesmosGraphOptions).Replace("\", "\\") & ", ""ExpressionCount"" : """ & _Beziers.Length.ToString() & """ } }"

							REM Cache a copy of the Response for this SVG
							Try
								My.Computer.FileSystem.WriteAllText(_CachedSVGBeziers_FilePath, _SVGBeziersAPIResponse, append:=False)
							Catch _Ex As Exception When True
								Throw New IO.IOException("Whilst saving a cached copy of the Beziers derived from the SVGFile: " & _Ex.Message, _Ex)
							End Try

							REM Now return this Response, what with it's having been cached
							_Response.ClearAndWriteAndEnd(_SVGBeziersAPIResponse)

						End If

					Catch _Ex As Global.System.Exception
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An error occoured whilst converting the SVG into a Desmos JSON State. The Error was: " & _Ex.Message & ". The Specified SVGFile (" & _Request.QueryString("SVGFileName") & ") may not exist."" }, ""ResponseContent"" : {} }")
					End Try

				Case "SVGFromRasterGraphic"

					REM QueryStrings:	[RasterGraphicFileName]
					REM Returns:		[SVGFileName]

					Try
						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "RasterGraphicFileName")

						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""SVGFileName"" : """ & Global.GraphPictures.WebParts.GenerateSVGFromUploadedImage(_Request.QueryString("RasterGraphicFileName")) & """ } }")
					Catch _Ex As Exception
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An error occoured whilst generating the SVG from the RasterGraphic File """ & _Request.QueryString("RasterGraphicFileName").ToString() & """. The Error was: " & _Ex.Message & ". Bear in mind that the Specified RasterGraphic File (" & _Request.QueryString("RasterGraphicFileName").ToString() & ") may not exist."" }, ""ResponseContent"" : {} }")
					End Try

				Case "DesmosStateForPlottedRasterGraphic"

					REM QueryStrings:	[RasterGraphicFileName]
					REM Returns:		[DesmosState], [ExpressionCount]

					Try
						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "RasterGraphicFileName")

						Dim _Points As Global.GraphPictures.Library.BezierObjects.IDesmosExpression() _
						 = (New GraphPictures.Library.PointPlottedRasterGraphic(WWWUploadedRasterImagesFolderPath & _Request.QueryString("RasterGraphicFileName"))).GetPoints()

						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""DesmosState"" : " & _Points.ToDesmosState(New Library.Resources.DesmosStateCreationOptions() With {.ShowGrid = False, .ColourAllocationMode = Library.ColourRange.ColourGenerationMode.UseExpressionColour}).Replace("\", "\\") & ", ""ExpressionCount"" : """ & _Points.Length.ToString() & """ } }")

					Catch _Ex As Exception When True
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An error occoured whilst Generating a Desmos JSON State for the RasterGraphic using Plotting. The Error was: " & _Ex.Message & ". The Specified ?RasterGraphicFileName (" & _Request.QueryString("RasterGraphicFileName").ToString() & ") may not exist."" }, ""ResponseContent"" : {} }")
					End Try

				Case "AwaitChangeInUploadedRasterImagesList"

					REM QueryStrings:	[ClientKnownUploadedRasterImages (|-deliminated)]
					REM Returns:		[RequestTimedOut], [UploadedRasterImages (Array)]

					Try
						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "ClientKnownUploadedRasterImages")

						Const _MaxCheckCount As UInt16 = 60
						Const _CheckInterval As UInt16 = 1000
						Dim _CheckCount As UInt16

						REM Wait here on the Server until [MaxCheckCount expirers] or [there is a change between the Client's FileList and the Contents of the Folder]
						For _CheckCount = 0 To _MaxCheckCount Step +1
							'If [there is a change between the Client's FileList and the Contents of the Folder] then Exit For

							Dim _UploadedRGs_OrDeliminated$() = (
							From _RasterGraphicFile As IO.FileInfo
							In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWUploadedRasterImagesFolderPath)).GetFiles()
							Where {".BMP", ".JFIF", ".JPEG", ".JPG", ".PNG", ".GIF"}.Contains(_RasterGraphicFile.Extension.ToUpper())
							Select _RasterGraphicFile.Name
							).OrderBy(Of String)(Function(_FileName$) _FileName).ToArray()

							'MsgBox("Comparing" & vbCrLf & vbCrLf & String.Join("|", _UploadedRGs_OrDeliminated).ToUpper() & vbCrLf & vbCrLf & "with" & vbCrLf & vbCrLf & _Request.QueryString.Item("ClientKnownUploadedRasterImages").ToUpper() & vbCrLf & vbCrLf & "Result: " & (String.Join("|", _UploadedRGs_OrDeliminated).ToUpper() = _Request.QueryString.Item("ClientKnownUploadedRasterImages").ToUpper()).ToString() & vbCrLf & vbCrLf & "Check " & _CheckCount.ToString() & " of " & _MaxCheckCount.ToString())

							If Not (String.Join("|", _UploadedRGs_OrDeliminated).ToUpper() = _Request.QueryString.Item("ClientKnownUploadedRasterImages").ToUpper()) Then
								Exit For
							Else 'They are the same; keep checking...
								Threading.Thread.Sleep(_CheckInterval)
							End If

						Next

						'Work out weather we've left the ForLoop because of MaxCheckCount being exceeded, or because the Folder has changed
						If _CheckCount >= _MaxCheckCount Then
							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""RequestTimedOut"" : ""true"", ""UploadedRasterImages"" : [] } }")
						Else 'We exited because there was a change

							Dim _UploadedRGsJSONStrings$() = (
							From _RasterGraphicFile As IO.FileInfo
							In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWUploadedRasterImagesFolderPath)).GetFiles()
							Where {".BMP", ".JFIF", ".JPEG", ".JPG", ".PNG", ".GIF"}.Contains(_RasterGraphicFile.Extension.ToUpper())
							Select ("{ ""FileName"" : """ & _RasterGraphicFile.Name & """, ""FileSizeKB"" : """ & _RasterGraphicFile.SizeInIntegralKB() & """ }")
							).OrderBy(Of String)(Function(_FileName$) _FileName).ToArray()

							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""RequestTimedOut"" : ""false"", ""UploadedRasterImages"" : [ " & String.Join(", ", _UploadedRGsJSONStrings) & " ] } }")

						End If

					Catch _Ex As Exception When True
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An error occoured whilst retreiving the UploadedRasterImagesList. The Error was: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
					End Try

				Case Else
					REM An unmatched ?Item was specified...
					_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An unrecognised ?Item was specified."" }, ""ResponseContent"" : {} }")
			End Select

		End Sub

#End Region

#Region "VideoAPI.ASPX"

		Public Sub VideoAPIASPX(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			_Response.ContentType = "application/json"

			_Request.CheckRequiredQueryStringsAreSpecified(_Response, "Action")

			Select Case _Request.QueryString("Action").ToString()

				Case "VideoToFrames"

					REM QueryStrings:	[VideoFile]
					REM Returns:		[GeneratedFramesCount]

					Try
						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "VideoFile")

						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""GeneratedFramesCount"" : """ & WebParts.Resources.GenerateFramesFromVideo(_Request.QueryString("VideoFile")).ToString() & """ } }")

					Catch _Ex As Exception When True
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The following Exception was thrown upon attempting to Generate the Frames for a Video: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
					End Try

				Case "DesmosStateForVideoFrame"

					REM QueryStrings:	[VideoName], [FrameNumber], [?ShowGrid], [?ColourAllocationMode], [?GraphZoomSize], [?LinesOpacity], [?LinesWidth], [?LinesStyle], [?Fill], [?FillOpacity]
					REM Returns:		[?SVGGenerationResultDataBase64], [ExpressionCount], [DesmosState]

					_Request.CheckRequiredQueryStringsAreSpecified(_Response, "VideoName", "FrameNumber")

					Try

						If Not IO.File.Exists(WWWVideosFolderPath & _Request.QueryString("VideoName").ToString()) Then
							Throw New GraphPictures.Library.GraphPicturesConversionException("The VideoFile Does not exist in the Directory. The requested file was: " & _Request.QueryString("VideoName"))
						ElseIf Not IO.File.Exists(WWWVideosFolderPath & _Request.QueryString("VideoName").ToString() & "_Frames\Frame" & _Request.QueryString("FrameNumber").ToString() & ".BMP") Then
							Throw New GraphPictures.Library.GraphPicturesConversionException("The Frame for the Video """ & _Request.QueryString("VideoName").ToString() & """ does not exist. The requested FrameNumber was: " & _Request.QueryString("FrameNumber"))
						End If

						'Resolve the Options for how a DesmosGraph should be made, for either [the beziers of the Frame's SVG] or [the Empty List(Of IDesmosExpression)]...
						Dim _DesmosGraphOptions As Library.Resources.DesmosStateCreationOptions _
						  = WebParts.Resources.DefaultVideoDesmosGraphOptions.ModifyByQueryStrings(_Request)
						'WITH ARTISTIC OPTIONS: = (New Library.Resources.DesmosStateCreationOptions() With {.ShowGrid = True, .ColourAllocationMode = Library.ColourRange.ColourGenerationMode.Rainbow, .Fill = True, .FillOpacity = 0.8D, .LinesOpacity = 0.4D, .LinesStyle = Library.DesmosStateCreationOptions.DesmosLineStyle.DASHED, .LinesWidth = 12}).ModifyByQueryStrings(_Request)

						Dim _SVGGenResData As New Library.ImagingResources.SVGGenerator.SVGGenerationResultData()
						Dim _SVGFileName$ = "GP_NONEXIST"
						'The _SVGFileName and _SVGGenResData will either be assigned below because no exceptions occour during the SVGGeneration,
						' ...Or, an Exception will be Thrown during the SVGGeneration, in which case the Catch Statment will
						' ... ClearAndWriteAndEnd() the Response Stream.

						REM If an EmptySVG is Generated, then it's probably because there was a blank VideoFrame, so send back the EmptyFrameDesmosGraphState
						Try
							_SVGGenResData = Global.GraphPictures.WebParts.Resources.GenerateSVGForVideoFrame(_Request.QueryString("VideoName"), _Request.QueryString("FrameNumber"))
							_SVGFileName$ = _SVGGenResData.LastSVGGenerated.Name
						Catch _EmptySVGEx As GraphPictures.Library.GeneratedEmptySVGException
							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""The VideoFrame Generated an EmptySVG, so the EmptyFrameDesmosGraphState has been returned."" }, ""ResponseContent"" : { ""ExpressionCount"" : ""0"", ""DesmosState"" : " & (New List(Of Library.IDesmosExpression)()).ToArray().ToDesmosState(_DesmosGraphOptions) & " } }")
						End Try

						REM If we're here, then there WASN'T an Exception whilst Generating the SVG; get the Beziers thereof...
						Dim _Beziers As Global.GraphPictures.Library.BezierObjects.IDesmosExpression() _
						 = Global.GraphPictures.Library.BezierResources.GetBeziersFromPotraceSVG(Global.GraphPictures.WebParts.WWWVideosFolderPath & _Request.QueryString("VideoName") & "_Frames\SVGs\" & _SVGFileName)

						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""SVGGenerationResultDataBase64"" : """ & _SVGGenResData.ToString().ToASCIIBase64() & """, ""ExpressionCount"" : """ & _Beziers.Length.ToString() & """, ""DesmosState"" : " & _Beziers.ToDesmosState(_DesmosGraphOptions).Replace("\", "\\") & " } }")

					Catch _Ex As Exception When True
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The following Exception was thrown upon attempting to Generate an SVG for a VideoFrame: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
					End Try

				Case "PointPlottedDesmosStateForFrame"

						REM QueryStrings:	[VideoName], [FrameNumber]
						REM Returns:		[DesmosState], [ExpressionCount]

						Try
							_Request.CheckRequiredQueryStringsAreSpecified(_Response, "VideoName", "FrameNumber")

							Dim _Points As Library.BezierObjects.IDesmosExpression() = (New GraphPictures.Library.PointPlottingObjects.PointPlottedRasterGraphic( _
							  _RasterGraphicFilePath:=WWWVideosFolderPath & _Request.QueryString("VideoName") & "_Frames\Frame" & _Request.QueryString!FrameNumber & ".BMP") _
							  ).GetPoints()

							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""DesmosState"" : " & _Points.ToDesmosState(New Library.Resources.DesmosStateCreationOptions() With {.ShowGrid = False, .ColourAllocationMode = GraphPictures.Library.ColourRange.ColourGenerationMode.UseExpressionColour}).Replace("\", "\\") & ", ""ExpressionCount"" : """ & _Points.Length.ToString() & """ } }")

						Catch _Ex As Exception When True
							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An error occoured whilst Generating a Desmos JSON State for the RasterGraphic using Plotting. The Error was: " & _Ex.Message & ". The Specified ?RasterGraphicFileName (" & _Request.QueryString("RasterGraphicFileName").ToString() & ") may not exist."" }, ""ResponseContent"" : {} }")
						End Try

				Case "SaveDesmosFrame"

						REM QueryStrings:	[VideoName], [FrameNumber]
						REM PostData:		[DesmosFrameBase64PNG]
						REM Returns:		(Just the MetaData E.g. IsValidRequest etc...)

						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "VideoName", "FrameNumber")
						_Request.CheckRequiredPostDataKeysAreSpecified(_Response, "DesmosFrameBase64PNG")

						Try
							If Not IO.Directory.Exists(WWWVideosFolderPath & _Request.QueryString("VideoName").ToString() & "_Frames\DesmosFrames\") Then IO.Directory.CreateDirectory(WWWVideosFolderPath & _Request.QueryString("VideoName").ToString() & "_Frames\DesmosFrames\")

							Global.GraphPictures.Library.ImagingResources.SaveBase64PNGToDisk(
							   _Base64PNG:=_Request.Form.Item("DesmosFrameBase64PNG"),
							   _OutputPNGPath:=(WWWVideosFolderPath & _Request.QueryString("VideoName") & "_Frames\DesmosFrames\Frame" & _Request.QueryString("FrameNumber").ToString() & ".PNG")
							)
							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : {} }")

						Catch _Ex As Exception
							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The following Exception was thrown upon attempting to Save a DesmosFrame: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
						End Try

				Case "DesmosFramesToVideo"

						REM QueryStrings:	[VideoName]
						REM Returns:		[FinalOutputHref]

						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "VideoName")

						Try
							If Not IO.Directory.Exists(WWWVideosFolderPath & _Request.QueryString!VideoName.ToString() & "_Frames\DesmosFrames\") Then Throw (New Library.[GraphPicturesConversionException]("The specified VideoFile does not exist, or has not had any DesmosFrames generated for it yet."))

							REM We should have all the DesmosFrames in \ClientData\Videos\{VideoName}_Frames\DesmosFrames\Frame00001.PNG
							GraphPictures.WebParts.Resources.GenerateVideoFromDesmosFrames(_Request.QueryString!VideoName.ToString())

							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""FinalOutputHref"" : ""/ClientPages/VideoTranscoding/ViewVideoFile.ASPX?VideoFileName=DesmosRendered_" & _Request.QueryString("VideoName").ToString() & """ } }")

						Catch _Ex As Exception
							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The following Exception was thrown upon attempting to Render DesmosFrames into a Video for """ & _Request.QueryString!VideoName.ToString() & """: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
						End Try

				Case "GenerateAudioVersionOfDesmosVideo"

						REM QueryStrings:	[VideoName]
						REM Returns:		[FinalOutputWithAudioHref]

						'The ?VideoName should just be Input.MP4, and not like DesmosRendered_Input.MP4
						'The _DesmosRendered part is removed in JavaScript

						_Request.CheckRequiredQueryStringsAreSpecified(_Response, "VideoName")

						Try
							'The existance of the DesmosRendered_*, and *, are bother checked for by the GenerateVersionOfVideoWithAudio() Sub in WebParts.

							GraphPictures.WebParts.Resources.GenerateVersionOfVideoWithAudio(_Request.QueryString!VideoName.ToString())

							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""FinalOutputWithAudioHref"" : ""/ClientPages/VideoTranscoding/ViewVideoFile.ASPX?VideoFileName=WithAudio_DesmosRendered_" & _Request.QueryString("VideoName").ToString() & """ } }")

						Catch _Ex As Exception
							_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The following Exception was thrown upon attempting to Apply and AudioTrack to a DesmosVideo for """ & _Request.QueryString!VideoName.ToString() & """: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
						End Try

				Case Else
						REM An unmatched ?Action was specified...
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An unrecognised ?Action was specified."" }, ""ResponseContent"" : {} }")
			End Select
			Return
		End Sub

#End Region

#Region "Upload.ASPX"

		Public Sub UploadASPX(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			_Response.ContentType = "application/json"

			'We need a ?Item, to know what they want to "Upload"...
			_Request.CheckRequiredQueryStringsAreSpecified(_Response, "Item")

			Select Case _Request.QueryString!Item

				Case "Base64RasterGraphic"

					REM QueryStrings:	{None}
					REM PostData:		[Base64PNG]
					REM Returns:		[RasterGraphicFileName]

					Try
						_Request.CheckRequiredPostDataKeysAreSpecified(_Response, "Base64PNG")

						Dim _PNGFileToBeSaved_Name$ = "APIUpload_" & Library.Resources.GetRandomString(10) & ".PNG"
						Dim _PNGFileToBeSaved_Path$ = WWWUploadedRasterImagesFolderPath & _PNGFileToBeSaved_Name

						Library.ImagingResources.SaveBase64PNGToDisk(_Request.Form!Base64PNG, _PNGFileToBeSaved_Path)

						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""RasterGraphicFileName"" : """ & _PNGFileToBeSaved_Name & """ } }")

					Catch _Ex As [Exception] When True
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An error occoured whilst Saving the Base64PNG to Disk. The Error was: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
					End Try

				Case "POSTUploadRasterGraphic"

					REM QueryStrings:	{None}
					REM PostData:		
					REM Returns:		[RasterGraphicFileName]

					Try
						'_Request.CheckRequiredPostDataKeysAreSpecified(_Response, "Base64PNG")

						

						'_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""RasterGraphicFileName"" : """ & _PNGFileToBeSaved_Name & """ } }")

					Catch _Ex As [Exception] When True
						_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An error occoured whilst Saving the RasterGraphic to Disk. The Error was: " & _Ex.Message & "."" }, ""ResponseContent"" : {} }")
					End Try

				Case Else
					REM An unmatched ?Item was specified...
					_Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""An unrecognised ?Item was specified."" }, ""ResponseContent"" : {} }")
			End Select

		End Sub

#End Region

	End Module

End Namespace