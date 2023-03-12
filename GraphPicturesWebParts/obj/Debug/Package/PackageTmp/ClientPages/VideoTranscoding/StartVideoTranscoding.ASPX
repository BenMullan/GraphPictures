<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>
	
	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>Start VideoTranscoding \\ GraphPictures</title>
	</head>
	
	<body onload="StartVideoTranscodingEntryPoint();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ Start VideoTranscoding</div>

		<!----------------------------- The Avaliable VideoFiles's Container ----------------------------->
		<div class="GreyBorder Shadow Container" id="VideoFileSelectionBox">
			
			<label for="AvaliableVideoFiles">Select an Avaliable Video File: </label>

			<!--
				We need to list all the avaliable Video Files,
				But because we're being Lazy, we're just shoring all files which don't:
					- Start \w "DesmosRednered_"
					- End \w *:
						- .MP3			(To exclude "AudioFor_*" Files)
						- .EXE			(To exclude FFMPEG.EXE)
						- .DB			(To exclude Thumbs.db)
			-->
			<select name="AvaliableVideoFiles" id="AvaliableVideoFilesSelector">
				<%
					For Each _File As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWVideosFolderPath)).GetFiles()
						
						If (
						 (Not _File.Name.StartsWith("DesmosRendered_", True, Global.System.Globalization.CultureInfo.CurrentCulture)) _
						 AndAlso (Not _File.Name.EndsWith(".MP3", True, Global.System.Globalization.CultureInfo.CurrentCulture)) _
						 AndAlso (Not _File.Name.EndsWith(".EXE", True, Global.System.Globalization.CultureInfo.CurrentCulture)) _
						 AndAlso (Not _File.Name.EndsWith(".DB", True, Global.System.Globalization.CultureInfo.CurrentCulture))
						) Then _
						 Response.Write("<option value=""" & _File.Name & """>" & _File.Name & "</option>" & vbCrLf)
					
					Next
				%>
			</select><br/><br/>

			<label for="VideoRenderingMode">Video Rendering Mode: </label>
			<select name="VideoRenderingMode" id="VideoRenderingModeSelector">
				<option value="SVGToBeziers">Frame -> SVG -> Beziers</option>
				<option value="SVGToBeziersShowExprList">Frame -> SVG -> Beziers (Show ExprList)</option>
				<option value="PlotRasterGraphic">Frame -> DesmosPlottedPixels</option>
			</select><br/><br/>

			<button type="button" class="Button" onclick="SendVideoToFramesAPIRequestAndGenerateHrefs($('#AvaliableVideoFilesSelector').val());">Generate Frames</button>

		</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="FrameGenerationLoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="/ClientPages/Resources/LoadingGif.GIF" height="15%"/>
			<br/>Generating Video Frames on Server...
		</div>

		<!----------------------------- The Container for the RenderingPage Hyperlinks ----------------------------->
		<div class="GreyBorder Shadow Container" id="RendererHyperlinks">
			
			<div id="VideoInfomationText" class="SubHeading">The Video () Produced * Frames</div><br/><br/>

			All Frames in one Renderer
			<ul><li><a id="OneRendererHref" href="">NotYetSet</a></li></ul>
			<br/><br/>

			Half the Frames in each of Two Renderers
			<ul>
				<li><a id="TwoRenderersHref1" href="">NotYetSet</a></li>
				<li><a id="TwoRenderersHref2" href="">NotYetSet</a></li>
			</ul><br/><br/>

			A Quarter of the Frames in each of Four Renderers
			<ul>
				<li><a id="FourRenderersHref1" href="">NotYetSet</a></li>
				<li><a id="FourRenderersHref2" href="">NotYetSet</a></li>
				<li><a id="FourRenderersHref3" href="">NotYetSet</a></li>
				<li><a id="FourRenderersHref4" href="">NotYetSet</a></li>
			</ul><br/><br/>

			<button type="button" class="Button" onclick="if (!(window.jQuery === undefined)) { window.Utilities.DownloadFileFromJavaScript($('#AvaliableVideoFilesSelector').val() + '_RendererHrefs.HTM', '<html><body>' + $('#RendererHyperlinks').html() + '</body></html>'); } else { window.alert('Please use StartVideoTranscoding.ASPX to redownload the Renderer Links...'); }">Download Renderer Links</button>

		</div>

	</body>
	
</html>

<% 	Response.SafelyEnd()%>