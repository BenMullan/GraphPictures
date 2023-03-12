/* <WebCamRelated> */
function InitialiseWebCam() {
	window.Utilities.LogDebugMessageToConsole("Initialising WebCam Objects...");

	window.WebCam = new Webcam(
		$("#WebCamVideoElement").get(0),
		"user",
		$("#WebCamCanvasElement").get(0),
		$("#WebCamPictureTakenSoundElement").get(0)
	);

	window.WebCam.start().then(
		function () {
			$("#WebCamNameText").text("Detected Camera: " + window.WebCam._webcamList[0].label);
			$("#LoadingMessage").hide();
			$("#WebCamContainer").show();

			//window.WebCam.flip();
		}
	).catch(
		function (_Error) {
			window.Utilities.LogErrorMessageToConsole("The following Error was thrown by the WebCam API: " + _Error);
			window.Utilities.ShowMessageToUser("The following Error was thrown by the WebCam API: " + _Error + "\r\n\r\nVideo Device issues can be caused by the lack of an encrypted connection (https), because Chrome's just so nice like that isn't it...");
		}
	);
}

function TakeAndStoreWebCamImage() {
	try {
		
		$("#LastPictureContainer").show();

		window.Utilities.LogDebugMessageToConsole("Taking an Image from the WebCam...");
		if (window.WebCam) {
			window.LastWebCamImage = window.WebCam.snap();

			/* Show the Button to upload the image, now that there is an image avaliable to be uploaded... */
			$("#UploadLastImageButton").show();
		} else {
			throw "window.WebCam was undefined";
		}
		window.Utilities.LogDebugMessageToConsole("Stored an Image from the WebCam in [window.LastWebCamImage]");
		window.Utilities.LogWarningMessageToConsole("It is normal, that a DOMException will now be thrown...");
		
	} catch(_Error) {
		window.Utilities.LogErrorMessageToConsole("The following JavaScript Error was thrown upon attempting to Take and Store an Image from the WebCam: " + _Error);
		window.Utilities.ShowMessageToUser("The following JavaScript Error was thrown upon attempting to Take and Store an Image from the WebCam: " + _Error);
	}
}

function UploadLastWebCamImage() {

	if (window.LastWebCamImage) {
		
		var _PreviousText = $("#LoadingMessageText").text();
		$("#LoadingMessageText").text("Uploading Image to Server...");
		$("#LoadingMessage").show();

		/* Send the contents of the window.LastWebCamImage Variable to the Server... */

		window.Utilities.SendAJAXPostRequest(
			("/API/Upload.ASPX?Item=Base64RasterGraphic"),
			{ Base64PNG : window.LastWebCamImage },
			function (_ResponseContent) {
				
				/* We have successfully Saved the Image to the Server. */
				window.Utilities.LogDebugMessageToConsole("Saved WebCam Image \"" + _ResponseContent.RasterGraphicFileName +" to the Server...");

				$("#LoadingMessageText").text(_PreviousText);
				$("#LoadingMessage").hide();

				/* Update the Message for the User... */
				$("#ImageUploadedText").html("<i>Uploaded Last Image as <a href='/ClientData/UploadedRasterImages/" + _ResponseContent.RasterGraphicFileName + "'>" + _ResponseContent.RasterGraphicFileName + "</a> to the Server<br/>Click <a href=\"javascript:GenerateSVGForRasterGraphicThenRedirectToDesmosRenderingOfIt('" + _ResponseContent.RasterGraphicFileName + "');\">here</a> to view the Image on a DesmosGraph...</i><br/><br/>");

			},
			function () {
				/* There was a Problem related to the POST Operation */
				window.Utilities.ShowMessageToUser("There was a problem posting the Rendered WebCam Image to the Server. See the console for more details...");

				/* We don't want to attempt to render more frames in the For loop, so... */
				throw new Error("There was a problem posting the Rendered WebCam Image to the Server. See the console for more details...");
			}
		);

	} else {
		window.Utilities.LogErrorMessageToConsole("The Image could not be uploaded, as it does not exist in the Global Variable \"window.LastWebCamImage\"...");
		window.Utilities.ShowMessageToUser("The Image could not be uploaded, as it does not exist in the Global Variable \"window.LastWebCamImage\". Assure that an Image has been captured using the other button first...");
	}
}
/* </WebCamRelated> */