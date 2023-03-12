<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>DesmosTile Links for SVG \\ GraphPictures</title>
	</head>
	
	<body onload="$('#LoadingMessage').hide();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ DesmosTile Links for SVG</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="LoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="./Resources/LoadingGif.GIF" height="15%"/>
			<br/>
			Loading avaliable SVGs from Server...
		</div>

		<!----------------------------- The TileCountSelection Container ----------------------------->
		<div class="GreyBorder Shadow Container">
			<span class="SubHeading">Enter the Number of Tiles sought:</span>
			<br/><br/>
			<label for="TilesWideSelector">→ Tiles Wide: </label>
			<input type="number" id="TilesWideSelector" name="TilesWideSelector" min="1" max="99" value="2" oninput="if (parseInt($(this.id))) {}"/>
			
			<label for="TilesHighSelector" style="margin-left: 8%">↑ Tiles High: </label>
			<input type="number" id="TilesHighSelector" name="TilesHighSelector" min="1" max="99" value="2"/>
		</div>

		<!----------------------------- The SVGFileSelector Container ----------------------------->
		<div id="SVGFileSelectorContainer" class="GreyBorder Shadow">
			<div class="SubHeading">Click on an SVGFile to Generate DesmosTile Links...</div><br/>
			
			<%
				For Each _SVGFile As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.WWWSVGsFolderPath)).GetFiles()
					Response.Write(String.Format("<div onclick=""window.location.href = ('/ClientPages/DesmosTileLinks.ASPX?SVGFileName={0}&TileCount=' + $('#TilesWideSelector').val() + ',' + $('#TilesHighSelector').val());"" class=""SVGFilePreview""><img src=""/ClientPages/ViewSVG.ASPX?SVGFileName={0}"" width=""100%"" height=""100%""/><a href=""/ClientPages/ViewSVG.ASPX?SVGFileName={0}"">{0}</a><br/>({1}KB)</div>", _SVGFile.Name, _SVGFile.SizeInIntegralKB()) & vbCrLf & vbCrLf & vbCrLf)
				Next
			%>
			
		</div>
		
	</body>
	
</html>
<% 	Response.SafelyEnd()%>