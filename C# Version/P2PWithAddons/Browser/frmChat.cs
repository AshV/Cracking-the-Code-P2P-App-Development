namespace Client
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading;
	using System.Net;
	using System.IO;
	using WorkingWithXML;

    /// <summary>
    ///    Summary description for frmChat.
    /// </summary>
    public class frmChat : System.Windows.Forms.Form
    {
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.RichTextBox sChatBox;
		private System.Windows.Forms.RichTextBox txtMessage;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Timer RecivingTimer;

		public	frmShare	ShareForm;
		public	XMLSTRUCT	xmlStruct;
		public  XMLParser	xmlParser;
		public  Thread		RecieveThread;
		private	clsChatSocket ClientSocket;
		private	bool		Connected;
		private	string		Computername;
		private	Stream		ClientStream;
		private	bool		Typing;
		private int			TimeElapse;
		private	bool		ListenerTyping;
		private int			ListenerTimeElapse;

		public frmChat()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

			// Puts the Computer.ico as the form icon
			this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\Computer.ico");

        }

		public frmChat(string computername)
		{
			Connected = false;
            //
            // Required for Windows Form Designer support
            //
			Computername = computername;
            InitializeComponent();

			// Puts the Computer.ico as the form icon
			this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\Computer.ico");

			statusBar1.Text = Computername + " is connected";
			Typing				= false;
			TimeElapse = 0;
			ListenerTyping = false;
			ListenerTimeElapse = 0;
			btnSend_Click(null,null);
		}

        /// <summary>
        ///    Clean up any resources being used.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            components.Dispose();
        }

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmChat));
			this.panel4 = new System.Windows.Forms.Panel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.txtMessage = new System.Windows.Forms.RichTextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.sChatBox = new System.Windows.Forms.RichTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.RecivingTimer = new System.Windows.Forms.Timer(this.components);
			this.panel4.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel4
			// 
			this.panel4.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.statusBar1,
																				 this.txtMessage,
																				 this.btnSend});
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(0, 230);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(532, 60);
			this.panel4.TabIndex = 6;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 43);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(532, 17);
			this.statusBar1.TabIndex = 4;
			// 
			// txtMessage
			// 
			this.txtMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.txtMessage.Location = new System.Drawing.Point(11, 6);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtMessage.Size = new System.Drawing.Size(437, 31);
			this.txtMessage.TabIndex = 3;
			this.txtMessage.Text = "";
			this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
			// 
			// btnSend
			// 
			this.btnSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSend.BackColor = System.Drawing.Color.Chocolate;
			this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSend.ForeColor = System.Drawing.Color.White;
			this.btnSend.Location = new System.Drawing.Point(451, 6);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(71, 31);
			this.btnSend.TabIndex = 2;
			this.btnSend.Text = "&Send";
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// sChatBox
			// 
			this.sChatBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.sChatBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.sChatBox.Location = new System.Drawing.Point(11, 29);
			this.sChatBox.Name = "sChatBox";
			this.sChatBox.ReadOnly = true;
			this.sChatBox.Size = new System.Drawing.Size(511, 196);
			this.sChatBox.TabIndex = 0;
			this.sChatBox.Text = "";
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.panel4,
																				 this.panel3});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(532, 290);
			this.panel1.TabIndex = 3;
			// 
			// panel3
			// 
			this.panel3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.label1,
																				 this.sChatBox});
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(532, 290);
			this.panel3.TabIndex = 5;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(335, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 14);
			this.label1.TabIndex = 4;
			this.label1.Text = "© 2001 www.dreamtechsoftware.com";
			// 
			// RecivingTimer
			// 
			this.RecivingTimer.Enabled = true;
			this.RecivingTimer.Interval = 200;
			this.RecivingTimer.Tick += new System.EventHandler(this.RecivingTimer_Tick);
			// 
			// frmChat
			// 
			this.AcceptButton = this.btnSend;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(532, 290);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmChat";
			this.Text = "Chat Window";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmChat_Closing);
			this.panel4.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		protected void txtMessage_KeyPress (object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			Typing = true;
			TimeElapse = 0;
 		}

		protected void btnSend_Click (object sender, System.EventArgs e)
		{
			try
			{
				if( !Connected )
				{
					if( null != Computername )
					{
						string AddressIP = Computername.Substring(Computername.IndexOf("(")+1);
						AddressIP = AddressIP.Substring(0, AddressIP.Length - 1);
						IPEndPoint EPhost = new IPEndPoint(IPAddress.Parse(AddressIP), 7070);
					
						//Create the Socket for sending data over TCP
						ClientSocket = new clsChatSocket();

						// Connect to host using IPEndPoint
						ClientSocket.Connect( EPhost );
						new frmShare().CreateRequest("CHAT",Computername,txtMessage.Text);
						FileStream fileStream = new FileStream(new frmShare().REQUESTFILE,FileMode.Open);
						Byte[] ReadBuffer = new Byte[1024];
						fileStream.Read(ReadBuffer,0,(int)fileStream.Length);
						ClientStream = ClientSocket.GetStream();
						ClientStream.Write(ReadBuffer,0,(int)fileStream.Length);
						ClientStream.Flush();
						fileStream.Close();
						new ServerCommunication().FileDelete(new frmShare().REQUESTFILE);
						Connected = true;
						RecivingTimer.Enabled=true;
					}
				}

				if(	txtMessage.Text.Length > 0 )
				{
					Typing = false;
					Byte[] buff = Encoding.ASCII.GetBytes(txtMessage.Text);
					ClientStream.Write(buff,0,buff.Length);
					ClientStream.Flush();
					sChatBox.Text += "I said : " + txtMessage.Text + "\r\n";
				}
				txtMessage.Text = "";
			}
			catch( Exception Err ) { MessageBox.Show(Err.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		protected void RecivingTimer_Tick (object sender, System.EventArgs e)
		{
			if( 0 < ClientSocket.GetThisSocket().Available )
			{
				string str="";
				int Bytes = 0;
				Byte[] msgRecv=new Byte[256];
				Bytes=ClientStream.Read(msgRecv,0,255);
				ClientStream.Flush();
				str=Encoding.ASCII.GetString(msgRecv);
				if( str.IndexOf("heistyping",0,11) != -1 )
				{
					ListenerTyping = true;
					statusBar1.Text = Computername + " is typing a message";
					return;
				}

				sChatBox.Text += Computername.Substring(0,Computername.IndexOf("(")) + " says : " + str;
				sChatBox.Text += "\r\n";
				statusBar1.Text = Computername + " is connected";
			}

			if( ListenerTyping )
			{
				ListenerTimeElapse += 1;
				if( ListenerTimeElapse >= 10 )
				{
					ListenerTimeElapse = 0;
					ListenerTyping = false;
					statusBar1.Text = Computername + " is connected";
				}
			}

			if( Typing )
			{
				if( Connected && TimeElapse == 1 )
				{
					Byte[] buff = Encoding.ASCII.GetBytes("heistyping");
					ClientStream.Write(buff,0,buff.Length);
					ClientStream.Flush();
				}
				
				TimeElapse += 1;
				if( TimeElapse >= 10 )
				{
					Typing = false;
					TimeElapse = 0;
				}
			}
		}

		protected void frmChat_Closing (object sender, System.ComponentModel.CancelEventArgs e)
		{
			RecivingTimer.Enabled = false;
			RecivingTimer.Stop();
			ClientSocket.GetThisSocket().Close();
		}
    }
}