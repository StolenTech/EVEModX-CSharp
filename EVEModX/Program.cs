using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Net;

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
            Debug.WriteLine(Path.GetTempPath() + @"tmp.bat");
            if (!File.Exists("pyi.dll"))
            {
                StreamWriter sw = new StreamWriter(Path.GetTempPath() + @"tmp.bat");
                sw.WriteLine(@"@echo off");
                sw.WriteLine(@"ping 127.0.0.1 > nul");
                sw.WriteLine("start  " + Application.StartupPath + @"\" + CurrentProcess.ProcessName + ".exe");
                sw.WriteLine(@"del /q %temp%\tmp.bat");
                sw.Flush();
                sw.Dispose();
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile("", "pyi.dll");
                    MessageBox.Show("未检测到pyi.dll, 已下载并重启", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Process.Start(Path.GetTempPath() + "tmp.bat");
                }catch (Exception ex)
                {
                    MessageBox.Show("未检测到pyi.dll并且无法下载,退出", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Error(ex.Message + "\r\n" + ex.HResult.ToString());
                }
                Environment.Exit(2);
            }
            FRM = new FormMain();
            Application.Run(FRM);
            
        }

        static public FormMain FRM;

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
