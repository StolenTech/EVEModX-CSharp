using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
