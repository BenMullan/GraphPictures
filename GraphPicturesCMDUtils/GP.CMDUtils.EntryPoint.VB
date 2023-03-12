Imports GraphPictures.Library.CompilerExtentions

Namespace CMDUtils

	Module EntryPoint

		Public Sub Main()

			Console.ForegroundColor = ConsoleColor.Blue
			Console.BackgroundColor = ConsoleColor.Gray
			Console.Title = "GraphPictures\CMDUtils"
			Console.WriteLine("Running GraphPictures Command-Line Utilities EntryPoint...")

			'Dim _MTParser As New Library.MarkedTextParser("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\GraphPictures\GraphPicturesWebParts\ClientPages\Resources\PageTemplate.MT")

			'Dim _PNGBytes As Byte() = Convert.FromBase64String(_Base64PNG.Substring(22))
			'My.Computer.FileSystem.WriteAllBytes("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\GraphPictures\GraphPicturesWebParts\ClientData\Videos\MrClark.PNG", _PNGBytes, False)
			'MsgBox("Done...")

			'Dim _ExpressionColour As Drawing.Color = Drawing.Color.MistyRose
			'MsgBox(String.Format("#{0:X2}{1:X2}{2:X2}", _ExpressionColour.R, _ExpressionColour.G, _ExpressionColour.B))




			'Dim _NOrderBezier As New GraphPictures.Library.BezierObjects.NOrderBezier(
			' {12, 9}, {-20, 4}, {9, -16}, {4, 14}, {20, -30}, {9, -16}, {-7, 4}
			')

			'Dim _Latex$ = _NOrderBezier.ToLatexExpression().LatexToMathsExpression()

			'My.Computer.Clipboard.SetText(_Latex)
			'MsgBox("Copied to Clipboard. Bezier Order Description:" & vbCrLf & vbCrLf & _NOrderBezier.BezierOrderDescription)




			'Global.GraphPictures.Library.ExcelResources.PlotBitmapOnExcelByRGB(New Drawing.Bitmap(
			' "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\ExtraSmall_MrClark.bmp"
			'), True)

			'My.Computer.Clipboard.SetText(GraphPictures.Library.CompilerExtentions.LatexToMathsExpression("\left(\left((1-t)^{3}\right)\cdot(2014)\right)+\left(3\cdot t\cdot\left((1-t)^{2}\right)(1079)\right)+\left(3\cdot\left(t^{2}\right)\cdot(1-t)\cdot(247)\right)+\left(\left(t^{3}\right)\cdot(163)\right)"))



			'My.Computer.Clipboard.SetText( _
			' GraphPictures.Library.OscilloscopeResources.GetPDEVectorShapeInstructionsFromBeziers( _
			' Library.EntryPoints.GetBeziersFromPotraceSVG( _
			'   "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\GraphPictures\GraphPicturesWebParts\ClientData\SVGFiles\MrClark.SVG"
			'   )
			' )
			')

			'HST		Härmann-Strauß Technologisch
			'GraphPictures.Library.OscilloscopeResources.GenerateAndRunPDEFileFromSVG(
			' "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\FromBravoSession0\OscilloscopeData\Images\Output.SVG"
			')
			'		Threading.Thread.Sleep(8000)

			'		Dim _ProcessingProcess As Diagnostics.Process = Process.GetProcessesByName("processing-java")(0)
			'		_ProcessingProcess.Kill()



			'GraphPictures.Library.OscilloscopeResources.RecordAndSaveSystemLineInAudio( _	
			' "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\RecordedLineInAudio\1.Wav", _
			' 10 _
			')



			'GraphPictures.Library.ImagingResources.ApplyTransformationToSavedImage("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\FromBravoSession0\GraphPicturesWebParts\ClientData\UploadedRasterImages\XHF7YMS11G.PNG")

			'Library.OscilloscopeResources.GenerateOscilloscopeVideoFramePDEsFor("FadeToGray_Face_Cropped.MP4")
			'GraphPictures.Library.OscilloscopeResources.SequentiallyRunVideoFramePDEFiles("FadeToGray_Face_Cropped.MP4")




			'Dim _SVGExpr$ = String.Empty
			'For Each _Bezier As Library.IDesmosExpression In Library.GetBeziersFromPotraceSVG("C:\inetpub\wwwroot\ClientData\SVGFiles\HandShadow.SVG")
			'	_SVGExpr &= _Bezier.ToLatexExpression().LatexToMathsExpression()
			'Next

			'My.Computer.Clipboard.SetText(_SVGExpr, Windows.Forms.TextDataFormat.Text)




			'Dim _Bitmap As New Drawing.Bitmap("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\FromBravoSession0\GraphPicturesWebParts\ClientData\UploadedRasterImages\ZoomTests\GLO.PNG")

			'For _ZoomFactor@ = 1D To 0.05D Step -0.05@
			'	_Bitmap.GetZoomedInCopy(_ZoomFactor).Save("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\FromBravoSession0\GraphPicturesWebParts\ClientData\UploadedRasterImages\ZoomTests\GLO_" & _ZoomFactor.ToString() & ".PNG", Drawing.Imaging.ImageFormat.Png)
			'Next





			'Dim _AttemptCount% = GraphPictures.Library.ImagingResources.GenerateSVG(
			'  New IO.FileInfo("D:\Benedict\Pictures\My Pictures\MullNetRemoteDesktopServices_PersonalVirtualDesktops.PNG"),
			'  "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\Output SVGs\GenerateSVGOutput.SVG",
			'  "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\BMPConvert\",
			'  "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\FromBravoSession0\GraphPicturesWebParts\ClientData\Potrace.EXE"
			')

			'MsgBox("Input was cropped this many times to generate that SVG: " & _AttemptCount.ToString())










			'Dim _FinalName$ = String.Empty : _FinalName$ = GraphPictures.Library.ImagingResources.GenerateSVGInFolder(
			'  New IO.FileInfo("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\Blank.BMP"),
			'  New IO.DirectoryInfo("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\SVGOut\"),
			'  "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\BMPConvert\",
			'  "D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\FromBravoSession0\GraphPicturesWebParts\ClientData\Potrace.EXE"
			')


			'MsgBox("Final Name was: " & _FinalName)







			EmptyDesmosStateTests()



			




			Console.ForegroundColor = ConsoleColor.Green
			Console.Write(vbCrLf & "Done...")
			Console.ReadLine()

		End Sub

		Sub EmptyDesmosStateTests()
			Dim _Exprs As New List(Of GraphPictures.Library.IDesmosExpression)()
			MsgBox(_Exprs.ToArray().ToDesmosState(New Library.Resources.DesmosStateCreationOptions()))
		End Sub

		Sub SVGGenerationTests()
			Dim _SVGGenerationOptions As New Library.ImagingResources.SVGGenerator.SVGGenerationOptions() With {
			 .BitmapConversionFolder = New IO.DirectoryInfo("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\BMPConvert\"),
			 .PotraceEXE = New IO.FileInfo("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\Solution\FromBravoSession0\GraphPicturesWebParts\ClientData\Potrace.EXE"),
			 .SVGSizeModerationMode = Library.SVGGenerator.SVGSizeModerationAlgorythm.AdjustInputQuality,
			 .TargetOrMaximun_SVGFileSizeKB = 40
			}

			Dim _SVGGenerator As New Library.ImagingResources.SVGGenerator(
			 New IO.FileInfo("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\Image (3).jpg"),
			 _SVGGenerationOptions
			)

			'_SVGGenerator.GenerateSVG("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\SVGOut\NewNewAlgoOutput.SVG")
			_SVGGenerator.GenerateSVGInFolder(New IO.DirectoryInfo("D:\Benedict\Documents\SchoolWork\Projects\GraphPictures\TestImages\SVGOut\"))

			Console.WriteLine(_SVGGenerator.SVGGenerationResults.ToString())
		End Sub

	End Module

End Namespace