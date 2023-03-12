<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<!-- Zoom out to 80% to have any hope of this page working properly... -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<script type="text/javascript" src="./Resources/WebCamEasy.JS"></script>
		<script type="text/javascript" src="./Resources/WebCamResources.JS"></script>
		<title>WebCam Upload \\ GraphPictures</title>
	</head>
	
	<body onload="WebCamUploadEntryPoint();" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ WebCam Upload</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="LoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="./Resources/LoadingGif.GIF" height="15%"/>
			<br/>
			<div id="LoadingMessageText">Loading Video Device...</div>
		</div>

		<!----------------------------- The WebCam Container ----------------------------->
		<div id="WebCamContainer" class="GreyBorder Shadow Container" style="display: none;">
			<b id="WebCamNameText" class="SubHeading" title="A Description of the Default VideoDevice on the Computer">{No WebCam Detected...}</b><br/><br/>
				
			<button class="Button" onclick="TakeAndStoreWebCamImage();" title="Takes a picture and stores it in this Client's RAM...">Take Picture...</button>
			<button id="UploadLastImageButton" style="display: none;" class="Button" onclick="UploadLastWebCamImage();" title="Uploads the Last-Taken Picture, from this Client's RAM, unto the GraphPictures WebServer...">Upload Last Taken...</button>

			<br/><br/>

			<!-- This is filled in upon successfully uploading an Image to the Server -->
			<div id="ImageUploadedText"></div>
			
			<div id="WebCamAndLastPictureContainer">

				<div id="VideoDeviceContainer" class="GreyBorder Shadow">
					<div class="SubHeading">VideoDevice Preview:</div><br/>
					<video id="WebCamVideoElement" autoplay playsinline title="The VideoDevice's Output Stream"></video>
				</div>

				<div id="LastPictureContainer" class="GreyBorder Shadow" title="The Container for the Last Picture taken by the VideoDevice">
					<div class="SubHeading">Last Picture Taken:</div><br/>
					<canvas id="WebCamCanvasElement" src="" class=""></canvas>
					<audio id="WebCamPictureTakenSoundElement" src="./Resources/WebCamSnap.Wav" preload="auto"></audio>
				</div>

			</div>

		</div>

	</body>
	
</html>
<%Response.SafelyEnd()%>