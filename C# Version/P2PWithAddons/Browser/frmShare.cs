namespace Client				// Copyright 2001 Dreamtech Software India Inc.
{								// All rights reserved
    using System;				// Provides the basic functionality of .NET
	using System.Drawing;		// Provides the Drawing features, Used for cursors
    using System.Windows.Forms;		// Provides the darwing of buttons, listviews etc	
	using System.Net;			// Provides the net related functionality 
	using System.Net.Sockets;	// Provides the functionality of sockets
	using System.Text;			// Provides the text manipulation functions
	using System.IO;			// Provides I/O features
	using WorkingWithXML;		// Custom class 
	using System.Collections;	// Provides the different type of class collections
    using System.ComponentModel;// Provides the facility of using components

	/// <summary>
    ///    Summary description for frmShare.
    /// </summary>
    public class frmShare : System.Windows.Forms.Form
    {
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.Button btnPrint;
		public System.Windows.Forms.Button btnChat;
		private	System.Windows.Forms.Label			label1;
		private	System.Windows.Forms.OpenFileDialog	FileOpenDialog;
		public	System.Windows.Forms.Button			btnClose;
		public	System.Windows.Forms.StatusBar		sBar;
		private System.Windows.Forms.SaveFileDialog	FileSaveDialog;
		private System.Windows.Forms.ToolTip		toolTipText;
		public	System.Windows.Forms.ListView		lvFiles;
		public	System.Windows.Forms.Button			btnSearch;
		public	System.Windows.Forms.Button			btnDownload;
		public	System.Windows.Forms.Button			btnUpload;
		private System.Windows.Forms.ColumnHeader	clhFilename;
		private System.Windows.Forms.ColumnHeader	clhFileSize;
		private System.Windows.Forms.ColumnHeader	clhType;

		/// <summary>
		/// User defined variables.
		/// </summary>
		
		/// <summary>
		/// stores the number of bytes written or read from any stream
		/// </summary>
		private int						iBytes;
		
		/// <summary>
		///	These variables are used to store the name of the computer
		///	to which you have connected and the parent folder name.
		///	Parent folder name is the name of the folder of the contents
		///	which you are viewing in the window
		/// </summary>
		private string					COMPUTERNAME,PARENTFOLDER;
		
		/// <summary>
		/// Stores a new created socket of type TCPClient(System defined class)
		/// used to communicate with the listener
		/// </summary>
		private clsChatSocket			ClientSocket;
		
		/// <summary>
		/// creates a variable for XMLCreater(User defined class) to 
		/// create XML requests for the listener
		/// </summary>
		private XMLCreater				xmlCreate;
		
		/// <summary>
		/// StreamTCP points to the NetworkStream(System defined class)
		/// which is used for transfer data over the Socket connection
		/// </summary>
		private NetworkStream			StreamTCP;
		
		/// <summary>
		/// fileStream is an object of type FileStream(System defined class)
		/// used to have I/O capabilities for files which are used
		/// in this program
		/// </summary>
		private FileStream				fileStream;
		
		/// <summary>
		/// ReadBuffer and WriteBuffer Byte arrays used for 
		/// Reading and Writing any file.
		/// </summary>
		private Byte[]					ReadBuffer,WriteBuffer;
		
		/// <summary>
		/// xmlParser is of type XMLParser(User defined class)
		/// It is used to have the access for Parsing the XML file
		/// This class is present in WorkingWithXML
		/// </summary>
		private	XMLParser				xmlParser;
		
		/// <summary>
		/// xmlStruct is of type XMLSTRUCT(User defined structure)
		/// It is used to store the different records obtained
		/// from parsing the XML. This structure is present in WorkingWithXML
		/// </summary>
		private	XMLSTRUCT				xmlStruct;
		
		/// <summary>
		/// strArray if of Type __SHOWFILES of XMLSTRUCT Structure and
		/// used to save the corresponding Files/Folder which are seen in the
		/// the List view at run time
		/// </summary>
		private XMLSTRUCT.__SHOWFILES[] strArray;

		
		/// <summary>
		/// The below variables are readonly variables that u cannot
		/// assign a new value to these variables again. They are
		/// constant type variables
		/// </summary>
		
		/// <summary>
		/// REQUESTFILE have the name of the file which have to be created
		/// for a particular request
		/// </summary>
		public readonly string REQUESTFILE = Application.StartupPath + "\\Request.xml";

		/// <summary>
		/// RESPONSEFILE have the name of the file which have to be created
		/// for a particular request
		/// </summary>
		public readonly string RESPONSEFILE = Application.StartupPath + "\\Response.xml";
		public System.Windows.Forms.Button btnStream;

		/// <summary>
		/// MAX_SIZE defines the maximum size of the read or write buffer
		/// which is used for reading or wrting a file
		/// </summary>
		public readonly int MAX_SIZE = 512;

		/// <summary>
		/// This is the default contructor of the this class
		/// This is called from the View files button
		/// </summary>
		public frmShare()
        {
            //
            // Required for Windows Form Designer support
            //
            // Auto generated code line by the IDE
			InitializeComponent();
			
			// Changes the Caption of this dialog box
			this.Text = "Search Result";
			sBar.Text = "Root";
			COMPUTERNAME = null;
			PARENTFOLDER = null;
        }

		/// <summary>
		/// This is a user defined constructor called from the 
		/// Open button in the frmClient
		/// Computername is passed to this function, which you
		/// have selected from the Main window
		/// </summary>
		/// <param name="Computername"> </param>
		public frmShare(string Computername)
		{
            //
            // Required for Windows Form Designer support
			// This line is not auto generated, instead it has been copied from
			// default constructor
            InitializeComponent();
			
			// COMPUTERNAME is a global variable used to store the name
			// and IP Address of the computer to which you are currently
			// connected
			COMPUTERNAME = Computername;
			
			// Open connection is a user defined function responsible for
			// opening a socket connection for listener. This function
			// returns a bool value
			if( OpenConnection(COMPUTERNAME) )
			{
				// This will creates a SHOWFILE request XML for seding it
				// to the listener
				CreateRequest("SHOWFILES","","");
				
				// This will actually sends the REQUESTFILE to the listener
				SendDataToListener(REQUESTFILE);
				
				// This will get the response of the above request from
				// listener and store that response in a RESPONSEFILE
				GetDataFromListener(RESPONSEFILE);
				
				// This will Parse that response XML File and results are
				// shown to the user
				Parsing(RESPONSEFILE);
				
				// Closes any Opened socket or stream connection
				CloseConnection();
				
				// Since this constructor is called at the root level
				// therefore no parent folder is associated with it
				PARENTFOLDER = null;
				
				// Changes the caption of this dialog box
				this.Text = "Shared contents on: " + Computername.ToUpper();
				
				// Sets the text for the Status bar
				sBar.Text = "Root";
			}
		}

		/// <summary>
		/// This is also a user defined constructor called from the
		/// Search button to view the search results
		/// This constructor is called from within this code only
		/// it is used to pass any type of request if needed at the time of
		/// its construction
		/// </summary>
		/// <param name="Computername stores the name of the computer to connect"> </param>
		/// <param name="Request stores what type of request you want to send to the listener"> </param>
		/// <param name="Scope scope value needed, if any"> </param>
		/// <param name="Mask mask value needed if any"> </param>
		public frmShare(string Computername, string Request, string Scope, string Mask)
		{
            //
            // Required for Windows Form Designer support
			// This line is not auto generated, instead it has been copied from
			// default constructor
            InitializeComponent();

			// COMPUTERNAME is a global variable used to store the name
			// and IP Address of the computer to which you are currently
			// connected
			COMPUTERNAME = Computername;
			
			// Open connection is a user defined function responsible for
			// opening a socket connection for listener. This function
			// returns a bool value
			if( OpenConnection(COMPUTERNAME) )
			{
				// Creates an XML request with scope and mask
				CreateRequest(Request,Scope,Mask);
				
				// Sends this request to listener
				SendDataToListener(REQUESTFILE);
				
				// get the response from listener
				GetDataFromListener(RESPONSEFILE);
				
				// Parse that response for records and show those records
				Parsing(RESPONSEFILE);
				
				// Closes the active connection
				CloseConnection();
				
				// Assign scope to sText local variable
				string sText = Scope;
				
				// Extract the name of the PARENTFOLDER
				sText = sText.Substring(0, sText.LastIndexOf("\\")+1);
				
				// changes the caption of this dialog box
				this.Text = sText + " on '" + Computername + "'";
				
				// Assign the value for PARENTFOLDER
				PARENTFOLDER = sText;
				
				// Changes the text of the status bar
				sBar.Text = sText;
			}
		}

		/// <summary>
        ///    Clean up any resources being used.
        ///    auto generated function by IDE
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            components.Dispose();
        }

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        ///    These are also auto generated lines of code
        /// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmShare));
			this.sBar = new System.Windows.Forms.StatusBar();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnChat = new System.Windows.Forms.Button();
			this.FileSaveDialog = new System.Windows.Forms.SaveFileDialog();
			this.FileOpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.clhFilename = new System.Windows.Forms.ColumnHeader();
			this.toolTipText = new System.Windows.Forms.ToolTip(this.components);
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnUpload = new System.Windows.Forms.Button();
			this.btnDownload = new System.Windows.Forms.Button();
			this.lvFiles = new System.Windows.Forms.ListView();
			this.clhFileSize = new System.Windows.Forms.ColumnHeader();
			this.clhType = new System.Windows.Forms.ColumnHeader();
			this.btnStream = new System.Windows.Forms.Button();
			this.btnPrint = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// sBar
			// 
			this.sBar.Location = new System.Drawing.Point(0, 288);
			this.sBar.Name = "sBar";
			this.sBar.Size = new System.Drawing.Size(510, 16);
			this.sBar.TabIndex = 8;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.Chocolate;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(430, 18);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(71, 31);
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "&Close";
			this.toolTipText.SetToolTip(this.btnClose, "Exist from this window");
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnChat
			// 
			this.btnChat.BackColor = System.Drawing.Color.Chocolate;
			this.btnChat.Enabled = false;
			this.btnChat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnChat.ForeColor = System.Drawing.Color.White;
			this.btnChat.Location = new System.Drawing.Point(359, 18);
			this.btnChat.Name = "btnChat";
			this.btnChat.Size = new System.Drawing.Size(71, 31);
			this.btnChat.TabIndex = 5;
			this.btnChat.Text = "C&hat";
			this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
			// 
			// FileOpenDialog
			// 
			this.FileOpenDialog.Filter = "*.* (All files)|";
			this.FileOpenDialog.Title = "Select a file to upload";
			// 
			// clhFilename
			// 
			this.clhFilename.Text = "File / Folder";
			this.clhFilename.Width = 303;
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.Chocolate;
			this.btnSearch.Enabled = false;
			this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSearch.ForeColor = System.Drawing.Color.White;
			this.btnSearch.Location = new System.Drawing.Point(146, 18);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(71, 31);
			this.btnSearch.TabIndex = 2;
			this.btnSearch.Text = "&Search";
			this.toolTipText.SetToolTip(this.btnSearch, "Search for file(s) or folder(s)");
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnUpload
			// 
			this.btnUpload.BackColor = System.Drawing.Color.Chocolate;
			this.btnUpload.Enabled = false;
			this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnUpload.ForeColor = System.Drawing.Color.White;
			this.btnUpload.Location = new System.Drawing.Point(4, 18);
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Size = new System.Drawing.Size(71, 31);
			this.btnUpload.TabIndex = 0;
			this.btnUpload.Text = "&Upload";
			this.toolTipText.SetToolTip(this.btnUpload, "Writes the file to current location");
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			// 
			// btnDownload
			// 
			this.btnDownload.BackColor = System.Drawing.Color.Chocolate;
			this.btnDownload.Enabled = false;
			this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDownload.ForeColor = System.Drawing.Color.White;
			this.btnDownload.Location = new System.Drawing.Point(75, 18);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(71, 31);
			this.btnDownload.TabIndex = 1;
			this.btnDownload.Text = "&Download";
			this.toolTipText.SetToolTip(this.btnDownload, "Download file to this computer");
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// lvFiles
			// 
			this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.clhFilename,
																					  this.clhFileSize,
																					  this.clhType});
			this.lvFiles.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lvFiles.FullRowSelect = true;
			this.lvFiles.HideSelection = false;
			this.lvFiles.Location = new System.Drawing.Point(3, 54);
			this.lvFiles.MultiSelect = false;
			this.lvFiles.Name = "lvFiles";
			this.lvFiles.Size = new System.Drawing.Size(502, 234);
			this.lvFiles.TabIndex = 7;
			this.toolTipText.SetToolTip(this.lvFiles, "Double click an entry to open");
			this.lvFiles.View = System.Windows.Forms.View.Details;
			this.lvFiles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvFiles_KeyPress);
			this.lvFiles.DoubleClick += new System.EventHandler(this.lvFiles_DoubleClick);
			this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
			// 
			// clhFileSize
			// 
			this.clhFileSize.Text = "Size";
			this.clhFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.clhFileSize.Width = 69;
			// 
			// clhType
			// 
			this.clhType.Text = "Type";
			this.clhType.Width = 108;
			// 
			// btnStream
			// 
			this.btnStream.BackColor = System.Drawing.Color.Chocolate;
			this.btnStream.Enabled = false;
			this.btnStream.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStream.ForeColor = System.Drawing.Color.White;
			this.btnStream.Location = new System.Drawing.Point(217, 18);
			this.btnStream.Name = "btnStream";
			this.btnStream.Size = new System.Drawing.Size(71, 31);
			this.btnStream.TabIndex = 3;
			this.btnStream.Text = "S&tream";
			this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.BackColor = System.Drawing.Color.Chocolate;
			this.btnPrint.Enabled = false;
			this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnPrint.ForeColor = System.Drawing.Color.White;
			this.btnPrint.Location = new System.Drawing.Point(288, 18);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(71, 31);
			this.btnPrint.TabIndex = 4;
			this.btnPrint.Text = "&Print";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(173, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 14);
			this.label1.TabIndex = 6;
			this.label1.Text = "© 2001 www.dreamtechsoftware.com";
			// 
			// frmShare
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(510, 304);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnStream,
																		  this.btnPrint,
																		  this.btnChat,
																		  this.label1,
																		  this.btnClose,
																		  this.sBar,
																		  this.lvFiles,
																		  this.btnSearch,
																		  this.btnDownload,
																		  this.btnUpload});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmShare";
			this.Text = "Shared Contents on : COMPUTERNAME";
			this.ResumeLayout(false);

		}

		protected void btnPrint_Click (object sender, System.EventArgs e)
		{
			string Filename;
			
			// Get the currently selected item from the List view
			int index = GetSelectedItemFromListView(out Filename);
			try
			{
				// Checks whether any entry is selected or not
				if( -1 != index)
					
					// Checks whether a filename is selected for printing
					// or not
					if( null != Filename )
					{
						// If filename is selected than print it
						if( OpenConnection(COMPUTERNAME) )
						{
							// Creates a PRINT request for a particular
							// file to PRINT.
							CreateRequest("PRINT",strArray[index].sFilename,"");
			
							// Sends this request to the listener
							SendDataToListener(REQUESTFILE);
						}
					}
					// else throw and exception, Folders cannot be printed
					else throw new Exception("Cannot print folder");
				
				// If nothing is selected than displays an error message
				else throw new Exception("Nothing Selected");
			}

			catch( Exception err ) { MessageBox.Show(err.Message, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		protected void btnChat_Click (object sender, System.EventArgs e)
		{
			frmChat chat = new frmChat(COMPUTERNAME);
			chat.ShowDialog();
		}

		/// <summary>
		/// This function is used to extract the mask value for
		/// a given filename
		/// </summary>
		/// <param name="Filename"> </param>
		private int GetMask(string Filename)
		{
			// assigns -1 to a local variable. If no matching file is found then return -1
			int iReturn = -1;
			
			// take one by one entry and check it for the matching value
			for(int i = 0; i < strArray.Length; i++)
				// if match is found
				if( 0 == strArray[i].sFilename.Substring(0,Filename.Length).CompareTo(Filename) ) 
					// assign the actual mask value of the match to iReturn
					iReturn = strArray[i].iMask; 
			
			//returns the value for iReturn
			return iReturn;
		}

		/// <summary>
		/// Invoked when Upload button is clicked
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnUpload_Click (object sender, System.EventArgs e)
		{
			try
			{
				//Since you cannot upload at root level
				// This line check that if you are at root level or not
				if( null != PARENTFOLDER )
				{
					// if Mask is 0 or less than 0 than you cannot upload a file
					if( 0 < GetMask(PARENTFOLDER) )
					{
						// Since we have used OpenFileDialog to select the
						// filename to save. This initialises InitialDirectory
						// property of the FileOpenDialog to The path from where
						// the application is running
						FileOpenDialog.InitialDirectory = Application.StartupPath; 
						
						// If use press OK in the FileOpenDialog box
						if( System.Windows.Forms.DialogResult.OK == FileOpenDialog.ShowDialog() )
						{
							// Assigns a local variable to the name of the
							// to upload
							string LocalFilename = FileOpenDialog.FileName;

							// Assigns a local variable to the name of the
							// to upload which is to be sent to the listener
							string RemoteFilename = LocalFilename;
							
							// Extract the filename from the Full Qualified name
							RemoteFilename = RemoteFilename.Substring(RemoteFilename.LastIndexOf("\\") + 1);
							
							// Open a connection
							if( OpenConnection(COMPUTERNAME) )
							{
								// Temporary varable used to store the current value of the
								// status bar text and to replace it further
								string sTemp = sBar.Text;
								
								// Creates an UPLOAD request with the filename and mask
								CreateRequest("UPLOAD",PARENTFOLDER + RemoteFilename,GetMask(PARENTFOLDER).ToString());
								
								// sends this request to the listener
								SendDataToListener(REQUESTFILE);
								
								// Show wait cursor while uploading the file
								Cursor = Cursors.WaitCursor; 
								
								// changes the status bar text
								sBar.Text = "Uploading file. Please wait...";
								
								// first process all the pending events from the message queue
								// so that application doesnt seems blocking
								Application.DoEvents();
								
								// Assign fileStream object to the local file which is
								// to be upload
								fileStream = new FileStream(LocalFilename,FileMode.Open,FileAccess.Read); 
								
								// bReader is used to read data from the file in
								// binary mode. BinaryReader is a System defined class
								// Create a new object for Binary reader link it 
								// to the filestream which is created above
								BinaryReader bReader = new BinaryReader(fileStream);
								
								// Initialized the ReadBuffer variable here
								// to read only 512 bytes at a time
								ReadBuffer = new Byte[MAX_SIZE];
								
								// Read only 512 bytes at a time from the file
								// and writes to the socket stream.
								// This read continues until the control reaches the
								// end of file
								while( 0 != (iBytes = bReader.Read(ReadBuffer,0,ReadBuffer.Length)) )
									StreamTCP.Write(ReadBuffer,0,iBytes);
								
								// now close the binary reader since it is no longer needed
								bReader.Close();
								
								// closes the fileStream object
								fileStream.Close();
								
								// close the socket connection
								CloseConnection();
								
								// Restore the staus bar text
								sBar.Text = sTemp;
								
								// restore the cursor
								Cursor = Cursors.Default; 
							}
						}
					}		
					
					// If mask is 0 or les than 0 than throw the exception
					else throw new Exception("Read Only folder detected. Access Denied");
				}
				// Throw the exception os root level it detected
				// since you cannot upload at root level	
				else throw new Exception("Cannot upload at Root level");
			}
			
			// Catches any thrown exception
			catch( Exception err ) { MessageBox.Show(err.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}
		
		/// <summary>
		/// Handles any key press events in the list view
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void lvFiles_KeyPress (object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// if Enter key(13) or Space bar Key(32) is pressed
			// than call the Listview double click function
			if( 13 == e.KeyChar || 32 == e.KeyChar )
				lvFiles_DoubleClick(null,System.EventArgs.Empty);
		}

		/// <summary>
		/// Called when close button is clicked
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnClose_Click (object sender, System.EventArgs e) {	this.Close(); /* closes this dialog box*/ }
		
		/// <summary>
		/// This Function creates all XML requests 
		/// by given Request Scope and Mask
		/// </summary>
		/// <param name="Request"> </param>
		/// <param name="Scope"> </param>
		/// <param name="Mask"> </param>
		public void CreateRequest(string Request, string Scope, string Mask)
		{
			// creates a new object for xmlCreate variable
			xmlCreate = new XMLCreater(REQUESTFILE,"");
			
			// Actually writes the XML request. This is a user defined function
			xmlCreate.WriteRequest(Request,Scope,Mask);
		}

		/// <summary>
		/// Opens a Socket connection for every transaction
		/// </summary>
		/// <param name="Computername"> </param>
		public bool OpenConnection(string Computername)
		{
			bool IsConnected = false;
			try
			{
				// Initializes the local bool variable to false
				// value of this variable will be returned by the function
			
				// extract the IP address from the Computername
				string AddressIP = Computername.Substring(Computername.IndexOf("(")+1);
				AddressIP = AddressIP.Substring(0, AddressIP.Length - 1);

				// defines the remote end point. That is where to connect
				// and at which port to connect to the listener
				// IPEndPoint is a system define class
				IPEndPoint RemoteEP = new IPEndPoint( IPAddress.Parse(AddressIP),7070 );
			
				// initializes the cLientSocket variable
				ClientSocket = new clsChatSocket();
			
				// Performs a remote connection operation
				ClientSocket.Connect(RemoteEP);
									     
				if( ClientSocket.IfActive() )
				{
					// Enable the various buttons
					btnSearch.Enabled = true;
					btnUpload.Enabled = true;
					btnChat.Enabled = true;
			
					// gets the stream for the currently connected socket
					// This stream is used to send and recieve data
					// from and to the listener
					StreamTCP = ClientSocket.GetStream();

					// assigns the true value to IsConnected variable
					IsConnected = true;
				}
			}
			catch( Exception err ) { MessageBox.Show(err.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error); }

			//return the value for IsConnected variable
			return IsConnected;
		}

		/// <summary>
		/// This function sends any type of request to the listener
		/// which is present in a file represented by filename
		/// </summary>
		/// <param name="Filename"> </param>
		public void SendDataToListener(string Filename)
		{
            // Creates a new object for fileStream with File open mode
			fileStream = new FileStream(Filename,FileMode.Open);
			
			// initializes and assigns the ReadBuffer with the
			// length of the file
			ReadBuffer = new Byte[Convert.ToInt32(fileStream.Length)];
			
			// read whole file in one shot in ReadBuffer variable
			fileStream.Read(ReadBuffer,0,ReadBuffer.Length);
			
			// closed the fileStream
			fileStream.Close();
			
			// Delete the RequestFile
			new ServerCommunication().FileDelete(REQUESTFILE);
			
			// Write the read data to the Socket stream...This data is read by the
			// listener
			StreamTCP.Write(ReadBuffer,0,ReadBuffer.Length);
		}
			
		/// <summary>
		/// After sending the request, the response will be handled
		/// by this function. The response will be written in a file
		/// represented by the filename. This is used to read the response
		/// that is sedn by the listener
		/// </summary>
		/// <param name="Filename"> </param>
		public void GetDataFromListener(string Filename)
		{
			// Initializes the WriteBuffer variable to hold
			// 512 character at a time
			WriteBuffer = new Byte[MAX_SIZE];

			// Creates a File stream to store the response data.
			fileStream = new FileStream(Filename,FileMode.Create);
			
			// Creates a binary write by which we can write to the file
			BinaryWriter bWriter = new BinaryWriter(fileStream);

			// Read only 512 bytes at a time from the socket
			// stream and writes to the File.
			// This read continues until the control reaches the
			// finds no more bytes to read
			while( 0 != (iBytes = StreamTCP.Read(WriteBuffer,0,WriteBuffer.Length)) )
				bWriter.Write(WriteBuffer,0,iBytes);
			
			// Closed the binary writer
			bWriter.Close();
			
			// Closed the fileStream Object
			fileStream.Close();
		}

		/// <summary>
		/// Invoked when user double clicks on the list view
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void lvFiles_DoubleClick (object sender, System.EventArgs e)
		{
			// local variable to store index value
			int		index;
			
			// local variable to store the name of the entry at which user 
			// double clicks
			string	Filename;
			
			try
			{
				// Get the selected entry from the list view,
				// its name and its index at which it is present
				index = GetSelectedItemFromListView(out Filename);	
				
				// Index must be greater than -1
				if( -1 != index )
				{
					// if Filename is null than directory is selected
					if( null == Filename )
					{
						// This will send the request to listener with the name of the folder
						// whose contents are to be shown
						frmShare ShareForm = new frmShare(COMPUTERNAME,"SEARCH",strArray[index].sFilename + "*.*",strArray[index].iMask.ToString());
						
						//Now show the contents here
						ShareForm.Show();
					}
					
					// if the user double clicks on the file than 
					// download that file
					else DownloadFile(index,Filename,false);
				}
				
				// if index is -1 than displays an approperiate error message
				else throw new Exception("Nothing Selected");
			}
			
			// Catch and show any system error message here
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }  
		}

		/// <summary>
		/// This function gets the value coressponding to the array
		/// which is currently selected in a list view
		/// It returns the value in sReturn and function returns the
		/// index of this value in the array
		/// </summary>
		/// <param name="sReturn"> </param>
		private int GetSelectedItemFromListView( out string sReturn )
		{
			// This will gets the selected items from the list
			// in our case only one item is selected
			System.Windows.Forms.ListView.SelectedListViewItemCollection   items = lvFiles.SelectedItems;
			
			// initialize ant assign iIndex to -1
			int iIndex = -1;
			
			// This value will be returned by the function in out string sReturn
			sReturn = null;
			
			// if nothing is selected dont go inside this iteration
			if( 0 < items.Count )
			{
				// Get the index of the selected item
				iIndex = items[0].Index;
				
				// finds the actual finame by the give index from the list
				// which we have maintained
				string Filename = strArray[iIndex].sFilename;
				
				// Checks whether the selected entry is a Folder or File
				// if Folder is selected than return the name of the folder
				// else return null
				if( !Filename.EndsWith("\\") )
					sReturn = Filename.Substring(Filename.LastIndexOf("\\") + 1);
				else sReturn = null;
			}
			return iIndex;
		}
	
		/// <summary>
		/// This function parse any XML response and displays it in the window
		/// The Response is in a file denoted by  Filename
		/// </summary>
		/// <param name="Filename"> </param>
		public bool Parsing(string Filename)
		{
			// This variable is returned by the function, if the value
			// of this variable is true than Parsing is successfull else
			// Parsing Failed
			bool bReturn = false;
			
			// This line of code will create a new instance of
			// XMLParser class and assigns it to the xmlParser variable
			xmlParser = new WorkingWithXML.XMLParser();
			
			// This will create a new instance of the XMLSTRUCT structure
			// and assigns it to the xmlStruct variable
			xmlStruct = new WorkingWithXML.XMLSTRUCT();

			// This will check whether the correct XML is recieved by the browser
			// or not if correct XML is recievd than only parse that XML
			// The XML response must me of type "SHOWFILES"
			if( 0 == new ServerCommunication().TypeOfXMLRecieved(RESPONSEFILE).CompareTo("SHOWFILES") )
			{
				// The ParseXML function is present in the XMLParser class
				// which is represented by xmlParser variable, and it will
				// return the number of records parsed by the parser
				int iEntries = xmlParser.ParseXML(RESPONSEFILE, out xmlStruct,new ServerCommunication().TypeOfXMLRecieved(RESPONSEFILE));
				
				// If number of records greater than zero than continue
				// further, else dont go inside
				if( 0 < iEntries )
				{
					/// This block of code is used to sort the records
					/// first the Folders should come and than the files
					/// are to be sorted by their sizes
					for( int i = 0; i < iEntries; i++ )
					{
						for( int k = i + 1 ; k < iEntries; k++ )
						{
							if( !xmlStruct.SHOWFILES[i].sFilename.EndsWith("\\") && xmlStruct.SHOWFILES[k].sFilename.EndsWith("\\") || xmlStruct.SHOWFILES[i].iFileSize > xmlStruct.SHOWFILES[k].iFileSize )
							{
								string Temp;
								Temp = xmlStruct.SHOWFILES[i].sFilename;
								xmlStruct.SHOWFILES[i].sFilename = xmlStruct.SHOWFILES[k].sFilename;
								xmlStruct.SHOWFILES[k].sFilename = Temp;

								Temp = xmlStruct.SHOWFILES[i].iFileSize.ToString(); 
								xmlStruct.SHOWFILES[i].iFileSize = xmlStruct.SHOWFILES[k].iFileSize;
								xmlStruct.SHOWFILES[k].iFileSize = Convert.ToInt32(Temp);

								Temp = xmlStruct.SHOWFILES[i].iMask.ToString();
								xmlStruct.SHOWFILES[i].iMask = xmlStruct.SHOWFILES[k].iMask;
								xmlStruct.SHOWFILES[k].iMask = Convert.ToInt32(Temp);
							}
						}
					}
					/////////////////////////////////////////////////////
					/////////////////////////////////////////////////////

					// A temporary array is maintained for the inner 
					// purpose of programming, the entries in this strArray
					// variable are not shown anywhere to the user
					
					// This line declares a instance of the __SHOWFILES
					// struct equals to the number of Records found in the
					// XML and assigns to the global variable strArray
					strArray = new XMLSTRUCT.__SHOWFILES[iEntries];
					
					System.Windows.Forms.ListViewItem lvItems = new System.Windows.Forms.ListViewItem();

					// declaration of a local variable fName
					// used to store temporary data
					string fName;
					
					// This will take one by one record from the SHOWFILES
					// structure of the XMLSTRUCT
					for( int i = 0; i < iEntries; i++ )
					{
						ImageList imgList = new ImageList();
						imgList.Images.Add(System.Drawing.Image.FromFile(Application.StartupPath+"\\Folder.ico"));
						imgList.Images.Add(System.Drawing.Image.FromFile(Application.StartupPath+"\\File.ico"));
						lvFiles.SmallImageList = imgList;

						// assigns the filename present in the SHOWFILES struct
						// to fName variable
						fName = xmlStruct.SHOWFILES[i].sFilename;
						
						// Fills the strArray structure with its values ////////
						strArray[i].sFilename = fName;
						strArray[i].iMask = xmlStruct.SHOWFILES[i].iMask;
						
						// not used in this version, but kept for future
						strArray[i].iFileSize = xmlStruct.SHOWFILES[i].iFileSize; 
						////////////////////////////////////////////////////////
						
						// Checks whether the value present in fName is Folder
						// or file, because folder name ends with backslash
						if( fName.EndsWith("\\") )
						{
							// This will remove the trailing backslashes from the
							// fName
							fName = fName.Substring(0,fName.Length - 1);
							
							// This will insert a single record in the listview
							// represented by fName and sSubItems which includes
							// size and type. 0 Here displays the 0th image from
							// the image list which is the image of the folder
							lvItems = lvFiles.Items.Insert(i,fName);

							// Item 0 represents to the size, Since there
							// is no size for the directory so this is null
							lvItems.SubItems.Add("");
							
							// Assigns the folder value to the 1 item, since
							// it is a folder
							lvItems.SubItems.Add("Folder");	

							lvItems.ImageIndex = 0;
						}
						
						// The control will come into this code only if the record is a
						// File not folder
						else 
						{
							// Extracts only filename from Full qualified path
							fName = fName.Substring(fName.LastIndexOf("\\")+1);
							
							// This will insert a single record in the listview
							// represented by fName and sSubItems which includes
							// size and type. 1 Here displays the 1st image from
							// the image list which is the image of the file
							lvItems = lvFiles.Items.Insert(i,fName);

							// Assignes the file size
							lvItems.SubItems.Add( xmlStruct.SHOWFILES[i].iFileSize.ToString());
							
							// extract the extension of the fileand assigns it to the
							// last element of sSubItem
							lvItems.SubItems.Add(fName.Substring(fName.LastIndexOf(".") + 1) + " File");	

							lvItems.ImageIndex = 1;
						}
					}
					// assigns a true value to bReturn variable to indicate that
					// parsing and displaying of the records are successful
					bReturn = true;
				}
				
				else
				{
					// Calls the DisableUI function to disablt the buttons
					DisableUI();
					
					// Displays the Message Box 
					MessageBox.Show("No result found for this request","Information",MessageBoxButtons.OK, MessageBoxIcon.Information); 
				}
			}

			// The case if any error has occured at the listener's side 
			// is informed by the Error XML. If SHOWFILES XML is not
			// returned than parse the ERROR XML and Show the error to the user
			else
			{
				// Parses the error XML returned by the listener
				xmlParser.ParseXML(RESPONSEFILE, out xmlStruct,new ServerCommunication().TypeOfXMLRecieved(RESPONSEFILE));
				
				// calls DisableUI to disable various the Userinterface controls
				DisableUI();
				
				// Popup the message box and displays the error message
				MessageBox.Show(xmlStruct.ERROR.sDescription, xmlStruct.ERROR.sSeverity,MessageBoxButtons.OK, MessageBoxIcon.Error); 
			}
			
			// Delete the File containing the XML Response from the listener
			new ServerCommunication().FileDelete(RESPONSEFILE);
			
			// returns the value for bReturn Variable
			return bReturn;
		}

		/// <summary>
		/// Disable the buttons when not needed
		/// </summary>
		private void DisableUI()
		{
			btnUpload.Enabled = false;
			btnDownload.Enabled = false;
			btnSearch.Enabled = false;
			btnChat.Enabled = false;
			btnPrint.Enabled = false;
			btnStream.Enabled = false;
			lvFiles.Enabled = false;
		}

		/// <summary>
		/// Closes the current TCP and Stream Connections
		/// </summary>
		public void CloseConnection() { StreamTCP.Close(); ClientSocket.Close(); }
		
		/// <summary>
		/// This function performs a download operation of a file
		/// from the listener's end. This function returns the actual
		/// download path from the remote machine. It takes the index
		/// of the selected item in list view, name of the file to download
		/// and bDelete, whether to delete a file after downloading bDelete
		/// is always false in this case. You can use it for any further
		/// purpose. If bDelete if true the file will be deleted after
		/// downloading.
		/// </summary>
		/// <param name="index"> </param>
		/// <param name="Filename"> </param>
		/// <param name="bDelete"> </param>
		private string DownloadFile(int index, string Filename, bool bDelete)
		{
			// Change the title of the File savedialog box
			FileSaveDialog.Title = "Download As";
			
			// Pops up the File save dialog box with the default filename
			FileSaveDialog.FileName = Filename;
			
			// Assigns the Initial directory of the File save dialog box
			// to the application's startup path
			FileSaveDialog.InitialDirectory = Application.StartupPath;
			
			// Declares an initialize the variable sReturn which is used
			// to return the value by function. This stores the name of the
			// file which is to be downloaded from the remote end
			string sReturn = null;		
			
			// If user choose OK from the File Save dialog box than only
			// download can begin
			if( System.Windows.Forms.DialogResult.OK == FileSaveDialog.ShowDialog() )
			{
				// Stores the current text of the status bar is temporary
				// variable sTemp
				string sTemp = sBar.Text;

				// Writes new text on the status bar
				sBar.Text = "Downloading File. Please wait...";
				
				// Show the hour glass cursor while downloading the file
				Cursor = Cursors.WaitCursor; 
				
				// process all the pending events first from the message loop
				Application.DoEvents(); 

				// Opens socket connection to the listener and checks
				// whether connected or not
				if( OpenConnection(COMPUTERNAME) )
				{
					// Creates a DOWNLOAD request for a particular
					// file to download.
					CreateRequest("DOWNLOAD",strArray[index].sFilename,"");
					
					// Sends this request to the listener
					SendDataToListener(REQUESTFILE);

					// Assigns the sReturn variable to the filename choosen for
					// download
					sReturn = FileSaveDialog.FileName;

					// Get the download data from the listener and save it
					// in the filename represented by the Filename property
					// of File save dialog box
					GetDataFromListener(FileSaveDialog.FileName);
					
					// Close the currently opened socket connection
					CloseConnection();
					
					//Delete the file if bDelete is true
					if( bDelete ) new ServerCommunication().FileDelete(Filename);
     				
					// Restore the previos text of the status bar
					sBar.Text = sTemp;
				}
			}
			
			// restore the default cursor state
			Cursor = Cursors.Default;
			
			// returns the filename with Full qualified path
			return sReturn;		
		}
		
		/// <summary>
		/// Invoked when the download button is clicked
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnDownload_Click (object sender, System.EventArgs e)
		{
 			// Declares a local Filename variable
			string Filename;
			
			// Get the currently selected item from the List view
			int index = GetSelectedItemFromListView(out Filename);
			try
			{
				// Checks whether any entry is selected or not
				if( -1 != index)
					
					// Checks whether a filename is selected for downloading
					// or not
					if( null != Filename )
						
						// If filename is selected than download it
						DownloadFile(index,Filename,false);
					
					// else throw and exception, Folders cannot be downloaded
					else throw new Exception("Cannot download folder");
				
				// If nothing is selected than displays an error message
				else throw new Exception("Nothing Selected");
			}
			
			// catches any system generated error message and displays it
			// to the user
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }  
		}

		/// <summary>
		/// Invoked when the search button is clicked
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				// Check that if search is performes at root level or not,
				// since root level search is not possible from here
				if( null != PARENTFOLDER )
				{
					// Creates a new object for frmSearch and initializes it
					frmSearch SearchForm = new frmSearch();
					
					// This statememt restrict the search form from
					// Showing Search on: criteria Textbox
					SearchForm.bFlag = false;
					
					// If user presses OK in the search form that go inside
					if( System.Windows.Forms.DialogResult.OK == SearchForm.ShowDialog() )
					{
						// Get the search criteria from the SearchFor variable
						// of the frmSearch class and passes it to the constructor
						// of this class with some more details
						frmShare ShareForm = new frmShare(COMPUTERNAME,"SEARCH",PARENTFOLDER + SearchForm.SearchFor,GetMask(PARENTFOLDER).ToString()); 
						
						// Again show this dialog with the search results
						ShareForm.ShowDialog(); 
					}
				}
				
				// Displays an error message
				else throw new Exception("Cannot search at root level here");
			}
			
			// Catches any system generated error and displays it
			catch( Exception err ) { MessageBox.Show(err.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		private void btnStream_Click(object sender, System.EventArgs e)
		{
			// Declares a local Filename variable
			string Filename;
			
			// Extract the IP address from the COMPUTERNAME variable
			string IPAddress = COMPUTERNAME.Substring(COMPUTERNAME.IndexOf("(")+1);
			IPAddress = IPAddress.Substring(0, IPAddress.Length - 1);

			string sText = this.sBar.Text;

			// Get the currently selected item from the List view
			int index = GetSelectedItemFromListView(out Filename);
			try
			{
				// Checks whether any entry is selected or not
				if( -1 != index)
				{	
					// Checks whether a filename is selected for Streaming
					// or not
					if( null != Filename )
					{
						OpenConnection(COMPUTERNAME);
						
						// Creates a stream request for a particular
						// file to stream.
						CreateRequest("STREAMING",strArray[index].sFilename,"");

						// Sends this request to the listener
						SendDataToListener(REQUESTFILE);

						// Get the response from the listener in lieu of the
						// above request
						GetDataFromListener(RESPONSEFILE);

						xmlParser = new WorkingWithXML.XMLParser();
						xmlStruct = new XMLSTRUCT(); 
						xmlParser.ParseXML(RESPONSEFILE,out xmlStruct,new ServerCommunication().TypeOfXMLRecieved(RESPONSEFILE));
							
						if( 0 == xmlStruct.ERROR.sDescription.CompareTo("No Error") )
						{
							this.sBar.Text = "Now Streaming. Please wait...";
							Application.DoEvents();
							frmStreamer StreamerForm = new frmStreamer(IPAddress);
							StreamerForm.ShowDialog();
						}
						else throw new Exception(xmlStruct.ERROR.sDescription);
					}
					else throw new Exception("Cannot stream a folder");
				}
				// If nothing is selected than displays an error message
				else throw new Exception("Nothing Selected");
			}

			// catches any system generated error message and displays it
			// to the user
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }
			finally 
			{ 
				new ServerCommunication().FileDelete(RESPONSEFILE); 
				new ServerCommunication().FileDelete(REQUESTFILE);
				this.sBar.Text = sText;
				
			}
		}

		private void lvFiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Declares a local Filename variable
			string Filename;
			
			// Get the currently selected item from the List view
			int index = GetSelectedItemFromListView(out Filename);
			
			// Checks whether any entry is selected or not
			if( -1 != index)
			{	
				// Checks whether a filename is selected or not
				if( null != Filename )
				{
					btnPrint.Enabled = true;
					btnStream.Enabled = true;
					btnDownload.Enabled = true;
				}
				else
				{
					btnPrint.Enabled = false;
					btnStream.Enabled = false;
					btnDownload.Enabled = false;
				}
			}
		}
    }
}