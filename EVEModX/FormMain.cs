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


namespace EVEModX
{

    public partial class FormMain : Form
    {

        public const string emxversion = "v0.3.3";

        [DllImport("Pyi.dll", EntryPoint = "InjectPythonCodeToPID", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InjectPythonCodeToPID(int pid, string code);

        public delegate void updatelistDeleg(ref ListViewItem ListItem, ref int option, bool isChecked = false);

        private FormWindowState currentStatus = FormWindowState.Normal;

        /// <summary>
        /// Async update listview
        /// </summary>
        /// <param name="ListItem">the item need to update</param>
        /// <param name="option">0=Add, 1=Clear</param>
        public void updateListView(ref ListViewItem ListItem, ref int option, bool isChecked = false)
        {
            switch (option)
            {
                case 0:
                    if (isChecked) ListItem.Checked = true;
                    listViewGameProcess.Items.Add(ListItem);
                    break;
                case 1:
                    listViewGameProcess.Items.Clear();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Mod Info Storage Structure
        /// </summary>
        public struct ModInfo
        {
            public string name;
            public string version;
            public string author;
            public string description;
        }

        /// <summary>
        /// Preferences Class
        /// </summary>
        public class Preferences
        {
            public IList<string> PrefMods { get; set; }
        }

        /// <summary>
        /// Initialize Form Main
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            Logger.Debug("Init\r\n");
            Logger.Debug("Version: " + emxversion);
            Logger.Debug("OS: " + Environment.OSVersion);
            Logger.Debug("Env version: " + Environment.Version);
            Logger.Debug("Curr directory: " + Environment.CurrentDirectory);
            Logger.Debug("Curr user: " + Environment.UserName);
            notifyIconMain.Icon = EVEModX.Properties.Resources.Medical_Health_Syringe_injection;
        }

        /// <summary>
        /// Process operations class
        /// </summary>
        class Proc
        {

            /// <summary>
            /// Get Process Info By Process Name
            /// </summary>
            /// <param name="processName">String, the name of the process</param>
            /// <returns></returns>
            public Dictionary<int, string> getProcessInfoByname(string processName)
            {
                Dictionary<int, string> ret = new Dictionary<int, string>();
                Process[] gameProcessLocalByName = Process.GetProcessesByName(processName);
                foreach (Process item in gameProcessLocalByName)
                {
                    if (item.MainWindowTitle.Length < 4)
                    {
                        ret.Add(item.Id, "(尚未选定角色)");
                        continue;
                    }
                    ret.Add(item.Id, item.MainWindowTitle.Remove(0, 6));
                }
                return ret;
            }
            /// <summary>
            /// inject code caller
            /// </summary>
            /// <param name="pid">the PID of the process</param>
            /// <param name="payload">the payload to inject</param>
            /// <returns>if the injection is successful</returns>
            public int Inject(int pid, string payload)
            {
                int ret = InjectPythonCodeToPID(pid, payload);
                return ret;
            }
        }

        /// <summary>
        /// Background update process list
        /// </summary>
        private void UpdateProcess()
        {
            Logger.Debug("Updating processes list");
            Proc p = new Proc();
            Dictionary<int, string> pps = p.getProcessInfoByname("exefile");
            BeginInvoke(new updatelistDeleg(updateListView), new ListViewItem(), 1, false);
            foreach (var d in pps)
            {
                BeginInvoke(new updatelistDeleg(updateListView), new ListViewItem(new string[] { d.Key.ToString(), d.Value }), 0, true);
            }
        }

        /// <summary>
        /// Update Mod List
        /// </summary>
        private void UpdateMod()
        {
            Logger.Debug("Updating mods list");

            if (Directory.Exists("mods") == false)
            {
                Logger.Error("Mod dir not found, exit with code 50");
                MessageBox.Show("Mod 文件夹未找到，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(50);
            }
            var d = Directory.GetDirectories("mods");

            foreach (var e in d)
            {

                if (File.Exists(e + "\\info.json") == false)
                {
                    //Logger.Error(e + " info.json not found, exit with code 51");
                    // MessageBox.Show(e + "\\info.json 缺失，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Environment.Exit(51);
                    continue;
                }
                string jsontext = File.ReadAllText(e + "\\info.json");
                if (isValidJson(jsontext) == false)
                {
                    Logger.Error(e + " info.json cannot be parsed, exit with code 52");
                    MessageBox.Show(e + "\\info.json 无法解析，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(52);
                }
                ModInfo o = JsonConvert.DeserializeObject<ModInfo>(jsontext);

                this.listViewMod.Items.Add(new ListViewItem(new string[] { e.Substring(5), o.description, o.version, o.author }));
            }

            if (File.Exists("preferences.json") == false)
            {
                if (!writeNewJson())
                {
                    Logger.Error("preferences.json write failed, exit with code 55");
                    MessageBox.Show("preferences.json 无法写入，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(55);
                }
            }
            string jsontext2 = File.ReadAllText("preferences.json");
            if (isValidJson(jsontext2) == false)
            {
                if (!writeNewJson())
                {
                    Logger.Error("preferences.json write failed, exit with code 55");
                    MessageBox.Show("preferences.json 无法写入，程序退出", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(55);
                }
            }
            Preferences pref = JsonConvert.DeserializeObject<Preferences>(jsontext2);
            var l = new List<string>();
            foreach (var item in pref.PrefMods)
            {
                l.Add(item);
            }
            foreach (var item in l)
            {
                foreach (ListViewItem lvi3 in listViewMod.Items)
                {
                    if (lvi3.SubItems[0].Text == item)
                    {
                        lvi3.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Overrite existing or non-existing preferences.json
        /// </summary>
        /// <returns>if the writing operation is successful</returns>
        private bool writeNewJson()
        {
            try
            {
                if (File.Exists("preferences.json")) { File.Delete("preferences.json"); }
                StreamWriter sw = File.CreateText("preferences.json");
                sw.WriteLine(EVEModX.Properties.Resources.Preferences_std_json);
                sw.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Called by Form Load Event, initialize the main form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            Logger.Debug("FormMain loaded");
            UpdateMod();
            checkBoxAutoRefresh.Checked = true;
            SizeChanged += new EventHandler(this.FormMain_SizeChanged);
            listViewMod.ContextMenuStrip = null;
        }


        private void listViewGameProcess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Enable or disable Process List auto refresh
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">eventargs</param>
        private void checkBoxAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            Logger.Debug("Updating processes automatically");
            if (checkBoxAutoRefresh.CheckState == CheckState.Checked)
            {
                timerRefreshProcess.Enabled = true;
                timerRefreshProcess.Interval = 1000;
            }
            else if (checkBoxAutoRefresh.CheckState == CheckState.Unchecked)
            {
                timerRefreshProcess.Enabled = false;
            }
        }

        /// <summary>
        /// Start second process to refresh process list, called by timerRefreshProcess tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRefreshProcess_Tick(object sender, EventArgs e)
        {
            Thread updateThread = new Thread(UpdateProcess);
            Logger.Debug("Ticker_refresh ticks\n");
            updateThread.Start();
        }

        /// <summary>
        /// check if the json is valid
        /// </summary>
        /// <param name="strInput">the JSON string to be validated</param>
        /// <returns></returns>
        private static bool isValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    JToken obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// On Button DoInject clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDoInject_Click(object sender, EventArgs e)
        {
            //RaisePrivileges();

            if ((listViewMod.CheckedItems.Count == 0) || (listViewGameProcess.CheckedItems.Count == 0))
            {
                MessageBox.Show("未选中游戏进程/Mod", "Informational", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            string pathPayload = "import sys;sys.path.append('" + Environment.CurrentDirectory + "\\mods\\');";

            Proc p = new Proc();
            int err = 0;

            foreach (ListViewItem lvi1 in listViewGameProcess.CheckedItems)
            {
                Logger.Debug("Inject path payload to " + lvi1.SubItems[0].Text + " using payload{" + pathPayload + "}\n");
                int ret = p.Inject(int.Parse(lvi1.SubItems[0].Text), pathPayload.Replace("\\", "/"));
                if (ret == 5)
                {
                    MessageBox.Show("检测到游戏进程变化，游戏已退出？", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    err = 1;
                    continue;
                }
                checkret(ret);

                foreach (ListViewItem lvi2 in listViewMod.CheckedItems)
                {

                    string payload = "import " + lvi2.SubItems[0].Text + ";";
                    Logger.Debug("Inject pid " + lvi1.SubItems[0].Text + " using payload{" + payload + "}");
                    ret = p.Inject(int.Parse(lvi1.SubItems[0].Text), payload.Replace("\\", "/"));
                    checkret(ret);

                }
            }
            if (err == 0)
            {
                MessageBox.Show("写入成功!", "Informational", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void listViewMod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region "Hide and display tray main window in tray icon"
        private void ToolStripMenuItemAbout_Click_1(object sender, EventArgs e)
        {
            FormAbout frmAbout = new FormAbout();
            frmAbout.Show();
        }

        private void ToolStripMenuItemExit_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ToolStripMenuItemModRepo_Click_1(object sender, EventArgs e)
        {
            Process.Start("https://repo.evemodx.com");
        }
        #endregion
        
        /// <summary>
        /// Check the return value of InjectPython
        /// </summary>
        /// <param name="ret">return value</param>
        private void checkret(int ret)
        {
            switch (ret)
            {
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

        private void groupBoxProcesses_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// on ToolStripMenuItemDevMode Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemDevMode_Click(object sender, EventArgs e)
        {
            if (ToolStripMenuItemDevMode.CheckState == CheckState.Checked)
            {
                listViewMod.ContextMenuStrip = contextMenuStripMods;
            }
            else if (ToolStripMenuItemDevMode.CheckState == CheckState.Unchecked)
            {
                listViewMod.ContextMenuStrip = null;
            }
        }

        /// <summary>
        /// On reload mods clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemReloadMods_Click(object sender, EventArgs e)
        {
            Proc p = new Proc();
            if ((listViewMod.CheckedItems.Count == 0) || (listViewGameProcess.CheckedItems.Count == 0))
            {
                MessageBox.Show("未选中游戏进程/Mod", "Informational", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            foreach (ListViewItem lvi1 in listViewGameProcess.CheckedItems)
            {
                foreach (ListViewItem lvi2 in listViewMod.SelectedItems)
                {
                    string reloadPayload = "reload(" + lvi2.SubItems[0].Text + ");";
                    Logger.Debug("Inject reload payload to pid " + lvi1.SubItems[0].Text + " using payload{" + reloadPayload + "}");
                    int ret = p.Inject(int.Parse(lvi1.SubItems[0].Text), reloadPayload);
                    checkret(ret);
                }
            }
        }

        /// <summary>
        /// on refresh process button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReloadProcesses_Click(object sender, EventArgs e)
        {
            Logger.Debug("Refresh Process\n");
            UpdateProcess();
        }

        /// <summary>
        /// process the form closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            var Mods = new List<string>();
            foreach (ListViewItem lvi2 in listViewMod.CheckedItems)
            {
                Mods.Add(lvi2.SubItems[0].Text);
            }
            Preferences pref = new Preferences
            {
                PrefMods = Mods
            };
            string json = JsonConvert.SerializeObject(pref, Formatting.Indented);
            FileStream fs = new FileStream("preferences.json", FileMode.Create);
            byte[] data = new UTF8Encoding().GetBytes(json);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            FormClosing -= new FormClosingEventHandler(FormMain_FormClosing);
            Application.Exit();
        }

        #region "tray icon processer"
        private void buttonRefreshModList_Click(object sender, EventArgs e)
        {
            listViewMod.Items.Clear();
            UpdateMod();
        }

        private void showMainWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showWindow();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                Visible = false;
                ShowInTaskbar = false;
            }
            else
            {
                Visible = true;
                ShowInTaskbar = true;
            }
        }

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showWindow();
        }

        private void showWindow()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Normal;
                BringToFront();
                Activate();
            }
            else if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Minimized;

            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }
    }
    #endregion

    /// <summary>
    /// logger class
    /// </summary>
    public class Logger
    {
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

        /// <summary>
        /// initial vars
        /// </summary>
        static Logger()
        {
            Continue_WriteSw = new Stopwatch();
        }

        /// <summary>
        /// get the full name of project
        /// </summary>
        private static string ProjectFullName
        {
            get
            {
                if (string.IsNullOrEmpty(GUID))
                    GUID = Guid.NewGuid().ToString();
                return PATH + "/" + "EMX_" + GUID + "_" + FILE_NAME;
            }
        }

        /// <summary>
        /// write log text into file
        /// </summary>
        /// <param name="msg"></param>
        private static void Write(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            lock (Locker)
            {
                if (Continue_WriteSw.IsRunning && Continue_WriteSw.ElapsedMilliseconds < ContinueTime)
                {
                    if (ContinueWriteCaches == null) ContinueWriteCaches = msg;
                    else ContinueWriteCaches += msg + "\r\n";
                    ContinueCount++;
                    if (ContinueCount > ContinueCountMax)
                    {
                        _Write();
                    }
                    return;
                }

                if (!Continue_WriteSw.IsRunning) Continue_WriteSw.Start();
                ContinueWriteCaches = msg;

                new Task(() =>
                {
                    Thread.Sleep(ContinueTime);
                    _Write();
                }).Start();
            }
        }

        /// <summary>
        /// writer initial
        /// </summary>
        private static void _Write()
        {
            if (ContinueWriteCaches != null)
            {
                if (!File.Exists(ProjectFullName))
                {
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


        public static void Debug(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Debug", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Info(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Info", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Error(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Error", DateTime.Now.ToString(), msg);
            Write(msg);
        }
    }
}

namespace B
{

}