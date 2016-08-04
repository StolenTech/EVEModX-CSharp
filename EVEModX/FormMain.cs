using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;


namespace EVEModX {
    public partial class FormMain : Form {

        public const string emxversion = "v0.1.1";

        [DllImport("Pyi.dll", EntryPoint = "InjectPythonCodeToPID", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InjectPythonCodeToPID(int pid, string code);
        
        public struct ModInfo {
            public string name;
            public string version;
            public string author;
            public string description;

        }
        
        public FormMain() {
            InitializeComponent();
            Logger.Debug("Init\r\n");
            Logger.Debug("Version: " + emxversion);
            Logger.Debug("OS: " + Environment.OSVersion);
            Logger.Debug("Env version: " + Environment.Version);
            Logger.Debug("Curr directory: " + Environment.CurrentDirectory);
            Logger.Debug("Curr user: " + Environment.UserName);

        }

        class Proc {
            
            public Dictionary<int,string> getProcessInfoByname(string processName) {
                Dictionary<int, string> ret = new Dictionary<int, string>();
                Process[] gameProcessLocalByName = Process.GetProcessesByName(processName);
                foreach (Process item in gameProcessLocalByName) {
                    if (item.MainWindowTitle.Length < 4) {
                        continue;
                    }
                    ret.Add(item.Id, item.MainWindowTitle.Remove(0,6));
                }
                return ret;
            }
            public int Inject(int pid, string payload) {

                int ret = InjectPythonCodeToPID(pid, payload);
                return ret;
            }
        }

        private void UpdateProcess() {
            Logger.Debug("Updating processes list");
            Proc p = new Proc();
            Dictionary<int, string> pps = p.getProcessInfoByname("exefile");
            this.listView1.Items.Clear();
            foreach (var d in pps) {
                this.listView1.Items.Add(new ListViewItem(new string[] { d.Key.ToString(), d.Value }));
            }
            foreach (ListViewItem item in listView1.Items) {
                item.Checked = true;
            }
        }

        private void UpdateMod() {
            Logger.Debug("Updating mods list");

            if (Directory.Exists("mods") == false) {
                Logger.Error("Mod dir not found, exit with code 50");
                MessageBox.Show("Mod 文件夹未找到，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(50);
            }
            var d = Directory.GetDirectories("mods");

            foreach (var e in d) {

                if (File.Exists(e + "\\info.json") == false) {
                    Logger.Error(e + " info.json not found, exit with code 51");
                    MessageBox.Show(e + "\\info.json 缺失，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(51);
                }
                string jsontext = File.ReadAllText(e + "\\info.json");
                if (isValidJson(jsontext) == false) {
                    Logger.Error(e + " info.json cannot be parsed, exit with code 52");
                    MessageBox.Show(e + "\\info.json 无法解析，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(52);
                }
                ModInfo o = JsonConvert.DeserializeObject<ModInfo>(jsontext);
                
                this.listView2.Items.Add(new ListViewItem(new string[] { o.name, o.description, o.version, o.author }));
            }
            foreach (ListViewItem item in listView2.Items) {
                item.Checked = true;
            }
        }

        private void FormMain_Load(object sender, EventArgs e) {
            Logger.Debug("FormMain loaded");
            UpdateMod();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void checkBoxAutoRefresh_CheckedChanged(object sender, EventArgs e) {
            Logger.Debug("Updating processes automatically");
            if (checkBoxAutoRefresh.CheckState == CheckState.Checked) {
                timerRefreshProcess.Enabled = true;
                timerRefreshProcess.Interval = 3000;
            } else if (checkBoxAutoRefresh.CheckState == CheckState.Unchecked) {
                timerRefreshProcess.Enabled = false;
            }
        }

        private void timerRefreshProcess_Tick(object sender, EventArgs e) {
            Logger.Debug("Ticker_refresh ticks\n");
            UpdateProcess();
        }
        
        private static bool isValidJson(string strInput) {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || (strInput.StartsWith("[") && strInput.EndsWith("]"))) {
                try {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex) {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex){
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else {
                return false;
            }
        }
    
        private void buttonDoInject_Click(object sender, EventArgs e) {
            //RaisePrivileges();
            if ((listView2.CheckedItems.Count == 0) || (listView1.CheckedItems.Count == 0)) {
                MessageBox.Show("未选中游戏进程/Mod", "Informational", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            string pathPayload = "import sys;sys.path.append('" + Environment.CurrentDirectory + "\\mods\\');";

            Proc p = new Proc();
            foreach(ListViewItem lvi1 in listView1.CheckedItems) {
                Logger.Debug("Inject path payload to " + lvi1.SubItems[0].Text + " using payload{" + pathPayload + "}\n");
                int ret = p.Inject(int.Parse(lvi1.SubItems[0].Text), pathPayload.Replace("\\", "/"));
                checkret(ret);

                foreach (ListViewItem lvi2 in listView2.CheckedItems) {
                    string payload = "import " + lvi2.SubItems[0].Text + ";";
                    Logger.Debug("Inject pid " + lvi1.SubItems[0].Text + " using payload{" + payload + "}");
                    ret = p.Inject(int.Parse(lvi1.SubItems[0].Text), payload.Replace("\\", "/"));
                    checkret(ret);
                }
  
            }
            MessageBox.Show("写入成功!", "Informational", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e) {

        }
        
        private void ToolStripMenuItemAbout_Click_1(object sender, EventArgs e) {
            FormAbout frmAbout = new FormAbout();
            frmAbout.Show();
        }

        private void ToolStripMenuItemExit_Click_1(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        private void ToolStripMenuItemModRepo_Click_1(object sender, EventArgs e) {
            Process.Start("https://github.com/EVEModX/Mods");
        }

        private void checkret(int ret) {
            switch (ret) {
                case 1:
                    Logger.Error("OpenProcessToken or hModule Failed, code 1");
                    MessageBox.Show("OpenProcessToken or hModule Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                    break;
                case 2:
                    Logger.Error("LookupPrivilegeValue or PyGILState_Ensure Failed, code 2");
                    MessageBox.Show("LookupPrivilegeValue or PyGILState_Ensure Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(2);
                    break;
                case 3:
                    Logger.Error("AdjustTokenPrivileges or PyRun_SimpleString Failed, code 3");
                    MessageBox.Show("AdjustTokenPrivileges or PyRun_SimpleString Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(3);
                    break;
                case 4:
                    Logger.Error("PyGILState_Release Failed, code 4");
                    MessageBox.Show("PyGILState_Release Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(4);
                    break;
                case 5:
                    Logger.Error("WriteProcessMemory Failed, code 5");
                    MessageBox.Show("WriteProcessMemory Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(5);
                    break;
                case 6:
                    Logger.Error("WriteProcessMemory2 Failed, code 6");
                    MessageBox.Show("WriteProcessMemory2 Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(6);
                    break;
                case 7:
                    Logger.Error("CreateRemoteThread Failed, code 7");
                    MessageBox.Show("CreateRemoteThread Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(7);
                    break;
                case 8:
                    Logger.Error("WaitForSingleObject Failed, code 8");
                    MessageBox.Show("WaitForSingleObject Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(8);
                    break;
                case 9:
                    Logger.Error("GetExitCodeThread Failed, code 9");
                    MessageBox.Show("GetExitCodeThread Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(9);
                    break;
                case 10:
                    Logger.Error("code 10");
                    MessageBox.Show("exitCode 10", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(10);
                    break;
                default:
                    break;
            }
        }

        private void groupBoxProcesses_Enter(object sender, EventArgs e) {

        }

        private void ToolStripMenuItemDevMode_Click(object sender, EventArgs e) {
            if (ToolStripMenuItemDevMode.CheckState == CheckState.Checked) {
                listView2.ContextMenuStrip = contextMenuStripMods;
            } else if (ToolStripMenuItemDevMode.CheckState == CheckState.Unchecked) {
                listView2.ContextMenuStrip = null;
            }
        }

        private void ToolStripMenuItemReloadMods_Click(object sender, EventArgs e) {
            Proc p = new Proc();
            if ((listView2.CheckedItems.Count == 0) || (listView1.CheckedItems.Count == 0)) {
                MessageBox.Show("未选中游戏进程/Mod", "Informational", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            foreach (ListViewItem lvi1 in listView1.CheckedItems) {
                foreach (ListViewItem lvi2 in listView2.SelectedItems) {
                    string reloadPayload = "reload(" + lvi2.SubItems[0].Text + ");";
                    Logger.Debug("Inject reload payload to pid " + lvi1.SubItems[0].Text + " using payload{" + reloadPayload + "}");
                    int ret = p.Inject(int.Parse(lvi1.SubItems[0].Text), reloadPayload);
                    checkret(ret);
                }
            }
        }

        private void buttonReloadProcesses_Click(object sender, EventArgs e) {
            Logger.Debug("Refresh Process\n");
            UpdateProcess();
        }
    }


    public class Logger {
        private const string PATH = "log";
        private const string FILE_NAME = "log.txt";
        private const string FULL_NAME = PATH + "/" + FILE_NAME;
        public static readonly object Locker = new object();
        private static StreamWriter WRITER;
        private static string GUID;

        private static string ContinueWriteCaches;
        private static readonly Stopwatch Continue_WriteSw;
        private static int ContinueTime = 300;
        private static int ContinueCountMax = 100;
        private static int ContinueCount = 0;
        public static int AllWriteCount = 0;

        static Logger() {
            Continue_WriteSw = new Stopwatch();
        }

        private static string ProjectFullName {
            get {
                if (string.IsNullOrEmpty(GUID))
                    GUID = Guid.NewGuid().ToString();
                return PATH + "/" + "EMX_" + GUID + "_" + FILE_NAME;
            }
        }

        private static void Write(string msg) {
            if (string.IsNullOrEmpty(msg)) return;

            lock (Locker) {
                if (Continue_WriteSw.IsRunning && Continue_WriteSw.ElapsedMilliseconds < ContinueTime) {
                    if (ContinueWriteCaches == null) ContinueWriteCaches = msg;
                    else ContinueWriteCaches += msg + "\r\n";
                    ContinueCount++;
                    if (ContinueCount > ContinueCountMax) {
                        _Write();
                    }
                    return;
                }

                if (!Continue_WriteSw.IsRunning) Continue_WriteSw.Start();
                ContinueWriteCaches = msg;

                new Task(() => {
                    Thread.Sleep(ContinueTime);
                    _Write();
                }).Start();
            }
        }

        private static void _Write() {
            if (ContinueWriteCaches != null) {
                if (!File.Exists(ProjectFullName)) {
                    if (!Directory.Exists(PATH))
                        Directory.CreateDirectory(PATH);
                    
                }

                WRITER = new StreamWriter(ProjectFullName, true, Encoding.UTF8);
                WRITER.WriteLine(ContinueWriteCaches);
                WRITER.Flush();
                WRITER.Close();
            }
            Continue_WriteSw.Stop();
            Continue_WriteSw.Reset();
            ContinueWriteCaches = null;
            ContinueCount = 0;

            Interlocked.Increment(ref AllWriteCount);
        }

        public static void Debug(string msg) {
            msg = string.Format("[{0} {1}] : {2}", "Debug", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Info(string msg) {
            msg = string.Format("[{0} {1}] : {2}", "Info", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Error(string msg) {
            msg = string.Format("[{0} {1}] : {2}", "Error", DateTime.Now.ToString(), msg);
            Write(msg);
        }
    }
}

namespace B {

}