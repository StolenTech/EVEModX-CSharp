using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace EVEModX
{
    public partial class FormModManagement : Form
    {

        private delegate void SetStatusLabelTextD(string t);
        private delegate void ClearListD();
        private delegate void AddListItemD(ListViewItem i);
        private delegate void SetProgressBarMaxValueD(int i);
        private delegate void AddOneProgressBarValueD();
        private delegate void SetProgressBarValueD(int i);
        private delegate void DisableBtnsD();
        private delegate void EnableBtnsD();

        private bool isFirstInit = true;
        private List<modInfo> Mods = new List<modInfo>();
        private Dictionary<int, MemoryStream> DownloadedData = new Dictionary<int, MemoryStream>();
        private long DownloadedBytes = 0;
        private bool threadPermFail = false;

        #region "Cross-Thread Operations";

        private void SetStatusLabelText(string t)
        {
            if (labelProgessText.InvokeRequired)
            {
                if (!labelProgessText.IsHandleCreated)
                {
                    return;
                }
                SetStatusLabelTextD d_sslt = new SetStatusLabelTextD(SetStatusLabelText);
                labelProgessText.BeginInvoke(d_sslt, t);
            }
            else
            {
                labelProgessText.Text = t;
            }
        }

        private void ClearList()
        {
            if (listView1.InvokeRequired)
            {
                if (!listView1.IsHandleCreated)
                    return;
                ClearListD d_cl = new ClearListD(ClearList);
                listView1.Invoke(d_cl);
            }
            else
            {
                listView1.Items.Clear();
            }
        }

        private void AddListItem(ListViewItem i)
        {
            if (listView1.InvokeRequired)
            {
                if (!listView1.IsHandleCreated)
                    return;
                AddListItemD d_ai = new AddListItemD(AddListItem);
                listView1.BeginInvoke(d_ai, i);
            }
            else
            {
                listView1.Items.Add(i);
            }
        }

        private void AddOneProgressBarValue()
        {
            if (progressBar1.InvokeRequired)
            {
                if (!progressBar1.IsHandleCreated)
                    return;
                AddOneProgressBarValueD d_ado = new AddOneProgressBarValueD(AddOneProgressBarValue);
                progressBar1.BeginInvoke(d_ado);
            }
            else
            {
                progressBar1.PerformStep();
            }
        }

        private void SetProgressBarMaxValue(int i)
        {
            if (progressBar1.InvokeRequired)
            {
                if (!progressBar1.IsHandleCreated)
                    return;
                SetProgressBarMaxValueD d_smv = new SetProgressBarMaxValueD(SetProgressBarMaxValue);
                progressBar1.BeginInvoke(d_smv, i);
            }
            else
            {
                progressBar1.Maximum = i;
            }
        }

        private void SetProgressBarValue(int i)
        {
            if (progressBar1.InvokeRequired)
            {
                if (!progressBar1.IsHandleCreated)
                    return;
                SetProgressBarValueD d_spv = new SetProgressBarValueD(SetProgressBarValue);
                progressBar1.BeginInvoke(d_spv, i);
            }
            else
            {
                progressBar1.Value = i;
            }
        }

        private void DisableBtns()
        {
            if (InvokeRequired)
            {
                if (!button1.IsHandleCreated)
                    return;
                DisableBtnsD d_db = new DisableBtnsD(DisableBtns);
                BeginInvoke(d_db);
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                groupBox1.Enabled = false;
            }
        }

        private void EnableBtns()
        {
            if (InvokeRequired)
            {
                if (!button1.IsHandleCreated)
                    return;
                EnableBtnsD d_eb = new EnableBtnsD(EnableBtns);
                BeginInvoke(d_eb);
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                groupBox1.Enabled = true;
            }
        }
        #endregion

        #region "UI & Main thread operations"
        public FormModManagement()
        {
            InitializeComponent();
        }

        private void FormModManagement_Load(object sender, EventArgs e)
        {
            bool trans = false;
            if (Program.FRM.isInDevMode)
            {
                trans = true;
                checkBox1.CheckState = CheckState.Checked;
            }
                
            UpdateModList(trans);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isFirstInit) return;

            if (checkBox1.Checked)
            {
                UpdateModList(true);
            }
            else
            {
                UpdateModList(false);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisableBtns();
            List<string> s = new List<string>();
            foreach (ListViewItem lvi in listView1.CheckedItems)
            {
                s.Add(lvi.SubItems[0].Text);
            }
            var beginDownloadTh = new Thread(new ParameterizedThreadStart(MTDownStarter));
            beginDownloadTh.Start(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearList();
            Mods = new List<modInfo>();
            UpdateModList(checkBox1.Checked);
        }
        #endregion


        #region "Functional operations";
        private void UpdateModList(bool AllowUnstable)
        {
            Thread th = new Thread(new ParameterizedThreadStart(UpdateModList));
            th.Start(AllowUnstable);
        }

        private void UpdateModList(object data)
        {
            ClearList();
            DisableBtns();
            SetProgressBarMaxValue(3);
            SetStatusLabelText("Updating mods...");
            bool AllowUnstable = (bool)data;
            try
            {
                WebClient wc = new WebClient();
                string jsonBack = wc.DownloadString(@"https://repo.evemodx.com/api/v1/getmods");
                ModsJsonStruct obj = JsonConvert.DeserializeObject<ModsJsonStruct>(jsonBack);
                SetStatusLabelText("validating...");
                SetProgressBarValue(2);
                foreach (modInfo mod in obj.data)
                {
                    if (!AllowUnstable && mod.type == "不稳定")
                    {
                        continue;
                    }
                    string InstV = "";
                    if (File.Exists(@"mod/" + mod.modname + ".zip"))
                    {
                        modInfo tempMI = EVEModX.Program.FRM.Mods.Find(i => i.modname == mod.modname);
                        InstV = tempMI.Version.ToString();
                    }
                    AddListItem(new ListViewItem(new string[] { mod.modname, mod.author, mod.brief, InstV, mod.Version.ToString(), mod.type }));
                    Mods.Add(mod);
                }
                SetStatusLabelText("Update completed.");
                SetProgressBarValue(3);
            }catch (Exception e)
            {
                MessageBox.Show("Update failed, reason:" + e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Logger.Warning("Warning: " + e.Message + "  (" + e.HResult + ")");
            }
            isFirstInit = false;
            EnableBtns();
        }

        private void MTDownStarter(object o)
        {
            List<string> items = (List<string>)o;
            if (checkBox2.Checked)
            {
                foreach (string Lvi in items)
                {
                    ModToDownload mtd;
                    mtd.modname = Lvi;
                    mtd.url = Mods.Find(x => x.modname == mtd.modname).download;
                    mtd.modname += ".zip";
                    MTDownloader(mtd);
                }
            }
            else
            {
                STDownloader(items);
            }

        }

        private void STDownloader(List<string> items)
        {
            SetProgressBarMaxValue(items.Count);
            SetProgressBarValue(0);
            SetStatusLabelText("");
            foreach(string item in items)
            {
                var fileStr = @"mods/" + item + ".zip";
                var uri = Mods.Find(x => x.modname == item).download;
                SetStatusLabelText(string.Format("下载{0}中...", fileStr));
                Logger.Info(string.Format("下载{0}中...", fileStr));
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(uri, fileStr);
                    SetStatusLabelText(string.Format("下载{0}完成.", fileStr));
                    Logger.Info(string.Format("下载{0}完成.", fileStr));
                }catch (Exception ex)
                {
                    SetStatusLabelText(string.Format("下载{0}失败.", fileStr));
                    MessageBox.Show(string.Format("下载{0}不成功, 错误:{1}\r\n{2}\r\nStack trace:{3}", item + ".zip", ex.Message, ex.HResult, ex.StackTrace));
                    Logger.Error(string.Format("下载{0}不成功, 错误:{1}\r\n{2}\r\nStack trace:{3}", item + ".zip", ex.Message, ex.HResult, ex.StackTrace));
                }
                AddOneProgressBarValue();
            }
            EnableBtns();
        }

        private bool MTDownloader(object data)
        {
            ModToDownload mtd = (ModToDownload)data;
            SetStatusLabelText("Downloading Mod " + mtd.modname);
            DownloadedData = new Dictionary<int, MemoryStream>();
            DownloadedBytes = 0;
            Logger.Info("Download process for mod " + mtd.modname + " starting, URI: " + mtd.url);
            long mSize = 0;
            
            HttpWebRequest wRequest = WebRequest.CreateHttp(mtd.url);
            wRequest.Method = "HEAD";
            wRequest.Timeout = 2000;
            HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
            if (wResponse.StatusCode == HttpStatusCode.OK)
            {
                mSize = wResponse.ContentLength;
            }
            else
            {
                Logger.Error("Cannot get length of mod file, exiting. " + mtd.url);
                return false;
            }
            wResponse.Dispose();
            SetProgressBarMaxValue((int)mSize + 1);
            SetProgressBarValue(0);

            WebClient wc = new WebClient();
            byte[] dt = wc.DownloadData(mtd.url);

            Logger.Info("Size:" + mSize);
            ModThread mds = new ModThread();
            mds.modname = mtd.modname;
            mds.threads = new List<BackgroundWorker>();
            int threadSize = (int)mSize / 4;
            int endingThreadSize = threadSize + (int)mSize % 4;
            threadPermFail = false;
            for (int i = 0; i < 4; i++)
            {
                ModDownloadInfo mdi = new ModDownloadInfo();
                if (i == 3)
                {
                    mdi.startPosition = i * threadSize;
                    mdi.Length = endingThreadSize + 1;
                }else
                {
                    mdi.startPosition = i * threadSize;
                    mdi.Length = threadSize;
                }
                mdi.url = mtd.url;
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(DownloadThread);
                mds.threads.Add(bw);
                mds.threads.Last().RunWorkerAsync(mdi);
            }
            Logger.Info("Download started, thread:1");
            while(true)
            {
                if ((DownloadedBytes % 50) == 0)
                    SetProgressBarValue((int)DownloadedBytes);
                if (DownloadedBytes >= mSize)
                {
                    SetProgressBarValue((int)mSize);
                    CompositeAndWriteFile(mtd.modname);
                    EnableBtns();
                    return true;
                }
                else
                {
                    if (threadPermFail == true)
                    {
                        MessageBox.Show("Error, a thread failed to download a part of the file.\r\nFile Name:" + mtd.modname);
                        Logger.Error("Download " + mtd.modname + " Failed.");
                        return false;
                    }
                }
            }
        }

        private void DownloadThread(object sender, DoWorkEventArgs e)
        {
            ModDownloadInfo MDI = (ModDownloadInfo)e.Argument;
            int RetryCounter = 0;
            while(RetryCounter<=5 && !threadPermFail)
            {
                try
                {
                    HttpWebRequest hRequest = WebRequest.CreateHttp(MDI.url);
                    hRequest.AddRange(MDI.startPosition, MDI.startPosition + MDI.Length);
                    WebResponse hResp = hRequest.GetResponse();
                    Stream nStream = hResp.GetResponseStream();
                    byte[] b = new byte[hResp.ContentLength];
                    Debug.WriteLine(MDI.startPosition + " " + MDI.Length + " " + hResp.ContentLength);
                    
                    nStream.Read(b, 0, (int)hResp.ContentLength);
                    if (MDI.Length > hResp.ContentLength )
                    {
                        Array.Resize(ref b, MDI.Length);
                        for(int i =0;i<MDI.Length - hResp.ContentLength;i++)
                        {
                            b[hResp.ContentLength + i] = 0;
                        }
                    }
                    MemoryStream mstmp = new MemoryStream(b);
                    //nStream.CopyTo(mstmp);
                    DownloadedData.Add(MDI.startPosition, mstmp);
                    //
                    DownloadedBytes += MDI.Length+1;
                    return;
                }
                catch (Exception ex)
                {
                    RetryCounter++;
                    if (RetryCounter > 5)
                    {
                        threadPermFail = true;
                        //Logger.Error(ex.Message + "\r\nThread: " + MDI.url +", start at " + MDI.startPosition);
                        return;
                    }
                }
            }
        }

        private void CompositeAndWriteFile(string filename)
        {
            try
            {
                FileStream fs = File.Create(@"mods/" + filename);
                foreach(KeyValuePair<int, MemoryStream> d in DownloadedData)
                {
                    Debug.WriteLine(d.Value.Length);
                }
                Dictionary<int, MemoryStream> sorted = 
                    (from d in DownloadedData orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (KeyValuePair<int, MemoryStream> data in sorted)
                {
                    Debug.WriteLine(data.Value.Length);
                    try
                    {
                        data.Value.Position = 0;
                        int rData = (int)data.Value.Length - 1;
                        
                        //Debug.WriteLine(rData);
                        byte[] stBytes = new byte[rData];
                        //Debug.WriteLine(data.Key + " " + data.Value.Length + " "+ stBytes.Length);
                        data.Value.Read(stBytes, 0, rData);
                        //Debug.WriteLine(stBytes[stBytes.Length - 1]);
                        fs.Write(stBytes, 0, rData);
                        fs.Flush();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine(ex.Source);
                        Debug.WriteLine(ex.StackTrace);
                    }
                    
                    fs.Flush();
                    /*
                    while(true)
                    {
                        rData = data.Value.ReadByte();
                        if (rData == -1)
                        {
                            fs.Flush();
                            fs.Dispose();
                            return;
                        }
                        dByte = (byte)rData;
                        fs.WriteByte(dByte);
                    }*/
                }
                
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(ex.Message);
            }
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}


public class ModDownloadInfo
{
    public string url { get; set; }
    public int startPosition { get; set; }
    public int Length { get; set; }
}

public struct ModToDownload
{
    public string modname;
    public string url;
}

public struct ModThread
{
    public string modname;
    public List<BackgroundWorker> threads;
}

public class modInfo
{
    public int modid { get; set; }
    public string url { get; set; }
    public string modname { get; set; } 
    public string modversion { get; set; }
    public string author { get; set; }
    public string autheremail { get; set; }
    public string brief { get; set; }
    public string description { get; set; }
    public string zipfilename { get; set; }
    public string md5sum { get; set; }
    public int length { get; set; }
    public string download { get; set; }
    public string creattime { get; set; }
    public string updatetime { get; set; }
    public string type { get; set; }
    public string repository { get; set; }
    public Version Version
    {
        get
        {
            return new Version(this.modversion.Replace("v",""));
        }
    }

    private string converUTFStringToString(string input)
    {
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrEmpty(input))
        {
            if (input.IndexOf(@"\u") >= 0)
            {
                string[] uList = input.Replace("\\", "").Split('u');
                foreach (string unInt in uList)
                {
                    byte[] bytes = new byte[2];
                    bytes[1] = byte.Parse(unInt.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    bytes[2] = byte.Parse(unInt.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    sb.Append(Encoding.UTF8.GetString(bytes));
                }
            }
            else
            {
                return input;
            }
        }
        return sb.ToString();
    }

    public modInfo() :base()
    {
        
    }

    public modInfo(string name, string desc, string ver, string auth) : this()
    {
        modname = name;
        description = desc;
        modversion = ver;
        auth = author;
    }
}

public struct ModsJsonStruct
{
    public int status { get; set; }
    public modInfo[] data { get; set; }
}