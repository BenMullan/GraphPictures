<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	If Request.QueryStringIsSpecified("RasterImageFileName") Then
		Try
			
			Dim _SpecifiedRasterImageName$ = Request.QueryString("RasterImageFileName").ToString()

			If IO.File.Exists(GraphPictures.WebParts.WWWUploadedRasterImagesFolderPath & _SpecifiedRasterImageName) Then
				REM The ?RasterImageFileName exists in the UploadedRasterImagesFolder
								
				Response.ContentType = "application/json"
				Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""GeneratedSVGFileName"" : """ & GraphPictures.WebParts.Resources.GenerateSVGFromUploadedImage(_SpecifiedRasterImageName) & """ } }")
								
			ElseIf IO.File.Exists(GraphPictures.WebParts.WWWUploadedRasterImageBitmapConversionFolderPath & _SpecifiedRasterImageName) Then
				REM The ?ConvertedToBMP exists in the UploadedRasterImagesFolder
				
				Dim _CopiedRasterImageName$ = String.Empty
				
				REM Find a Random and not already-taken FileName in the [ClientData\UploadedRasterImages] Folder
				Do : _CopiedRasterImageName = (GraphPictures.Library.Resources.GetRandomString(10) & _SpecifiedRasterImageName.Split("."c).Last()).ToUpper()
				Loop Until (Not IO.File.Exists(GraphPictures.WebParts.WWWUploadedRasterImagesFolderPath & _CopiedRasterImageName))
				
				REM Copy the File back into the Standard Directory, where the Conversion Function in WebParts will look for it.
				IO.File.Copy(
				 sourceFileName:=GraphPictures.WebParts.WWWUploadedRasterImageBitmapConversionFolderPath & _SpecifiedRasterImageName,
				 destFileName:=GraphPictures.WebParts.WWWUploadedRasterImagesFolderPath & _CopiedRasterImageName,
				 overwrite:=False
				)
				
				Response.ContentType = "application/json"
				Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""true"", ""Reason"" : ""Not Applicable"" }, ""ResponseContent"" : { ""GeneratedSVGFileName"" : """ & GraphPictures.WebParts.Resources.GenerateSVGFromUploadedImage(_CopiedRasterImageName) & """ } }")
								
			Else
				REM The ?RasterImageFileName does not exist in either the UploadedRasterImages or ConvertedToBMP Folders
				Throw (New GraphPictures.Library.GraphPicturesConversionException("The Specified RasterImageFile (" & _SpecifiedRasterImageName & ") could not be found in either the [ClientData\UploadedRasterImages] or [ClientData\UploadedRasterImages\ConvertedToBMP] Folders."))
			End If
				
		Catch _Ex As Exception When True
			
			Response.ContentType = "application/json"
			Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The following Exception was Thrown upon attempting to Generate an SVG File for an Uploaded Raster Image: " & _Ex.Message & """ }, ""ResponseContent"" : {} }")
		
		End Try
	Else
		REM No ?RasterImageFileName is specified; list the avaliable ones for Viewing
		
		'{Write the rest of the page...}
		
	End If
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>RasterImage to SVG \\ GraphPictures</title>
	</head>
	
	<body onload="" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ RasterImage to SVG</div>

		<!----------------------------- The Avaliable SVG's Container ----------------------------->
		<div class="GreyBorder Shadow Container">
			Avaliable RasterImages in <a href="/ClientData/UploadedRasterImages/">[ClientData\UploadedRasterImages]</a>:<br/><br/>
			<ul>
			<%
				For Each _File As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWUploadedRasterImagesFolderPath)).GetFiles()
					Response.Write("<li><a href=""/ClientPages/RasterImageToSVG.ASPX?RasterImageFileName=" & _File.Name & """>" & _File.Name & "</a></li>")
				Next
			%>
			</ul>
			<br/><br/><br/>
			Avaliable RasterImages in <a href="/ClientData/UploadedRasterImages/ConvertedToBMP/">[ClientData\UploadedRasterImages\ConvertedToBMP]</a>:<br/><br/>
			<ul>
			<%
				For Each _File As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWUploadedRasterImageBitmapConversionFolderPath)).GetFiles()
					Response.Write("<li><a href=""/ClientPages/RasterImageToSVG.ASPX?RasterImageFileName=" & _File.Name & """>" & _File.Name & "</a></li>")
				Next
			%>
			</ul>
		</div>

	</body>
	
</html>
<% Response.SafelyEnd() %>