<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	Request.CheckRequiredQueryStringsAreSpecified(Response, "VideoFileName")
	If Not IO.File.Exists(GraphPictures.WebParts.WWWVideosFolderPath & Request.QueryString!VideoFileName.ToString()) Then Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The Specified ?VideoFileName does not Exits on Disk"" }, ""ResponseContent"" : {} }")
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title><%= Request.QueryString("VideoFileName").ToString()%> in [Video Viewer \\ GraphPictures]</title>
	</head>
	
	<body onload="" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ Video Viewer</div>

		<!----------------------------- The Video Container ----------------------------->
		<div class="GreyBorder Shadow Container">
			<div class="SubHeading">Viewing "<%= Request.QueryString("VideoFileName").ToString()%>"</div><br/>
			<video height="75%" controls>
				<source src="/ClientData/Videos/<%= Request.QueryString!VideoFileName.ToString() %>">
				This WebBrowser does not support HTML5 Embedded Video
			</video>
		</div>

	</body>
	
</html>

<% 	Response.SafelyEnd()%>