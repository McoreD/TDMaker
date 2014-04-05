using HelpersLib;
using MediaInfoLib;
using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TDMakerLib;
using UploadersLib;

namespace TDMaker
{
    public partial class AboutBox : Form
    {
        public const string URL_UPDATE = "http://tdmaker.googlecode.com/svn/trunk/Update.xml";

        public AboutBox()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            StringBuilder sbDesc = new StringBuilder();
            sbDesc.AppendLine(AssemblyDescription);
            sbDesc.AppendLine();
            sbDesc.AppendLine("Running from:");
            sbDesc.AppendLine(Application.StartupPath);
            sbDesc.AppendLine();
            sbDesc.AppendLine("Settings file:");
            sbDesc.AppendLine(Program.AppConf.XMLSettingsFile);
            MediaInfoLib.MediaInfo mi = new MediaInfoLib.MediaInfo();
            sbDesc.AppendLine();
            sbDesc.AppendLine("Using:");
            sbDesc.AppendLine(mi.Option("Info_Version") + " from http://sourceforge.net/projects/mediainfo");
            sbDesc.AppendLine("Movie Thumbnailer v200808a:");
            this.textBoxDescription.Text = sbDesc.ToString();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion Assembly Attribute Accessors

        private void AboutBox_Shown(object sender, EventArgs e)
        {
            CheckUpdate();
        }

        public UpdateChecker CheckUpdate()
        {
            UpdateChecker updateChecker = new GitHubUpdateChecker("ShareX", "ShareX");
            updateChecker.CurrentVersion = ProgramUI.AssemblyVersion;
            updateChecker.Proxy = ProxyInfo.Current.GetWebProxy();
            updateChecker.CheckUpdate();

            // Fallback if GitHub API fails
            if (updateChecker.UpdateInfo == null || updateChecker.UpdateInfo.Status == UpdateStatus.UpdateCheckFailed)
            {
                updateChecker = new XMLUpdateChecker("http://getsharex.com/Update.xml", "ShareX");
                updateChecker.CurrentVersion = ProgramUI.AssemblyVersion;
                updateChecker.Proxy = ProxyInfo.Current.GetWebProxy();
                updateChecker.CheckUpdate();
            }

            return updateChecker;
        }
    }
}