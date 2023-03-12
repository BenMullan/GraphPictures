function SVGToDesmosEntryPoint() {

	/* This EntryPoint is ONLY USED for this Page, WHEN there are not a complete set of QueryStrings required to Render an SVGFile Specified. */

	window.Utilities.LogDebugMessageToConsole("Starting SVGToDesmos Page Scripts...");

	/* Show the PageElements which we didn't want to be seen initiallly in case the page had a ?SVGFileName specifed, in which case we'd want to load Desmos straight away, and it looked dodgy to have these elements hidden shortly afterwards... */
	$("#SVGInterpretationOptionsContainer").show();
	$("#SVGFileSelectorContainer").show();

	/* The #CalcuatorContainer is hidden with [display:none;] until the CallBack from the AJAX Request shows it... */
	$("#CalcuatorLoadingMessage").hide();

	/* The User will click a Button on the Webpage now to View the correct SVG in Desmos */

}

function StartVideoTranscodingEntryPoint() {

	window.Utilities.LogDebugMessageToConsole("Starting StartVideoTranscoding Page Scripts...");

	/* This is shown when the VideoToFrames API Response is received */
	$("#RendererHyperlinks").hide();
	$("#FrameGenerationLoadingMessage").hide();

}

function EndVideoTranscodingEntryPoint() {

	window.Utilities.LogDebugMessageToConsole("Starting EndVideoTranscoding Page Scripts...");

	/* This is shown when the DesmosFramesToVideo API Response is received */
	$("#VideoGenerationLoadingMessage").hide();
	$("#OutputHyperlinkContainer").hide();

}

function RenderDesmosVideoFramesEntryPoint() {

	window.Utilities.LogDebugMessageToConsole("Starting RenderDesmosVideoFrames Page Scripts...");

	// Get the Desmos JavaScript to make the "Calcuator" Element into a Desmos Frame
	window.DesmosCalculator = Desmos.GraphingCalculator(document.getElementById("Calculator"));

	$("#CalcuatorContainer").hide();
	$("#LoadingMessageContainer").hide();
	$("#DesmosScreenshotContainer").hide();

	/*
		- For _Frame = ?StartFrame To ?EndFrame:
			- Make request: [VideoAPI.ASPX?Action=DesmosStateForVideoFrame&VideoName={?VideoName}&FrameNumber={_Frame}], which returns the DesmosState for the _Frame
			- Call asyncScreenshot() in JS, and in callback, post PNG Data to API: [VideoAPI.ASPX?Action=SaveDesmosFrame&FrameNumber={_Frame}]
			- Next _Frame (Recursive)
	*/

	window.VideoName = $("#RendererInstanceData").attr("data-VideoName");
	window.StartFrame = parseInt($("#RendererInstanceData").attr("data-StartFrame"));
	window.EndFrame = parseInt($("#RendererInstanceData").attr("data-EndFrame"));

	/* Since the Call to RenderNextDesmosFrame() is only made recursively in the innermost callback for successful events, we must define what the FrameNumber is in a Window Variable */
	window.FrameNumber = window.StartFrame;

	/* Specify what to do after all Frames have been rendered */
	window.AllFramesRenderedCallback = function () {
		window.Utilities.ShowMessageToUser("Finished Rendering all DesmosFrames...");
		window.location.href = "/ClientPages/VideoTranscoding/EndVideoTranscoding.ASPX";
	};

	/* Start timing how long we've been rendering Frames for... */
	window.Utilities.MakeTimeCounter("#RendererRunningTimeText");

	/* This will, in turn, recursivly call RenderNextDesmosFrame() until window.FrameNumber >= window.EndFrame */
	RenderNextDesmosFrame();

}

function ShowExprListRenderDesmosVideoFramesEntryPoint() {

	window.Utilities.LogDebugMessageToConsole("Starting ShowExprListRenderDesmosVideoFrames Page Scripts...");

	// Get the Desmos JavaScript to make the "Calcuator" Element into a Desmos Frame
	window.DesmosCalculator = Desmos.GraphingCalculator(document.getElementById("Calculator"));

	$("#CalcuatorContainer").hide();
	$("#LoadingMessageContainer").hide();
	$("#DesmosScreenshotContainer").hide();

	/*
	- For _Frame = ?StartFrame To ?EndFrame:
	- Make request: [VideoAPI.ASPX?Action=DesmosStateForVideoFrame&VideoName={?VideoName}&FrameNumber={_Frame}], which returns the DesmosState for the _Frame
	- Call html2canvas() in JS, and in callback, post PNG Data to API: [VideoAPI.ASPX?Action=SaveDesmosFrame&FrameNumber={_Frame}]
	- Next _Frame (Recursive)
	*/

	window.VideoName = $("#RendererInstanceData").attr("data-VideoName");
	window.StartFrame = parseInt($("#RendererInstanceData").attr("data-StartFrame"));
	window.EndFrame = parseInt($("#RendererInstanceData").attr("data-EndFrame"));

	/* Since the Call to RenderNextDesmosFrame() is only made recursively in the innermost callback for successful events, we must define what the FrameNumber is in a Window Variable */
	window.FrameNumber = window.StartFrame;

	/* Specify what to do after all Frames have been rendered */
	window.AllFramesRenderedCallback = function () {
		window.Utilities.ShowMessageToUser("Finished Rendering all ShowExprListDesmosFrames. Click OK to be Redirected to the EndVideoTranscoding Page...");
		window.location.href = "/ClientPages/VideoTranscoding/EndVideoTranscoding.ASPX";
	};

	/* Start timing how long we've been rendering Frames for... */
	window.Utilities.MakeTimeCounter("#RendererRunningTimeText");

	/* This will, in turn, recursivly call RenderNextDesmosFrame() until [window.FrameNumber >= window.EndFrame] */
	ShowExprListRenderNextDesmosFrame();

}

function PointPlotRenderDesmosVideoFramesEntryPoint() {

	window.Utilities.LogDebugMessageToConsole("Starting PointPlotRenderDesmosVideoFrames Page Scripts...");

	//Get the Desmos JavaScript to make the "Calcuator" Element into a Desmos Frame
	window.DesmosCalculator = Desmos.GraphingCalculator(document.getElementById("Calculator"));

	$("#CalcuatorContainer").hide();
	$("#LoadingMessageContainer").hide();
	$("#DesmosScreenshotContainer").hide();

	/*
		- For _Frame = ?StartFrame To ?EndFrame:
			- Make request: [VideoAPI.ASPX?Action=PointPlottedDesmosStateForFrame&VideoName={?VideoName}&FrameNumber={_Frame}], which returns the DesmosState for the _Frame
			- Call asyncScreenshot() in JS, and in callback, post PNG Data to API: [VideoAPI.ASPX?Action=SaveDesmosFrame&FrameNumber={_Frame}]
			- Next _Frame (Recursive)
	*/

	window.VideoName = $("#RendererInstanceData").attr("data-VideoName");
	window.StartFrame = parseInt($("#RendererInstanceData").attr("data-StartFrame"));
	window.EndFrame = parseInt($("#RendererInstanceData").attr("data-EndFrame"));

	/* Since the Call to RenderNextDesmosFrame() is only made recursively in the innermost callback for successful events, we must define what the FrameNumber is in a Window Variable */
	window.FrameNumber = window.StartFrame;

	/* Specify what to do after all Frames have been rendered */
	window.AllFramesRenderedCallback = function () {
		window.Utilities.ShowMessageToUser("Finished Rendering all DesmosFrames...");
		window.location.href = "/ClientPages/VideoTranscoding/EndVideoTranscoding.ASPX";
	};

	/* Start timing how long we've been rendering Frames for... */
	window.Utilities.MakeTimeCounter("#RendererRunningTimeText");

	/* This will, in turn, recursivly call PointPlotRenderNextDesmosFrame() until (window.FrameNumber >= window.EndFrame) */
	PointPlotRenderNextDesmosFrame();

}

function ApplyAudioToDesmosVideoEntryPoint() {

	window.Utilities.LogDebugMessageToConsole("Starting ApplyAudioToDesmosVideo Page Scripts...");

	/* This is shown when the ?Action=GenerateAudioVersionOfDesmosVideo [API Response] is received */
	$("#LoadingMessageContainer").hide();
	$("#OutputHyperlinkContainer").hide();
}

function PlotRasterGraphicEntryPoint() {
	window.Utilities.LogDebugMessageToConsole("Starting PlotRasterGraphic Page Scripts...");

	$("#CalcuatorContainer").hide();

	SetDesmosStateForPlottedRasterGraphic($("#CalcuatorContainer").attr("data-RasterGraphic"));

}

function DesmosTileEntryPoint() {
	window.Utilities.LogDebugMessageToConsole("Starting DesmosTile Page Scripts...");

	window.PostDesmosRenderCallback = function () {
		/* This FUNCTION runs after Desmos has loaded in our SVGFile's Beziers. Now we need to zoom into the correct place... */

		var _TilesWide = parseInt($("#CalcuatorContainer").attr("data-tilecount").toString().split(",")[0]);
		var _TilesHigh = parseInt($("#CalcuatorContainer").attr("data-tilecount").toString().split(",")[1]);

		var _xmin = window.LastAJAXResponse.ResponseContent.DesmosState.graph.viewport.xmin;
		var _xmax = window.LastAJAXResponse.ResponseContent.DesmosState.graph.viewport.xmax;
		var _ymin = window.LastAJAXResponse.ResponseContent.DesmosState.graph.viewport.ymin;
		var _ymax = window.LastAJAXResponse.ResponseContent.DesmosState.graph.viewport.ymax;

		var _tx = parseInt($("#CalcuatorContainer").attr("data-quadrant").toString().split(",")[0]);
		var _ty = parseInt($("#CalcuatorContainer").attr("data-quadrant").toString().split(",")[1]);

		var _MathBounds = {
			left: (_xmin + (((_xmax / _TilesWide) * _tx) - (_xmax / _TilesWide))),
			right: (_xmin + ((_xmax / _TilesWide) * _tx)),
			bottom: ((_ymin + ((_ymax / _TilesHigh) * ((_TilesHigh + 1) - _ty))) - (_ymax / _TilesHigh)),
			top: (_ymin + ((_ymax / _TilesHigh) * ((_TilesHigh + 1) - _ty)))
		};

		window.DesmosCalculator.setMathBounds(_MathBounds);
		window.Utilities.LogDebugMessageToConsole("Set the Graph Bounds to: " + JSON.stringify(_MathBounds));

		/* Hide unneeded parts of Desmos... */
		$(".dcg-overgraph-pillbox-elements").css("display", "none");

		/* Make all of the Lines on the Graph Bolder */
		var _OrigionalState = window.DesmosCalculator.getState();

		var _BoldExpressions = [];

		_OrigionalState.expressions.list.forEach(
			function (_Expression) {
    			_Expression.lineWidth = "6";
    			_BoldExpressions.push(_Expression);
			}
		);

    	_OrigionalState.expressions.list = _BoldExpressions;

    	window.DesmosCalculator.setState(_OrigionalState);

		// Doesn't work because chrome dosen't like webpages fullscreening on load...
		//window.Utilities.FullScreenElement("#Calculator");

	};

	// Automatically get the page to render that SVG (with the Server's Default DesmosGraphCreationOptions), as soon as it loads...
	SetDesmosStateForSVG($("#CalcuatorContainer").attr("data-svgfilename"));
}

function WebCamUploadEntryPoint() {
	window.Utilities.LogDebugMessageToConsole("Starting WebCamUpload Page Scripts...");

	InitialiseWebCam();
}

function WhiteBoardEntryPoint() {
	window.Utilities.LogDebugMessageToConsole("Starting WhiteBoard Page Scripts...");

	/*
		Make an AJAX Request to retreive the avaliable RasterGraphics from the Server.
		This will respond with a boolean "RequestTimedOut", indicating if we need to send another request.

		If "RequestTimesOut" == "false", then we will also have received the new list of avaliable RasterGraphics,
		... which we now need to Render onto the page
	*/

	/* The |-deliminated String sent to the Server to know if our FileList is current. */
	window.KnownUploadedRasterImages = "_";

	var _CallbackToProcessRGList = function (_UploadedRasterImagesJSONArray) {

		window.Utilities.LogDebugMessageToConsole("Processing " + _UploadedRasterImagesJSONArray.length.toString() + " Received RasterGraphics in the List...");

		var _HTMLFor_RasterGraphicFileSelectorContainer = "";

		for (var _Index = 0; _Index != _UploadedRasterImagesJSONArray.length; ++_Index) {
			_HTMLFor_RasterGraphicFileSelectorContainer +=
				"<div onclick=\"GenerateSVGForRasterGraphicThenRedirectToDesmosRenderingOfIt('" + _UploadedRasterImagesJSONArray[_Index].FileName + "');\" class=\"RasterGraphicFilePreview\">"
					+ "<img src=\"/ClientData/UploadedRasterImages/" + _UploadedRasterImagesJSONArray[_Index].FileName + "\" width=\"100%\"/>"
					+ "<a href=\"/ClientData/UploadedRasterImages/" + _UploadedRasterImagesJSONArray[_Index].FileName + "\">"
						+ _UploadedRasterImagesJSONArray[_Index].FileName
					+ "</a><br/>"
					+ "(" + _UploadedRasterImagesJSONArray[_Index].FileSizeKB + "KB)"
				+ "</div>";
		}

		$("#RasterGraphicFileSelectorContainer").html(_HTMLFor_RasterGraphicFileSelectorContainer);

		$("#RasterGraphicFileSelectorContainer").show();
		$("#LoadingMessage").hide();

		/* Now Check for any more changes to the folder... */
		GetLatestUploadedRGsList(_CallbackToProcessRGList);
	};

	GetLatestUploadedRGsList(_CallbackToProcessRGList);
}

function UploadRasterImageAJAXEntryPoint() {
	window.Utilities.LogDebugMessageToConsole("Starting UploadRasterImageAJAX Page Scripts...");

	$("#FileUploaderContainer").show();
	$("#LoadingMessage").hide();
}