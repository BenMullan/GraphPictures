Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Namespace CompilerExtentions

		REM There exists also the WebObjectExtentions Module in the Library
		Public Module HTTPObjectExtentions

			''' <summary>
			''' (MullNet CompilerExtention) Writes the opening html, head (etc...) elements, importing the required scripts too.
			''' </summary>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Sub WriteScriptAndCSSImports(ByRef _ResponseObject As HttpResponse)

				_ResponseObject.Write(
				 (<resources>
					  <!--JavaScript-->
					  <script type="text/javascript" src="/ClientPages/Resources/JQuery-3.5.1.Min.JS"></script>
					  <script type="text/javascript" src="/ClientPages/Resources/JQuery-UI-1.12.1.JS"></script>
					  <script type="text/javascript" src="/ClientPages/Resources/General.JS"></script>
					  <script type="text/javascript" src="/ClientPages/Resources/Utilities.JS"></script>
					  <script type="text/javascript" src="/ClientPages/Resources/EntryPoints.JS"></script>
					  <script type="text/javascript" src="/ClientPages/Resources/AJAX.JS"></script>
					  <!---CSS-->
					  <link rel="stylesheet" href="/ClientPages/Resources/General.CSS"/>
					  <link rel="stylesheet" href="/ClientPages/Resources/Utilities.CSS"/>
				  </resources>).ToString()
				 )

				Return
			End Sub

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Specified Required QueryString Keys are contained in the current Request Object.
			''' </summary>
			''' <param name="_RequestObject"></param>
			''' <param name="_ResponseObject">This is needed in case a QueryString is missing, in which case a JSON Response is sent back, informing the user of the missing Parameter</param>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Sub CheckRequiredQueryStringsAreSpecified(ByRef _RequestObject As HttpRequest, ByRef _ResponseObject As HttpResponse, ByVal ParamArray _RequiredQueryStringKeys$())

				For Each _RequiredQueryStringKey$ In _RequiredQueryStringKeys

					If Not _RequestObject.QueryStringIsSpecifiedAndHasValue(_RequiredQueryStringKey) Then
						_ResponseObject.ContentType = "application/json"
						_ResponseObject.Write("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""No ?" & _RequiredQueryStringKey$ & " was specified with a Value."" }, ""ResponseContent"" : {} }")
						_ResponseObject.End()
					End If

				Next

				Return

			End Sub

			''' <summary>
			''' (MullNet CompilerExtention) Checks that the Specified Required Post Data Keys are contained in the current Request Object.
			''' </summary>
			''' <param name="_RequestObject"></param>
			''' <param name="_ResponseObject">This is needed in case a PostData Key is missing, in which case a JSON Response is sent back, informing the user of the missing Parameter</param>
			''' <remarks></remarks>
			<Global.System.Runtime.CompilerServices.Extension()>
			Public Sub CheckRequiredPostDataKeysAreSpecified(ByRef _RequestObject As HttpRequest, ByRef _ResponseObject As HttpResponse, ByVal ParamArray _RequiredPostDataKeys$())

				For Each _RequiredPostDataKey$ In _RequiredPostDataKeys

					If Not _RequestObject.PostDataItemIsSpecified(_RequiredPostDataKey) Then
						_ResponseObject.ContentType = "application/json"
						_ResponseObject.Write("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The Post Data Key """ & _RequiredPostDataKey$ & """ was not specified."" }, ""ResponseContent"" : {} }")
						_ResponseObject.End()
					End If

				Next

				Return

			End Sub

		End Module

	End Namespace

End Namespace