using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace EVEModX {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Process CurrentProcess = Process.GetCurrentProcess();
            List<Process> AllProc =new List<Process>( Process.GetProcessesByName(CurrentProcess.ProcessName));
            if (AllProc.Count>1) { Application.Exit(); }
            try
            {
                CheckDotNetVersion();
            }catch (Exception e)
            {
                Logger.Error(e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            
        }

        static string DotNetErrorStr = ".Net framework 4.5 or higher not detected. Please install .Net framework 4.5 or higher version.";
        static void CheckDotNetVersion()
        {
            RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\");
            if (ndpKey == null)
            {
                ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client\\");
            }
            if (ndpKey == null)
            {
                throw new InvalidOperationException(DotNetErrorStr);
            }
            int RelValue = (int) ndpKey.GetValue("Release");
            if (RelValue < 378389)
            {
                throw new InvalidOperationException(DotNetErrorStr);
            }
        }
    }
}
