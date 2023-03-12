Imports GraphPictures.Library.CompilerExtentions

Public Class Home

	Public InputRasterGraphic As Drawing.Bitmap = Nothing
	Public Event IRGChosen(ByVal _IRG As IO.FileInfo)

	Protected Sub HandleFileDragOver(ByVal _Sender As Object, ByVal _E As DragEventArgs) Handles Me.DragOver
		If _E.Data.GetDataPresent(DataFormats.FileDrop) Then
			_E.Effect = DragDropEffects.Link
		Else
			_E.Effect = DragDropEffects.None
		End If
	End Sub

	Protected Sub HandleFileDragDrop(ByVal _Sender As Object, ByVal _E As DragEventArgs) Handles Me.DragOver
		Dim _Files$() = TryCast(_E.Data.GetData(DataFormats.FileDrop), String()) 'Get all Files Dropped
		If (Not (_Files Is Nothing)) AndAlso (_Files.Any()) Then Me.IRGTextBox.Text = _Files.First()
		RaiseEvent IRGChosen(New IO.FileInfo(_Files.First()))
	End Sub

	Public Sub PickIRGWithFileDialog() Handles BrowseForFileButton.Click

		Using _OpenFileDialog As New OpenFileDialog() With {.Filter = "GraphPictures RasterGraphics (*.BMP;*.JPG;*.GIF;*.JPEG;*.JFIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.JPEG;*.JFIF;*.PNG|All files (*.*)|*.*"}
			If (_OpenFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) AndAlso (Not String.IsNullOrEmpty(_OpenFileDialog.FileName)) Then
				Me.IRGTextBox.Text = _OpenFileDialog.FileName
				RaiseEvent IRGChosen(New IO.FileInfo(_OpenFileDialog.FileName))
			Else
				'MsgBox("The Chosen File was not valid")
			End If
		End Using

	End Sub

	Public Sub ShowIRGPreview(ByVal _IRG As IO.FileInfo) Handles Me.IRGChosen
		Try
			If Not {".BMP", ".JPEG", ".JPG", ".JFIF", ".PNG", ".GIF"}.Contains(_IRG.Extension.ToUpper()) Then Throw New Exception("The Specified Input RasterGraphic did not have a valid FileExtention. It was: " & _IRG.Extension)

			Me.InputRasterGraphic = (New Drawing.Bitmap(_IRG.FullName))
			Me.IRGPictureBox.Image = Me.InputRasterGraphic
			Me.IRGDimensionsLabel.Text = (Me.InputRasterGraphic.Width.ToString() & " x " & Me.InputRasterGraphic.Height.ToString()) : Me.IRGDimensionsLabel.Visible = True
			Me.DragFileHereLabel.Hide()

			Me.RenderProgressBar.Style = ProgressBarStyle.Continuous
			Me.RenderProgressBar.Value = 0

			Me.RenderProgressLabel.Text = "Render Progress (0.0%)"
			Me.RenderProgressLabel.ForeColor = Windows.Forms.Control.DefaultForeColor
			Me.RenderProgressLabel.Font = New Font("Segoe UI", 9, FontStyle.Regular)

			Me.AcceptButton = Me.RenderOnExcelButton
			Me.Activate()

		Catch _Ex As Exception
			MsgBox(_Ex.Message, MsgBoxStyle.Critical, "Whilst Showing Preview...")
		End Try
	End Sub

	Public Sub RenderIRGOnExcel() Handles RenderOnExcelButton.Click

		Try
			If (Me.InputRasterGraphic Is Nothing) Then Throw New Exception("The Input RasterGraphic was not Defined. One must be dragged onto the Form or chosen by the OpenFileDialog")

			Me.RenderProgressBar.Style = ProgressBarStyle.Marquee
			Me.TheTabControl.Enabled = False

			Me.ShowExcel_ = Me.ShowExcelCheckBox.Checked
			Me.ExcelIRGMaxDimension_ = If(Me.EnforceMaxDimensionCheckBox.Checked, CUInt(Me.MaxDimensionSelector.Value), CUInt(10000))

			Dim _ExcelRendererThread As New Threading.Thread(New Threading.ThreadStart(Sub() RenderIRGOnExcel_ThreadStart(Me.RGBModeCheckBox.Checked)))
			_ExcelRendererThread.Start()

		Catch _Ex As Exception
			MsgBox(_Ex.Message, MsgBoxStyle.Critical, "Whilst Starting ExcelRendering Thread...")
		End Try

	End Sub

#Region "For ExcelRenderer Thread"

	Private ExcelRenderedBitmap_ As Bitmap
	Private ShowExcel_ As Boolean = False
	Private ExcelIRGMaxDimension_ As UInt32

	Public Sub RenderIRGOnExcel_ThreadStart(ByVal _UseRGBMode As Boolean)

		Try
			AddHandler Library.ExcelResources.PixelCopyComplete, AddressOf UpdateRenderProgress

			Me.InvokeIfRequired(
			 Sub()
				 Me.RenderProgressBar.Style = ProgressBarStyle.Continuous
				 Me.RenderProgressBar.Value = 0

				 Me.RenderProgressLabel.Text = "Render Progress (0.0%)"
				 Me.RenderProgressLabel.ForeColor = Windows.Forms.Control.DefaultForeColor
				 Me.RenderProgressLabel.Font = New Font("Segoe UI", 9, FontStyle.Regular)
			 End Sub
			)

			If _UseRGBMode Then
				ExcelRenderedBitmap_ = GraphPictures.Library.ExcelResources.PlotBitmapOnExcelByRGB(Me.InputRasterGraphic, Me.ShowExcel_, Me.ExcelIRGMaxDimension_)
			Else
				ExcelRenderedBitmap_ = GraphPictures.Library.ExcelResources.PlotBitmapOnExcel(Me.InputRasterGraphic, Me.ShowExcel_, Me.ExcelIRGMaxDimension_)
			End If

			Me.InvokeIfRequired(Sub() Me.ExcelOutputPictureBox.Image = ExcelRenderedBitmap_)

			Me.InvokeIfRequired(
			 Sub()
				 Me.RenderProgressLabel.Text = "Render Progress (100.0%)"
				 Me.RenderProgressLabel.ForeColor = Color.Green
				 Me.RenderProgressLabel.Font = New Font("Segoe UI", 9, FontStyle.Bold)

				 Me.RenderProgressBar.Value = 100
				 Me.TheTabControl.Enabled = True
				 Me.Activate()
			 End Sub
			)

		Catch _Ex As Exception
			MsgBox(_Ex.Message, MsgBoxStyle.Critical, "Whilst Rendering on Excel...")
		End Try

	End Sub

	Public Sub UpdateRenderProgress(ByVal _XPixelCopied%, ByVal _YPixelCopied%)	'Handler Added in RenderiRGOnExcel()

		'Dim _XProgressPercentage% = CInt((_XPixelCopied / _IRG.Width) * 100)
		'Dim _YProgressPercentage% = CInt((_YPixelCopied / _IRG.Height) * 100)

		'MsgBox("IRG Width: " & _IRG.Width.ToString() & vbCrLf & "IRG Height: " & _IRG.Height.ToString() & vbCrLf & "_XPixelCopied: " & _XPixelCopied.ToString() & vbCrLf & "_YPixelCopied: " & _YPixelCopied.ToString() & vbCrLf & vbCrLf & "XProgress: " & _XProgressPercentage.ToString() & vbCrLf & "YProgress: " & _YProgressPercentage.ToString())

		Me.InvokeIfRequired(
		 Sub()
			 'MsgBox("YPart: " & (CInt((_YPixelCopied / Me.InputRasterGraphic.Height) * 100)).ToString() & vbCrLf & "YPixelCopied: " & _YPixelCopied & vbCrLf & "Me.InputRasterGraphic.Height: " & Me.InputRasterGraphic.Height.ToString() & vbCrLf & vbCrLf & "XPart: " & (CInt((_XPixelCopied / Me.InputRasterGraphic.Width) * 100)).ToString() & vbCrLf & "XPixelCopied: " & _XPixelCopied & vbCrLf & "Me.InputRasterGraphic.Width: " & Me.InputRasterGraphic.Width.ToString())

			 Dim _EnforcedMaxDimension As UInt32 = If(Me.EnforceMaxDimensionCheckBox.Checked, CUInt(Me.MaxDimensionSelector.Value), CUInt(10000))
			 Dim _YPercent% = CInt((_YPixelCopied / Me.InputRasterGraphic.GetConstrictedSize(_EnforcedMaxDimension).Height) * 100)
			 Dim _XPercent% = CInt((_XPixelCopied / Me.InputRasterGraphic.GetConstrictedSize(_EnforcedMaxDimension).Width) * 100)

			 Me.RenderProgressLabel.Text = String.Format("Render Progress ({0}.{1}%)", _YPercent.ToString(), _XPercent.ToString())
			 Me.RenderProgressBar.Value = _YPercent

		 End Sub
		)

	End Sub

#End Region

	Private Sub EnforceMaxDimensionCheckBox_CheckedChanged() Handles EnforceMaxDimensionCheckBox.CheckedChanged
		Me.MaxDimensionSelector.Enabled = Me.EnforceMaxDimensionCheckBox.Checked
	End Sub

	Public Shared Sub CloseExelInstances() Handles CloseExcelsLinkLabel.LinkClicked
		For Each _Process As Process In Process.GetProcesses()
			If _Process.ProcessName = "EXCEL" Then _Process.Kill()
		Next
	End Sub

	Public Sub NOrderBezierButton_Click() Handles NOrderBezierButton.Click

		Dim _Bezier As New GraphPictures.Library.BezierObjects.NOrderBezier( _
		 {12, 14}, {56, 6}, {-14, 78}, {43, 76}, {65, 54}, {43, 87}, {90, 76}
		)

		Dim _BezierExpression$ = _Bezier.ToLatexExpression()

		My.Computer.Clipboard.SetText(_BezierExpression, TextDataFormat.Text)
		MsgBox("Final Expression:" & vbCrLf & vbCrLf & vbCrLf & _BezierExpression.LatexToMathsExpression())

	End Sub

	Public Sub CopyDraggableBezierExprs() Handles CopyDraggableBezierButton.Click

		Dim _Bezier As New GraphPictures.Library.BezierObjects.NOrderBezier( _
		 {12, 14}, {23, -6}, {4, -13}, {34, 0}, {6, 13}, {45, -45}, {32, -4}
		)

		My.Computer.Clipboard.SetText(_Bezier.GetExpressionsForGraphWithDragablePoints().ToDesmosPastable())
		MsgBox("Copied to Clipboard for Bezier of Order: " & _Bezier.BezierOrderDescription)

	End Sub

	Sub CopyBezWithSubBezs() Handles CopyBezWithSubBezsButton.Click

		Dim _Bezier As New GraphPictures.Library.BezierObjects.NOrderBezier( _
		 {0, 10}, {40, -10}, {-30, -30}, {30, -40}, {90, -30}, {20, -10}, {60, 10}
		)

		My.Computer.Clipboard.SetText(_Bezier.GetExpressionsForGraphWithDragablePointsAndShowAllSubBeziers().ToDesmosPastable())
		MsgBox("Copied Bezier and Construction SubBeziers to Clipboard for Curve of Order: " & _Bezier.BezierOrderDescription, MsgBoxStyle.Information, "PH, PH")

	End Sub

End Class