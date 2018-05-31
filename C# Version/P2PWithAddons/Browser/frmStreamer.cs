namespace Client
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// Summary description for frmStreamer.
	/// </summary>
	public class frmStreamer : System.Windows.Forms.Form
	{
		private AxMediaPlayer.AxMediaPlayer axMediaPlayer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmStreamer(string IPAddress)
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			try { axMediaPlayer.FileName = "http://" + IPAddress + ":9090";	}
			catch( Exception err ) { MessageBox.Show(err.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error); }
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmStreamer));
			this.axMediaPlayer = new AxMediaPlayer.AxMediaPlayer();
			((System.ComponentModel.ISupportInitialize)(this.axMediaPlayer)).BeginInit();
			this.SuspendLayout();
			// 
			// axMediaPlayer
			// 
			this.axMediaPlayer.Location = new System.Drawing.Point(1, 1);
			this.axMediaPlayer.Name = "axMediaPlayer";
			this.axMediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMediaPlayer.OcxState")));
			this.axMediaPlayer.Size = new System.Drawing.Size(436, 387);
			this.axMediaPlayer.TabIndex = 1;
			this.axMediaPlayer.Error += new System.EventHandler(this.axMediaPlayer_Error);
			// 
			// frmStreamer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(438, 389);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.axMediaPlayer});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmStreamer";
			this.Text = "Streaming Window";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmStreamer_Closing);
			((System.ComponentModel.ISupportInitialize)(this.axMediaPlayer)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void axMediaPlayer_Error(object sender, System.EventArgs e)
		{
			MessageBox.Show("Some error has been occured while streaming. Cannot continue further","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			axMediaPlayer.Dispose();
			this.Dispose();
			this.Close();
		}

		private void frmStreamer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			axMediaPlayer.Dispose();
			this.Dispose();
		}
	}
}