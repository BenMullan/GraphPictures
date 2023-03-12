function SetDesmosStateForSVG(_SVGFileName, _SVGInterpretationOptions) {

	/*
		Optional QueryStrings for DesmosState Generation:
		[?ShowGrid], [?ColourAllocationMode], [?GraphZoomSize], [?LinesOpacity], [?LinesWidth], [?LinesStyle], [?Fill], [?FillOpacity]
	*/
	
	/* If an _SVGInterpretationOption isn't specified in this JSON Object, then default ones are used on the Server. (The || Operator returns the first operand which isn't "undefined") */

	/* As this is the EntryPoint for SVGToDesmos, WHEN NOT ALL Required QueryStrings to render an SVGFile are specified, Hide these unneeded XMLElements */
	$("#SVGFileSelectorContainer").hide();
	$("#SVGInterpretationOptionsContainer").hide();

	/* This is hidden upon executing a SuccessCallBack for the AJAX Request */
	$("#CalcuatorLoadingMessage").show();

	var _RequestURI = "/API/Get.ASPX?Item=DesmosStateForSVG&SVGFileName=" + encodeURI(_SVGFileName);

	/* Add each SVGInterpretationOption which has been specified, to this request URI */
	for (var _Key in _SVGInterpretationOptions) {
		_RequestURI += ("&" + _Key + "=" + encodeURI(_SVGInterpretationOptions[_Key]));
	}

	window.Utilities.SendAJAXRequest(
		_RequestURI,
		function (_ResponseContent) {

			window.Utilities.LogDebugMessageToConsole("DesmosState for \"" + _SVGFileName + "\" Received successfully from the Server. Setting the State and showing the #CalcuatorContainer...");
			$("#CalcuatorLoadingMessage").hide();

			/* Set the SVGInfoText */
			$("#SVGInfoText").html("Showing <a href=\"/ClientPages/ViewSVG.ASPX?SVGFileName=" + _SVGFileName + "\">" + _SVGFileName + "</a> using " + _ResponseContent.ExpressionCount + " Beziers...");

			//Get the Desmos JavaScript to make the "Calcuator" Element into a Desmos Frame
			window.DesmosCalculator = Desmos.GraphingCalculator(document.getElementById("Calculator"));
			window.DesmosCalculator.setState(_ResponseContent.DesmosState);

			//Show the #CalcuatorContainer, as it was hidden until ready
			$("#CalcuatorContainer").show();

			/* In the DesmosTileASPX, we want to run stuff after Desmos has loaded the SVG... */
			if (window.PostDesmosRenderCallback) {
				window.PostDesmosRenderCallback();
			}
		},
		function () {
			window.Utilities.LogErrorMessageToConsole("There was a problem getting the Desmos State for: " + _SVGFileName);
			window.Utilities.ShowMessageToUser("There was a problem getting the Desmos State for: " + _SVGFileName + ". View the Verbose Debugging Messages in the Console for the Exception Message...\r\n\r\nThe Image probably produced too many Beziers, which would have taken hours to compute.");
		}
	);
}

function SetDesmosStateForPlottedRasterGraphic(_RasterGraphicFileName) {

	/* This is hidden upon executing a SuccessCallBack for the AJAX Request */
	$("#CalcuatorLoadingMessage").show();

	window.Utilities.SendAJAXRequest(
		("/API/Get.ASPX?Item=DesmosStateForPlottedRasterGraphic&RasterGraphicFileName=" + encodeURI(_RasterGraphicFileName)),
		function (_ResponseContent) {

			window.Utilities.LogDebugMessageToConsole("DesmosState for \"" + _RasterGraphicFileName + "\" Received successfully from the Server. Setting the State and showing the #CalcuatorContainer...");
			$("#CalcuatorLoadingMessage").hide();

			/* Set the GraphInfoText */
			$("#GraphInfoText").html("Showing <a href=\"/ClientData/UploadedRasterImages/" + _RasterGraphicFileName + "\">" + _RasterGraphicFileName + "</a> using " + _ResponseContent.ExpressionCount + " Points...");

			//Get the Desmos JavaScript to make the "Calcuator" Element into a Desmos Frame
			window.DesmosCalculator = Desmos.GraphingCalculator(document.getElementById("Calculator"));
			window.DesmosCalculator.setState(_ResponseContent.DesmosState);

			//Show the #CalcuatorContainer, as it was hidden until ready
			$("#CalcuatorContainer").show();
		},
		function () {
			window.Utilities.LogErrorMessageToConsole("There was a problem getting the Desmos State for: " + _RasterGraphicFileName);
			window.Utilities.ShowMessageToUser("There was a problem getting the Desmos State for: " + _RasterGraphicFileName + ". View the Verbose Debugging Messages in the Console for the Exception Message...");
		}
	);
}

/* <VideoRelated> */

function SendVideoToFramesAPIRequestAndGenerateHrefs(_VideoFileName) {

	$("#FrameGenerationLoadingMessage").show();

	if (_VideoFileName) {

		window.Utilities.SendAJAXRequest(
			("/API/VideoAPI.ASPX?Action=VideoToFrames&VideoFile=" + _VideoFileName),
			function (_ResponseContent) {
				/* We should have had returned to us, the number of frames generated.
				Now we must Generate href's to [RenderDesmosVideoFrames.ASPX?VideoName=Input.MP4&StartFrame=1&EndFrame=250] etc... */

				$("#FrameGenerationLoadingMessage").hide();
				$("#VideoFileSelectionBox").hide();
				$("#RendererHyperlinks").show();

				$("#VideoInfomationText").text("The Video (" + _VideoFileName + ") Produced " + _ResponseContent.GeneratedFramesCount.toString() + " Frames");

				/* Make the Href's for the Renderers fully-qualified, so that they are still valid if the RendererLinks Div is downloaded */
				var _RendererHrefBaseURL = (location.protocol + '//' + location.host) + "/ClientPages/VideoTranscoding/"

				switch ($("#VideoRenderingModeSelector").val()) {
					case "SVGToBeziers":
						_RendererHrefBaseURL += ("RenderDesmosVideoFrames.ASPX?VideoName=" + _VideoFileName);
						break;
					case "SVGToBeziersShowExprList":
						_RendererHrefBaseURL += ("ShowExprListRenderDesmosVideoFrames.ASPX?VideoName=" + _VideoFileName);
						break;
					case "PlotRasterGraphic":
						_RendererHrefBaseURL += ("PointPlotRenderDesmosVideoFrames.ASPX?VideoName=" + _VideoFileName);
						break;
					default:
						window.Utilities.LogErrorMessageToConsole("The VideoRendering mode was not able to be resolved (in the switch statment)...");
						window.Utilities.ShowMessageToUser("The VideoRendering mode was not able to be resolved (in the switch statment)...");
						throw new Error("The VideoRendering mode was not able to be resolved (in the switch statment)...");
				}

				/* All Frames in one Renderer */
				$("#OneRendererHref").attr("href", _RendererHrefBaseURL + "&StartFrame=1&EndFrame=" + _ResponseContent.GeneratedFramesCount.toString());
				$("#OneRendererHref").text("Frames (1 to " + _ResponseContent.GeneratedFramesCount.toString() + ") of " + _ResponseContent.GeneratedFramesCount.toString());

				/* Half the Frames in each of Two Renderers */
				var _RangeMidPoint = Math.ceil(parseInt(_ResponseContent.GeneratedFramesCount) * 0.5);
				$("#TwoRenderersHref1").attr("href", _RendererHrefBaseURL + "&StartFrame=1&EndFrame=" + _RangeMidPoint.toString());
				$("#TwoRenderersHref1").text("Frames (1 to " + _RangeMidPoint.toString() + ") of " + _ResponseContent.GeneratedFramesCount.toString());
				/**/
				$("#TwoRenderersHref2").attr("href", _RendererHrefBaseURL + "&StartFrame=" + (_RangeMidPoint + 1).toString() + "&EndFrame=" + _ResponseContent.GeneratedFramesCount.toString());
				$("#TwoRenderersHref2").text("Frames (" + (_RangeMidPoint + 1).toString() + " to " + _ResponseContent.GeneratedFramesCount.toString() + ") of " + _ResponseContent.GeneratedFramesCount.toString());

				/* A Quarter of the Frames in each of Four Renderers */
				var _RangeMidPoints = [Math.ceil(parseInt(_ResponseContent.GeneratedFramesCount) * 0.25), Math.ceil(parseInt(_ResponseContent.GeneratedFramesCount) * 0.5), Math.ceil(parseInt(_ResponseContent.GeneratedFramesCount) * 0.75)];
				$("#FourRenderersHref1").attr("href", _RendererHrefBaseURL + "&StartFrame=1&EndFrame=" + _RangeMidPoints[0].toString());
				$("#FourRenderersHref1").text("Frames (1 to " + _RangeMidPoints[0].toString() + ") of " + _ResponseContent.GeneratedFramesCount.toString());
				/**/
				$("#FourRenderersHref2").attr("href", _RendererHrefBaseURL + "&StartFrame=" + (_RangeMidPoints[0] + 1).toString() + "&EndFrame=" + _RangeMidPoints[1].toString());
				$("#FourRenderersHref2").text("Frames (" + (_RangeMidPoints[0] + 1).toString() + " to " + _RangeMidPoints[1].toString() + ") of " + _ResponseContent.GeneratedFramesCount.toString());
				/**/
				$("#FourRenderersHref3").attr("href", _RendererHrefBaseURL + "&StartFrame=" + (_RangeMidPoints[1] + 1).toString() + "&EndFrame=" + _RangeMidPoints[2].toString());
				$("#FourRenderersHref3").text("Frames (" + (_RangeMidPoints[1] + 1).toString() + " to " + _RangeMidPoints[2].toString() + ") of " + _ResponseContent.GeneratedFramesCount.toString());
				/**/
				$("#FourRenderersHref4").attr("href", _RendererHrefBaseURL + "&StartFrame=" + (_RangeMidPoints[2] + 1).toString() + "&EndFrame=" + _ResponseContent.GeneratedFramesCount.toString());
				$("#FourRenderersHref4").text("Frames (" + (_RangeMidPoints[2] + 1).toString() + " to " + _ResponseContent.GeneratedFramesCount.toString() + ") of " + _ResponseContent.GeneratedFramesCount.toString());
			},
			function () {
				window.Utilities.LogErrorMessageToConsole("There was a problem Generating the Frame Bitmaps for: " + _VideoFileName);
				window.Utilities.ShowMessageToUser("There was a problem Generating the Frame Bitmaps for: " + _VideoFileName + ". View the Verbose Debugging Messages in the Console for the Exception Message...");
			}
		);
	} else {
		window.Utilities.ShowMessageToUser("An attempt was made to Request the Frames of a VideoFile, however the VideoFileName was black.");
	}
}

function RenderNextDesmosFrame() {
	
	window.Utilities.LogDebugMessageToConsole("Rendering Next DesmosFrame. [window.FrameNumber] == " + window.FrameNumber.toString());

	/* Update the #RendererProgressText */
	$("#RendererProgressText").text("Progress: " + Math.round(((window.FrameNumber - window.StartFrame) / (window.EndFrame - window.StartFrame)) * 100).toString() + "%");

	$("#LoadingMessageText").text("Generating Beziers for Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP...");
	$("#LoadingMessageText").show();
	$("#LoadingMessageContainer").show();

	window.Utilities.SendAJAXRequest(
		("/API/VideoAPI.ASPX?Action=DesmosStateForVideoFrame&VideoName=" + window.VideoName + "&FrameNumber=" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber)),
		function (_ResponseContent) {
			/* We should have received the [DesmosState], and [ExpressionCount] */

			$("#LoadingMessageContainer").hide();

			/* Hide the Screenshot Preview, which would still be showing from a previous Call if there was one */
			$("#DesmosScreenshotContainer").hide();

			/* Set the SVGInfoText */
			$("#SVGInfoText").html("Rendering \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".SVG\" using " + _ResponseContent.ExpressionCount + " Beziers...");

			/* Set the State received during the request */
			window.DesmosCalculator.setState(_ResponseContent.DesmosState);

			//Show the #CalcuatorContainer, as it was hidden until ready
			$("#CalcuatorContainer").show();

			/* Show Message "Waiting for Screenshot to be taken", until the async callback to  */
			$("#LoadingMessageText").text("Waiting for Desmos to Render Screenshot of Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".SVG...");
			$("#LoadingMessageContainer").show();

			var _DesmosState = window.DesmosCalculator.getState();
			
			/*
				Common _DesmosFrameZoom Sizes:
					- 4:3 in 16:9 Video:	0.87	WhoDoneThat
					- 4:3 Video:			1.0		Wombles
				Lower = More Zoomed in
				Higher = More zoomed out

				Licky	0.9		+4500,-3000,+4500,-3000

				var _DesmosFrameZoom = 0.3;

				mathBounds : {
					left : (_DesmosState.graph.viewport.xmin * _DesmosFrameZoom) + 4500,
					right : (_DesmosState.graph.viewport.xmax * _DesmosFrameZoom) - 3000,
					bottom : (_DesmosState.graph.viewport.ymin * _DesmosFrameZoom) + 4500,
					top : (_DesmosState.graph.viewport.ymax * _DesmosFrameZoom) - 3000
				}
				(((_DesmosState.graph.viewport.xmax - _DesmosState.graph.viewport.xmin) < (_DesmosState.graph.viewport.ymax - _DesmosState.graph.viewport.ymin)) ? "preserveX" : "preserveY")
				contain|stretch|preserveX|preserveY
			*/


			window.DesmosCalculator.asyncScreenshot(
				{
					mode: "preserveY",
					width : 1280,
					height : 720
				},
				function (_ScreenshotPNGData) {

					/* Now save to the Server, the DesmosFrame */
					$("#CalcuatorContainer").hide();
					$("#LoadingMessageText").text("Saving the Rendered DesmosFrame (Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".SVG) to the Server...");

					/* Set the image for, and show, the #DesmosScreenshotPreview */
					$("#DesmosScreenshotPreview").attr("src", _ScreenshotPNGData);
					$("#DesmosScreenshotContainer").show();

					window.Utilities.SendAJAXPostRequest(
						("/API/VideoAPI.ASPX?Action=SaveDesmosFrame&VideoName=" + window.VideoName + "&FrameNumber=" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber)),
						{ DesmosFrameBase64PNG: _ScreenshotPNGData },
						function (_ResponseContent) {
							/* We have successfully Saved the Image to the Server. */
							window.Utilities.LogDebugMessageToConsole("Saved DesmosFrame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".PNG to the Server...");

							if (window.FrameNumber >= window.EndFrame) {
								window.Utilities.LogDebugMessageToConsole("All DesmosFrames have been Rendered and saved. Press OK, to be redirected to the DesmosFrames to Video Page.");
								window.AllFramesRenderedCallback();
							} else {
								/* The Next _FrameNumber should now be processed. */
								window.FrameNumber += 1;
								$("#RendererFramesPerMinuiteText").text(window.Utilities.RoundTo2DP((window.FrameNumber - window.StartFrame) / parseInt($("#RendererRunningTimeText").text().split(":")[0])));
								RenderNextDesmosFrame();
							}
						},
						function () {
							/* There was a Problem related to the POST Operation */
							window.Utilities.ShowMessageToUser("There was a problem posting the Rendered DesmosImage to the Server. See the console for more details...");

							/* We don't want to attempt to render more frames in the For loop, so... */
							throw new Error("There was a problem posting the Rendered DesmosImage to the Server. See the console for more details...");
						}
					);

				}
			);

		},
		function () {
			/* There was an error getting the DesmosState for the VideoFrame */
			window.Utilities.ShowMessageToUser("There was an error getting the DesmosState for the VideoFrame \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP\". Review messages in the Console for debugging infomation...");

			/* We don't want to attempt to render more frames in the For loop, so... */
			throw new Error("There was an error getting the DesmosState for the VideoFrame \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP\". Review messages in the Console for debugging infomation...");
		}
	);
}

function ShowExprListRenderNextDesmosFrame() {

	window.Utilities.LogDebugMessageToConsole("Rendering Next (ShowExprList) DesmosFrame. [window.FrameNumber] == " + window.FrameNumber.toString());

	/* Update the #RendererProgressText */
	$("#RendererProgressText").text("Progress: " + Math.round(((window.FrameNumber - window.StartFrame) / (window.EndFrame - window.StartFrame)) * 100).toString() + "%");

	$("#LoadingMessageText").text("Generating Beziers for Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP...");
	$("#LoadingMessageText").show();
	$("#LoadingMessageContainer").show();

	window.Utilities.SendAJAXRequest(
		("/API/VideoAPI.ASPX?Action=DesmosStateForVideoFrame&VideoName=" + window.VideoName + "&FrameNumber=" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber)),
		function (_ResponseContent) {
			/*
			We should have received the [DesmosState], and [ExpressionCount]

			If the VideoFrame wasn't blank, then there should also be a [SVGGenerationResultDataBase64] KeyValuePair in the JSON Response.
			*/

			$("#LoadingMessageContainer").hide();

			/* Hide the Screenshot Preview, which would still be showing from a previous Call if there was one */
			$("#DesmosScreenshotContainer").hide();

			/* Set the SVGInfoText */
			$("#SVGInfoText").html("Rendering \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".SVG\" using " + _ResponseContent.ExpressionCount + " Beziers...");

			/* Set the State received during the request */
			window.DesmosCalculator.setState(_ResponseContent.DesmosState);

			//Show the #CalcuatorContainer, as it was hidden until ready
			$("#CalcuatorContainer").show();

			/* Show Message "Waiting for Screenshot to be taken", until the async html2canvas callback changes it */
			$("#LoadingMessageText").text("Waiting for Desmos to Render Screenshot of Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".SVG...");

			/* If there is some SVGGenerationResultData, show it. */
			if (_ResponseContent.SVGGenerationResultDataBase64) {
				var _SVGGenResData = ((new DOMParser()).parseFromString(atob(_ResponseContent.SVGGenerationResultDataBase64), "text/xml").getElementsByTagName("SVGGenerationResultData")[0]);
				$("#SVGGenData").html(
					"Data from Last SVG Generation: " +
					"[RGDimensionsScale: " + _SVGGenResData.getAttribute("TerminalRG_DimensionsChangePercentage") + "%]; " +
					"[GeneratedSVGFileSizes: " + _SVGGenResData.getAttribute("GeneratedSVGFileSizes") + "]; " +
					"[UsedTSizes: " + _SVGGenResData.getAttribute("UsedTSizes") + "]"
				);
				window.Utilities.LogDebugMessageToConsole("ResultData from last SVG Generation:\r\n\r\n" + (new XMLSerializer()).serializeToString(_SVGGenResData));
				/*
				To get an Attribute from XML:
				((new DOMParser()).parseFromString(atob(_ResponseContent.SVGGenerationResultDataBase64), "text/xml").getElementsByTagName("SVGGenerationResultData")[0].getAttribute("GeneratedSVGFileSizes"))
				*/
			} else {
				window.Utilities.LogDebugMessageToConsole("No SVGGenerationResultData was received for the most recent Frame's DesmosState...");
			}

			/* Show the MessageContainer, in case it was hidden */
			$("#LoadingMessageContainer").show();

			var _DesmosState = window.DesmosCalculator.getState();

			/*
			Common _DesmosFrameZoom Sizes:
			- 4:3 in 16:9 Video:	0.87	WhoDoneThat
			- 4:3 Video:			1.0		Wombles
			Lower = More Zoomed in
			Higher = More zoomed out

			Licky	0.9		+4500,-3000,+4500,-3000

			var _DesmosFrameZoom = 0.3;

			mathBounds : {
			left : (_DesmosState.graph.viewport.xmin * _DesmosFrameZoom) + 4500,
			right : (_DesmosState.graph.viewport.xmax * _DesmosFrameZoom) - 3000,
			bottom : (_DesmosState.graph.viewport.ymin * _DesmosFrameZoom) + 4500,
			top : (_DesmosState.graph.viewport.ymax * _DesmosFrameZoom) - 3000
			}
			(((_DesmosState.graph.viewport.xmax - _DesmosState.graph.viewport.xmin) < (_DesmosState.graph.viewport.ymax - _DesmosState.graph.viewport.ymin)) ? "preserveX" : "preserveY")
			contain|stretch|preserveX|preserveY
			*/

			/*
			We firstly need to wait for Desmos to be able to Render a Screenshot of the Graph,
			because only after that can we know that the Graph is actually ready to take the screenshot of...
			*/

			window.DesmosCalculator.asyncScreenshot(
				{ mode: "preserveY" },
				function (_ScreenshotPNGData) {

					/* We now know that the Desmos Rendering is ready to be screenshot... */
					$("#LoadingMessageText").text("Waiting for HTML2Canvas to Render Screenshot of Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".SVG...");

					/* Render a ScreenShot of the Desmos Element  */
					html2canvas(
						document.querySelector("#Calculator"),
						{ scale: 1.5 }
					).then(
						function (_ScreenshotCanvas) {
							/* We now have a canvas containing the Screenshot */

							window.Utilities.LogDebugMessageToConsole("Obtained Canvas containing Calcuator Screenshot; POSTing to Server...");

							var _ScreenshotPNGData = _ScreenshotCanvas.toDataURL(); /* Starts with data:image/png;base64, */

							/* Now save to the Server, the ShowExprListDesmosFrame */
							$("#CalcuatorContainer").hide();
							$("#LoadingMessageText").text("Saving the Rendered ShowExprList DesmosFrame (Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".SVG) to the Server...");

							/* Set the image for, and show, the #DesmosScreenshotPreview */
							$("#DesmosScreenshotPreview").attr("src", _ScreenshotPNGData);
							$("#DesmosScreenshotContainer").show();

							window.Utilities.SendAJAXPostRequest(
								("/API/VideoAPI.ASPX?Action=SaveDesmosFrame&VideoName=" + window.VideoName + "&FrameNumber=" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber)),
								{ DesmosFrameBase64PNG: _ScreenshotPNGData },
								function (_ResponseContent) {
									/* We have successfully Saved the Image to the Server. */
									window.Utilities.LogDebugMessageToConsole("Saved ShowExprList DesmosFrame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".PNG to the Server...");

									if (window.FrameNumber >= window.EndFrame) {
										window.Utilities.LogDebugMessageToConsole("All DesmosFrames have been Rendered and saved. Press OK, to be redirected to the DesmosFrames to Video Page.");
										window.AllFramesRenderedCallback();
									} else {
										/* The Next _FrameNumber should now be processed. */
										window.FrameNumber += 1;
										$("#RendererFramesPerMinuiteText").text(window.Utilities.RoundTo2DP((window.FrameNumber - window.StartFrame) / parseInt($("#RendererRunningTimeText").text().split(":")[0])));
										ShowExprListRenderNextDesmosFrame();
									}
								},
								function () {
									/* There was a Problem related to the HTTP-POST Operation */
									window.Utilities.ShowMessageToUser("There was a problem posting the Rendered DesmosImage to the Server. See the console for more details...");

									/* We don't want to attempt to render more frames in the For loop, so... */
									throw new Error("There was a problem posting the Rendered DesmosImage to the Server. See the console for more details...");
								}
							);
						}
					);
				}
			);
		},
		function () {
			/* There was an error getting the DesmosState for the VideoFrame */
			window.Utilities.ShowMessageToUser("There was an error getting the DesmosState for the VideoFrame \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP\". Review messages in the Console for debugging infomation...");

			/* We don't want to attempt to render more frames in the For loop, so... */
			throw new Error("There was an error getting the DesmosState for the VideoFrame \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP\". Review messages in the Console for debugging infomation...");
		}
	);
}

function PointPlotRenderNextDesmosFrame() {

	window.Utilities.LogDebugMessageToConsole("PointPlotRendering Next DesmosFrame. [window.FrameNumber] == " + window.FrameNumber.toString());

	/* Update the #RendererProgressText */
	$("#RendererProgressText").text("Progress: " + Math.round(((window.FrameNumber - window.StartFrame) / (window.EndFrame - window.StartFrame)) * 100).toString() + "%");

	$("#LoadingMessageText").text("Requesting PointPlot DesmosState for Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP...");
	$("#LoadingMessageText").show();
	$("#LoadingMessageContainer").show();

	window.Utilities.SendAJAXRequest(
		("/API/VideoAPI.ASPX?Action=PointPlottedDesmosStateForFrame&VideoName=" + window.VideoName + "&FrameNumber=" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber)),
		function (_ResponseContent) {
			/* We should have received the [DesmosState], and [ExpressionCount] */

			$("#LoadingMessageContainer").hide();

			/* Hide the Screenshot Preview, which would still be showing from a previous Call if there was one */
			$("#DesmosScreenshotContainer").hide();

			/* Set the SVGInfoText */
			$("#DesmosStateInfoText").html("Rendering \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP\" using " + _ResponseContent.ExpressionCount + " Points...");

			/* Set the State received during the request */
			window.DesmosCalculator.setState(_ResponseContent.DesmosState);

			//Show the #CalcuatorContainer, as it was hidden until ready
			$("#CalcuatorContainer").show();

			/* Show Message "Waiting for Screenshot to be taken", until the async callback to  */
			$("#LoadingMessageText").text("Waiting for Desmos to Render Screenshot of Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP...");
			$("#LoadingMessageContainer").show();

			/*
			Common _DesmosFrameZoom Sizes:
			- 4:3 in 16:9 Video:	0.87	WhoDoneThat
			- 4:3 Video:			1.0		Wombles
			Lower = More Zoomed in
			Higher = More zoomed out

			Licky	0.9		+4500,-3000,+4500,-3000

			var _DesmosFrameZoom = 0.3;

			mathBounds : {
				left : (_DesmosState.graph.viewport.xmin * _DesmosFrameZoom) + 4500,
				right : (_DesmosState.graph.viewport.xmax * _DesmosFrameZoom) - 3000,
				bottom : (_DesmosState.graph.viewport.ymin * _DesmosFrameZoom) + 4500,
				top : (_DesmosState.graph.viewport.ymax * _DesmosFrameZoom) - 3000
			}
			(((_DesmosState.graph.viewport.xmax - _DesmosState.graph.viewport.xmin) < (_DesmosState.graph.viewport.ymax - _DesmosState.graph.viewport.ymin)) ? "preserveX" : "preserveY")
			contain|stretch|preserveX|preserveY
			*/

			var _DesmosState = window.DesmosCalculator.getState();
			var _DesmosFrameZoom = 0.6;

			window.DesmosCalculator.asyncScreenshot(
				{
					mathBounds : {
						left : (_DesmosState.graph.viewport.xmin * _DesmosFrameZoom),
						right : (_DesmosState.graph.viewport.xmax * _DesmosFrameZoom),
						bottom : (_DesmosState.graph.viewport.ymin * _DesmosFrameZoom),
						top : (_DesmosState.graph.viewport.ymax * _DesmosFrameZoom)
					},
					mode: "preserveY",
					width: 1280,
					height: 720
				},
				function (_ScreenshotPNGData) {

					/* Now save to the Server, the DesmosFrame */
					$("#CalcuatorContainer").hide();
					$("#LoadingMessageText").text("Saving the Rendered DesmosFrame (Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP) to the Server...");

					/* Set the image for, and show, the #DesmosScreenshotPreview */
					$("#DesmosScreenshotPreview").attr("src", _ScreenshotPNGData);
					$("#DesmosScreenshotContainer").show();

					window.Utilities.SendAJAXPostRequest(
						("/API/VideoAPI.ASPX?Action=SaveDesmosFrame&VideoName=" + window.VideoName + "&FrameNumber=" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber)),
						{ DesmosFrameBase64PNG: _ScreenshotPNGData },
						function (_ResponseContent) {
							/* We have successfully Saved the Image to the Server. */
							window.Utilities.LogDebugMessageToConsole("Saved DesmosFrame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".PNG to the Server...");

							if (window.FrameNumber >= window.EndFrame) {
								window.Utilities.LogDebugMessageToConsole("All DesmosFrames have been Rendered and saved. Press OK, to be redirected to the DesmosFrames to Video Page.");
								window.AllFramesRenderedCallback();
							} else {
								/* The Next _FrameNumber should now be processed. */
								window.FrameNumber += 1;
								$("#RendererFramesPerMinuiteText").text(window.Utilities.RoundTo2DP((window.FrameNumber - window.StartFrame) / parseInt($("#RendererRunningTimeText").text().split(":")[0])));
								PointPlotRenderNextDesmosFrame();
							}
						},
						function () {
							/* There was a Problem related to the POST Operation */
							window.Utilities.ShowMessageToUser("There was a problem posting the Rendered DesmosImage to the Server. See the console for more details...");

							/* We don't want to attempt to render more frames in the For loop, so... */
							throw new Error("There was a problem posting the Rendered DesmosImage to the Server. See the console for more details...");
						}
					);

				}
			);

		},
		function () {
			/* There was an error getting the DesmosState for the VideoFrame */
			window.Utilities.ShowMessageToUser("There was an error getting the DesmosState for the VideoFrame \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP\". Review messages in the Console for debugging infomation...");

			/* We don't want to attempt to render more frames in the For loop, so... */
			throw new Error("There was an error getting the DesmosState for the VideoFrame \"Frame" + window.Utilities.MakeFiveDigitNumber(window.FrameNumber) + ".BMP\". Review messages in the Console for debugging infomation...");
		}
	);
}

function SendDesmosFramesToVideoAPIRequest(_VideoName) {

	$("#VideoGenerationLoadingMessage").show();

	if (_VideoName) {

		window.Utilities.SendAJAXRequest(
			("/API/VideoAPI.ASPX?Action=DesmosFramesToVideo&VideoName=" + _VideoName),
			function (_ResponseContent) {
				/* We should have had returned to us, the HREF to the Final Output generated. */

				$("#VideoNameSelectionContainer").hide();

				$("#OutputVideoHref").attr("href", _ResponseContent.FinalOutputHref);
				$("#OutputHyperlinkContainer").show();
				$("#VideoGenerationLoadingMessage").hide();
			},
			function () {
				window.Utilities.LogErrorMessageToConsole("There was a problem Generating the Video, from DesmosFrames for: " + _VideoName);
				window.Utilities.ShowMessageToUser("There was a problem Generating the Video, from DesmosFrames, for: " + _VideoName + ". View the Verbose Debugging Messages in the Console for the Exception Message...");
			}
		);
	} else {
		window.Utilities.ShowMessageToUser("An attempt was made to Request the VideoFile from some DesmosFrames, however the _VideoName was blank.");
	}
}

function SendApplyAudioAPIRequest(_VideoName) {
	
	if (_VideoName) {

		/* The _VideoName here does contain the DesmosRendered Part, so remove this by removing the first 15 Chars */
		_VideoName = _VideoName.substring(15);

		window.Utilities.SendAJAXRequest(
			("/API/VideoAPI.ASPX?Action=GenerateAudioVersionOfDesmosVideo&VideoName=" + _VideoName),
			function (_ResponseContent) {
				/* We should have had returned to us, the HREF to the Final Output WITH AUDIO generated. */

				$("#LoadingMessageContainer").hide();
				$("#VideoNameSelectionContainer").hide();

				$("#OutputVideoHref").attr("href", _ResponseContent.FinalOutputWithAudioHref);
				$("#OutputHyperlinkContainer").show();
			},
			function () {
				window.Utilities.LogErrorMessageToConsole("There was a problem Applying the AudioTrack for: " + _VideoName);
				window.Utilities.ShowMessageToUser("There was a problem Applying an AudioTrack for: " + _VideoName + ". View the Verbose Debugging Messages in the Console for the Exception Message...");
			}
		);

	} else {
		window.Utilities.ShowMessageToUser("An attempt was made to Request the Application of Audio to a DesmosVideo, however the _VideoName was blank.");
	}
}

/* </VideoRelated> */

function GenerateSVGForRasterGraphicThenRedirectToDesmosRenderingOfIt(_RasterGraphicFileName) {
	
	/* Send an API Request, to Generate an SVG for the _RasterGraphicFileName */
	if (_RasterGraphicFileName) {

		var _PreviousText = $("#LoadingMessageText").text();
		$("#LoadingMessageText").text("Generating SVG for " + _RasterGraphicFileName + "...");
		$("#LoadingMessage").show();

		window.Utilities.SendAJAXRequest(
			("/API/Get.ASPX?Item=SVGFromRasterGraphic&RasterGraphicFileName=" + _RasterGraphicFileName),
			function (_ResponseContent) {
				
				/* We should have had returned to us, the "SVGFileName", of the SVG generated. */

				/* Now we can just redirect the user to the Desmos Page for that SVGFileName */
				window.location.href = ("/ClientPages/SVGToDesmos.ASPX?SVGFileName=" + _ResponseContent.SVGFileName);
				
			},
			function () {
				window.Utilities.LogErrorMessageToConsole("There was a problem Generating an SVG for: " + _RasterGraphicFileName);
				window.Utilities.ShowMessageToUser("There was a problem Generating an SVG for: " + _RasterGraphicFileName + ". View the Verbose Debugging Messages in the Console for the Exception Message...");
			}
		);

	} else {
			window.Utilities.ShowMessageToUser("An attempt was made to Redirect to the DesmosRenditioin of a RasterGraphic, however the _RasterGraphicFileName was blank.");
	}
}

function GetLatestUploadedRGsList(_CallbackOnReceivedList) {
	
	/* This function will update window.KnownUploadedRasterImages with any new lists received */

	if (!window.KnownUploadedRasterImages) {
		window.Utilities.ShowMessageToUser("An attempt was made to get the Latest Uploaded RasterGraphics as a list, however window.KnownUploadedRasterImages was undefined.");
	} else {

		window.Utilities.SendAJAXRequest(
			("/API/Get.ASPX?Item=AwaitChangeInUploadedRasterImagesList&ClientKnownUploadedRasterImages=" + encodeURIComponent(window.KnownUploadedRasterImages)),
			function (_ResponseContent) {
				
				/* We should have had returned to us, "RequestTimedOut", and "KnownUploadedRasterImages" */

				if (_ResponseContent.RequestTimedOut.toUpperCase() == "TRUE") {
					/* Send another request... */
					window.Utilities.LogDebugMessageToConsole("Request to check for a new UploadedRasterGraphic File List from the Server timed out; sending another one...");
					GetLatestUploadedRGsList(_CallbackOnReceivedList);
				} else {
					/* The Request didn't Time Out, so it was sent back to us because the Folder Changed */
					
					window.Utilities.LogDebugMessageToConsole("Received a new UploadedRasterGraphic File List from the Server.");

					var _NewKnownUploadedRGsList = "";
					for (var _Index = 0; _Index != _ResponseContent.UploadedRasterImages.length; ++_Index) {
						_NewKnownUploadedRGsList += (_ResponseContent.UploadedRasterImages[_Index].FileName + "|");
					}

					/* Remove the last "|" */
					_NewKnownUploadedRGsList = _NewKnownUploadedRGsList.substring(0, _NewKnownUploadedRGsList.length - 1);
					
					window.KnownUploadedRasterImages = _NewKnownUploadedRGsList;
					window.Utilities.LogDebugMessageToConsole("Updated the window.KnownUploadedRasterImages list to: " + window.KnownUploadedRasterImages);

					_CallbackOnReceivedList(_ResponseContent.UploadedRasterImages);
				}
			},
			function () {
				window.Utilities.LogErrorMessageToConsole("There was a problem getting the Latest Uploaded RasterGraphics as a list");
				window.Utilities.ShowMessageToUser("There was a problem getting the Latest Uploaded RasterGraphics as a list; View the Verbose Debugging Messages in the Console for the Exception Message...");
			}
		);
	}

}

function UploadRasterGraphicViaAJAX(_HTMLInputFileObject) {
	window.Utilities.LogDebugMessageToConsole("Uploading this file to the Server: " + _HTMLInputFileObject.name + "...");
	/* 07-02-2022	NOT WRITTEN YET */
}