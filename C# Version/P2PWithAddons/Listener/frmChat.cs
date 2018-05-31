namespace Listener
{
	// Library includes
    using System;
	using System.Text;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
	using System.IO;
	using WorkingWithXML;
	using System.Net.Sockets;
	using System.Threading;

	/// <summary>
    ///    Summary description for frmChat.
    /// </summary>
    /// 
    public class frmChat : System.Windows.Forms.Form
    {
        /// <summary>
        ///    Required designer variable.
        /// </summary>
		public string	LoginName,
						ChatUserName,
						ChatFilePath,
						ReqPath,
						ResPath;
		public int		WindowNo;
		public Socket	Sock;
		public bool		ClientTyping;
		public int		ClientTypingTimeElapsed;
		public bool		Typing;
		public int		TimeElapsed;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lbCopyright;
		private System.Windows.Forms.StatusBarPanel ClientsStatus;
		private System.Windows.Forms.StatusBar lChatSttsBar;
		private System.Windows.Forms.RichTextBox rtxtSend;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RichTextBox rtxtMessages;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Timer RecivingTimer;

        public frmChat() 
		{
			InitializeComponent();
		}

        public frmChat(string[] Info, Socket sock,string LName, string ChatfilePath, int WNo)
        {
			// Required for Windows Form Designer support
			InitializeComponent();

			ClientTyping = false;
			ClientTypingTimeElapsed = 0;
			ChatUserName	= Info[0];
			WindowNo		= WNo;
			ChatFilePath	= ChatfilePath;
			LoginName		= LName;
			lChatSttsBar.Text = ChatUserName + "is connected";

			string Msg = ChatUserName + ":  Says: " + Info[2] + "\r\n";
			Sock = sock;
			if( !Sock.Connected )
			{
				MessageBox.Show("Client Disconnected","Chat",MessageBoxButtons.OK,MessageBoxIcon.Information);
				return;
			}
			RecivingTimer.Enabled = true;
			Typing = false;
			TimeElapsed = 0;
		}

		~frmChat()
		{
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
        ///    the contents of this method with the code editor, 
        ///    unless you are sure of what you are doing
        /// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmChat));
			this.rtxtSend = new System.Windows.Forms.RichTextBox();
			this.rtxtMessages = new System.Windows.Forms.RichTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lChatSttsBar = new System.Windows.Forms.StatusBar();
			this.btnSend = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.ClientsStatus = new System.Windows.Forms.StatusBarPanel();
			this.RecivingTimer = new System.Windows.Forms.Timer(this.components);
			this.lbCopyright = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ClientsStatus)).BeginInit();
			this.SuspendLayout();
			// 
			// rtxtSend
			// 
			this.rtxtSend.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.rtxtSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rtxtSend.ForeColor = System.Drawing.Color.Olive;
			this.rtxtSend.Location = new System.Drawing.Point(6, 0);
			this.rtxtSend.Name = "rtxtSend";
			this.rtxtSend.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rtxtSend.Size = new System.Drawing.Size(220, 34);
			this.rtxtSend.TabIndex = 1;
			this.rtxtSend.Text = "";
			this.rtxtSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtxtSend_KeyPress);
			// 
			// rtxtMessages
			// 
			this.rtxtMessages.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.rtxtMessages.AutoSize = true;
			this.rtxtMessages.AutoWordSelection = true;
			this.rtxtMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rtxtMessages.ForeColor = System.Drawing.Color.DimGray;
			this.rtxtMessages.Location = new System.Drawing.Point(6, 22);
			this.rtxtMessages.Name = "rtxtMessages";
			this.rtxtMessages.ReadOnly = true;
			this.rtxtMessages.Size = new System.Drawing.Size(290, 178);
			this.rtxtMessages.TabIndex = 1;
			this.rtxtMessages.Text = "";
			this.rtxtMessages.WordWrap = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.lChatSttsBar,
																				 this.rtxtSend,
																				 this.btnSend});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 208);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(304, 53);
			this.panel1.TabIndex = 3;
			// 
			// lChatSttsBar
			// 
			this.lChatSttsBar.Location = new System.Drawing.Point(0, 35);
			this.lChatSttsBar.Name = "lChatSttsBar";
			this.lChatSttsBar.Size = new System.Drawing.Size(304, 18);
			this.lChatSttsBar.TabIndex = 2;
			// 
			// btnSend
			// 
			this.btnSend.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnSend.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnSend.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.btnSend.Location = new System.Drawing.Point(232, 0);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(64, 36);
			this.btnSend.TabIndex = 0;
			this.btnSend.Text = "&Send";
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click_1);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Control;
			this.panel2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.rtxtMessages});
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(304, 261);
			this.panel2.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.Silver;
			this.panel3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.panel1,
																				 this.panel2});
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(304, 261);
			this.panel3.TabIndex = 5;
			// 
			// ClientsStatus
			// 
			this.ClientsStatus.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.ClientsStatus.ToolTipText = "Listener";
			// 
			// RecivingTimer
			// 
			this.RecivingTimer.Enabled = true;
			this.RecivingTimer.Interval = 200;
			this.RecivingTimer.Tick += new System.EventHandler(this.RecivingTimer_Tick);
			// 
			// lbCopyright
			// 
			this.lbCopyright.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.lbCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbCopyright.Location = new System.Drawing.Point(93, 5);
			this.lbCopyright.Name = "lbCopyright";
			this.lbCopyright.Size = new System.Drawing.Size(200, 12);
			this.lbCopyright.TabIndex = 6;
			this.lbCopyright.Text = "© 2001 www.dreamtechsoftware.com";
			// 
			// frmChat
			// 
			this.AcceptButton = this.btnSend;
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(304, 261);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lbCopyright,
																		  this.panel3});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(312, 288);
			this.Name = "frmChat";
			this.Text = "Chat";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmChat_Closing);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ClientsStatus)).EndInit();
			this.ResumeLayout(false);

		}

		protected void rtxtSend_KeyPress (object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			Typing = true;
			TimeElapsed = 0;
		}

		protected void btnSend_Click_1 (object sender, System.EventArgs e)
		{
			// Write code to send data on the server bestowed to this classes object
			if(rtxtSend.Text.Length>0)
			{
				Byte[] by = Encoding.ASCII.GetBytes(rtxtSend.Text);
				int bytes = Sock.Send(by,by.Length,0);
				rtxtMessages.Text += LoginName + " says :  " + rtxtSend.Text + "\r\n";
			}
			rtxtSend.Text = "";
		}

		protected void RecivingTimer_Tick (object sender, System.EventArgs e)
		{
			if(!Sock.Connected)
			{
				MessageBox.Show("User Disconnected","Chat Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
				RecivingTimer.Stop();
				RecivingTimer.Enabled = false;
				return;
			}

			if(Typing)
			{
				if(Sock.Connected && TimeElapsed == 1)
				{
					Byte[] buff = Encoding.ASCII.GetBytes("heistyping");
					Sock.Send(buff,buff.Length,0);
				}

				TimeElapsed += 1;
				if(TimeElapsed >= 10)
				{
					Typing = false;
					TimeElapsed = 0;
				}
			}

			if(ClientTyping)
			{
				ClientTypingTimeElapsed += 1;
				if(ClientTypingTimeElapsed >= 10)
				{
					ClientTypingTimeElapsed = 0;
					lChatSttsBar.Text = ChatUserName + " is connected";
					ClientTyping = false;
				}
			}

			if(Sock.Available>0)
			{
				Byte[] RecvBytes = new Byte[Sock.Available];
   				Sock.Receive(RecvBytes, Sock.Available, 0);

				if(Encoding.ASCII.GetString(RecvBytes,0,RecvBytes.Length).CompareTo("heistyping") == 0)
				{
					ClientTyping = true;
					lChatSttsBar.Text = ChatUserName + " is typing a message.";
					return;
				}
				
				rtxtMessages.ForeColor = Color.DarkGray;
				Font f = new Font("Palatino Linotype",8,FontStyle.Bold);
				rtxtMessages.Font = f;
				rtxtMessages.ForeColor = Color.Aquamarine;
				rtxtMessages.Text += ChatUserName + " says :  ";

				Font f2 = new Font("Palatino Linotype",8,FontStyle.Bold);
				rtxtMessages.Font = f2;
				rtxtMessages.ForeColor = Color.Black;
				rtxtMessages.Text += Encoding.ASCII.GetString(RecvBytes, 0, RecvBytes.Length) + "\r\n";
				lChatSttsBar.Text = ChatUserName + " is connected";
			}
		}

		protected void frmChat_Closing (object sender, System.ComponentModel.CancelEventArgs e)
		{
			RecivingTimer.Enabled = false;
		}
    }
}
