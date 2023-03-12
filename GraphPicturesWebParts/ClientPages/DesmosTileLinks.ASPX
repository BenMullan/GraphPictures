<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	Request.CheckRequiredQueryStringsAreSpecified(Response, "SVGFileName", "TileCount")
	
	REM Syntactically Validate the ?TileCount
	If Not Request.QueryString!TileCount.MatchesRegEx("^\d{1,2}\,\d{1,2}$") Then Response.ClearAndWriteAndEnd("{ ""DataAboutResponse"" : { ""IsValidRequest"" : ""false"", ""Reason"" : ""The specified ?TileCount was syntactically-invalid."" }, ""ResponseContent"" : {} }")
%>
<!-- Thomas Brockett and Ben Mullan 2021 -->
<html>

	<head>
		<% Response.WriteScriptAndCSSImports() %>
		<title>DesmosTile Links \\ GraphPictures</title>
	</head>
	
	<body onload="" class="Centered">
		
		<div class="Heading"><a href="/ClientPages/MainMenu.ASPX">GraphPictures</a> \ DesmosTile Links</div>

		<!----------------------------- The Links ----------------------------->
		<div class="GreyBorder Shadow Container">
			
			<b class="SubHeading">Tile Links for <a href="/ClientPages/ViewSVG.ASPX?SVGFileName=<%= Request.QueryString!SVGFileName %>"><%= Request.QueryString!SVGFileName %></a></b>
			
			<div id="TileLinkSquaresContainer" class="GreyBorder Shadow Container" style="background: #9999F0;">
				
				<%
					For _XTile% = 1 To CInt(Request.QueryString!TileCount.Split(","c)(0)) Step +1
						
						Response.Write(IIf(_XTile = 1, String.Empty, vbTab & vbTab & vbTab & vbTab).ToString() & "<div class=""XTilesContainer"">" & vbCrLf)
						
						For _YTile% = 1 To CInt(Request.QueryString!TileCount.Split(","c)(1)) Step +1
							
							Response.Write(String.Format(vbTab & vbTab & vbTab & vbTab & vbTab & "<a href=""{2}"" class=""TileLink"">{0},{1}</a><br/>" & VBCRLF, _XTile.ToString(), _YTile.ToString(), "/ClientPages/DesmosTile.ASPX?SVGFileName=" & Request.QueryString!SVGFileName & "&Quadrant=" & _XTile.ToString() & ","c & _YTile.ToString() & "&TileCount=" & Request.QueryString!TileCount))
							
						Next
						
						Response.Write(vbTab & vbTab & vbTab & vbTab & "</div>" & VBCRLF & VBCRLF)
						
					Next
				%>
			</div>

		</div>

	</body>
	
</html>
<% Response.SafelyEnd() %>