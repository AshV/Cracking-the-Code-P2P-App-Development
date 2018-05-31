using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace Listener
{
	public class BrowseFolder : System.Windows.Forms.Design.FolderNameEditor
	{
		public string SelectFolder()
		{
			FolderBrowser folder = new FolderBrowser();
			folder.Description = "Share Folders";
			InitializeDialog(folder);
			folder.Style |= FolderBrowserStyles.ShowTextBox;
			if(DialogResult.OK == folder.ShowDialog())
				return folder.DirectoryPath;
			return "";
		}
	}
}
