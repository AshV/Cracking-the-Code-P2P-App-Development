This file contains the information about running the C# version of the application. You must have the following Software installed on your machine in order to run this application.

1.	Microsoft Visual Studio .NET Beta 2
2.	Microsoft XML Parser 3.0
3.	DirectX	SDK 8.0 *
4.	Windows Media Encoder SDK 7.1 *
5.	Windows Media Player 7.0 or higher *
6.	Windows 2000 Professional with IIS 5.0
7.	SQL Server 2000


Setting up your Computer for Web server
1.	You will need setup a website on your computer using IIS
2.	Copy all the files present in the "SEREVER" folder of this CD-ROM to the web site path 		that you had set up.
3.	Check your site by running any of the asp (even if the asp returns some error 		information, that means your site has been set up properly)
4.	We are using "developers" as the computername, You will need to change the name of your 	computer or will have to change the asp's according to your computername.

Setting up the SQL Server database
1.	Make a database on the SQL server and name it "p2p"
2.	Create two tables, "peer" and "share" in your database (see chapter 3 for details)

Change the web address in both the Listener and the Browser so that it points to the local website on your machine that you have created. You will find this in the "frmLogin" class of the Listener and "frmClient" class of the Browser



* You need to have these installed on your computer to run the P2PWithAddons Version of the application. You will need to Register "\VC code for Streaming\BroadcastDll.dll" using "Regsvr32.exe" utility befor doing anything.
syntax : Regsvr32.exe \VC code for Streaming\BroadcastDll.dll
