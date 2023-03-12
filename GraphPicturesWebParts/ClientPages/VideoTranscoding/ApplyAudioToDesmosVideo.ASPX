<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>Apply Audio to DesmosVideo \\ GraphPictures</title>
	</head>
	
	<body onload="ApplyAudioToDesmosVideoEntryPoint();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ Apply Audio to DesmosVideo</div>

		<!----------------------------- The Avaliable DesmosRendered_VideoFile's Container ----------------------------->
		<div id="VideoNameSelectionContainer" class="GreyBorder Shadow Container">
			
			<label for="AvaliableVideoFiles">Select an Avaliable DesmosRendered Video File, to apply an AudioTrack to.<br/></label>

			<select name="AvaliableVideoFiles" id="AvaliableVideoFilesSelector">
				<%
					For Each _File As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWVideosFolderPath)).GetFiles()
						If _File.Name.StartsWith("DesmosRendered_", True, Global.System.Globalization.CultureInfo.CurrentCulture) Then Response.Write("<option value=""" & _File.Name & """>" & _File.Name & "</option>")
					Next
				%>
			</select><br/><br/>

			<button type="button" class="Button" onclick="SendApplyAudioAPIRequest($('#AvaliableVideoFilesSelector').val());">Apply Audio</button>

		</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="LoadingMessageContainer" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="/ClientPages/Resources/LoadingGif.GIF" height="15%"/>
			<br/>Applying Audio to DesmosVideo on Server...
		</div>

		<!----------------------------- The Container for the Final Output Hyperlink ----------------------------->
		<div id="OutputHyperlinkContainer" class="GreyBorder Shadow Container">
			<br/><br/>
			<a id="OutputVideoHref" href="">Click here for the Final Output Video with Audio...</a>
			<br/><br/>
		</div>

	</body>
	
</html>

<% 	Response.SafelyEnd()%>