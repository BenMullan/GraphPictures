<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>Main Menu \\ GraphPictures</title>
	</head>
	
	<body onload="" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ Main Menu</div>

		<!----------------------------- The Links ----------------------------->
		<div class="GreyBorder Shadow Container" style="line-height: 140%;">
			
			<b class="SubHeading">Image-Related Functions</b>
			<ul>
				<li><a href="/ClientPages/RasterImageToDesmos.ASPX"><b>Convert a RasterImage to a DesmosGraph</b></a></li>
				<li><a href="/ClientPages/SVGToDesmos.ASPX">Convert an already-existant SVGFile to a DesmosGraph</a></li>
				<li><a href="/ClientPages/UploadRasterImage.ASPX">Upload a RasterImage (WebForm)</a></li>
				<li><a href="/ClientPages/UploadRasterImageAJAX.ASPX">Upload a RasterImage (AJAX)</a></li>
				<li><a href="/ClientPages/ViewSVG.ASPX">View an already-exisant SVGFile</a></li>
				<li><a href="/ClientPages/RasterImageToSVG.ASPX">Convert an already-existant RasterImage to an SVGFile</a></li>
				<li><a href="/ClientPages/PlotRasterGraphic.ASPX">Plot the Pixels of a RasterGraphic on a DesmosGraph</a></li>
				<li><a href="/ClientPages/DesmosTile.ASPX">View a Tile of a DesmosGraph</a></li>
				<li><a href="/ClientPages/DesmosTileLinks.ASPX">Generate Links for Tiles of a DesmosGraph</a></li>
				<li><a href="/ClientPages/DesmosTileLinksForSVG.ASPX">Generate DesmosTile Links, for a chosen SVG</a></li>
				<li><a href="/ClientPages/WebCamUpload.ASPX">Upload a RasterImage from a VideoDevice such as a WebCam</a></li>
				<li><a href="/ClientPages/WhiteBoard.ASPX">View the contents of the	UploadedRasterImages Folder in RealTime</a></li>
			</ul>
			<br/><br/>

			<b class="SubHeading">Video-Related Functions</b>
			<ul>
				<li><a href="/ClientPages/VideoTranscoding/StartVideoTranscoding.ASPX"><b>Select a Video to Transcode into a DesmosVideo</b></a></li>
				<li><a href="/ClientPages/VideoTranscoding/RenderDesmosVideoFrames.ASPX">Render a Range of Frames for a DesmosVideo</a></li>
				<li><a href="/ClientPages/VideoTranscoding/PointPlotRenderDesmosVideoFrames.ASPX">PointPlot Render a Range of Frames for a DesmosVideo</a></li>
				<li><a href="/ClientPages/VideoTranscoding/ShowExprListRenderDesmosVideoFrames.ASPX">Render a Range of Frames for a DesmosVideo, showing the ExprList</a></li>
				<li><a href="/ClientPages/VideoTranscoding/EndVideoTranscoding.ASPX">Encode DesmosFrames into a DesmosVideo</a></li>
				<li><a href="/ClientPages/VideoTranscoding/ViewVideoFile.ASPX">View an already-existant DesmosVideo</a></li>
				<li><a href="/ClientPages/VideoTranscoding/ApplyAudioToDesmosVideo.ASPX">Apply the AudioTrack to an already-existant DesmosVideo</a></li>
			</ul>
			<br/><br/>

			<!-- These Verknüpfungen can be removed when the site is put into Production... -->
			<b class="SubHeading">(Helpful Debugging Links)</b>
			<ul>
				<li><a href="/ClientPages/DesmosTile.ASPX?SVGFileName=JHGS_Swan.SVG&Quadrant=1,1&TileCount=2,1"><b>First half of Swan as a DesmosTile</b></a></li>
				<li><a href="/ClientPages/DesmosTileLinks.ASPX?SVGFileName=JHGS_Swan.SVG&TileCount=3,3">3x3 TileLinks for JHGS_SWAN.SVG</a></li>
				<li><a href="/ClientPages/VideoTranscoding/ShowExprListRenderDesmosVideoFrames.ASPX?VideoName=Car.MP4&StartFrame=1&EndFrame=10">Render the first 10 Frames of Car.MP4</a></li>
			</ul>
		</div>

	</body>

	<!--
		** Car.MP4 10-Frame Render Speed Test - Results **

		Computer:			Frames Per Min.
		-----------------------------------
		BM Latitude			0.67
		TB MacBuch Luft		7.6
		OM Lenovo			1.75
		Dell Precision T	3
		BM Precision M6800	4
	-->

</html>
<% 	Response.SafelyEnd()%>