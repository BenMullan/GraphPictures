Imports GraphPictures.Library.CompilerExtentions

Namespace Library
	Public Module ExcelResources

		Public Event PixelCopyComplete(ByVal _XPixel%, ByVal _YPixel%)

		REM Excel Limit: No more than 64,000 Different Colours are allowed.
		Public Const MaxExcelRasterGraphicDimension As UInt32 = 400
		Public Const MaxExcelRasterGraphicDimensionRGB As UInt32 = 200

		Public Function PlotBitmapOnExcel(ByVal _Bitmap As Drawing.Bitmap, Optional ByVal _ShowExcelDuringRender As Boolean = False, Optional ByVal _MaxIRGDimension As UInt32 = MaxExcelRasterGraphicDimension) As Drawing.Bitmap

			Try

				REM Make sure that the Input RasterImage isn't to large
				_Bitmap.MakeNoLargerThan(_MaxIRGDimension)

				Dim _ExcelApplication As New Microsoft.Office.Interop.Excel.Application() With {.Visible = _ShowExcelDuringRender, .DisplayAlerts = True}
				Dim _Workbook As Microsoft.Office.Interop.Excel.Workbook = _ExcelApplication.Workbooks.Add(Type.Missing)
				Dim _Worksheet As Microsoft.Office.Interop.Excel.Worksheet

				Dim _BitmapCellRangeString$ = String.Format("A1:{0}{1}", ExcelColumnFromNumber(_Bitmap.Width), (_Bitmap.Height).ToString())
				Dim _BitmapColumnRangeString$ = String.Format("A:{0}", ExcelColumnFromNumber(_Bitmap.Width))

				_Workbook = _ExcelApplication.Workbooks.Add(Type.Missing)

				_Worksheet = CType(_Workbook.ActiveSheet, Microsoft.Office.Interop.Excel.Worksheet)
				_Worksheet.Name = "GraphPictures BitmapToExcel"

				REM Make the Required cells Square...
				CType(_Worksheet.Columns(_BitmapColumnRangeString), Microsoft.Office.Interop.Excel.Range).ColumnWidth = 2.2

				For _YPixelNumber% = 1 To _Bitmap.Height Step +1
					For _XPixelNumber% = 1 To _Bitmap.Width Step +1

						CType(_Worksheet.Cells.Item(_YPixelNumber%, _XPixelNumber%), Microsoft.Office.Interop.Excel.Range).Interior.Color = System.Drawing.ColorTranslator.ToOle(_Bitmap.GetPixel(_XPixelNumber% - 1, _YPixelNumber% - 1))

						RaiseEvent PixelCopyComplete(_XPixelNumber, _YPixelNumber)

					Next
				Next

				_ExcelApplication.Visible = True

				'REM Now take a Screenshot of the Cells, and Return it.
				Dim _BitmapCellRange As Microsoft.Office.Interop.Excel.Range
				_BitmapCellRange = _Worksheet.Range(_BitmapCellRangeString)
				_BitmapCellRange.CopyPicture(Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen, Microsoft.Office.Interop.Excel.XlCopyPictureFormat.xlBitmap)

				REM Get the Image from the Clipboard and Return it
				If My.Computer.Clipboard.ContainsImage() Then Return (New Drawing.Bitmap(My.Computer.Clipboard.GetImage())) Else Return ImagingResources.GenerateBitmapFromText("Could not Extract the Excel Rendering from the Clipboard")

			Catch _Ex As Exception When True
				Throw (New Exception("The following Exception was thrown upon attempting to Plot a RasterGraphic on Excel: " & _Ex.Message))
			Finally
				'Process.GetProcessesByName("EXCEL.EXE").ToList().ForEach(Sub(_Process As Process) _Process.Kill())
			End Try

		End Function

		''' <summary>
		''' Instead of colouring 1 square cell per pixel, 3 cells are used, one for each of the Red, Green, and Blue.
		''' </summary>
		Public Function PlotBitmapOnExcelByRGB(ByVal _Bitmap As Drawing.Bitmap, Optional ByVal _ShowExcelDuringRender As Boolean = False, Optional ByVal _MaxIRGDimension As UInt32 = MaxExcelRasterGraphicDimensionRGB) As Drawing.Bitmap

			Try

				REM Make sure that the Input RasterImage isn't to large
				_Bitmap.MakeNoLargerThan(_MaxIRGDimension)

				Dim _ExcelApplication As New Microsoft.Office.Interop.Excel.Application() With {.Visible = _ShowExcelDuringRender, .DisplayAlerts = True}
				Dim _Workbook As Microsoft.Office.Interop.Excel.Workbook = _ExcelApplication.Workbooks.Add(Type.Missing)
				Dim _Worksheet As Microsoft.Office.Interop.Excel.Worksheet

				'The Image is 3x Taller due to the 3 cells per Pixel
				Dim _BitmapCellRangeString$ = String.Format("A1:{0}{1}", ExcelColumnFromNumber(_Bitmap.Width), (_Bitmap.Height * 3).ToString())
				Dim _BitmapColumnRangeString$ = String.Format("A:{0}", ExcelColumnFromNumber(_Bitmap.Width))
				Dim _BitmapRowRangeString$ = String.Format("1:{0}", ((_Bitmap.Height * 3)).ToString())

				_Worksheet = CType(_Workbook.ActiveSheet, Microsoft.Office.Interop.Excel.Worksheet)
				_Worksheet.Name = "GraphPictures BitmapToExcel RGB"

				REM Make the Required cells Smaller...
				CType(_Worksheet.Columns(_BitmapColumnRangeString), Microsoft.Office.Interop.Excel.Range).ColumnWidth = 1.57
				CType(_Worksheet.Rows(_BitmapRowRangeString), Microsoft.Office.Interop.Excel.Range).RowHeight = 3.75

				REM These _XPixelNumber and _YPixel Number Variables, are the current Pixel of the RASTER GRAPHIC, not the cell,
				REM ...Because each Pixel takes up 3 Cells, E.g.
				REM Pixel (1,1) == Cells (A1:A3)

				For _YPixelNumber% = 1 To _Bitmap.Height Step +1
					For _XPixelNumber% = 1 To _Bitmap.Width Step +1

						CType(_Worksheet.Cells.Item((_YPixelNumber% * 3) - 2, _XPixelNumber%), Microsoft.Office.Interop.Excel.Range).Interior.Color = Drawing.ColorTranslator.ToOle(Drawing.Color.FromArgb(red:=_Bitmap.GetPixel(_XPixelNumber% - 1, _YPixelNumber% - 1).R, green:=0, blue:=0))
						CType(_Worksheet.Cells.Item((_YPixelNumber% * 3) - 1, _XPixelNumber%), Microsoft.Office.Interop.Excel.Range).Interior.Color = Drawing.ColorTranslator.ToOle(Drawing.Color.FromArgb(red:=0, green:=_Bitmap.GetPixel(_XPixelNumber% - 1, _YPixelNumber% - 1).G, blue:=0))
						CType(_Worksheet.Cells.Item((_YPixelNumber% * 3) + 0, _XPixelNumber%), Microsoft.Office.Interop.Excel.Range).Interior.Color = Drawing.ColorTranslator.ToOle(Drawing.Color.FromArgb(red:=0, green:=0, blue:=_Bitmap.GetPixel(_XPixelNumber% - 1, _YPixelNumber% - 1).B))

						RaiseEvent PixelCopyComplete(_XPixelNumber, _YPixelNumber)

					Next
				Next

				'This makes sure the Window is visible, now that the rendering is complete. The Argument to this Method decides if the Window is visible during the Rendering itself.
				_ExcelApplication.Visible = True

				'REM Now take a Screenshot of the Cells, and Return it.
				Dim _BitmapCellRange As Microsoft.Office.Interop.Excel.Range
				_BitmapCellRange = _Worksheet.Range(_BitmapCellRangeString)
				_BitmapCellRange.CopyPicture(Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen, Microsoft.Office.Interop.Excel.XlCopyPictureFormat.xlBitmap)

				REM Get the Image from the Clipboard and Return it
				If My.Computer.Clipboard.ContainsImage() Then Return (New Drawing.Bitmap(My.Computer.Clipboard.GetImage())) Else Return ImagingResources.GenerateBitmapFromText("Could not Extract the Excel Rendering from the Clipboard")

			Catch _Ex As Exception When True
				Throw (New Exception("The following Exception was thrown upon attempting to Plot a RasterGraphic on Excel (Via RGB Cells): " & _Ex.Message))
			Finally
				'Process.GetProcessesByName("EXCEL.EXE").ToList().ForEach(Sub(_Process As Process) _Process.Kill())
			End Try

		End Function

		Public Function ExcelColumnFromNumber(ByVal _ColumnNumber As Integer) As String

			Dim _ColumnName As String = String.Empty

			While _ColumnNumber > 0
				Dim _Modulous As Integer = (_ColumnNumber - 1) Mod 26
				_ColumnName = Convert.ToChar(Asc("A"c) + _Modulous) & _ColumnName
				_ColumnNumber = CInt((_ColumnNumber - _Modulous) / 26)
			End While

			Return _ColumnName

		End Function

	End Module
End Namespace