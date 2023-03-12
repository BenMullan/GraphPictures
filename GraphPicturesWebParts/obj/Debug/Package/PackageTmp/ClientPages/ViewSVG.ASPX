<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	If Request.QueryStringIsSpecified("SVGFileName") Then
	
		Dim _RequestedSVGFileFullPath$ = GraphPictures.WebParts.Resources.WWWSVGsFolderPath & Request.QueryString("SVGFileName").ToString()

		If (Not IO.File.Exists(_RequestedSVGFileFullPath)) Then
			Response.ContentType = "application/json"
			Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The specified SVG File (" & Request.QueryString("SVGFileName") & ") does not exist in the [/ClientData/SVGFiles/] Directory."" }, ""ResponseContent"" : {} }")
		End If
	
		Response.ContentType = "image/svg+xml"
		Response.WriteFile(_RequestedSVGFileFullPath) : Response.SafelyEnd()
	Else
		REM No ?SVGFileName is specified; list the avaliable ones for Viewing
		
		'{Write the rest of the page...}
		
	End If
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title><%= If(Request.QueryStringIsSpecified("SVGFileName"), Request.QueryString("SVGFileName").ToString(), "*")%> in [SVG Viewer \\ GraphPictures]</title>
	</head>
	
	<body onload="" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ SVG Viewer</div>

		<!----------------------------- The Avaliable SVG's Container ----------------------------->
		<div class="GreyBorder Shadow Container">
			Avaliable SVGFiles:<br/><br/>
			<ul>
			<%
				For Each _File As IO.FileInfo In (New IO.DirectoryInfo(GraphPictures.WebParts.Resources.WWWSVGsFolderPath)).GetFiles()
					Response.Write("<li><a href=""/ClientPages/ViewSVG.ASPX?SVGFileName=" & _File.Name & """>" & _File.Name & "</a></li>")
				Next
			%>
			</ul>
		</div>

	</body>
	
</html>

<% 	Response.SafelyEnd()%>