namespace Listener
{
	// Library includes
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
	using System.Windows.Forms.Design;
	using System.Text;
	using System.IO;

    ///    Share Files/Folders form class.
    public class frmSelection : System.Windows.Forms.Form
    {
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lbCopyright;
		private System.Windows.Forms.Button btnBrowseFileFolder;
		private System.Windows.Forms.ToolTip toolTipText;
		private System.Windows.Forms.CheckBox chkEntry;
		private System.Windows.Forms.RadioButton rbWrite;
		private System.Windows.Forms.RadioButton rbRead;
		private System.Windows.Forms.GroupBox grpRights;
		private System.Windows.Forms.OpenFileDialog FileOpenDialog;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFoldername;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.TabPage tpFolder;
		private System.Windows.Forms.TabPage tpShare;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TabControl tabShare;

        public frmSelection() { InitializeComponent(); }

        // Free resources it was using.
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
			this.grpRights = new System.Windows.Forms.GroupBox();
			this.rbWrite = new System.Windows.Forms.RadioButton();
			this.rbRead = new System.Windows.Forms.RadioButton();
			this.chkEntry = new System.Windows.Forms.CheckBox();
			this.toolTipText = new System.Windows.Forms.ToolTip(this.components);
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tpShare = new System.Windows.Forms.TabPage();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.tabShare = new System.Windows.Forms.TabControl();
			this.tpFolder = new System.Windows.Forms.TabPage();
			this.btnBrowseFileFolder = new System.Windows.Forms.Button();
			this.txtFoldername = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.FileOpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.lbCopyright = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpRights.SuspendLayout();
			this.tpShare.SuspendLayout();
			this.tabShare.SuspendLayout();
			this.tpFolder.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpRights
			// 
			this.grpRights.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.rbWrite,
																					this.rbRead});
			this.grpRights.Location = new System.Drawing.Point(16, 128);
			this.grpRights.Name = "grpRights";
			this.grpRights.Size = new System.Drawing.Size(124, 56);
			this.grpRights.TabIndex = 3;
			this.grpRights.TabStop = false;
			this.grpRights.Text = "Rights";
			// 
			// rbWrite
			// 
			this.rbWrite.Location = new System.Drawing.Point(6, 36);
			this.rbWrite.Name = "rbWrite";
			this.rbWrite.Size = new System.Drawing.Size(96, 14);
			this.rbWrite.TabIndex = 3;
			this.rbWrite.Text = "Read/Write";
			this.toolTipText.SetToolTip(this.rbWrite, "Allows the user to upload to this file or folder");
			// 
			// rbRead
			// 
			this.rbRead.Checked = true;
			this.rbRead.Location = new System.Drawing.Point(6, 16);
			this.rbRead.Name = "rbRead";
			this.rbRead.Size = new System.Drawing.Size(95, 14);
			this.rbRead.TabIndex = 2;
			this.rbRead.TabStop = true;
			this.rbRead.Text = "Read only";
			this.toolTipText.SetToolTip(this.rbRead, "Allows the user to read");
			// 
			// chkEntry
			// 
			this.chkEntry.Location = new System.Drawing.Point(296, 128);
			this.chkEntry.Name = "chkEntry";
			this.chkEntry.Size = new System.Drawing.Size(120, 16);
			this.chkEntry.TabIndex = 4;
			this.chkEntry.Text = "Add this entry only";
			this.toolTipText.SetToolTip(this.chkEntry, "Quits after adding the entry, if checked");
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 14);
			this.label1.TabIndex = 2;
			this.label1.Text = "Type a filename here or click browse to select:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(232, 14);
			this.label2.TabIndex = 1;
			this.label2.Text = "Type a folder name here:";
			// 
			// tpShare
			// 
			this.tpShare.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.btnBrowse,
																				  this.label1,
																				  this.txtFilename});
			this.tpShare.Location = new System.Drawing.Point(4, 25);
			this.tpShare.Name = "tpShare";
			this.tpShare.Size = new System.Drawing.Size(388, 89);
			this.tpShare.TabIndex = 0;
			this.tpShare.Text = "Share File";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(296, 56);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(80, 24);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "&Browse";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// txtFilename
			// 
			this.txtFilename.Location = new System.Drawing.Point(13, 26);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(363, 20);
			this.txtFilename.TabIndex = 0;
			this.txtFilename.Text = "";
			// 
			// tabShare
			// 
			this.tabShare.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabShare.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.tpShare,
																				   this.tpFolder});
			this.tabShare.Location = new System.Drawing.Point(16, 8);
			this.tabShare.Name = "tabShare";
			this.tabShare.SelectedIndex = 0;
			this.tabShare.Size = new System.Drawing.Size(396, 118);
			this.tabShare.TabIndex = 2;
			// 
			// tpFolder
			// 
			this.tpFolder.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.btnBrowseFileFolder,
																				   this.label2,
																				   this.txtFoldername});
			this.tpFolder.Location = new System.Drawing.Point(4, 25);
			this.tpFolder.Name = "tpFolder";
			this.tpFolder.Size = new System.Drawing.Size(388, 89);
			this.tpFolder.TabIndex = 1;
			this.tpFolder.Text = "Share Folder";
			// 
			// btnBrowseFileFolder
			// 
			this.btnBrowseFileFolder.Location = new System.Drawing.Point(296, 56);
			this.btnBrowseFileFolder.Name = "btnBrowseFileFolder";
			this.btnBrowseFileFolder.Size = new System.Drawing.Size(80, 24);
			this.btnBrowseFileFolder.TabIndex = 2;
			this.btnBrowseFileFolder.Text = "Browse";
			this.btnBrowseFileFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
			// 
			// txtFoldername
			// 
			this.txtFoldername.Location = new System.Drawing.Point(13, 26);
			this.txtFoldername.Name = "txtFoldername";
			this.txtFoldername.Size = new System.Drawing.Size(363, 20);
			this.txtFoldername.TabIndex = 0;
			this.txtFoldername.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
			this.btnOK.ForeColor = System.Drawing.Color.LemonChiffon;
			this.btnOK.Location = new System.Drawing.Point(240, 168);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(85, 25);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "&Share it";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lbCopyright
			// 
			this.lbCopyright.Location = new System.Drawing.Point(208, 8);
			this.lbCopyright.Name = "lbCopyright";
			this.lbCopyright.Size = new System.Drawing.Size(200, 12);
			this.lbCopyright.TabIndex = 5;
			this.lbCopyright.Text = "© 2001 www.dreamtechsoftware.com";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(336, 168);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(85, 24);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Close";
			// 
			// frmSelection
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(432, 203);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lbCopyright,
																		  this.chkEntry,
																		  this.grpRights,
																		  this.btnCancel,
																		  this.btnOK,
																		  this.tabShare});
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.HelpButton = true;
			this.Icon = null;
			this.Name = "frmSelection";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Share Dialog";
			this.grpRights.ResumeLayout(false);
			this.tpShare.ResumeLayout(false);
			this.tabShare.ResumeLayout(false);
			this.tpFolder.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		//	Browse folder Button clicked, open folder open dialog
		protected void btnBrowseFolder_Click (object sender, System.EventArgs e)
		{
			BrowseFolder Folder = new BrowseFolder();
			txtFoldername.Text = Folder.SelectFolder();
 		}

		//	Checkes whether file or folder user's trying to share 
		//	is already shared 
		private bool DoesEntryExists(string Filename, string Data)
		{
			bool bFound = false;
			if( ! File.Exists(Filename) )
				return bFound;
			else
			{
				StreamReader stReader = new StreamReader(Filename);
				string readData;
				while((readData = stReader.ReadLine()) != null)
					// what u read's equal or shorter in length than what ur intend to share
					// that means if i've shared "C:\" it'll allow u to share "C:\Shared"
					// finding substring alone wouldn't have worked here.
					if(readData.Length <= Data.Length)
						if( -1 != readData.Substring(0,readData.IndexOf("=")).Trim().IndexOf(Data.Substring(0,Data.IndexOf("="))) )
						{
							bFound = true;
							break;
						}
				stReader.Close();
			}
			return bFound;
		}

		// checks the file of folder passed to it is not shared already,
		// checks it exists and writes that to the share.ini file.
		private void WriteDataToFile(string Filename, string Data)
		{
			try
			{
				// if it's a folder
				if( 0 == Data.Substring(Data.Length-3,1).CompareTo("\\"))
				{
					// check if this entry already exists
					if( !DoesEntryExists(Filename, Data) )
					{
						// if not append this entry to share.ini with its mask
						StreamWriter stWriter = File.AppendText(Filename);
						stWriter.WriteLine(Data);
						MessageBox.Show("'" + Data.Substring(0,Data.IndexOf("=")) + "' has been successfully shared.","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
						stWriter.Close();
					}
					else throw new Exception("Entry already exists"); 
				}

				else if( File.Exists(Data.Substring(0,Data.IndexOf("=",0))) )
				{
					// else if it's a file and it exists 
					if( !DoesEntryExists(Filename, Data) )
					{
						// and if it doesn't exist already append this entry 
						// to share.ini with its size and mask
						StreamWriter stWriter = File.AppendText(Filename);
						stWriter.WriteLine(Data);
						MessageBox.Show("'" + Data.Substring(0,Data.IndexOf("=")) + "' is successfully shared.","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
						stWriter.Close();
					}
					else throw new Exception("Entry already exists"); 
				}
				else throw new Exception("File/Folder does not exists");
			}
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }
		}

		// Share file or folder selected
		protected void btnOK_Click (object sender, System.EventArgs e)
		{
			string ResourceName = null;
			long fileSize = 0;
			bool bIsFile = false;
			try
			{
				System.Windows.Forms.TabPage tp = tabShare.SelectedTab;
				// find the file or folder seleceted 
				if( 0 == tp.Text.Trim().CompareTo("Share File") )
				{
					ResourceName = txtFilename.Text;
					Stream s = File.Open(txtFilename.Text,FileMode.Open); 
					fileSize = s.Length;
					bIsFile = true;
				}
				else if( 0 == tp.Text.Trim().CompareTo("Share Folder") )
				{
					ResourceName = txtFoldername.Text;
					if( !Directory.Exists(ResourceName) )
					{
						DialogResult = System.Windows.Forms.DialogResult.None;
						throw new Exception("Directory does not exist");						
					}

					if(!ResourceName.Trim().EndsWith("\\"))
						ResourceName += "\\";
				}

				// add append mask to it
				if( 0 < ResourceName.Trim().Length )
				{
					if( rbRead.Checked )
						ResourceName += "=0";
					else if( rbWrite.Checked )
						ResourceName += "=1";
					else 
					{
						DialogResult = System.Windows.Forms.DialogResult.None;
						throw new Exception("Rights are missing");
					}

					if(bIsFile)
						ResourceName += "=" + fileSize.ToString();

					// and write this entry to share.ini
					WriteDataToFile(Application.StartupPath + "\\Share.ini",ResourceName);
					if( !chkEntry.Checked )
						DialogResult = System.Windows.Forms.DialogResult.None;
				}
				else
				{
					DialogResult = System.Windows.Forms.DialogResult.None;
					throw new Exception("Cannot add a blank entry");
				}	
			}	
			catch( Exception err ) { MessageBox.Show(err.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning); }
		}

		//	Browse file Button clicked, open file open dialog
		protected void btnBrowseFile_Click (object sender, System.EventArgs e)
		{
			FileOpenDialog.Title = "Select a file to share";
			if( System.Windows.Forms.DialogResult.OK == FileOpenDialog.ShowDialog() )
				txtFilename.Text = FileOpenDialog.FileName;  
		}
    }
}