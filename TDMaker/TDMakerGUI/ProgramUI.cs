using ShareX.HelpersLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TDMakerLib;

namespace TDMaker
{
    internal static class ProgramUI
    {
        public static List<string> ExplorerFilePaths = new List<string>();
        public static GitHubUpdateManager UpdateManager { get; private set; }

        public static TaskType CurrentTask { get; set; }

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (File.Exists(arg))
                    {
                        ExplorerFilePaths.Add(arg);
                    }
                }
            }

            UpdateManager = new GitHubUpdateManager("McoreD", "TDMaker", beta: false, portable: false);
            App.TurnOn();
            Application.Run(new MainWindow());
        }
    }
}