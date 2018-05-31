<%@ Language=VBScript %>
<%Option Explicit%>
<!--#include file="adovbs.inc"-->
<%
	Dim conn,sqlPeer,rsPeer
	dim sUserId,sIP,sString
	
	set conn = Server.CreateObject("ADODB.Connection")
	conn.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=p2p;Data Source=developers"
	conn.Open
		
	Set rsPeer = Server.CreateObject("ADODB.Recordset")
	sqlPeer="select * from peer"
		
	rsPeer.CursorType = adOpenStatic
	rsPeer.Open sqlPeer,conn
%><?xml version='1.0' encoding='utf-8'?>
<p2p_lng>
	<response type="USERLIST">
<%
	while not(rsPeer.EOF)
		sUserId=trim(rsPeer.Fields("user_name"))
		sIP=trim(rsPeer.Fields("ip_address"))
%>		<userinfo username='<%=sUserId%>' ip='<%=sIP%>' />
<%		
		rsPeer.MoveNext
	wend
%>	</response>
</p2p_lng>
<%	
	rsPeer.Close
	set rsPeer=Nothing
	conn.Close
	set conn=Nothing
	Response.End
%>
