#region License Information (GPL v2)

/*
    TDMaker - A program that allows you to upload screenshots in one keystroke.
    Copyright (C) 2008-2009  Brandon Zimmerman

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v2)

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UploadersLib;

namespace TDMakerLib
{
    public partial class ConfigWizard : Form
    {
        public bool PreferSystemFolders { get; private set; }

        public string RootFolder { get; private set; }

        public ImageDestination ImageDestinationType { get; private set; }

        public ConfigWizard(string rootDir)
        {
            InitializeComponent();
            this.Text = string.Format("TDMaker {0} - Configuration Wizard", Application.ProductVersion);
            chkPreferSystemFolders.Checked = Program.AppConf.PreferSystemFolders;
            txtRootFolder.Text = rootDir;
            this.RootFolder = rootDir;
            foreach (ImageDestination sdt in Enum.GetValues(typeof(ImageDestination)))
            {
                cboScreenshotDest.Items.Add(sdt.ToDescriptionString());
            }
            cboScreenshotDest.SelectedIndex = (int)ImageDestination.ImageShack;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Program.AppConf.RootDir = this.RootFolder;
            Program.AppConf.PreferSystemFolders = this.PreferSystemFolders;
            Program.AppConf.ImageUploaderType = this.ImageDestinationType;
            Program.AppConf.PtpImgCode = this.txtPtpImgCode.Text;

            Program.InitializeDefaultFolderPaths();
            Debug.WriteLine(Program.AppConf.XMLSettingsFile);
            this.Close();
        }

        private void btnBrowseRootDir_Click(object sender, EventArgs e)
        {
            string oldDir = txtRootFolder.Text;
            string newDir = string.Empty;

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Configure Root diretory...";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                newDir = dlg.SelectedPath;
            }

            if (!string.IsNullOrEmpty(newDir))
            {
                txtRootFolder.Text = newDir;
                RootFolder = txtRootFolder.Text;
                FileSystem.MoveDirectory(oldDir, txtRootFolder.Text);
            }
        }

        private void btnViewRootDir_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtRootFolder.Text))
            {
                Process.Start(txtRootFolder.Text);
            }
        }

        private void cboScreenshotDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageDestinationType = (ImageDestination)cboScreenshotDest.SelectedIndex;
        }

        private void chkPreferSystemFolders_CheckedChanged(object sender, EventArgs e)
        {
            gbRoot.Enabled = !chkPreferSystemFolders.Checked;
            this.PreferSystemFolders = chkPreferSystemFolders.Checked;
        }

        private void ConfigWizard_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("If enabled {0} will create the data folders at the following locations:", Application.ProductName));
            sb.AppendLine();
            sb.AppendLine(string.Format("Settings:\t{0}", Program.zSettingsDir));
            sb.AppendLine(string.Format("Screenshots:\t{0}", Program.zPicturesDir));
            sb.AppendLine(string.Format("Torrents:\t{0}", Program.zTorrentsDir));
            sb.AppendLine(string.Format("Logs:\t\t{0}", Program.zLogsDir));
            ttApp.SetToolTip(chkPreferSystemFolders, sb.ToString());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}