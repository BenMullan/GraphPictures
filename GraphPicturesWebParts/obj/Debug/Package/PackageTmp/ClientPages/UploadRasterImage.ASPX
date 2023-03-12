<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="UploadRasterImage.aspx.vb" Inherits="GraphPictures.UploadRasterImage" %>
<%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>

<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>Upload Raster Image \\ GraphPictures</title>
	</head>
	
	<body onload="" class="Centered">
		
		<form id="ServerSideForm" runat="server">
		
			<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ Upload Raster Image</div>

			<!----------------------------- The FileUpload Container ----------------------------->
			<div class="GreyBorder Shadow Container">
				<b class="SubHeading">Upload an Image File (Max Size 3 MB)</b><br/><br/>
				Click, or Drag and Drop into the box below, to Upload an Image...<br/><br/>
				<asp:FileUpload ID="RasterImageUploader" runat="server"/><br/><br/>
				<asp:Button ID="PerformUploadButton" runat="server" Text="Upload" CssClass="Button"/><br/><br/>
				<asp:Label ID="StatusLabel" runat="server" Text="Status: File not yet uploaded"></asp:Label>		
			</div>

		</form>

	</body>
	
</html>