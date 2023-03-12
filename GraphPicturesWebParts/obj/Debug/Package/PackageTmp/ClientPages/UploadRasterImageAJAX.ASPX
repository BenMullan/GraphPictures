<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>Upload RasterImage (AJAX) \\ GraphPictures</title>
	</head>
	
	<body onload="UploadRasterImageAJAXEntryPoint();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ Upload RasterImage (AJAX)</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="LoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="./Resources/LoadingGif.GIF" height="15%"/>
			<br/>
			<div id="LoadingMessageText">Loading FileUpload Components...</div>
		</div>

		<!----------------------------- The FileUploader Container ----------------------------->
		<div id="FileUploaderContainer" class="GreyBorder Shadow Container" style="display: none;">
			
			<b class="SubHeading" title="Instructions">Drag and Drop, or Browse for an Image File</b><br/>
			Max 3MB. Supported FileExtentions: BMP, PNG, JFIF, JPEG, JPG, GIF<br/><br/>

			<input type="file" id="RasterImageUploader" accept=".BMP,.PNG,.JFIF,.JPEG,.JPG,.GIF"/>
			<br/><br/><br/>
			<button class="Button" onclick="();" title="Begin the AJAX Uploading of the specified Image File...">Upload...</button>
			
		</div>

	</body>
	
</html>
<%Response.SafelyEnd()%>