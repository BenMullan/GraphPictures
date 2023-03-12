<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	Request.CheckRequiredQueryStringsAreSpecified(Response, "RasterGraphicFileName")
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<script type="text/javascript" src="https://www.desmos.com/api/v1.6/calculator.js?apiKey=dcb31709b452b1cf9dc26972add0fda6"></script>
		<title><%= Request.QueryString("RasterGraphicFileName").ToString() %> in [Plot RasterGraphic \\ GraphPictures]</title>
	</head>
	
	<body onload="PlotRasterGraphicEntryPoint();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ Plot RasterGraphic</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="CalcuatorLoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="./Resources/LoadingGif.GIF" height="15%"/>
			<br/>
			Requesting Graph Data from Server...
		</div>

		<!--------------------------- The Desmos Calcuator Box --------------------------->
		<div id="CalcuatorContainer" class="Shadow GreyBorder Container" data-RasterGraphic="<%= Request.QueryString("RasterGraphicFileName").ToString() %>">
			<div id="GraphInfoText"></div>
			<br/>
			Click <a href="javascript:window.Utilities.FullScreenElement('#Calculator');" title="Make the Desmos Element FullScreen...">here</a> to Fullscreen the Graph
			<br/>
			<div id="Calculator"/>
		</div>

	</body>
	
</html>

<% 	Response.SafelyEnd()%>