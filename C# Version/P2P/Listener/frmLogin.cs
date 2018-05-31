namespace Listener
{
	// Library includes
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
	using System.Net;			// added for httprequest and responses and hostname resolving
	using System.IO;			// added for file streaming
	using System.Text;			// added for text encodings
	using WorkingWithXML;
	using System.Net.Sockets;
    using System.Threading;
	using System.Runtime.InteropServices;
	using System.Windows.Forms.Design;

    public class frmLogin : System.Windows.Forms.Form
    {
		Socket newSock;
		[DllImport("Shell32.dll")]
		public static extern int ShellExecute(int hwnd, 
			string lpVerb,
			string lpFile, 
			string lpParameters, 
			string lpDirectory,
			int nShowCmd );

		// State maintainig variables;
		private bool bLoggedIn;
		private int IconNo;
		private bool bShareDialogOpend;
		private bool bDownLoading;

		private Byte[] buffer;
		private string LoginName;
		private int ChatWindowCount;
        //    Variable required in communication with server
		private ServerCommunication xmlServerComm;

        //    Communcation facilitators on sockets.
		private Socket servSock;

        //    Xml reuests and responses are created using these objects
		private XMLCreater xmlCreater;
		private XMLParser  xmlParser;

        //    Threads of this application's being.
        //    Threads for continious update on sys tray
        //    and for continious seeking for connections 
        //    from browsers, respactively.
		private Thread AcceptThread,ThreadIcon,RespondingThread;

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lbCopyright;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.MenuItem ctxMenuShare;
		private System.Windows.Forms.MenuItem ctxMenuQuit;
		private System.Windows.Forms.TextBox textLoginID;
		private System.Windows.Forms.Button btnShareFileFolder;
		private System.Windows.Forms.Label labelLoginID;
		private System.Windows.Forms.CheckBox chkRemeberID;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Button btnQuit;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem ListenCntxtMenu;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.NotifyIcon ListenIcon;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.Button button1;

		// First functions that gets called in this form's life cycle
		// Initailizes all the GUI controls and reads username last 
		//used, if any and puts that on the form.
        public frmLogin()
        {
            InitializeComponent();
			// initialise objects getting used while communication
			// and parsing 
			xmlServerComm = new WorkingWithXML.ServerCommunication();
			xmlParser = new WorkingWithXML.XMLParser();
			xmlParser.LOGINXML = Application.StartupPath + "\\Login.xml";
			textLoginID.Text = ReadUsername();
			bLoggedIn			= false;
			bShareDialogOpend	= false;
			bDownLoading		= false;
			IconNo				= 1;
			ChatWindowCount		= 0;
			LoginName			= "";
			AcceptThread		= null; 
			ThreadIcon			= null;
			RespondingThread	= null;
        }

		// Reads user name and returns that to the caller
		public string ReadUsername()
		{
			string Username, sTemp;
			Username = null;
			if ( File.Exists(Application.StartupPath + "\\UserInfo.ini") )
			{
				// open userinof.ini file and read user name
				Stream fstr = File.OpenRead(Application.StartupPath + "\\UserInfo.ini");
				Byte[] buffer = new Byte[Convert.ToInt32(fstr.Length)];
				fstr.Read(buffer,0,Convert.ToInt32(fstr.Length));
				sTemp = Encoding.ASCII.GetString(buffer,0,Convert.ToInt32(buffer.Length));
				Username = sTemp.Substring(sTemp.IndexOf("=")+1);
				chkRemeberID.Checked = true;
				fstr.Close();
			}
			else 
				chkRemeberID.Checked = false;
			return Username;
		}

		// Writes user name to a files for next instenciation
		public void WriteUsername()
		{
			string buffer = "username=";
			Stream fstr = File.OpenWrite(Application.StartupPath + "\\UserInfo.ini");
			buffer += textLoginID.Text;
			fstr.Write(Encoding.ASCII.GetBytes(buffer),0,buffer.Length);
			fstr.Close();
		}

        ///  Clean up resources this application was using.
        public override void Dispose()
        {
            base.Dispose();
            components.Dispose();
        }

        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmLogin));
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.ctxMenuQuit = new System.Windows.Forms.MenuItem();
			this.ctxMenuShare = new System.Windows.Forms.MenuItem();
			this.ListenIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.btnQuit = new System.Windows.Forms.Button();
			this.btnLogin = new System.Windows.Forms.Button();
			this.labelLoginID = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.btnShareFileFolder = new System.Windows.Forms.Button();
			this.textLoginID = new System.Windows.Forms.TextBox();
			this.chkRemeberID = new System.Windows.Forms.CheckBox();
			this.ListenCntxtMenu = new System.Windows.Forms.MenuItem();
			this.lbCopyright = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.ctxMenuQuit,
																						this.ctxMenuShare});
			// 
			// ctxMenuQuit
			// 
			this.ctxMenuQuit.Index = 0;
			this.ctxMenuQuit.Text = "Quit";
			this.ctxMenuQuit.Click += new System.EventHandler(this.ctxMenuQuit_Click);
			// 
			// ctxMenuShare
			// 
			this.ctxMenuShare.Index = 1;
			this.ctxMenuShare.Text = "Share File/Folders";
			this.ctxMenuShare.Click += new System.EventHandler(this.ctxMenuShare_Click);
			// 
			// ListenIcon
			// 
			this.ListenIcon.ContextMenu = this.contextMenu;
			this.ListenIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ListenIcon.Icon")));
			this.ListenIcon.Text = "Peering Peers";
			this.ListenIcon.Visible = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(895, 211);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "";
			this.textBox1.Visible = false;
			this.textBox1.WordWrap = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(450, 72);
			this.button1.Name = "button1";
			this.button1.TabIndex = 7;
			this.button1.Visible = false;
			// 
			// menuItem1
			// 
			this.menuItem1.Index = -1;
			this.menuItem1.Text = "";
			// 
			// btnQuit
			// 
			this.btnQuit.BackColor = System.Drawing.SystemColors.Control;
			this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnQuit.Location = new System.Drawing.Point(349, 145);
			this.btnQuit.Name = "btnQuit";
			this.btnQuit.Size = new System.Drawing.Size(75, 24);
			this.btnQuit.TabIndex = 5;
			this.btnQuit.Text = "Quit";
			this.toolTip1.SetToolTip(this.btnQuit, "Quit and Close");
			this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
			// 
			// btnLogin
			// 
			this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnLogin.ForeColor = System.Drawing.Color.LemonChiffon;
			this.btnLogin.Location = new System.Drawing.Point(74, 76);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(83, 29);
			this.btnLogin.TabIndex = 4;
			this.btnLogin.Text = "Login";
			this.toolTip1.SetToolTip(this.btnLogin, "Log-in with this LoginID.");
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// labelLoginID
			// 
			this.labelLoginID.Location = new System.Drawing.Point(18, 30);
			this.labelLoginID.Name = "labelLoginID";
			this.labelLoginID.Size = new System.Drawing.Size(54, 14);
			this.labelLoginID.TabIndex = 2;
			this.labelLoginID.Text = "Login ID :";
			// 
			// btnShareFileFolder
			// 
			this.btnShareFileFolder.BackColor = System.Drawing.SystemColors.Control;
			this.btnShareFileFolder.Location = new System.Drawing.Point(199, 145);
			this.btnShareFileFolder.Name = "btnShareFileFolder";
			this.btnShareFileFolder.Size = new System.Drawing.Size(143, 24);
			this.btnShareFileFolder.TabIndex = 1;
			this.btnShareFileFolder.Text = "Share Files/Folders";
			this.toolTip1.SetToolTip(this.btnShareFileFolder, "Share your folders with other peers");
			this.btnShareFileFolder.Click += new System.EventHandler(this.btnShareFileFolder_Click);
			// 
			// textLoginID
			// 
			this.textLoginID.Location = new System.Drawing.Point(74, 28);
			this.textLoginID.Name = "textLoginID";
			this.textLoginID.Size = new System.Drawing.Size(281, 20);
			this.textLoginID.TabIndex = 0;
			this.textLoginID.Text = "";
			this.toolTip1.SetToolTip(this.textLoginID, "Write your Login name here.");
			// 
			// chkRemeberID
			// 
			this.chkRemeberID.Location = new System.Drawing.Point(74, 54);
			this.chkRemeberID.Name = "chkRemeberID";
			this.chkRemeberID.Size = new System.Drawing.Size(134, 18);
			this.chkRemeberID.TabIndex = 3;
			this.chkRemeberID.Text = "Remeber My Login ID";
			this.toolTip1.SetToolTip(this.chkRemeberID, "System will remember LoginID.");
			// 
			// ListenCntxtMenu
			// 
			this.ListenCntxtMenu.Index = -1;
			this.ListenCntxtMenu.Text = "";
			// 
			// lbCopyright
			// 
			this.lbCopyright.Location = new System.Drawing.Point(226, 6);
			this.lbCopyright.Name = "lbCopyright";
			this.lbCopyright.Size = new System.Drawing.Size(196, 12);
			this.lbCopyright.TabIndex = 8;
			this.lbCopyright.Text = "© 2001 www.dreamtechsoftware.com";
			// 
			// frmLogin
			// 
			this.AcceptButton = this.btnLogin;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnQuit;
			this.ClientSize = new System.Drawing.Size(438, 181);
			this.ContextMenu = this.contextMenu;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lbCopyright,
																		  this.textLoginID,
																		  this.btnShareFileFolder,
																		  this.labelLoginID,
																		  this.chkRemeberID,
																		  this.btnLogin,
																		  this.btnQuit,
																		  this.textBox1,
																		  this.button1});
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmLogin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Peering Peers";
			this.TransparencyKey = System.Drawing.Color.Olive;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmLogin_Closing);
			this.ResumeLayout(false);

		}

		// Quit clicked pack up, cleen up and leave, and yes
		// inform server calling "Logout()".
		protected void ctxMenuQuit_Click (object sender, System.EventArgs e)
		{
 			Logout();
			this.Dispose();
			this.Close();
			Application.Exit();
		}

		// Time to be kind to share resoures with others
		// through "Share Files/Folders" dialog box
		protected void ctxMenuShare_Click (object sender, System.EventArgs e)
		{
 			if(!bShareDialogOpend)
			{
				// open this dialog bax only if it is not opened already
				bShareDialogOpend = true;
				// make and object of frmSelection form and show it
				frmSelection formSelection = new frmSelection();
				formSelection.ShowDialog();
				bShareDialogOpend = false;
			}			
		}

		// another way of popin-up the "Share Files/Folders" dialog box just
		// in case u did,nt like the first one
		protected void btnShareFileFolder_Click (object sender, System.EventArgs e)
		{
 			frmSelection formSelection = new frmSelection();
			formSelection.ShowDialog();
		}

		// Quit clicked pack up, cleen up and leave.
		protected void btnQuit_Click (object sender, System.EventArgs e)
		{
 			Logout();
			this.Dispose();
			this.Close();
			Application.Exit();
		}

		// Unless u log-in the log book on server u dont exist for browser
		protected void btnLogin_Click (object sender, System.EventArgs e)
		{
			string ServerAddress;
			// URL for ASP to pass on your information to server
			ServerAddress = "http://SERVERNAME/login.asp?USERID=" + textLoginID.Text + "&IP=" + xmlServerComm.GetIPAddress("");
			XMLSTRUCT xmlStruct;
			LoginName = textLoginID.Text;

			try
			{
				if( 0 != textLoginID.Text.Trim().Length )
				{
					string Share = "";
					
					// if remember user name check box's checked write username to "userinfo.ini"
					if( chkRemeberID.Checked ) WriteUsername();
					
					// else delete that file as it is no longer needed
					else xmlServerComm.FileDelete(Application.StartupPath+"\\Userinfo.ini");

					// if u had never shared anything set SHARE parameter to ASP to empty
					if( ! File.Exists(Application.StartupPath+"\\Share.ini") )
						Share = "";
					else
					{
						// else read share.ini and read all the files and folders 
						// u had shared and set them to SHARE parameter
						StreamReader stReader = new StreamReader(Application.StartupPath+"\\Share.ini");
						string readData;
						while((readData = stReader.ReadLine()) != null)
						{
							if(!readData.Substring(0,readData.IndexOf("=")).EndsWith("\\"))
								Share += readData.Substring(0,readData.IndexOf("=")).Substring(readData.Substring(0,readData.IndexOf("=")).LastIndexOf("\\")+1) + "*";
							else
								Share += readData.Substring(0,readData.IndexOf("=")) + "*";
						}
						stReader.Close();
					}

					ServerAddress += "&SHARE=" + Share;

					// make a call to loging.asp and recieve the response in an XML file
					string XMLData = xmlServerComm.GetDataFromServer(ServerAddress);
					XMLData = XMLData.Substring(0,XMLData.LastIndexOf("</p2p_lng>")+ "</p2p_lng>".Length);
					xmlServerComm.WriteDataToFile(xmlParser.LOGINXML,XMLData);
					// parse that file and store in XMLSTRUCT
					xmlParser.ParseXML(xmlParser.LOGINXML,out xmlStruct, xmlServerComm.TypeOfXMLRecieved(xmlParser.LOGINXML));
					// delete XML file
					xmlServerComm.FileDelete(xmlParser.LOGINXML);

					// now check whether login was successful  
					if( 0 == xmlStruct.AUTH.iCode )
					{

						buffer = new Byte[256];

						servSock = new Socket(AddressFamily.InterNetwork, 
							SocketType.Stream,
							ProtocolType.Tcp);

						// if successfull make a socket giving the IP and port
						IPAddress localIP = IPAddress.Parse((xmlServerComm.GetIPAddress("")));
						IPEndPoint localEP = new IPEndPoint(localIP,7070);
						servSock.Bind(localEP);
						servSock.Listen(40);
					

						AcceptThread = new Thread( new ThreadStart(AcceptFunction));
						//AcceptThread.Priority = ThreadPriority.BelowNormal;
						AcceptThread.Start();

						// make a new thread to keep updateing the icons on sys tray
						ThreadIcon = new Thread( new ThreadStart(IconUpdate));
						ThreadIcon.Priority = ThreadPriority.BelowNormal;
						ThreadIcon.Start();
						
						// hide the login form
						this.Hide();
					}
					else
					{
						// if unsuccessfull show user the error occured 
						MessageBox.Show(xmlStruct.AUTH.sStatus,"Peering Peers",MessageBoxButtons.OK, MessageBoxIcon.Information); 
						textLoginID.Focus();
						textLoginID.SelectAll();
					}
				}
				else
				{
					// before actually writing your username u cannot loggin
					MessageBox.Show("Blank LoginID detected","Peering Peers",MessageBoxButtons.OK, MessageBoxIcon.Error);
					textLoginID.Focus();
					textLoginID.SelectAll();
				}
			}
			catch( Exception err )
			{
				// if logging fails tell user why it did
				// "Could'nt Login."
				MessageBox.Show(err.Message,"Peering Peers",MessageBoxButtons.OK, MessageBoxIcon.Error);
				// and close application
				this.Dispose();
				this.Close();
				Application.Exit();
			}
			// declare that loging process's successfull
			bLoggedIn = true;
		}

		// keep updating ur looks to avoid being overlooked
		protected void IconUpdate()
		{
			try
			{
				while(true)
				{
					IconNo += 1;

					string Filename;
					// if something's getting downloaded or uploaded
					// pick different icons to put on sys tray.
					if(bDownLoading)
					{
						if((IconNo) == 5)
							IconNo = 1;
						Filename = "\\Trans" + IconNo.ToString() + ".ico";
						ListenIcon.Text = "Downloading or Uploading in Progress";
					}
					else
					{
						// otherwise keep cycling these four icons on sys tray.
						if((IconNo) == 5)
							IconNo = 1;
						Filename = "\\Listener" + IconNo.ToString() + ".ico";
						ListenIcon.Text = "Peering Peers";
					}
					System.Drawing.Icon icon = new System.Drawing.Icon(Application.StartupPath + Filename);
					ListenIcon.Icon = icon;
					// make the process a bit slow for user to be able to 
					// notice it
					System.Threading.Thread.Sleep(500);
				}
			}
			catch(Exception e) { WriteErrorLog("Updating Sys-tray icon.",e.Message); }
		}

		// Keep a log of all the exceptions and errors.
		protected void WriteErrorLog(string Origin, string Message)
		{
			StreamWriter stWriter = File.AppendText(Application.StartupPath + "\\Error.log");
			// write the time of occurence of this error
			stWriter.WriteLine("Origin = " + Origin + 
				"		Date & Time (ms-s-m-h  dd/mm/yy/) = "+ 
				DateTime.Now.Millisecond.ToString() +
				"-" +
				DateTime.Now.Second.ToString() +
				"-" +
				DateTime.Now.Minute.ToString() +
				"-" +
				DateTime.Now.Hour.ToString() +
				"	" +
				DateTime.Now.Day.ToString() +
				"/" +
				DateTime.Now.Month.ToString() +
				"/" +
				DateTime.Now.Year.ToString() + 
				"		Error Message = " + Message);

			stWriter.Close();
		}

		// Form closeing, dispose all the resources used and Exit the Application
		protected void frmLogin_Closing (object sender, System.ComponentModel.CancelEventArgs e)
		{
			Logout();

			this.Dispose();
			this.Close();

			Application.Exit();
		}
		
		// Keeps accepting connections
		protected void AcceptFunction()
		{
			try
			{
				while(true)
				{
					newSock = servSock.Accept();

					RespondingThread = new Thread(new ThreadStart(RequestResponse));
					RespondingThread.Priority = ThreadPriority.Highest;
					RespondingThread.Start();
				}
			}catch( Exception e ) { WriteErrorLog("Client's attempt of establishing connection failed.",e.Message); }
		}
		
		// Resposnd the requests made
		protected void RequestResponse()
		{
			Encoding ASCII = Encoding.ASCII;     
			NetworkStream DataStream = new NetworkStream(newSock);
			
			try
			{
				Byte[] readBytes = new Byte[1024],read = new Byte[1024];
				string[] ChatInfo;

				int bytes = 0,UploadDownloadPrint;
				bool responseWritten = false;
				FileStream fs1 = null;

				// read what the browser wrote on its stream and 
				bytes = DataStream.Read(read, 0, read.Length);
				// make a temporary file 
				FileStream fs = new FileStream(Application.StartupPath + "\\Temp.xml", FileMode.OpenOrCreate);
				
				// to store that data to
				BinaryWriter w = new BinaryWriter(fs);
				w.Write(read, 0, bytes);
				w.Close();
				fs.Close();

				// then read file and detrmine the kind of request
				xmlCreater = new XMLCreater(Application.StartupPath + "\\Temp1.xml",Application.StartupPath + "\\Share.ini");
				string DnLoadFile = xmlCreater.DetermineRequestType(Application.StartupPath + "\\Temp.xml",out UploadDownloadPrint,out ChatInfo);

				// if it's download/upload request make file object for
                // the file browser has requested to upload to
				// or download from his comp
				if(DnLoadFile.Length != 0)
				{
					if(UploadDownloadPrint == 1)
					{
						fs1 = new FileStream(DnLoadFile,FileMode.CreateNew);
						DnLoadFile.EndsWith("mp3");
					}
					else if(UploadDownloadPrint == 2)
						fs1 = new FileStream(DnLoadFile,FileMode.Open);
					else if(UploadDownloadPrint == 3)
					{
						ShellExecute(0,"print",DnLoadFile,"","",1);
						File.Delete(Application.StartupPath + "\\Temp.xml");
						DataStream.Flush();
						newSock.Close();
						bDownLoading = false;
						return;
					}
					bDownLoading = true;
				}
				else
				{
					// else make a file object for responding in xml formate
					fs1 = new FileStream(Application.StartupPath + "\\Temp1.xml",FileMode.Open);
					responseWritten = true;
				}

				// write resoponse
				if(UploadDownloadPrint == 1)
				{
					// if it was upload request
					BinaryWriter wr = new BinaryWriter(fs1);
					int nreadBytes;

					// read network stream and write data to a loacal file
					while( (nreadBytes = DataStream.Read(readBytes, 0, readBytes.Length)) > 0) 
						wr.Write(readBytes, 0, nreadBytes);

					wr.Close();
					fs1.Close();
				}
				else
				{
					// write the response or file requested for download
					// in browsers stream
					BinaryReader r = new BinaryReader(fs1);
					int nreadBytes;

					// write upon the network stream
					while( (nreadBytes = r.Read(readBytes, 0, readBytes.Length)) > 0) 
						DataStream.Write(readBytes, 0, nreadBytes);

					r.Close();
					fs1.Close();
				}

				// delete all temporary files
				if(responseWritten)
					File.Delete(Application.StartupPath + "\\Temp1.xml");

				File.Delete(Application.StartupPath + "\\Temp.xml");
				DataStream.Flush();
				newSock.Close();
				bDownLoading = false;
			}
			catch(SocketException e) {	WriteErrorLog("Responding Browser",e.Message); }
		}
        /// The main entry point for the application.
        /// 
		[STAThread]
        public static void Main(string[] args) 
        {
            Application.Run(new frmLogin());
        }

		// Listener's through, it will inform server that it is making an exit.
		public void Logout ()
		{
			try
			{
				if(bLoggedIn)
				{
					xmlServerComm.GetDataFromServer("http://SERVERNAME/Logout.asp?ip=" + xmlServerComm.GetIPAddress(""));
					MessageBox.Show("Listener Logged-out.","Process Successfull",MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				//release threads
				if(ThreadIcon!=null)
					if(ThreadIcon.IsAlive)
						ThreadIcon.Abort();

				if(AcceptThread!=null)
					if(AcceptThread.IsAlive)
						AcceptThread.Abort();

				if(RespondingThread!=null)
					if(RespondingThread.IsAlive)
						RespondingThread.Abort();

			}
			catch( Exception err ) { MessageBox.Show(err.Message,"Could not Logout properly.",MessageBoxButtons.OK, MessageBoxIcon.Error);}
		}
    }
}