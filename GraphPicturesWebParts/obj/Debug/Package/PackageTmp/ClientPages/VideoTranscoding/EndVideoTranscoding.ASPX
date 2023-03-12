<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>End VideoTranscoding \\ GraphPictures</title>
	</head>
	
	<body onload="EndVideoTranscodingEntryPoint();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ End VideoTranscoding</div>

		<!----------------------------- The Avaliable VideoFile's Container ----------------------------->
		<div id="VideoNameSelectionContainer" class="GreyBorder Shadow Container">
			
			<label for="AvaliableVideoFiles">Select a Video File which has already had it's DesmosFrames Rendered:<br/></label>

			<select name="AvaliableVideoFiles" id="AvaliableVideoFilesSelector">
				<%
					For Each _File As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWVideosFolderPath)).GetFiles()
						If ((Not _File.Name.StartsWith("DesmosRendered_", True, Global.System.Globalization.CultureInfo.CurrentCulture)) AndAlso (Not _File.Name.EndsWith(".MP3", True, Global.System.Globalization.CultureInfo.CurrentCulture)) AndAlso (Not _File.Name.EndsWith(".EXE", True, Global.System.Globalization.CultureInfo.CurrentCulture)) AndAlso (Not _File.Name.EndsWith(".DB", True, Global.System.Globalization.CultureInfo.CurrentCulture))) Then Response.Write("<option value=""" & _File.Name & """>" & _File.Name & "</option>")
					Next
				%>
			</select><br/><br/>

			<button type="button" class="Button" onclick="SendDesmosFramesToVideoAPIRequest($('#AvaliableVideoFilesSelector').val());">Generate Video</button>

		</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="VideoGenerationLoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="/ClientPages/Resources/LoadingGif.GIF" height="15%"/>
			<br/>Generating Video From DesmosFrames on Server...
		</div>

		<!----------------------------- The Container for the Final Output Hyperlink ----------------------------->
		<div id="OutputHyperlinkContainer" class="GreyBorder Shadow Container">
			
			<br/><br/>

			<a id="OutputVideoHref" href="">Click here for the Final Output Video...</a>
			<br/><br/>
			<a href="/ClientPages/VideoTranscoding/ApplyAudioToDesmosVideo.ASPX">Click here To Apply the Origional AudioTrack to this Output Video...</a>
			
			<br/><br/>

		</div>

	</body>
	
</html>

<% 	Response.SafelyEnd()%>