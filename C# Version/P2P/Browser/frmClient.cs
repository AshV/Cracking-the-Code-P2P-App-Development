namespace Client				// Copyright 2001 Dreamtech Software India Inc.
{								// All rights reserved
    using System;				// Provides the basic functionality of .NET
	using System.Drawing;		// Provides the Drawing features, Used for cursors
	using System.Collections;	// Provides the different type of class collections
	using System.ComponentModel;// Provides the facility of using components
	using System.Windows.Forms;	// Provides the darwing of buttons, listviews etc	
	using System.Net;			// Provides the net related functionality 
	using System.Text;			// Provides the text manipulation functions
	using System.IO;			// Provides I/O features
	using WorkingWithXML;		// Custom Class
	
	/// <summary>
	/// This structure is used for book keeping purpose
	/// The currently running listener list will be requsted
	/// by the server and server sends that list
	/// Each record in this structure represent the data for the single
	/// running listener
	/// </summary>
	public struct __CONNECTEDCOMPUTERS
	{
		/// <summary>
		/// Declares the string used to store the name of the listener
		/// This name is in human redable format
		/// </summary>
		public string	sComputername;
		
		/// <summary>
		/// sIPAddress variable is used to store the IP address of 
		/// the listener
		/// </summary>
		public string	sIPAddress;
		
		/// <summary>
		/// This will be 0 in starting and consequently filled
		/// by every search operation
		/// </summary>
		public int		iFilesFound;
	}

	/// <summary>
    ///    Summary description for frmClient class.
    /// </summary>
    public class frmClient : System.Windows.Forms.Form
    {
		private System.ComponentModel.IContainer	components;
		private System.Windows.Forms.Button			btnViewFiles;
		private System.Windows.Forms.Button			btnSearch;
		private System.Windows.Forms.ToolTip		ToolTipText;
		private System.Windows.Forms.Button			btnQuit;
		private System.Windows.Forms.Button			btnRefresh;
		private System.Windows.Forms.Button			btnOpen;

		/// <summary>
		/// The below declared variables are the user defined variables
		/// used within this class
		/// </summary>
		
		/// <summary>
		/// Stores the total number of listeners found 
		/// </summary>
		private int						iConnectedComputers;
		
		/// <summary>
		/// Stores the number of search results found for 
		/// the matching criteria
		/// </summary>
		private int						iSearchResult;
		
		/// <summary>
		/// Declares a xmlParser variable of type XMLParser (User defined class)
		/// </summary>
		private XMLParser	 			xmlParser;
		
		/// <summary>
		/// Declares a xmlServerComm variable of type ServerCommunication 
		/// (User defined class)
		/// </summary>
		private ServerCommunication		xmlServerComm;

		/// <summary>
		/// Declares an object xmlStruct of type XMLSTRUCT (User defined class)
		/// </summary>
		private XMLSTRUCT				xmlStruct;
		
		/// <summary>
		/// Declares an array of ConnectedComputers of type 
		/// __CONNECTEDCOMPUTERS structures
		/// </summary>
		private __CONNECTEDCOMPUTERS[]				ConnectedComputers;
		private System.Windows.Forms.ListView		lvComputers;
		private System.Windows.Forms.ColumnHeader	clhComputername;
		private System.Windows.Forms.ColumnHeader	clhIPAddress;
		private System.Windows.Forms.ColumnHeader	clhObjects;
		private System.Windows.Forms.Label lblCopyright;
		
		/// <summary>
		/// declares sSubItems variable as an array of string
		/// </summary>
		private System.Windows.Forms.ListViewItem	lvItems;

		/// <summary>
		/// This is the default constructor of the class
		/// </summary>
		public frmClient()
        {
            //
            // Required for Windows Form Designer support
            //
            // Auto generated function by the IDE
			InitializeComponent();

			// Puts the Computer.ico as the form icon
			this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\Computer.ico");
			
			// Creates a new instance of XMLParser class
			xmlParser = new WorkingWithXML.XMLParser();
			
			// Creates a new instance of XMLSTRUCT structure
			xmlStruct = new WorkingWithXML.XMLSTRUCT();
			
			// Creates a new instance for ServerCommunication class
			xmlServerComm = new WorkingWithXML.ServerCommunication();
			
			// Assigns the global value for USERLISTXML
			xmlParser.USERLISTXML = Application.StartupPath + "\\userlist.xml";
			
			// Assigns the global value for SERVERSEARCHXML
			xmlParser.SERVERSEARCHRESULTXML = Application.StartupPath + "\\search.xml";
 			try
			{
				// Fills the List view with the values
				// these values are the response from the server
				if( 0 ==  PopulateList() )
					// Displays a message if no computer is connected to the network
					throw new Exception("No computer is connected to the network. The list will be empty"); 
			}
			
			// Hanldes every exceptions that is thrown 
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        /// <summary>
        ///    Clean up any resources being used.
        ///    This is auto generated by the IDE
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
			components.Dispose();
        }

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        ///     this code is generated automatically by the IDE
        /// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ToolTipText = new System.Windows.Forms.ToolTip(this.components);
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnQuit = new System.Windows.Forms.Button();
			this.btnViewFiles = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnOpen = new System.Windows.Forms.Button();
			this.lvComputers = new System.Windows.Forms.ListView();
			this.clhComputername = new System.Windows.Forms.ColumnHeader();
			this.clhIPAddress = new System.Windows.Forms.ColumnHeader();
			this.clhObjects = new System.Windows.Forms.ColumnHeader();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.Chocolate;
			this.btnSearch.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSearch.ForeColor = System.Drawing.Color.White;
			this.btnSearch.Location = new System.Drawing.Point(137, 1);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(67, 35);
			this.btnSearch.TabIndex = 2;
			this.btnSearch.Text = "&Search";
			this.ToolTipText.SetToolTip(this.btnSearch, "Search on computers for filenames");
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnQuit
			// 
			this.btnQuit.BackColor = System.Drawing.Color.Chocolate;
			this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnQuit.ForeColor = System.Drawing.Color.White;
			this.btnQuit.Location = new System.Drawing.Point(270, 1);
			this.btnQuit.Name = "btnQuit";
			this.btnQuit.Size = new System.Drawing.Size(67, 35);
			this.btnQuit.TabIndex = 3;
			this.btnQuit.Text = "&Quit";
			this.ToolTipText.SetToolTip(this.btnQuit, "Quit this application");
			this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
			// 
			// btnViewFiles
			// 
			this.btnViewFiles.BackColor = System.Drawing.Color.Chocolate;
			this.btnViewFiles.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnViewFiles.Enabled = false;
			this.btnViewFiles.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnViewFiles.ForeColor = System.Drawing.Color.White;
			this.btnViewFiles.Location = new System.Drawing.Point(203, 1);
			this.btnViewFiles.Name = "btnViewFiles";
			this.btnViewFiles.Size = new System.Drawing.Size(67, 35);
			this.btnViewFiles.TabIndex = 5;
			this.btnViewFiles.Text = "&View Files";
			this.ToolTipText.SetToolTip(this.btnViewFiles, "View the searched files");
			this.btnViewFiles.Click += new System.EventHandler(this.btnViewFiles_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.Color.Chocolate;
			this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRefresh.ForeColor = System.Drawing.Color.White;
			this.btnRefresh.Location = new System.Drawing.Point(70, 1);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(67, 35);
			this.btnRefresh.TabIndex = 1;
			this.btnRefresh.Text = "&Refresh";
			this.ToolTipText.SetToolTip(this.btnRefresh, "Refresh the computer list");
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.BackColor = System.Drawing.Color.Chocolate;
			this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnOpen.ForeColor = System.Drawing.Color.White;
			this.btnOpen.Location = new System.Drawing.Point(3, 1);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(67, 35);
			this.btnOpen.TabIndex = 0;
			this.btnOpen.Text = "&Open";
			this.ToolTipText.SetToolTip(this.btnOpen, "Connect to the selected computer");
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// lvComputers
			// 
			this.lvComputers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.clhComputername,
																						  this.clhIPAddress,
																						  this.clhObjects});
			this.lvComputers.Location = new System.Drawing.Point(3, 41);
			this.lvComputers.Name = "lvComputers";
			this.lvComputers.Size = new System.Drawing.Size(530, 146);
			this.lvComputers.TabIndex = 7;
			this.ToolTipText.SetToolTip(this.lvComputers, "Select a computer to connect");
			this.lvComputers.View = System.Windows.Forms.View.Details;
			this.lvComputers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvComputers_KeyPress);
			this.lvComputers.DoubleClick += new System.EventHandler(this.lvComputers_DoubleClick);
			this.lvComputers.SelectedIndexChanged += new System.EventHandler(this.lvComputers_SelectedIndexChanged);
			// 
			// clhComputername
			// 
			this.clhComputername.Text = "Computername";
			this.clhComputername.Width = 222;
			// 
			// clhIPAddress
			// 
			this.clhIPAddress.Text = "IP Address";
			this.clhIPAddress.Width = 142;
			// 
			// clhObjects
			// 
			this.clhObjects.Text = "No. of Objects found";
			this.clhObjects.Width = 112;
			// 
			// lblCopyright
			// 
			this.lblCopyright.Location = new System.Drawing.Point(348, 10);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(192, 14);
			this.lblCopyright.TabIndex = 6;
			this.lblCopyright.Text = "© 2001 www.dreamtechsoftware.com";
			// 
			// frmClient
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnQuit;
			this.ClientSize = new System.Drawing.Size(536, 190);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lvComputers,
																		  this.lblCopyright,
																		  this.btnViewFiles,
																		  this.btnSearch,
																		  this.btnQuit,
																		  this.btnRefresh,
																		  this.btnOpen});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "frmClient";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Peer to Peer Browser";
			this.ResumeLayout(false);

		}
		
		/// <summary>
		/// Fills the List view with initial values
		/// This function is called from the constructor of this class
		/// </summary>
		private int PopulateList()
		{
			try 
			{
				// This line of code gets the response from the server 
				// by calling an asp page and writes it in a file
				// represented by xmlParser.USERLISTXML
				xmlServerComm.WriteDataToFile(xmlParser.USERLISTXML,xmlServerComm.GetDataFromServer("http://SERVERNAME/userlist.asp"));

				// This line will parse the returned XML by the server
				// and saves it in the xmlStruct variable
				// iConnectedComputers will have value for total
				// number of connected listeners
				iConnectedComputers = xmlParser.ParseXML(xmlParser.USERLISTXML,out xmlStruct, xmlServerComm.TypeOfXMLRecieved(xmlParser.USERLISTXML));
				
				// Clears the every item of the list view
				lvComputers.Items.Clear();
				
				// Deletes the File represented by xmlParser.USERLISTXML
				xmlServerComm.FileDelete(xmlParser.USERLISTXML);
				
				// initialize the ConnectedComputers array to
				// the number of listeners found connected
				ConnectedComputers = new __CONNECTEDCOMPUTERS[iConnectedComputers];
				
				// Initializes lvItems object
				lvItems = new System.Windows.Forms.ListViewItem();

				/// The below 17 lines of code is used for sorting the
				/// USERLIST alphabetically
				for( int i = 0; i < iConnectedComputers; i++ )
				{
					for( int j = i+1; j < iConnectedComputers; j++ )
					{
						if( xmlStruct.USERLIST[i].sUsername.GetHashCode() < xmlStruct.USERLIST[j].sUsername.GetHashCode() )
						{
							string sTemp;
							sTemp = xmlStruct.USERLIST[j].sUsername;
							xmlStruct.USERLIST[j].sUsername = xmlStruct.USERLIST[i].sUsername;
							xmlStruct.USERLIST[i].sUsername = sTemp;

							sTemp = xmlStruct.USERLIST[j].sIPAddress;
							xmlStruct.USERLIST[j].sIPAddress = xmlStruct.USERLIST[i].sIPAddress;
							xmlStruct.USERLIST[i].sIPAddress = sTemp;
						}
					}
				}
				/////////////////////////////////////////////////////////
				///

				
				// Fills the ConnectedComputer array with user list values
				for( int i = 0; i < iConnectedComputers; i++ )
				{
					ImageList imgList = new ImageList();
					imgList.Images.Add(System.Drawing.Image.FromFile(Application.StartupPath+"\\Computer.ico"));
					lvComputers.SmallImageList = imgList;
					ConnectedComputers[i].sComputername = xmlStruct.USERLIST[i].sUsername.Trim();
					ConnectedComputers[i].sIPAddress = xmlStruct.USERLIST[i].sIPAddress.Trim();
					ConnectedComputers[i].iFilesFound = 0;
					
					// Insert the records one by one in the list view
					lvItems = lvComputers.Items.Insert(i,ConnectedComputers[i].sComputername); 

					lvItems.SubItems.Add(ConnectedComputers[i].sIPAddress);
					lvItems.SubItems.Add("");
					lvItems.ImageIndex = 0;
				}
			}
			
			// Cathces any exception that is thrown by the application
			// and display message
			catch( Exception err ) { MessageBox.Show(err.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error); }
			
			// Return the number of connected computers
			return iConnectedComputers;
		}
		
		/// <summary>
		/// Fills the Listview with the entries as well as with
		/// the search criteria if performed any
		/// </summary>
		private void PopulateWithSearchResults()
		{
			// Decrales and initializes the iCounter value to zero
			int iCounter = 0;
			try
			{
				// Parse the XML Request returned by the server and store
				// the number of total searches found in iSearchResult
				iSearchResult = xmlParser.ParseXML(xmlParser.SERVERSEARCHRESULTXML ,out xmlStruct, xmlServerComm.TypeOfXMLRecieved(xmlParser.SERVERSEARCHRESULTXML));
	
				// Delete the SEARVERSEARCHXML file after parsing
				xmlServerComm.FileDelete(xmlParser.SERVERSEARCHRESULTXML);
				
				// The 8 lines of code will find in the search results
				// and counts the number of matched files found on each computer
				// as the result of the search i.e for example
				// on computer A 10 files are found , on B 3 files are found etc
				// This code will do the individual level breakup for the
				// search results
				for( int i = 0; i < ConnectedComputers.Length; i++)
				{
					for( int j = 0; j < iSearchResult; j++ )
						if( 0 == xmlStruct.SERVERSEARCH[j].sIPAddress.Trim().CompareTo(ConnectedComputers[i].sIPAddress.Trim()) )
							iCounter++;
					ConnectedComputers[i].iFilesFound = iCounter;
					iCounter = 0;
				}
				/////////////////////////////////////////////////////////////////
				///

				// initialize lvItems 
				lvItems = new System.Windows.Forms.ListViewItem();

				// Clears the list view items
				lvComputers.Items.Clear();

				for( int i = 0; i < ConnectedComputers.Length; i++)
				{
					ImageList imgList = new ImageList();
					imgList.Images.Add(System.Drawing.Image.FromFile(Application.StartupPath+"\\Computer.ico"));
					lvComputers.SmallImageList = imgList;

					// Insert the items in the list view
					lvItems = lvComputers.Items.Insert(i,ConnectedComputers[i].sComputername); 

					// Assigns the IP Address at first subscript
					lvItems.SubItems.Add(ConnectedComputers[i].sIPAddress.Trim());

					// Assigns the no of search results found 
					// at second subscript
					lvItems.SubItems.Add(ConnectedComputers[i].iFilesFound.ToString().Trim()); 

					lvItems.ImageIndex = 0;

				}
			}
			
			// catches any exception that is trown by the application
			catch( Exception err ) { MessageBox.Show(err.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		/// <summary>
		/// Handles the key press events on the list view
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void lvComputers_KeyPress (object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if( 13 == e.KeyChar || 32 == e.KeyChar )
				btnOpen_Click(null,System.EventArgs.Empty);
		}

		/// <summary>
		/// Invoked when the user double clicks on the list view
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void lvComputers_DoubleClick (object sender, System.EventArgs e) { btnOpen_Click(null,System.EventArgs.Empty); }
		
		/// <summary>
		/// Invoked, when the user clicks the refresh button
		/// This will refresh the contents of the listview
		/// by getting the USERLIST from the server again and
		/// shows it in the list view 
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnRefresh_Click (object sender, System.EventArgs e)
		{
 			try
			{
				if( 0 == PopulateList() )
					throw new Exception("No computer is connected to the network. The list will be empty"); 
			}
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }
		}

		/// <summary>
		/// Invoked when the user clicks on the Open button
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnOpen_Click (object sender, System.EventArgs e)
		{
			// Get the currently selected item from the list view
			System.Windows.Forms.ListView.SelectedListViewItemCollection items = lvComputers.SelectedItems;

			try
			{
				// if item is found or not 
				if( 0 < items.Count )
				{
					// Makes the string that is required to pass to the contructor
					// of the frmShare class 

					// Appends the "(" and ")" at the begin and end of the IPAddress
					string SelectedIP = " (" + items[0].SubItems[1].Text + ")" ;
					
					// gets the name of the computer from Computername column
					// of the list view
					string Computername = items[0].Text;
					
					// Concatenates the Computername to IP address
					Computername = Computername + SelectedIP;

					// declares a variable ShareForm of type frmShare class
					// and passes the Computername to it
					frmShare ShareForm = new frmShare(Computername);
					
					// Shows the frmShare dialog window
					ShareForm.Show();
				}
				
				// Throws the exception
				else throw new Exception("No selected computer found"); 
			}
			
			// cathces the exception and shows it in a message box
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }  
		}

		/// <summary>
		/// Performs a quit operation when the user clicks the quit button
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnQuit_Click (object sender, System.EventArgs e) { Application.Exit(); }
		
		/// <summary>
		/// This function is called when you click the View files button
		/// </summary>
		/// <param name="sender"> </param>

		/// <param name="e"> </param>
		protected void btnViewFiles_Click (object sender, System.EventArgs e)
		{
			// Creates an ShareForm object from frmShare class and
			// initializes it
			frmShare ShareForm = new frmShare();
			
			// Get the currently selected item from the list view
			System.Windows.Forms.ListView.SelectedListViewItemCollection   items = lvComputers.SelectedItems;
			
			// Get the IPAddress of the Selected item from IP Address
			// column
			string SelectedIP = items[0].SubItems[1].Text.Trim();

			// The followinf 12 lines of code Scans through the 
			// SERVERSEARCH list and insert the values in the List view 
			// of the frmShare class Folder and file wise
			for( int i = 0; i < iSearchResult; i++ )
			{
				if( 0 == xmlStruct.SERVERSEARCH[i].sIPAddress.Trim().CompareTo(SelectedIP) )
				{
					ImageList imgList = new ImageList();
					imgList.Images.Add(System.Drawing.Image.FromFile(Application.StartupPath+"\\Folder.ico"));
					imgList.Images.Add(System.Drawing.Image.FromFile(Application.StartupPath+"\\File.ico"));
					ShareForm.lvFiles.SmallImageList = imgList;

					if( xmlStruct.SERVERSEARCH[i].sFilename.EndsWith("\\") )
					{
						lvItems = ShareForm.lvFiles.Items.Insert(i,xmlStruct.SERVERSEARCH[i].sFilename); 
						lvItems.ImageIndex = 0;
					}
					else
					{
						lvItems = ShareForm.lvFiles.Items.Insert(i,xmlStruct.SERVERSEARCH[i].sFilename); 
						lvItems.ImageIndex = 1;
					}
				}
			}
			/////////////////////////////////////////////////////////////////
			///
			
			// Shows the ShareForm window
			ShareForm.ShowDialog();
		}

		/// <summary>
		/// Whenever the user changes the selection in the List view
		/// This function is called
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void lvComputers_SelectedIndexChanged (object sender, System.EventArgs e)
		{
			// Declare a local variable iTemp to store the number of 
			// individual search result return by the GetNumberOfObjectsFound
			// function
			int iTemp;
			
			// calls to the GetNumberOfObjectsFound function and the
			// value is returned in iTemp, out here is declared because the
			// GetNumberOfObjectsFound function throws the value out in iTemp
			// variable
			GetNumberOfObjectsFound(out iTemp);
			
			// Checks if the iTemp is zero than disbale view file button
			// else enable that button
			if( iTemp > 0 )
				btnViewFiles.Enabled = true;
			else
				btnViewFiles.Enabled = false;
		}

		/// <summary>
		///	This function returns the total object found in search
		///	for a perticular selected entry so the the state of
		///	View Files button can be toggled
		/// </summary>
		/// <param name="iReturn"> </param>
		private int GetNumberOfObjectsFound( out int iReturn )
		{
 			// get the currently selecteditem from the list view
			System.Windows.Forms.ListView.SelectedListViewItemCollection   items = lvComputers.SelectedItems;
			
			// Declares and initialize the iIndex variable to -1
			int iIndex = -1;
			
			// initilize the iReturn valiable to -1
			iReturn = -1;

			// check that if items variable have some data or not
			if( 0 < items.Count )
			{
				// get the index of the selected item
				iIndex = items[0].Index;
				
				// get the corresponding iFilesFound value from the
				// List that we have maintained by supplying the 
				// iIndex value to it
				iReturn = ConnectedComputers[iIndex].iFilesFound;
			}
			
			// Also returns the iIndex number which is selected,
			// iIndex contains -1 if nothing is selected
			return iIndex;
		}

		/// <summary>
		/// Invoked when the search button is clicked
		/// Shows the search form to enter the search criteria
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		protected void btnSearch_Click (object sender, System.EventArgs e)
		{
			try
			{
				// Declares and initialize a new variable SearchForm
				// of type frmSearch class
				frmSearch SearchForm = new frmSearch();
				
				// enables the text box control of the search Form
				SearchForm.txtSearchOn.Enabled = true;

				// enables the label control of the search Form
				SearchForm.lblSearchOn.Enabled = true;
				
				// Sets the bFlag of search form to true
				SearchForm.bFlag = true;
				
				// if Search is pressed from within the search form
				if( System.Windows.Forms.DialogResult.OK == SearchForm.ShowDialog() )
				{
					// get the search response from the server by calling
					// and asp file and getting the data returned by that file
					// which is in XML format. Later save that data in the
					// SERVERSEARCHRESULTXML for parsing
					xmlServerComm.WriteDataToFile(xmlParser.SERVERSEARCHRESULTXML,xmlServerComm.GetDataFromServer("http://SERVERNAME/search.asp?US=" + SearchForm.SearchOn + "&FS=" + SearchForm.SearchFor));

					// calls the PopulateWithSearchResults function
					PopulateWithSearchResults();
				}
			}

			// Catches any exception thrown by the application and shows it in the
			// message box
			catch( Exception err ) { MessageBox.Show(err.Message, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		/// <summary>
        /// The main entry point for the application.
        /// This is auto generated by the IDE
        /// </summary>
	    [STAThread] public static void Main(string[] args) { Application.Run(new frmClient()); }
    }
}