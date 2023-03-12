<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	REM This page takes in a ?SVGFileName and ?Quadrant and ?TileCount
	REM The ?TileCount is in the format TilesWide,TilesHigh
	REM The ?Quadrant is in the format x,y
	REM The Quadrant 1,1 is TopMost and LeftMost
	
	REM Once the page loads, an AJAX request is made to /API/Get.ASPX?Item=DesmosStateForSVG&SVGFileName={?SVGFileName}
	REM Upon receiving that response, the rest of the standard SetDesmosStateForSVG() function is let to run, leaving us with the fully-[zoomed-out], normal, rendering of our ?SVGFileName, in Desmos.
	REM Now, we can run window.DesmosCalcuator.setMathBounds() with the correct bounds, which we can calcuate, based off of the ?Quadrant:
	'	Use our ?TileCount, to work out the proportion of the way accross the current bounds of the graph to go, for this specific "DesmosTile"
	
	Request.CheckRequiredQueryStringsAreSpecified(Response, "SVGFileName", "Quadrant", "TileCount")
	
	Try
	
		'Check that the ?SVGFileName actually exists on Disk...
		If (Not IO.File.Exists(GraphPictures.WebParts.Resources.WWWSVGsFolderPath & Request.QueryString("SVGFileName"))) Then
			Response.ContentType = "application/json"
			Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The specified SVG File (" & Request.QueryString("SVGFileName") & ") does not exist in the [/ClientData/SVGFiles/] Directory."" }, ""ResponseContent"" : {} }")
		End If
	
		REM Syntactically Validate the ?Quadrant and ?TileCount
		If Not Request.QueryString!Quadrant.MatchesRegEx("^\d{1,2}\,\d{1,2}$") Then Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The specified ?Quadrant was syntactically-invalid."" }, ""ResponseContent"" : {} }")
		If Not Request.QueryString!TileCount.MatchesRegEx("^\d{1,2}\,\d{1,2}$") Then Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The specified ?TileCount was syntactically-invalid."" }, ""ResponseContent"" : {} }")
	
	Catch _Ex As Exception When True
		Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The following Exception was Thrown upon attempting to Process Server-Side PageContent for DesmosTile.ASPX: " & _Ex.Message & """ }, ""ResponseContent"" : {} }")
	End Try
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<script type="text/javascript" src="https://www.desmos.com/api/v1.6/calculator.js?apiKey=dcb31709b452b1cf9dc26972add0fda6"></script>
		<title><%= If(Request.QueryStringIsSpecified("SVGFileName"), Request.QueryString("SVGFileName").ToString(), "*") %> in [DesmosTile \\ GraphPictures]</title>
	</head>
	
	<body onload="DesmosTileEntryPoint();<%
		
	%>" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ View DesmosTile</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="CalcuatorLoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="./Resources/LoadingGif.GIF" height="15%"/>
			<br/>
			Preparing Tile...
		</div>

		<!--------------------------- The Desmos Calcuator Box --------------------------->
		<!-- The QueryStrings to this Page are hard-coded in to the HTMarkup, so that we can access them from the JavaScript later on... -->
		<div id="CalcuatorContainer" class="Shadow GreyBorder Container" style="display: none;" data-svgfilename="<%= Request.QueryString!SVGFileName %>" data-quadrant="<%= Request.QueryString!Quadrant %>" data-tilecount="<%= Request.QueryString!TileCount %>">
			<span id="SVGInfoText"></span><span>Tile <%= Request.QueryString!Quadrant %> of <a href="/ClientPages/DesmosTileLinks.ASPX?SVGFileName=<%= Request.QueryString!SVGFileName %>&TileCount=<%= Request.QueryString!TileCount %>" title="Redirects to the Tiles Page for <%= Request.QueryString!SVGFileName %>"><%= Request.QueryString!TileCount %></a></span>
			<br/><br/>

			<!-- GraphEnlargement Options -->
			<a href="javascript:window.Utilities.FullScreenElement('#Calculator');" title="Make the Desmos Element FullScreen...">FullScreen</a>
			<div style="padding-left:10%; display:inline;"/>
			<a href="javascript:$('#Calculator').attr('style', 'position: absolute; top: 0; left: 0; width: 100%; height: 100%;');$('body').css('overflow', 'hidden');" title="Make the Desmos Element occoupy the full space of the WebPage...">FullPage</a>

			<br/>
			<div id="Calculator"/>
		</div>

	</body>
	
</html>
<% Response.SafelyEnd() %>