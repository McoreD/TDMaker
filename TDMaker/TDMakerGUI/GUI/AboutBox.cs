using ShareX.HelpersLib;
using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TDMakerLib;

namespace TDMaker
{
    public partial class AboutBox : Form
    {
        public const string URL_UPDATE = "https://raw.githubusercontent.com/McoreD/TDMaker/master/Update.xml";

        public AboutBox()
        {
            InitializeComponent();
            Text = String.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            StringBuilder sbDesc = new StringBuilder();
            sbDesc.AppendLine(AssemblyDescription);
            sbDesc.AppendLine();
            sbDesc.AppendLine("Running from:");
            sbDesc.AppendLine(Application.StartupPath);
            sbDesc.AppendLine();
            sbDesc.AppendLine("Settings file:");
            sbDesc.AppendLine(App.SettingsFilePath);
            sbDesc.AppendLine();
            sbDesc.AppendLine("External libraries:");
            sbDesc.AppendLine("ShareX: http://getsharex.com");
            sbDesc.AppendLine("MediaInfo: http://sourceforge.net/projects/mediainfo");
            textBoxDescription.Text = sbDesc.ToString();

            UpdateChecker updateChecker = ProgramUI.UpdateManager.CreateUpdateChecker();
            uclUpdate.CheckUpdate(updateChecker);
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

        public static UpdateChecker CheckUpdate()
        {
            UpdateChecker updateChecker = new GitHubUpdateChecker("McoreD", "TDMaker");
            updateChecker.IsBeta = false;
            updateChecker.Proxy = HelpersOptions.CurrentProxy.GetWebProxy();
            updateChecker.CheckUpdate();

            // Fallback if GitHub API fails
            if (updateChecker.Status == UpdateStatus.None || updateChecker.Status == UpdateStatus.UpdateCheckFailed)
            {
                updateChecker = new XMLUpdateChecker(URL_UPDATE, "TDMaker");
                updateChecker.Proxy = HelpersOptions.CurrentProxy.GetWebProxy();
                updateChecker.CheckUpdate();
            }

            return updateChecker;
        }
    }
}