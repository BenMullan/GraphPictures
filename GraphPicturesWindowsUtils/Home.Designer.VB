<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Home
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Home))
		Me.TheTabControl = New System.Windows.Forms.TabControl()
		Me.ExcelTabPage = New System.Windows.Forms.TabPage()
		Me.CloseExcelsLinkLabel = New System.Windows.Forms.LinkLabel()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.MaxDimensionSelector = New System.Windows.Forms.NumericUpDown()
		Me.ShowExcelCheckBox = New System.Windows.Forms.CheckBox()
		Me.EnforceMaxDimensionCheckBox = New System.Windows.Forms.CheckBox()
		Me.RGBModeCheckBox = New System.Windows.Forms.CheckBox()
		Me.IRGDimensionsLabel = New System.Windows.Forms.Label()
		Me.DragFileHereLabel = New System.Windows.Forms.Label()
		Me.ExcelOutputPictureBox = New System.Windows.Forms.PictureBox()
		Me.IRGPictureBox = New System.Windows.Forms.PictureBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.IRGTextBox = New System.Windows.Forms.TextBox()
		Me.BrowseForFileButton = New System.Windows.Forms.Button()
		Me.RenderOnExcelButton = New System.Windows.Forms.Button()
		Me.BezUtilsTabPage = New System.Windows.Forms.TabPage()
		Me.DesmosLinkLabel = New System.Windows.Forms.LinkLabel()
		Me.DontUseLatex_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.BezierPointsTextBox = New System.Windows.Forms.TextBox()
		Me.DesmosPastable_DraggableBezierWithSubExpressions_Button = New System.Windows.Forms.Button()
		Me.DesmosPastable_DraggableBezier_Button = New System.Windows.Forms.Button()
		Me.DesmosPastable_SingleBezier_Button = New System.Windows.Forms.Button()
		Me.CopyBezWithSubBezsButton = New System.Windows.Forms.Button()
		Me.CopyDraggableBezierButton = New System.Windows.Forms.Button()
		Me.NOrderBezierButton = New System.Windows.Forms.Button()
		Me.TheStatusStrip = New System.Windows.Forms.StatusStrip()
		Me.RenderProgressLabel = New System.Windows.Forms.ToolStripStatusLabel()
		Me.RenderProgressBar = New System.Windows.Forms.ToolStripProgressBar()
		Me.TheTabControl.SuspendLayout()
		Me.ExcelTabPage.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		CType(Me.MaxDimensionSelector, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ExcelOutputPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.IRGPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.BezUtilsTabPage.SuspendLayout()
		Me.TheStatusStrip.SuspendLayout()
		Me.SuspendLayout()
		'
		'TheTabControl
		'
		Me.TheTabControl.Controls.Add(Me.ExcelTabPage)
		Me.TheTabControl.Controls.Add(Me.BezUtilsTabPage)
		Me.TheTabControl.Location = New System.Drawing.Point(12, 12)
		Me.TheTabControl.Name = "TheTabControl"
		Me.TheTabControl.SelectedIndex = 0
		Me.TheTabControl.Size = New System.Drawing.Size(600, 413)
		Me.TheTabControl.TabIndex = 0
		'
		'ExcelTabPage
		'
		Me.ExcelTabPage.Controls.Add(Me.CloseExcelsLinkLabel)
		Me.ExcelTabPage.Controls.Add(Me.GroupBox1)
		Me.ExcelTabPage.Controls.Add(Me.IRGDimensionsLabel)
		Me.ExcelTabPage.Controls.Add(Me.DragFileHereLabel)
		Me.ExcelTabPage.Controls.Add(Me.ExcelOutputPictureBox)
		Me.ExcelTabPage.Controls.Add(Me.IRGPictureBox)
		Me.ExcelTabPage.Controls.Add(Me.Label3)
		Me.ExcelTabPage.Controls.Add(Me.Label2)
		Me.ExcelTabPage.Controls.Add(Me.Label1)
		Me.ExcelTabPage.Controls.Add(Me.IRGTextBox)
		Me.ExcelTabPage.Controls.Add(Me.BrowseForFileButton)
		Me.ExcelTabPage.Controls.Add(Me.RenderOnExcelButton)
		Me.ExcelTabPage.Location = New System.Drawing.Point(4, 22)
		Me.ExcelTabPage.Name = "ExcelTabPage"
		Me.ExcelTabPage.Padding = New System.Windows.Forms.Padding(3)
		Me.ExcelTabPage.Size = New System.Drawing.Size(592, 387)
		Me.ExcelTabPage.TabIndex = 0
		Me.ExcelTabPage.Text = "Excel"
		Me.ExcelTabPage.UseVisualStyleBackColor = True
		'
		'CloseExcelsLinkLabel
		'
		Me.CloseExcelsLinkLabel.AutoSize = True
		Me.CloseExcelsLinkLabel.Location = New System.Drawing.Point(475, 368)
		Me.CloseExcelsLinkLabel.Name = "CloseExcelsLinkLabel"
		Me.CloseExcelsLinkLabel.Size = New System.Drawing.Size(111, 13)
		Me.CloseExcelsLinkLabel.TabIndex = 6
		Me.CloseExcelsLinkLabel.TabStop = True
		Me.CloseExcelsLinkLabel.Text = "Close Excel Instances"
		'
		'GroupBox1
		'
		Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox1.Controls.Add(Me.Label4)
		Me.GroupBox1.Controls.Add(Me.MaxDimensionSelector)
		Me.GroupBox1.Controls.Add(Me.ShowExcelCheckBox)
		Me.GroupBox1.Controls.Add(Me.EnforceMaxDimensionCheckBox)
		Me.GroupBox1.Controls.Add(Me.RGBModeCheckBox)
		Me.GroupBox1.Location = New System.Drawing.Point(24, 283)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(258, 98)
		Me.GroupBox1.TabIndex = 5
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Options"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(223, 66)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(18, 13)
		Me.Label4.TabIndex = 2
		Me.Label4.Text = "px"
		'
		'MaxDimensionSelector
		'
		Me.MaxDimensionSelector.Location = New System.Drawing.Point(173, 64)
		Me.MaxDimensionSelector.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
		Me.MaxDimensionSelector.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.MaxDimensionSelector.Name = "MaxDimensionSelector"
		Me.MaxDimensionSelector.Size = New System.Drawing.Size(50, 20)
		Me.MaxDimensionSelector.TabIndex = 6
		Me.MaxDimensionSelector.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'ShowExcelCheckBox
		'
		Me.ShowExcelCheckBox.AutoSize = True
		Me.ShowExcelCheckBox.Checked = True
		Me.ShowExcelCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ShowExcelCheckBox.Location = New System.Drawing.Point(6, 19)
		Me.ShowExcelCheckBox.Name = "ShowExcelCheckBox"
		Me.ShowExcelCheckBox.Size = New System.Drawing.Size(194, 17)
		Me.ShowExcelCheckBox.TabIndex = 4
		Me.ShowExcelCheckBox.Text = "Show Excel Window during Render"
		Me.ShowExcelCheckBox.UseVisualStyleBackColor = True
		'
		'EnforceMaxDimensionCheckBox
		'
		Me.EnforceMaxDimensionCheckBox.AutoSize = True
		Me.EnforceMaxDimensionCheckBox.Checked = True
		Me.EnforceMaxDimensionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.EnforceMaxDimensionCheckBox.Location = New System.Drawing.Point(6, 65)
		Me.EnforceMaxDimensionCheckBox.Name = "EnforceMaxDimensionCheckBox"
		Me.EnforceMaxDimensionCheckBox.Size = New System.Drawing.Size(170, 17)
		Me.EnforceMaxDimensionCheckBox.TabIndex = 4
		Me.EnforceMaxDimensionCheckBox.Text = "Make Image's Max Dimension:"
		Me.EnforceMaxDimensionCheckBox.UseVisualStyleBackColor = True
		'
		'RGBModeCheckBox
		'
		Me.RGBModeCheckBox.AutoSize = True
		Me.RGBModeCheckBox.Location = New System.Drawing.Point(6, 42)
		Me.RGBModeCheckBox.Name = "RGBModeCheckBox"
		Me.RGBModeCheckBox.Size = New System.Drawing.Size(154, 17)
		Me.RGBModeCheckBox.TabIndex = 4
		Me.RGBModeCheckBox.Text = "Use 3 Cells (RGB) per Pixel"
		Me.RGBModeCheckBox.UseVisualStyleBackColor = True
		'
		'IRGDimensionsLabel
		'
		Me.IRGDimensionsLabel.AutoSize = True
		Me.IRGDimensionsLabel.BackColor = System.Drawing.SystemColors.Control
		Me.IRGDimensionsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.IRGDimensionsLabel.Location = New System.Drawing.Point(27, 257)
		Me.IRGDimensionsLabel.Name = "IRGDimensionsLabel"
		Me.IRGDimensionsLabel.Size = New System.Drawing.Size(39, 16)
		Me.IRGDimensionsLabel.TabIndex = 2
		Me.IRGDimensionsLabel.Text = "0 x 0"
		Me.IRGDimensionsLabel.Visible = False
		'
		'DragFileHereLabel
		'
		Me.DragFileHereLabel.AutoSize = True
		Me.DragFileHereLabel.BackColor = System.Drawing.SystemColors.Control
		Me.DragFileHereLabel.Location = New System.Drawing.Point(114, 164)
		Me.DragFileHereLabel.Name = "DragFileHereLabel"
		Me.DragFileHereLabel.Size = New System.Drawing.Size(84, 13)
		Me.DragFileHereLabel.TabIndex = 2
		Me.DragFileHereLabel.Text = "Drag File Here..."
		'
		'ExcelOutputPictureBox
		'
		Me.ExcelOutputPictureBox.BackColor = System.Drawing.SystemColors.Control
		Me.ExcelOutputPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.ExcelOutputPictureBox.Location = New System.Drawing.Point(307, 61)
		Me.ExcelOutputPictureBox.Name = "ExcelOutputPictureBox"
		Me.ExcelOutputPictureBox.Size = New System.Drawing.Size(258, 214)
		Me.ExcelOutputPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.ExcelOutputPictureBox.TabIndex = 3
		Me.ExcelOutputPictureBox.TabStop = False
		'
		'IRGPictureBox
		'
		Me.IRGPictureBox.BackColor = System.Drawing.SystemColors.Control
		Me.IRGPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.IRGPictureBox.Location = New System.Drawing.Point(24, 61)
		Me.IRGPictureBox.Name = "IRGPictureBox"
		Me.IRGPictureBox.Size = New System.Drawing.Size(258, 214)
		Me.IRGPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.IRGPictureBox.TabIndex = 3
		Me.IRGPictureBox.TabStop = False
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(383, 45)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(118, 13)
		Me.Label3.TabIndex = 2
		Me.Label3.Text = "ExcelRendering Result:"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(77, 45)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(146, 13)
		Me.Label2.TabIndex = 2
		Me.Label2.Text = "Input RasterGraphic Preview:"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(6, 11)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(105, 13)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Input RasterGraphic:"
		'
		'IRGTextBox
		'
		Me.IRGTextBox.Location = New System.Drawing.Point(117, 8)
		Me.IRGTextBox.Name = "IRGTextBox"
		Me.IRGTextBox.Size = New System.Drawing.Size(388, 20)
		Me.IRGTextBox.TabIndex = 1
		'
		'BrowseForFileButton
		'
		Me.BrowseForFileButton.Location = New System.Drawing.Point(511, 6)
		Me.BrowseForFileButton.Name = "BrowseForFileButton"
		Me.BrowseForFileButton.Size = New System.Drawing.Size(75, 23)
		Me.BrowseForFileButton.TabIndex = 0
		Me.BrowseForFileButton.Text = "Browse..."
		Me.BrowseForFileButton.UseVisualStyleBackColor = True
		'
		'RenderOnExcelButton
		'
		Me.RenderOnExcelButton.Location = New System.Drawing.Point(405, 321)
		Me.RenderOnExcelButton.Name = "RenderOnExcelButton"
		Me.RenderOnExcelButton.Size = New System.Drawing.Size(75, 23)
		Me.RenderOnExcelButton.TabIndex = 0
		Me.RenderOnExcelButton.Text = "&Render"
		Me.RenderOnExcelButton.UseVisualStyleBackColor = True
		'
		'BezUtilsTabPage
		'
		Me.BezUtilsTabPage.Controls.Add(Me.DesmosLinkLabel)
		Me.BezUtilsTabPage.Controls.Add(Me.DontUseLatex_CheckBox)
		Me.BezUtilsTabPage.Controls.Add(Me.Label6)
		Me.BezUtilsTabPage.Controls.Add(Me.Label5)
		Me.BezUtilsTabPage.Controls.Add(Me.BezierPointsTextBox)
		Me.BezUtilsTabPage.Controls.Add(Me.DesmosPastable_DraggableBezierWithSubExpressions_Button)
		Me.BezUtilsTabPage.Controls.Add(Me.DesmosPastable_DraggableBezier_Button)
		Me.BezUtilsTabPage.Controls.Add(Me.DesmosPastable_SingleBezier_Button)
		Me.BezUtilsTabPage.Controls.Add(Me.CopyBezWithSubBezsButton)
		Me.BezUtilsTabPage.Controls.Add(Me.CopyDraggableBezierButton)
		Me.BezUtilsTabPage.Controls.Add(Me.NOrderBezierButton)
		Me.BezUtilsTabPage.Location = New System.Drawing.Point(4, 22)
		Me.BezUtilsTabPage.Name = "BezUtilsTabPage"
		Me.BezUtilsTabPage.Padding = New System.Windows.Forms.Padding(3)
		Me.BezUtilsTabPage.Size = New System.Drawing.Size(592, 387)
		Me.BezUtilsTabPage.TabIndex = 2
		Me.BezUtilsTabPage.Text = "Bezier Utilities"
		Me.BezUtilsTabPage.UseVisualStyleBackColor = True
		'
		'DesmosLinkLabel
		'
		Me.DesmosLinkLabel.AutoSize = True
		Me.DesmosLinkLabel.Location = New System.Drawing.Point(502, 368)
		Me.DesmosLinkLabel.Name = "DesmosLinkLabel"
		Me.DesmosLinkLabel.Size = New System.Drawing.Size(83, 13)
		Me.DesmosLinkLabel.TabIndex = 6
		Me.DesmosLinkLabel.TabStop = True
		Me.DesmosLinkLabel.Text = "Open Desmos..."
		'
		'DontUseLatex_CheckBox
		'
		Me.DontUseLatex_CheckBox.AutoSize = True
		Me.DontUseLatex_CheckBox.ForeColor = System.Drawing.SystemColors.ControlDarkDark
		Me.DontUseLatex_CheckBox.Location = New System.Drawing.Point(7, 365)
		Me.DontUseLatex_CheckBox.Name = "DontUseLatex_CheckBox"
		Me.DontUseLatex_CheckBox.Size = New System.Drawing.Size(275, 17)
		Me.DontUseLatex_CheckBox.TabIndex = 5
		Me.DontUseLatex_CheckBox.Text = "Make expression human-readable (instead of LaTeX)"
		Me.DontUseLatex_CheckBox.UseVisualStyleBackColor = True
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark
		Me.Label6.Location = New System.Drawing.Point(16, 61)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(137, 13)
		Me.Label6.TabIndex = 4
		Me.Label6.Text = "Copy desmos-pastable for..."
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(16, 14)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(103, 13)
		Me.Label5.TabIndex = 4
		Me.Label5.Text = "Enter bezier points..."
		'
		'BezierPointsTextBox
		'
		Me.BezierPointsTextBox.Location = New System.Drawing.Point(15, 29)
		Me.BezierPointsTextBox.Name = "BezierPointsTextBox"
		Me.BezierPointsTextBox.Size = New System.Drawing.Size(559, 20)
		Me.BezierPointsTextBox.TabIndex = 3
		Me.BezierPointsTextBox.Text = "{12, 14} ~ {56, 6} ~ {-14, 78} ~ {43, 76} ~ {65, 54} ~ {43, 87}"
		'
		'DesmosPastable_DraggableBezierWithSubExpressions_Button
		'
		Me.DesmosPastable_DraggableBezierWithSubExpressions_Button.Location = New System.Drawing.Point(398, 56)
		Me.DesmosPastable_DraggableBezierWithSubExpressions_Button.Name = "DesmosPastable_DraggableBezierWithSubExpressions_Button"
		Me.DesmosPastable_DraggableBezierWithSubExpressions_Button.Size = New System.Drawing.Size(176, 23)
		Me.DesmosPastable_DraggableBezierWithSubExpressions_Button.TabIndex = 2
		Me.DesmosPastable_DraggableBezierWithSubExpressions_Button.Text = "D&raggable Bezier && sub-exprs..."
		Me.DesmosPastable_DraggableBezierWithSubExpressions_Button.UseVisualStyleBackColor = True
		'
		'DesmosPastable_DraggableBezier_Button
		'
		Me.DesmosPastable_DraggableBezier_Button.Location = New System.Drawing.Point(277, 56)
		Me.DesmosPastable_DraggableBezier_Button.Name = "DesmosPastable_DraggableBezier_Button"
		Me.DesmosPastable_DraggableBezier_Button.Size = New System.Drawing.Size(115, 23)
		Me.DesmosPastable_DraggableBezier_Button.TabIndex = 2
		Me.DesmosPastable_DraggableBezier_Button.Text = "&Draggable Bezier..."
		Me.DesmosPastable_DraggableBezier_Button.UseVisualStyleBackColor = True
		'
		'DesmosPastable_SingleBezier_Button
		'
		Me.DesmosPastable_SingleBezier_Button.Location = New System.Drawing.Point(156, 56)
		Me.DesmosPastable_SingleBezier_Button.Name = "DesmosPastable_SingleBezier_Button"
		Me.DesmosPastable_SingleBezier_Button.Size = New System.Drawing.Size(115, 23)
		Me.DesmosPastable_SingleBezier_Button.TabIndex = 2
		Me.DesmosPastable_SingleBezier_Button.Text = "&Single Bezier..."
		Me.DesmosPastable_SingleBezier_Button.UseVisualStyleBackColor = True
		'
		'CopyBezWithSubBezsButton
		'
		Me.CopyBezWithSubBezsButton.Enabled = False
		Me.CopyBezWithSubBezsButton.Location = New System.Drawing.Point(6, 314)
		Me.CopyBezWithSubBezsButton.Name = "CopyBezWithSubBezsButton"
		Me.CopyBezWithSubBezsButton.Size = New System.Drawing.Size(180, 23)
		Me.CopyBezWithSubBezsButton.TabIndex = 1
		Me.CopyBezWithSubBezsButton.Text = "Copy Bezier Exprs \w SubBeziers"
		Me.CopyBezWithSubBezsButton.UseVisualStyleBackColor = True
		Me.CopyBezWithSubBezsButton.Visible = False
		'
		'CopyDraggableBezierButton
		'
		Me.CopyDraggableBezierButton.Enabled = False
		Me.CopyDraggableBezierButton.Location = New System.Drawing.Point(6, 285)
		Me.CopyDraggableBezierButton.Name = "CopyDraggableBezierButton"
		Me.CopyDraggableBezierButton.Size = New System.Drawing.Size(168, 23)
		Me.CopyDraggableBezierButton.TabIndex = 0
		Me.CopyDraggableBezierButton.Text = "Copy Draggable Bezier Exprs"
		Me.CopyDraggableBezierButton.UseVisualStyleBackColor = True
		Me.CopyDraggableBezierButton.Visible = False
		'
		'NOrderBezierButton
		'
		Me.NOrderBezierButton.Enabled = False
		Me.NOrderBezierButton.Location = New System.Drawing.Point(6, 256)
		Me.NOrderBezierButton.Name = "NOrderBezierButton"
		Me.NOrderBezierButton.Size = New System.Drawing.Size(147, 23)
		Me.NOrderBezierButton.TabIndex = 0
		Me.NOrderBezierButton.Text = "Generate N-Order Bezier"
		Me.NOrderBezierButton.UseVisualStyleBackColor = True
		Me.NOrderBezierButton.Visible = False
		'
		'TheStatusStrip
		'
		Me.TheStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RenderProgressLabel, Me.RenderProgressBar})
		Me.TheStatusStrip.Location = New System.Drawing.Point(0, 439)
		Me.TheStatusStrip.Name = "TheStatusStrip"
		Me.TheStatusStrip.Size = New System.Drawing.Size(624, 22)
		Me.TheStatusStrip.TabIndex = 1
		Me.TheStatusStrip.Text = "StatusStrip1"
		'
		'RenderProgressLabel
		'
		Me.RenderProgressLabel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.RenderProgressLabel.Name = "RenderProgressLabel"
		Me.RenderProgressLabel.Size = New System.Drawing.Size(128, 17)
		Me.RenderProgressLabel.Text = "Render Progress (0.0%)"
		'
		'RenderProgressBar
		'
		Me.RenderProgressBar.MarqueeAnimationSpeed = 20
		Me.RenderProgressBar.Name = "RenderProgressBar"
		Me.RenderProgressBar.Size = New System.Drawing.Size(100, 16)
		Me.RenderProgressBar.ToolTipText = "The Current YPixel Progress"
		'
		'Home
		'
		Me.AllowDrop = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(624, 461)
		Me.Controls.Add(Me.TheStatusStrip)
		Me.Controls.Add(Me.TheTabControl)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(640, 500)
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(640, 500)
		Me.Name = "Home"
		Me.Text = "GraphPictures Windows Utilities"
		Me.TheTabControl.ResumeLayout(False)
		Me.ExcelTabPage.ResumeLayout(False)
		Me.ExcelTabPage.PerformLayout()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.MaxDimensionSelector, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ExcelOutputPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.IRGPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.BezUtilsTabPage.ResumeLayout(False)
		Me.BezUtilsTabPage.PerformLayout()
		Me.TheStatusStrip.ResumeLayout(False)
		Me.TheStatusStrip.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents TheTabControl As System.Windows.Forms.TabControl
	Friend WithEvents ExcelTabPage As System.Windows.Forms.TabPage
	Friend WithEvents RenderOnExcelButton As System.Windows.Forms.Button
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents IRGTextBox As System.Windows.Forms.TextBox
	Friend WithEvents BrowseForFileButton As System.Windows.Forms.Button
	Friend WithEvents IRGPictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents ExcelOutputPictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents RGBModeCheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents ShowExcelCheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents DragFileHereLabel As System.Windows.Forms.Label
	Friend WithEvents TheStatusStrip As System.Windows.Forms.StatusStrip
	Friend WithEvents RenderProgressLabel As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents RenderProgressBar As System.Windows.Forms.ToolStripProgressBar
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents EnforceMaxDimensionCheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents MaxDimensionSelector As System.Windows.Forms.NumericUpDown
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents IRGDimensionsLabel As System.Windows.Forms.Label
	Friend WithEvents CloseExcelsLinkLabel As System.Windows.Forms.LinkLabel
	Friend WithEvents BezUtilsTabPage As System.Windows.Forms.TabPage
	Friend WithEvents NOrderBezierButton As System.Windows.Forms.Button
	Friend WithEvents CopyDraggableBezierButton As System.Windows.Forms.Button
	Friend WithEvents CopyBezWithSubBezsButton As System.Windows.Forms.Button
	Friend WithEvents BezierPointsTextBox As System.Windows.Forms.TextBox
	Friend WithEvents DesmosPastable_SingleBezier_Button As System.Windows.Forms.Button
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents DontUseLatex_CheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents DesmosPastable_DraggableBezier_Button As System.Windows.Forms.Button
	Friend WithEvents DesmosPastable_DraggableBezierWithSubExpressions_Button As System.Windows.Forms.Button
	Friend WithEvents DesmosLinkLabel As System.Windows.Forms.LinkLabel
End Class
