using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Listener
{
	/// <summary>
	/// Summary description for StreamIt.
	/// </summary>
	public class StreamIt : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnPrepareToEncode;
		private System.Windows.Forms.Button btnMkSession;
		private System.Windows.Forms.Button btnBrowseAudio;
		private System.Windows.Forms.Button btnBrowseVideo;
		private System.Windows.Forms.Button btnCloseSession;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RichTextBox rtxStatus;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Label label3;
		private AxBROADCASTDLLLib.AxBroadcastIt axBroadcastIt1;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button btnBroadcast;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string strMediaSource;
		private int connected,disconnected;
		public	int ConnectedClients;
		private bool ClientConnectedOnce;

		StreamingCallBackDeligates callbackFunc;
		public StreamIt(string MediaFile,StreamingCallBackDeligates callback)
		{			
			ClientConnectedOnce	= false;
			strMediaSource	= MediaFile;
			callbackFunc	= callback;
			connected		= 0;
			disconnected	= 0;
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			ConnectedClients = 0;
			this.btnApply_Click(null,null);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(StreamIt));
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.btnMkSession = new System.Windows.Forms.Button();
			this.btnBroadcast = new System.Windows.Forms.Button();
			this.rtxStatus = new System.Windows.Forms.RichTextBox();
			this.btnBrowseVideo = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.axBroadcastIt1 = new AxBROADCASTDLLLib.AxBroadcastIt();
			this.btnBrowseAudio = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.btnCloseSession = new System.Windows.Forms.Button();
			this.btnPrepareToEncode = new System.Windows.Forms.Button();
			this.txtPort = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.axBroadcastIt1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnMkSession
			// 
			this.btnMkSession.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnMkSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnMkSession.Location = new System.Drawing.Point(138, 291);
			this.btnMkSession.Name = "btnMkSession";
			this.btnMkSession.Size = new System.Drawing.Size(119, 33);
			this.btnMkSession.TabIndex = 1;
			this.btnMkSession.Text = "Make Session";
			this.btnMkSession.Click += new System.EventHandler(this.btnMkSession_Click);
			// 
			// btnBroadcast
			// 
			this.btnBroadcast.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnBroadcast.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.btnBroadcast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnBroadcast.ForeColor = System.Drawing.Color.White;
			this.btnBroadcast.Location = new System.Drawing.Point(162, 204);
			this.btnBroadcast.Name = "btnBroadcast";
			this.btnBroadcast.Size = new System.Drawing.Size(360, 27);
			this.btnBroadcast.TabIndex = 12;
			this.btnBroadcast.Text = "Sing To The Tunes Of Ankur\'s Broadcaster";
			this.btnBroadcast.Click += new System.EventHandler(this.btnBroadcast_Click);
			// 
			// rtxStatus
			// 
			this.rtxStatus.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.rtxStatus.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.rtxStatus.Cursor = System.Windows.Forms.Cursors.PanNW;
			this.rtxStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rtxStatus.ForeColor = System.Drawing.Color.White;
			this.rtxStatus.Location = new System.Drawing.Point(8, 8);
			this.rtxStatus.Name = "rtxStatus";
			this.rtxStatus.ReadOnly = true;
			this.rtxStatus.Size = new System.Drawing.Size(514, 184);
			this.rtxStatus.TabIndex = 7;
			this.rtxStatus.Text = "";
			// 
			// btnBrowseVideo
			// 
			this.btnBrowseVideo.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnBrowseVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnBrowseVideo.Location = new System.Drawing.Point(6, 261);
			this.btnBrowseVideo.Name = "btnBrowseVideo";
			this.btnBrowseVideo.Size = new System.Drawing.Size(99, 24);
			this.btnBrowseVideo.TabIndex = 3;
			this.btnBrowseVideo.Text = "Video Source";
			this.btnBrowseVideo.Click += new System.EventHandler(this.btnBrowseVideo_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label1.Location = new System.Drawing.Point(111, 237);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(411, 13);
			this.label1.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label2.Location = new System.Drawing.Point(111, 267);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(411, 13);
			this.label2.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label3.Location = new System.Drawing.Point(6, 207);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(30, 18);
			this.label3.TabIndex = 10;
			this.label3.Text = "Port:";
			// 
			// axBroadcastIt1
			// 
			this.axBroadcastIt1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.axBroadcastIt1.Location = new System.Drawing.Point(249, 210);
			this.axBroadcastIt1.Name = "axBroadcastIt1";
			this.axBroadcastIt1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBroadcastIt1.OcxState")));
			this.axBroadcastIt1.Size = new System.Drawing.Size(234, 6);
			this.axBroadcastIt1.TabIndex = 11;
			this.axBroadcastIt1.Visible = false;
			this.axBroadcastIt1.BroadcastStatus += new AxBROADCASTDLLLib._IBroadcastItEvents_BroadcastStatusEventHandler(this.axBroadcastIt1_BroadcastStatus);
			// 
			// btnBrowseAudio
			// 
			this.btnBrowseAudio.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnBrowseAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnBrowseAudio.Location = new System.Drawing.Point(6, 231);
			this.btnBrowseAudio.Name = "btnBrowseAudio";
			this.btnBrowseAudio.Size = new System.Drawing.Size(99, 24);
			this.btnBrowseAudio.TabIndex = 2;
			this.btnBrowseAudio.Text = "Audio Source";
			this.btnBrowseAudio.Click += new System.EventHandler(this.btnBrowseAudio_Click);
			// 
			// btnApply
			// 
			this.btnApply.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnApply.Location = new System.Drawing.Point(6, 291);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(119, 33);
			this.btnApply.TabIndex = 8;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnCloseSession
			// 
			this.btnCloseSession.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnCloseSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCloseSession.Location = new System.Drawing.Point(402, 291);
			this.btnCloseSession.Name = "btnCloseSession";
			this.btnCloseSession.Size = new System.Drawing.Size(119, 33);
			this.btnCloseSession.TabIndex = 4;
			this.btnCloseSession.Text = "Close Session";
			this.btnCloseSession.Click += new System.EventHandler(this.btnCloseSession_Click);
			// 
			// btnPrepareToEncode
			// 
			this.btnPrepareToEncode.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnPrepareToEncode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPrepareToEncode.Location = new System.Drawing.Point(270, 291);
			this.btnPrepareToEncode.Name = "btnPrepareToEncode";
			this.btnPrepareToEncode.Size = new System.Drawing.Size(123, 33);
			this.btnPrepareToEncode.TabIndex = 0;
			this.btnPrepareToEncode.Text = "Prepare To Encode";
			this.btnPrepareToEncode.Click += new System.EventHandler(this.btnPrepareToEncode_Click);
			// 
			// txtPort
			// 
			this.txtPort.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.txtPort.Location = new System.Drawing.Point(39, 204);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(111, 20);
			this.txtPort.TabIndex = 9;
			this.txtPort.Text = "";
			// 
			// StreamIt
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(529, 330);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnBroadcast,
																		  this.axBroadcastIt1,
																		  this.label3,
																		  this.txtPort,
																		  this.btnApply,
																		  this.rtxStatus,
																		  this.label2,
																		  this.label1,
																		  this.btnCloseSession,
																		  this.btnBrowseVideo,
																		  this.btnBrowseAudio,
																		  this.btnMkSession,
																		  this.btnPrepareToEncode});
			this.MinimumSize = new System.Drawing.Size(537, 357);
			this.Name = "StreamIt";
			this.Text = "StreamIt";
			this.Activated += new System.EventHandler(this.StreamIt_Activated);
			((System.ComponentModel.ISupportInitialize)(this.axBroadcastIt1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		protected int axBroadcastIt1_BroadcastStatus(object sender, AxBROADCASTDLLLib._IBroadcastItEvents_BroadcastStatusEvent e)
		{
			//rtxStatus.Text += e.status + "\r\n";
			Application.DoEvents();
			if(e.status.IndexOf("Client") != -1)
			{
				if(e.status.IndexOf("Connected") != -1) 
				{
					connected++;
					if(connected == 2)
					{
						ConnectedClients++;
						callbackFunc("Connected",this);
						ClientConnectedOnce = true;
						connected = 0;
						disconnected = 0;
					}
				}
				if(e.status.IndexOf("Disconnected") != -1)
				{
					disconnected++;
					if(disconnected == 2)
					{
						if(ConnectedClients >=0)
							ConnectedClients--;
						connected = 0;
						disconnected = 0;
					}
				}

				/*if((ConnectedClients == 0) && ClientConnectedOnce)
				{
					ClientConnectedOnce = false;
					callbackFunc("Disconnected",this);
					/*this.btnCloseSession_Click(null,null);
					this.Dispose();
					this.Close();
				}*/
			}

			if((e.status.ToString().CompareTo("Second attaemp of calling PrepareToEncode failed") == 0)
				|| (e.status.ToString().CompareTo("Couldn't Start Encoder") == 0))
			{
				callbackFunc("Stopped",this);
				this.btnCloseSession_Click(null,null);
				this.Dispose();
				this.Close();
			}
			if((e.status.ToString().CompareTo("Encoder Stopped") == 0))
			{
				MessageBox.Show("asdfasdfasdfasdfasfda");
				callbackFunc("Encoder Stopped",this);
				this.btnCloseSession_Click(null,null);
				this.Dispose();
				this.Close();
			}

			if((e.status.ToString().CompareTo("Copy Complete") == 0) || (e.status.ToString().CompareTo("Started") == 0))
				callbackFunc("Copy Complete",this);
			return 1;
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			axBroadcastIt1.InitializeBroadcaster();
			axBroadcastIt1.Port = 9090;
			axBroadcastIt1.AudioMedia = strMediaSource;
			axBroadcastIt1.VideoMedia = strMediaSource;
			axBroadcastIt1.MakeSession();
			axBroadcastIt1.PrepareToEncode();
			axBroadcastIt1.Broadcast();
		}

		private void btnBrowseAudio_Click(object sender, System.EventArgs e)
		{
			if(DialogResult.OK == openFileDialog.ShowDialog())
				label1.Text = openFileDialog.FileName;
		}

		private void btnBrowseVideo_Click(object sender, System.EventArgs e)
		{
			if(DialogResult.OK == openFileDialog.ShowDialog())
				label2.Text = openFileDialog.FileName;
		}

		private void btnPrepareToEncode_Click(object sender, System.EventArgs e)
		{
			axBroadcastIt1.PrepareToEncode();
		}

		private void btnMkSession_Click(object sender, System.EventArgs e)
		{
			axBroadcastIt1.MakeSession();
		}

		private void btnCloseSession_Click(object sender, System.EventArgs e)
		{
			axBroadcastIt1.CloseSession();
		}

		private void btnBroadcast_Click(object sender, System.EventArgs e)
		{
			axBroadcastIt1.Broadcast();
		}
		private void StreamIt_Activated(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	}
}
