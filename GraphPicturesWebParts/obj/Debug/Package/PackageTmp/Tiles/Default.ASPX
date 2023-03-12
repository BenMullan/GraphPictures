<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	REM This Page is the default one served from the WebServer Directory, /Tiles/
	Response.Redirect("/ClientPages/DesmosTileLinks.ASPX?SVGFileName=JHGS_Swan.SVG&TileCount=2,2", endResponse:=True)
%>