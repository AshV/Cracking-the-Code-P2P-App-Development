<%@ Language=VBScript %>
<%Option Explicit%>
<!--#include file="adovbs.inc"-->
<%
	Dim sIPaddress
	Dim conn
		
	sIPaddress = trim(Request.QueryString("IP"))

	if sIPaddress="" then
		Response.End
	end if
	
	set conn = Server.CreateObject("ADODB.Connection")
	conn.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=p2p;Data Source=developers"
	conn.Open
	
	conn.execute "delete from peer where ip_address='" & sIPaddress & "'"
	conn.Close
	set conn=Nothing
%>
