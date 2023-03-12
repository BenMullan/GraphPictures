<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	Request.CheckRequiredQueryStringsAreSpecified(Response, "VideoName", "StartFrame", "EndFrame")
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<script type="text/javascript" src="https://www.desmos.com/api/v1.6/calculator.js?apiKey=dcb31709b452b1cf9dc26972add0fda6"></script>
		<script type="text/javascript" src="../Resources/HTML2Canvas.Min.JS"></script>
		<title><%= Request.QueryString!VideoName %> in [ShowExprList Render DesmosVideo Frames \\ GraphPictures]</title>
	</head>
	
	<body onload="ShowExprListRenderDesmosVideoFramesEntryPoint();" class="Centered">
		
		<div id="RendererInstanceData"
			data-VideoName="<%= Request.QueryString!VideoName.ToString() %>"
			data-StartFrame="<%= Request.QueryString!StartFrame.ToString() %>"
			data-EndFrame="<%= Request.QueryString!EndFrame.ToString() %>"
		/>
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ ShowExprList Render DesmosVideo Frames</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="LoadingMessageContainer" class="GreyBorder Shadow Container">
			
			This Renderer will transcode Frames <%= Request.QueryString!StartFrame.ToString() %> to <%= Request.QueryString!EndFrame.ToString() %> of <%= Request.QueryString!VideoName.ToString()%>.<br/>
			<div id="RendererProgressText" style="display: inline;">Progress: 0%</div>&emsp;&emsp;
			Running Time: <div id="RendererRunningTimeText" style="display: inline;">{Timer Not Started}</div>&emsp;&emsp;
			Frames per Minuite: <div id="RendererFramesPerMinuiteText" style="display: inline;">{No Frames Rendered Yet}</div><br/>

			<img alt="Loading Wheel" src="/ClientPages/Resources/LoadingGif.GIF" height="8%"/>
			<br/><div id="SVGGenData" style="font-size: 80%; font-style: italic;">{No SVGs Generated yet...}</div>
			<br/><b><div id="LoadingMessageText">Text for Loading Message...</div></b>
		</div>

		<!--------------------------- The Preview of the DesmosScreenshot --------------------------->
		<div id="DesmosScreenshotContainer" class="Shadow GreyBorder Container">
			Preview of last Desmos Screenshot:<br/>
			<img id="DesmosScreenshotPreview" alt="DesmosScreenshot Preview" src="" height="65%"/>
		</div>

		<!--------------------------- The Desmos Calcuator Box --------------------------->
		<div id="CalcuatorContainer" class="Shadow GreyBorder Container">
			<div id="SVGInfoText"></div><br/>
			<div id="Calculator"/>
		</div>

	</body>
	
</html>

<% 	Response.SafelyEnd()%>