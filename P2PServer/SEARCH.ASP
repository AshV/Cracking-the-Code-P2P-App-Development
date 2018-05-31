<%@ Language=VBScript %>
<!--#include file="adovbs.inc"-->
<%	
	UserId = trim(Request.QueryString("US"))
	filename = UCase(trim(Request.QueryString("FS")))
	filename = changestar(filename)
	UserId = changestar(UserId)
		
	Function changestar(tmpstring)
		tmpstring = REPLACE(tmpstring,"*","%")
		changestar = tmpstring
	End Function
	
	set conn = Server.CreateObject("ADODB.Connection")
	conn.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=p2p;Data Source=developers"
	conn.Open
	
	Set  rsUser= Server.CreateObject("ADODB.Recordset")
	Set  rsFile= Server.CreateObject("ADODB.Recordset")
		
	sqlIP = "SELECT ip_address,user_name from peer where user_name like '" & UserId & "'"
	rsUser.Open sqlIP,conn %>
	
	<?xml version='1.0' encoding='utf-8'?>
	<p2p_language>
	<response type='SERVERSEARCH'>
	
	<%while not(rsUser.EOF)
			sqlFile = "SELECT * from share where ip_address='" & rsUser.Fields("ip_address") & "' and file_name like '" & filename & "'"
			rsFile.Open sqlFile,conn
			ip = rsUser.Fields("ip_address")
			uname = rsUser.Fields("user_name")
			while not(rsFile.EOF)
				fname = rsFile.Fields("file_name")%>
				<result ip="<%=ip%>" username="<%=uname%>" filename="<%=fname%>" />
			
			<%rsFile.MoveNext 
			wend
			rsFile.Close 
			
	rsUser.MoveNext
	wend
	
	
				
	conn.Close
	set conn=Nothing
%>
</response>
</p2p_language>


