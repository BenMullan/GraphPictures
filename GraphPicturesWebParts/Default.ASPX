<%@ Page Language="VB" %> <%@ Import Namespace="GraphPictures.Library.CompilerExtentions" %>
<%
	REM This Page is the default one served from the WebServer Directory, /
	Response.Redirect("/ClientPages/MainMenu.ASPX", endResponse:=True)
%>