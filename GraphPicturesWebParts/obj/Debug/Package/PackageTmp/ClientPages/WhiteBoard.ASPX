<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>[WhiteBoard \\ GraphPictures]</title>
	</head>
	
	<body onload="WhiteBoardEntryPoint();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ WhiteBoard</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="LoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="./Resources/LoadingGif.GIF" height="15%"/>
			<br/>
			<div id="LoadingMessageText">Loading Avaliable RasterGraphics...</div>
		</div>

		<!----------------------------- The RasterGraphicFileSelector Container ----------------------------->
		<div id="RasterGraphicFileSelectorContainer" class="GreyBorder Shadow" style="display: none;">
			<div class="SubHeading">Click on a RasterGraphic to Render it as a DesmosGraph:</div><br/>
		</div>

	</body>
	
</html>
<% 	Response.SafelyEnd()%>