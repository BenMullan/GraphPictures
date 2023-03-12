window.Utilities = {

	FullScreenElement: function (_ElementJQuerySelectionString) {
		var _Element = $(_ElementJQuerySelectionString).get(0);
		if (_Element.requestFullscreen) {
			_Element.requestFullscreen();
		} else if (_Element.msRequestFullscreen) {
			_Element.msRequestFullscreen();
		} else if (_Element.mozRequestFullScreen) {
			_Element.mozRequestFullScreen();
		} else if (_Element.webkitRequestFullscreen) {
			_Element.webkitRequestFullscreen();
		}
		window.Utilities.LogDebugMessageToConsole("Made element \"" + _ElementJQuerySelectionString + "\" FullScreen...");
	},

	RoundTo2DP: function (_Number) {
		return (Math.round((_Number + Number.EPSILON) * 100) / 100);
	},

	MakeTimeCounter: function (_JQuerySelectorForTimerText) {

		/* Outputs the time in the format MM:SS */

		window.TimeCounterStartTime = (new Date()).getTime();

		window.UpdateTimeCounter = setInterval(
			function () {

				var _Seconds = Math.floor((((new Date()).getTime()) - window.TimeCounterStartTime) / 1000);
				var _Minuites = Math.floor(_Seconds / 60);
				_Seconds -= (_Minuites * 60);

				$(_JQuerySelectorForTimerText).text(_Minuites.toString() + ":" + window.Utilities.MakeTwoDigitNumber(_Seconds.toString()));

			},
			1000
		);
	},

	DownloadFileFromJavaScript: function (_FileName, _Text) {

		var element = document.createElement('a');
		element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(_Text));
		element.setAttribute('download', _FileName);

		element.style.display = 'none';
		document.body.appendChild(element);

		element.click();

		document.body.removeChild(element);
	},

	MakeFiveDigitNumber: function (_Number) {
		if (_Number) {
			_Number = _Number.toString();
			while (_Number.length < 5) { _Number = ("0" + _Number); }
			return _Number;
		} else {
			window.Utilities.LogErrorMessageToConsole("An attempt was made to Make a Number into a Five-Digit String, however the _Number was null...");
		}
	},

	MakeTwoDigitNumber: function (_Number) {
		if (_Number) {
			_Number = _Number.toString();
			while (_Number.length < 2) { _Number = ("0" + _Number); }
			return _Number;
		} else {
			window.Utilities.LogErrorMessageToConsole("An attempt was made to Make a Number into a Five-Digit String, however the _Number was null...");
		}
	},

	ShowMessageToUser: function (_MessageText) {
		window.alert(_MessageText);
	},

	GetCurrentDateTimeString: function () {
		return ("[" + (new Date()).toUTCString() + "]");
	},

	LogDebugMessageToConsole: function (_MessageText) {

		/* Attempt to Resolve who just called this Function */
		var _CallerName = "(Unresolved Caller)";
		try {
			_CallerName = ("[Invoker: " + window.Utilities.LogDebugMessageToConsole.caller.toString().substring(9).split("(")[0] + "()]");
		} catch (_Error) {
			_CallerName += " (Error on resolving caller: " + _Error + ")";
		}

		window.console.debug("%c" + window.Utilities.GetCurrentDateTimeString() + "\t" + _CallerName + "\r\n%c" + _MessageText, "background: #575757; color: #FFFFFF;", "background: #FFFFFF; color: #000000;");
	},

	LogWarningMessageToConsole: function (_MessageText) {
		window.console.warn(window.Utilities.GetCurrentDateTimeString() + "	" + _MessageText);
	},

	LogErrorMessageToConsole: function (_MessageText) {
		window.console.error(window.Utilities.GetCurrentDateTimeString() + "	" + _MessageText);
	},

	SendAJAXRequest: function (_URL, _CallbackForSuccessfulRequest, _CallbackForUnsuccessfulRequest) {
		try {
			/* Start to time how long this AJAX Request takes */
			window.AJAXRequestStartTime = performance.now();

			var _RequestString = _URL;
			window.Utilities.LogDebugMessageToConsole("Making AJAX Request: " + _RequestString + "...");

			$.get(
				_RequestString,
				function (_DataJSON, _StatusText) {

					window.Utilities.LogDebugMessageToConsole("\r\n----------------------------------------------------------\r\n\r\nRecieved data from server during AJAX request. First 300 Chars of Response:\r\n\r\n" + (JSON.stringify(_DataJSON).length > 300 ? JSON.stringify(_DataJSON).substring(0, 300) : JSON.stringify(_DataJSON)) + "\r\n\r\nStatusText: " + _StatusText + "\r\nResponse Length: " + JSON.stringify(_DataJSON).length.toString() + " Chars\r\nTime Taken to receive Response: " + ((performance.now() - window.AJAXRequestStartTime) / 1000).toString() + " Seconds\r\n\r\n----------------------------------------------------------");

					/* Check that the Server's Response contains what we want; if "DataAboutRequest.IsValidRequest" == "false", then there was probably a problem on the Server */
					if (_DataJSON.DataAboutResponse.IsValidRequest.toLowerCase() == "true") {

						/* A cached copy of the AJAX Response is saved, in case it is needed later... */
						window.LastAJAXResponse = _DataJSON;

						/* At this Point, there has been no error sending the AJAX Request, and the Server has responded with an IsValidRequest == "true" */
						_CallbackForSuccessfulRequest(_DataJSON.ResponseContent);

					} else {

						/* The Server has responded with IsValidRequest = "false"; there may have been a problem on the Server */
						window.Utilities.LogErrorMessageToConsole("The Server has just sent back an [IsValidRequest == \"false\"] during an AJAX Request; there may have been a problem on the Server. The Reason was:\r\n\r\n" + _DataJSON.DataAboutResponse.Reason);
						_CallbackForUnsuccessfulRequest();
					}
				}
			).fail(
			/* jQuery has detected that the $.get() was not successfull */
				function (_Error, _Status) {
					window.Utilities.LogErrorMessageToConsole("$.get() faliure during AJAX request: " + JSON.stringify(_Error) + ", " + JSON.stringify(_Status));
					_CallbackForUnsuccessfulRequest();
				}
			);
		} catch (_Error) {
			/* The $.get() was not able to run (either entirely, or partially) */
			window.Utilities.LogErrorMessageToConsole("$.get() Fell into Catch Statement during AJAX request. Error: " + JSON.stringify(_Error));
			_CallbackForUnsuccessfulRequest();
		}
	},

	SendAJAXPostRequest: function (_URL, _FormData, _CallbackForSuccessfulRequest, _CallbackForUnsuccessfulRequest) {
		try {
			var _RequestString = _URL;
			window.Utilities.LogDebugMessageToConsole("Making AJAX Post Request: " + _RequestString + "...");

			$.post(
				_RequestString,
				_FormData,
				function (_DataJSON) {

					window.Utilities.LogDebugMessageToConsole("Recieved data from server during AJAX Post request. Data: " + JSON.stringify(_DataJSON));

					/* Check that the Server's Response contains what we want; if "DataAboutRequest.IsValidRequest" == "false", then there was probably a problem on the Server */
					if (_DataJSON.DataAboutResponse.IsValidRequest.toLowerCase() == "true") {

						/* At this Point, there has been no error sending the AJAX Request, and the Server has responded with an IsValidRequest == "true" */
						_CallbackForSuccessfulRequest(_DataJSON.ResponseContent);

					} else {

						/* The Server has responded with IsValidRequest = "false"; there may have been a problem on the Server */
						window.Utilities.LogErrorMessageToConsole("The Server has just sent back an [IsValidRequest == \"false\"] during an AJAX Post Request; there may have been a problem on the Server. The Reason was: " + _DataJSON.DataAboutResponse.Reason);
						_CallbackForUnsuccessfulRequest();
					}
				}
			).fail(
			/* jQuery has detected that the $.get() was not successfull */
				function (_Error, _Status) {
					window.Utilities.LogErrorMessageToConsole("$.get() faliure during AJAX Post request: " + JSON.stringify(_Error) + ", " + JSON.stringify(_Status));
					_CallbackForUnsuccessfulRequest();
				}
			);
		} catch (_Error) {
			/* The $.get() was not able to run (either entirely, or partially) */
			window.Utilities.LogErrorMessageToConsole("$.get() Fell into Catch Statement during AJAX Post request. Error: " + JSON.stringify(_Error));
			_CallbackForUnsuccessfulRequest();
		}
	}

};