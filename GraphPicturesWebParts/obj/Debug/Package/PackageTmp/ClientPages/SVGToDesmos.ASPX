<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	If Request.QueryStringIsSpecified("SVGFileName") Then
	
		'Check that the ?SVGFileName actually exists on Disk...
		If (Not IO.File.Exists(GraphPictures.WebParts.Resources.WWWSVGsFolderPath & Request.QueryString("SVGFileName"))) Then
			Response.ContentType = "application/json"
			Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The specified SVG File (" & Request.QueryString("SVGFileName") & ") does not exist in the [/ClientData/SVGFiles/] Directory."" }, ""ResponseContent"" : {} }")
		End If
		
		'Else, the File does exist, so Write the rest of the page...
		
	Else
		REM No ?SVGFileName is specified; Write the rest of the page
		
	End If
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<script type="text/javascript" src="https://www.desmos.com/api/v1.6/calculator.js?apiKey=dcb31709b452b1cf9dc26972add0fda6"></script>
		<title><%= If(Request.QueryStringIsSpecified("SVGFileName"), Request.QueryString("SVGFileName").ToString(), "*") %> in [SVGToDesmos \\ GraphPictures]</title>
	</head>
	
	<body onload="<%
		If Request.QueryStringIsSpecified("SVGFileName") Then
			'Automatically get the page to render that SVG (with the Server's Default DesmosGraphCreationOptions), as soon as it loads...
			Response.Write("SetDesmosStateForSVG('" & Request.QueryString("SVGFileName") & "');")
		Else
			'Let the user choose which SVGFile to render...
			Response.Write("SVGToDesmosEntryPoint();")
		End If
	%>" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ SVG to Desmos</div>

		<!----------------------------- The Loading Message ----------------------------->
		<div id="CalcuatorLoadingMessage" class="GreyBorder Shadow Container">
			<img alt="Loading Wheel" src="./Resources/LoadingGif.GIF" height="15%"/>
			<br/>
			Requesting Graph Data from Server...
		</div>

		<!----------------------------- The SVGInterpretationOptions Container ----------------------------->
		<div id="SVGInterpretationOptionsContainer" class="GreyBorder Shadow Container" style="display: none;">
			
			<div class="SubHeading">SVG Interpretation Options:</div><br/><br/>

			<label for="ColourAllocationModeSelector">LineColour Algorithm: </label>
			<select name="ColourAllocationModeSelector" id="ColourAllocationModeSelector">
				<%
					For Each _ColourAllocationMode$ In [Enum].GetNames(GetType(GraphPictures.Library.ColourResources.ColourRange.ColourGenerationMode))
						Response.Write("<option value=""" & _ColourAllocationMode & """>" & _ColourAllocationMode & "</option>")
					Next
				%>
			</select><br/>

			<label for="LineStyleSelector">Line Style: </label>
			<select name="LineStyleSelector" id="LineStyleSelector">
				<%
					For Each _LineStyle$ In [Enum].GetNames(GetType(GraphPictures.Library.Resources.DesmosStateCreationOptions.DesmosLineStyle))
						Response.Write("<option value=""" & _LineStyle & """>" & _LineStyle & "</option>")
					Next
				%>
			</select><br/>

			Show GridLines<input type="checkbox" id="ShowGridLinesCheckBox" checked="checked"/><br/>
			Fill<input type="checkbox" id="FillCheckBox"/><br/>
			Fill Opacity<input oninput="$('#FillOpacityValueText').text('(' + this.value.toString() + ')');" onchange="$('#FillOpacityValueText').text('(' + this.value.toString() + ')');" type="range" min="0" max="1" step="0.01" value="0.42" id="FillOpacitySlider"/><div id="FillOpacityValueText" style="display: inline;">(0.42)</div><br/>
			Line Opacity<input oninput="$('#LineOpacityValueText').text('(' + this.value.toString() + ')');" onchange="$('#LineOpacityValueText').text('(' + this.value.toString() + ')');" type="range" min="0" max="1" step="0.01" value="1" id="LineOpacitySlider"/><div id="LineOpacityValueText" style="display: inline;">(1)</div><br/>
			Line Thickness<input oninput="$('#LineThicknessValueText').text('(' + this.value.toString() + ')');" onchange="$('#LineThicknessValueText').text('(' + this.value.toString() + ')');" type="range" min="1" max="30" step="1" value="2" id="LineThicknessSlider"/><div id="LineThicknessValueText" style="display: inline;">(2)</div>

		</div>

		<!----------------------------- The SVGFileSelector Container ----------------------------->
		<div id="SVGFileSelectorContainer" class="GreyBorder Shadow" style="display: none;">
			<div class="SubHeading">Click on an SVGFile to Render it as a DesmosGraph:</div><br/>
			
			<%
				For Each _SVGFile As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.WWWSVGsFolderPath)).GetFiles()
					Response.Write(String.Format("<div onclick=""SetDesmosStateForSVG('{0}', {{ " & vbCrLf & _
					 "ShowGrid : document.getElementById('ShowGridLinesCheckBox').checked.toString(), " & vbCrLf & _
					 "ColourAllocationMode : $('#ColourAllocationModeSelector').find(':selected').text(), " & vbCrLf & _
					 "LinesOpacity : $('#LineOpacitySlider').val(), " & vbCrLf & _
					 "LinesWidth : $('#LineThicknessSlider').val(), " & vbCrLf & _
					 "LinesStyle : $('#LineStyleSelector').find(':selected').text(), " & vbCrLf & _
					 "Fill : document.getElementById('FillCheckBox').checked.toString(), " & vbCrLf & _
					 "FillOpacity : $('#FillOpacitySlider').val()" & vbCrLf & _
					 "}});"" class=""SVGFilePreview""><img src=""/ClientPages/ViewSVG.ASPX?SVGFileName={0}"" width=""100%"" height=""100%""/><a href=""/ClientPages/ViewSVG.ASPX?SVGFileName={0}"">{0}</a><br/>({1}KB)</div>", _SVGFile.Name, _SVGFile.SizeInIntegralKB()) & vbCrLf & vbCrLf & vbCrLf)
				Next
			%>
			
		</div>

		<!--------------------------- The Desmos Calcuator Box --------------------------->
		<div id="CalcuatorContainer" class="Shadow GreyBorder Container" style="display: none;">
			<div id="SVGInfoText"></div>
			<br/>

			<!-- GraphEnlargement Options -->
			<a href="javascript:window.Utilities.FullScreenElement('#Calculator');" title="Make the Desmos Element FullScreen...">FullScreen</a>
			<div style="padding-left:10%; display:inline;"/>
			<a href="javascript:$('#Calculator').attr('style', 'position: absolute; top: 0; left: 0; width: 100%; height: 100%;');$('body').css('overflow', 'hidden');" title="Make the Desmos Element occoupy the full space of the WebPage...">FullPage</a>
			
			<br/>
			<div id="Calculator"/>
		</div>
		
	</body>
	
</html>
<% 	Response.SafelyEnd()%>