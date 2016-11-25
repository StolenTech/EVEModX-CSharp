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
            Version cv = new Version("0.0");
            try
            {
                var vstr = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").GetValue("CurrentMajorVersionNumber");
                if (vstr == null)
                {
                    cv = new Version((string)Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").GetValue("CurrentVersion"));
                }
                else
                {
                    int vma, vmi;
                    vma = (int)Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").GetValue("CurrentMajorVersionNumber");
                    vmi = (int)Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").GetValue("CurrentMinorVersionNumber");
                    cv = new Version(vma.ToString() + "." + vmi.ToString());
                }
            }catch (Exception exc)
            {
                Logger.Error(exc.Message);
                MessageBox.Show(exc.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(4);
            }
            
            Version cpv = new Version("6.3");
            if(cv.CompareTo(cpv) <= 0)
            {
                Logger.Error("Unsuporrted OS. DO NOT PROVIDE TECHNICAL SUPPORT.");
                var o = MessageBox.Show("Unsuporrted OS, are you sure you want to continue?\r\nNo support or warrnetry will be applied.", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (o == DialogResult.No)
                {
                    Environment.Exit(3);
                }
                Logger.Info("Forced start on unsupported OS.");
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
                sw.WriteLine(@"del /q %%temp%%\tmp.bat");
                sw.Flush();
                sw.Dispose();
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(@"https://repo.evemodx.com/uploads/emx/components/Pyi.dll", "pyi.dll");
                    MessageBox.Show("未检测到pyi.dll, 已下载并重启", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Process proc = new Process();
                    proc.StartInfo.FileName = Path.GetTempPath() + "tmp.bat";
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    //Process.Start();
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
