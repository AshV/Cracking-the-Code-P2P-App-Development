<%@ Language=VBScript %>
<!--#include file="adovbs.inc"-->

<%	
'//Variable declaration----------//
	dim sUserId,sIPaddress,sError,str,sharestring,count,iloop,remoteIP
	dim conn,rsUser,rsIP,rsPeer,rsShare
	dim sqlUser,sqlIP,sqlPeer
	dim bError,bUserExist,bIPexist
	dim tConnectedTime
'//----------------------------//
	
	
'//Getting values from Query String variables-------------//
	sUserId = Ucase(trim(Request.QueryString("USERID")))
	sIPaddress = trim(Request.QueryString("IP"))
	sharestring = trim(Request.QueryString("share"))
'//--------------------------------------------//


'//Getting the remoteIP address of the machine--------//
	remoteIP = trim(Request.ServerVariables("REMOTE_ADDR"))
'//--------------------------------------------------//

		
	
'//Check for userid, if empty initialize variables for error message-----//
	if sUserId="" then
		bError=true
		sError="User Id can't be blank."
	end if
'//---------------------------------------------------------------------//
	

'//Check for IPaddress or remoteIP, if both are empty then initialize variables for error message-----//
	if (sIPaddress="" and remoteIP ="")then
		bError=true
		sError=sError & "IP can't be blank."
	end if
'//---------------------------------------------------------------------//
	
	
	
	
'//Writing XML if either userid or IPaddress does not exist---//	
	if bError then%>	
	
	<?xml version='1.0' encoding='utf-8'?>
	<p2p_lng>
	<response type='AUTH'>
		<connection code ="1" status="<%=sError%>" ip="0" />
	</response>
	</p2p_lng>

<%
	Response.End	
	end if

'//--------------------------------------------------------//
	


'//Creating object for connection string------------//
	set conn = Server.CreateObject("ADODB.Connection")
	conn.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=p2p;Data Source=developers"
	conn.Open
	
'//--------------------------------------------------//

	

'//Creating objects for recordset--------------------//	
	Set  rsIP= Server.CreateObject("ADODB.Recordset")
	Set rsPeer = Server.CreateObject("ADODB.Recordset")
	Set  rsShare= Server.CreateObject("ADODB.Recordset")
	Set  rsDeleteShare= Server.CreateObject("ADODB.Recordset")
	rsPeer.CursorType=adOpenDynamic
	rsIP.CursorType=adOpenStatic
'//--------------------------------------------------//



'//Writing SQL queries and open recordset-------------//		
	sqlIP = "SELECT * from peer where ip_address='"  & remoteIP & "'" 
	rsIP.Open sqlIP,conn
	rsPeer.open "peer",conn,adOpenDynamic,adLockOptimistic
'//--------------------------------------------------//

	
	bIPexist=false
	


'//Check for no. of records in both the recordsets-----//	
	if rsIP.RecordCount<>0 then bIPexist=true
'//--------------------------------------------------//
	
	


'//Delete the record if IP address already exists----//
	if(bIPexist) then
		conn.Execute "Delete from peer where ip_address='" & remoteIP & "'"
	end if	
'//-------------------------------------------------//



'//Close and destroy objects of recordsets------------//	
	rsIP.Close
	set rsIP=Nothing
'//-------------------------------------------------//



'//Add a new record in peer table----------------//
	rspeer.AddNew
	rsPeer("ip_address")=remoteIP
	rsPeer("user_name")=sUserId
	rsPeer("status")=1
	rsPeer("connected_time")=now()
	rsPeer.Update
'//-------------------------------------------//	



'//Writing XML if neither userid nor IPaddress already exists--//
%>
		<?xml version='1.0' encoding='utf-8'?>
		<p2p_lng>
		<response tag='AUTH'>
		<connection code ="0" status="Successful" ip="<%=remoteIP%>" />
		</response>
		</p2p_lng>

<%

'//----------------------------------------------------------//




'//Delete records from share table where that IPaddress already exists--//
Set rsShare = conn.Execute("delete share where ip_address='" & remoteIP & "'")	
'//----------------------------------------------------------//



'//Findout the filenames from the QuerySting variable------// 
sharestring = CStr((trim(sharestring)))
count = 0
str = split(sharestring, "*")

'//-------------------------------------------------------//




'//Insert ipaddress and filenames in share table----------// 
For iloop = LBound(str) To (UBound(str) - 1)
Response.Write str(iloop)
	Set rsShare = conn.Execute("INSERT share (ip_address,file_name) VALUES (" & "'" & remoteIP  & "' , '" & str(iloop) &  "')")	
	
Next	
'//-----------------------------------------------------//



'//Close the connection----------//
conn.Close
set conn=Nothing
'//----------------------------//

%>
