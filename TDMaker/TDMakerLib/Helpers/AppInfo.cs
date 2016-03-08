using System;
using System.Diagnostics;
using System.Drawing;

namespace TDMakerLib
{
    /// <summary>
    /// Summary description for cSingleInstance.
    /// </summary>
    public class AppInfo
    {
        public string AppName { get; set; }
        private string mLocalVersion;
        private bool mShowFinalState;

        public Icon AppIcon { get; set; }
        public Image AppImage { get; set; }

        private string mAppSuffix = "setup.zip";
        public string AppSuffix { get { return mAppSuffix; } set { mAppSuffix = value; } }

        private SoftwareCycle mSoftwareState = SoftwareCycle.Final;

        public SoftwareCycle ApplicationState
        {
            get { return mSoftwareState; }
        }

        #region "Enumerations"

        public enum VersionDepth
        {
            Major = 1,
            MajorMinor = 2,
            MajorMinorBuild = 3,
            MajorMinorBuildRevision = 4
        }

        public enum OutdatedMsgStyle
        {
            NewVersionOfAppAvailable,
            AppVerNumberAvailable,
            BetaVersionAvailble
        }

        public enum SoftwareCycle
        {
            Alpha,
            Beta,
            Final
        }

        #endregion "Enumerations"

        #region "Constructors"

        public AppInfo()
        {
        }

        public AppInfo(string ProductName, string Version)
        {
            this.AppName = ProductName;
            this.mLocalVersion = Version;
        }

        public AppInfo(string ProductName, string Version, SoftwareCycle state)
            : this(ProductName, Version)
        {
            this.mSoftwareState = state;
        }

        public AppInfo(string ProductName, string Version, SoftwareCycle state, bool showFinal)
            : this(ProductName, Version, state)
        {
            this.mShowFinalState = showFinal;
        }

        #endregion "Constructors"

        public int GetNumberOfInstances()
        {
            string aModuleName = Process.GetCurrentProcess().MainModule.ModuleName;
            string aProcName = System.IO.Path.GetFileNameWithoutExtension(aModuleName);
            return Process.GetProcessesByName(aProcName).Length;
        }

        public string GetApplicationTitle(VersionDepth depth)
        {
            return GetApplicationTitle(depth, mShowFinalState);
        }

        public string GetApplicationTitle(VersionDepth depth, bool showFinal)
        {
            return GetApplicationTitle(depth, mSoftwareState, showFinal);
        }

        public string GetApplicationTitle(VersionDepth depth, SoftwareCycle cycle, bool showFinal)
        {
            if (!string.IsNullOrEmpty(AppName) && !string.IsNullOrEmpty(mLocalVersion))
            {
                if (cycle == SoftwareCycle.Final && showFinal || cycle != SoftwareCycle.Final)
                {
                    return GetApplicationTitle(AppName, mLocalVersion, depth, cycle);
                }
                return GetApplicationTitle(AppName, mLocalVersion, depth);
            }
            throw new Exception("Product Name or Product Version is Empty.");
        }

        public string GetApplicationTitle()
        {
            return this.GetApplicationTitle(this.AppName, this.mLocalVersion);
        }

        public string GetApplicationTitleFull()
        {
            return this.AppName + " " + mLocalVersion + " " + mSoftwareState;
        }

        public String GetApplicationTitle(string ProductName, string ProductVersion)
        {
            // Default Format: ApplicationTitle 1.0

            String[] version = ProductVersion.Split('.');

            string betaTag = "";
            switch (mSoftwareState)
            {
                case SoftwareCycle.Alpha:
                    betaTag = " ALPHA";
                    break;
                case SoftwareCycle.Beta:
                    betaTag = " BETA";
                    break;
            }

            String title = ProductName + " " + version[0] + "." + version[1] + betaTag;

            return title;
        }

        public String GetApplicationTitle(string productName, string ProductVersion, VersionDepth Depth, SoftwareCycle state)
        {
            return GetApplicationTitle(productName, ProductVersion, Depth) + " " + state;
        }

        public String GetApplicationTitle(string ProductName, string ProductVersion, VersionDepth Depth)
        {
            return ProductName + " " + GetVersion(ProductVersion, Depth);
        }

        public string GetVersion(string ProductVersion, VersionDepth Depth)
        {
            string title = "";
            int i;
            int d = Convert.ToInt32(Depth);
            String[] version = ProductVersion.Split('.');

            if (d > version.Length)
            {
                d = version.Length;
            }

            for (i = 0; i < d - 1; i++)
            {
                title += version[i] + ".";
            }
            title += version[i];

            return title;
        }

        public string GetLocalVersion()
        {
            return this.mLocalVersion;
        }

        private string CompareVersion(string version1, string version2)
        {
            string[] ver1 = version1.Split('.');
            string[] ver2 = version2.Split('.');

            int[] vers1 = new int[ver1.Length];
            int[] vers2 = new int[ver2.Length];

            for (int i = 0; i < ver1.Length; i++)
            {
                vers1[i] = Convert.ToInt32(ver1[i]);
                vers2[i] = Convert.ToInt32(ver2[i]);
            }

            int r = CompareVersion(vers1, vers2, 0);
            switch (r)
            {
                case 1:
                    return version1;
                case 2:
                    return version2;
                default:
                    return version1;
            }
        }

        private int CompareVersion(int[] ver1, int[] ver2, int index)
        {
            if (ver1[index] > ver2[index])
                return 1;
            if (ver1[index] < ver2[index])
                return 2;
            if (++index < 4)
            {
                return CompareVersion(ver1, ver2, index);
            }
            return 0;
        }
    }
}