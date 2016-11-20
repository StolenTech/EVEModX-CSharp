namespace EVEModX {
    partial class FormMain {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBoxProcesses = new System.Windows.Forms.GroupBox();
            this.listViewGameProcess = new System.Windows.Forms.ListView();
            this.columnProcHeaderPid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProcHeaderCharName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxMod = new System.Windows.Forms.GroupBox();
            this.listViewMod = new System.Windows.Forms.ListView();
            this.columnModHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnModHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnModHeaderVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnModHeaderAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripMods = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemReloadMods = new System.Windows.Forms.ToolStripMenuItem();
            this.timerRefreshProcess = new System.Windows.Forms.Timer(this.components);
            this.buttonDoInject = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOption = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDevMode = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemModRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonReloadProcesses = new System.Windows.Forms.Button();
            this.checkBoxAutoRefresh = new System.Windows.Forms.CheckBox();
            this.buttonRefreshModList = new System.Windows.Forms.Button();
            this.contextMenuStripIconRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMainWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBoxProcesses.SuspendLayout();
            this.groupBoxMod.SuspendLayout();
            this.contextMenuStripMods.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStripIconRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxProcesses
            // 
            this.groupBoxProcesses.Controls.Add(this.listViewGameProcess);
            this.groupBoxProcesses.Location = new System.Drawing.Point(12, 30);
            this.groupBoxProcesses.Name = "groupBoxProcesses";
            this.groupBoxProcesses.Size = new System.Drawing.Size(304, 327);
            this.groupBoxProcesses.TabIndex = 4;
            this.groupBoxProcesses.TabStop = false;
            this.groupBoxProcesses.Text = "游戏进程列表";
            this.groupBoxProcesses.Enter += new System.EventHandler(this.groupBoxProcesses_Enter);
            // 
            // listViewGameProcess
            // 
            this.listViewGameProcess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewGameProcess.CheckBoxes = true;
            this.listViewGameProcess.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnProcHeaderPid,
            this.columnProcHeaderCharName});
            this.listViewGameProcess.GridLines = true;
            this.listViewGameProcess.Location = new System.Drawing.Point(6, 16);
            this.listViewGameProcess.MultiSelect = false;
            this.listViewGameProcess.Name = "listViewGameProcess";
            this.listViewGameProcess.Size = new System.Drawing.Size(292, 305);
            this.listViewGameProcess.TabIndex = 0;
            this.listViewGameProcess.UseCompatibleStateImageBehavior = false;
            this.listViewGameProcess.View = System.Windows.Forms.View.Details;
            this.listViewGameProcess.SelectedIndexChanged += new System.EventHandler(this.listViewGameProcess_SelectedIndexChanged);
            // 
            // columnProcHeaderPid
            // 
            this.columnProcHeaderPid.Text = "PID";
            this.columnProcHeaderPid.Width = 85;
            // 
            // columnProcHeaderCharName
            // 
            this.columnProcHeaderCharName.Text = "角色名称";
            this.columnProcHeaderCharName.Width = 203;
            // 
            // groupBoxMod
            // 
            this.groupBoxMod.Controls.Add(this.listViewMod);
            this.groupBoxMod.Location = new System.Drawing.Point(322, 30);
            this.groupBoxMod.Name = "groupBoxMod";
            this.groupBoxMod.Size = new System.Drawing.Size(543, 327);
            this.groupBoxMod.TabIndex = 5;
            this.groupBoxMod.TabStop = false;
            this.groupBoxMod.Text = "Mod 列表";
            // 
            // listViewMod
            // 
            this.listViewMod.CheckBoxes = true;
            this.listViewMod.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnModHeaderName,
            this.columnModHeaderDescription,
            this.columnModHeaderVersion,
            this.columnModHeaderAuthor});
            this.listViewMod.ContextMenuStrip = this.contextMenuStripMods;
            this.listViewMod.GridLines = true;
            this.listViewMod.Location = new System.Drawing.Point(6, 16);
            this.listViewMod.Name = "listViewMod";
            this.listViewMod.Size = new System.Drawing.Size(531, 305);
            this.listViewMod.TabIndex = 0;
            this.listViewMod.UseCompatibleStateImageBehavior = false;
            this.listViewMod.View = System.Windows.Forms.View.Details;
            this.listViewMod.SelectedIndexChanged += new System.EventHandler(this.listViewMod_SelectedIndexChanged);
            // 
            // columnModHeaderName
            // 
            this.columnModHeaderName.Text = "名称";
            this.columnModHeaderName.Width = 126;
            // 
            // columnModHeaderDescription
            // 
            this.columnModHeaderDescription.Text = "描述";
            this.columnModHeaderDescription.Width = 209;
            // 
            // columnModHeaderVersion
            // 
            this.columnModHeaderVersion.Text = "版本";
            this.columnModHeaderVersion.Width = 72;
            // 
            // columnModHeaderAuthor
            // 
            this.columnModHeaderAuthor.Text = "作者";
            this.columnModHeaderAuthor.Width = 118;
            // 
            // contextMenuStripMods
            // 
            this.contextMenuStripMods.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemReloadMods});
            this.contextMenuStripMods.Name = "contextMenuStripMods";
            this.contextMenuStripMods.Size = new System.Drawing.Size(118, 26);
            // 
            // ToolStripMenuItemReloadMods
            // 
            this.ToolStripMenuItemReloadMods.Name = "ToolStripMenuItemReloadMods";
            this.ToolStripMenuItemReloadMods.Size = new System.Drawing.Size(117, 22);
            this.ToolStripMenuItemReloadMods.Text = "Reload";
            this.ToolStripMenuItemReloadMods.Click += new System.EventHandler(this.ToolStripMenuItemReloadMods_Click);
            // 
            // timerRefreshProcess
            // 
            this.timerRefreshProcess.Tick += new System.EventHandler(this.timerRefreshProcess_Tick);
            // 
            // buttonDoInject
            // 
            this.buttonDoInject.Location = new System.Drawing.Point(780, 373);
            this.buttonDoInject.Name = "buttonDoInject";
            this.buttonDoInject.Size = new System.Drawing.Size(85, 31);
            this.buttonDoInject.TabIndex = 10;
            this.buttonDoInject.Text = "执行 Mod";
            this.buttonDoInject.UseVisualStyleBackColor = true;
            this.buttonDoInject.Click += new System.EventHandler(this.buttonDoInject_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFile,
            this.ToolStripMenuItemOption,
            this.ToolStripMenuItemHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(877, 25);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // ToolStripMenuItemFile
            // 
            this.ToolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemExit});
            this.ToolStripMenuItemFile.Name = "ToolStripMenuItemFile";
            this.ToolStripMenuItemFile.Size = new System.Drawing.Size(44, 21);
            this.ToolStripMenuItemFile.Text = "文件";
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemExit.Text = "退出";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click_1);
            // 
            // ToolStripMenuItemOption
            // 
            this.ToolStripMenuItemOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemDevMode});
            this.ToolStripMenuItemOption.Name = "ToolStripMenuItemOption";
            this.ToolStripMenuItemOption.Size = new System.Drawing.Size(44, 21);
            this.ToolStripMenuItemOption.Text = "选项";
            // 
            // ToolStripMenuItemDevMode
            // 
            this.ToolStripMenuItemDevMode.CheckOnClick = true;
            this.ToolStripMenuItemDevMode.Name = "ToolStripMenuItemDevMode";
            this.ToolStripMenuItemDevMode.Size = new System.Drawing.Size(137, 22);
            this.ToolStripMenuItemDevMode.Text = "Dev Mode";
            this.ToolStripMenuItemDevMode.CheckStateChanged += new System.EventHandler(this.ToolStripMenuItemDevMode_CheckStateChanged);
            this.ToolStripMenuItemDevMode.Click += new System.EventHandler(this.ToolStripMenuItemDevMode_Click);
            // 
            // ToolStripMenuItemHelp
            // 
            this.ToolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAbout,
            this.ToolStripMenuItemModRepo});
            this.ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp";
            this.ToolStripMenuItemHelp.Size = new System.Drawing.Size(44, 21);
            this.ToolStripMenuItemHelp.Text = "帮助";
            // 
            // ToolStripMenuItemAbout
            // 
            this.ToolStripMenuItemAbout.Name = "ToolStripMenuItemAbout";
            this.ToolStripMenuItemAbout.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemAbout.Text = "关于";
            this.ToolStripMenuItemAbout.Click += new System.EventHandler(this.ToolStripMenuItemAbout_Click_1);
            // 
            // ToolStripMenuItemModRepo
            // 
            this.ToolStripMenuItemModRepo.Name = "ToolStripMenuItemModRepo";
            this.ToolStripMenuItemModRepo.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemModRepo.Text = "Mod Repository";
            this.ToolStripMenuItemModRepo.Click += new System.EventHandler(this.ToolStripMenuItemModRepo_Click_1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 413);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(877, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(202, 17);
            this.toolStripStatusLabel1.Text = "©2016 the EVEModX Developers";
            // 
            // buttonReloadProcesses
            // 
            this.buttonReloadProcesses.Location = new System.Drawing.Point(217, 373);
            this.buttonReloadProcesses.Name = "buttonReloadProcesses";
            this.buttonReloadProcesses.Size = new System.Drawing.Size(99, 31);
            this.buttonReloadProcesses.TabIndex = 13;
            this.buttonReloadProcesses.Text = "立即刷新进程";
            this.buttonReloadProcesses.UseVisualStyleBackColor = true;
            this.buttonReloadProcesses.Click += new System.EventHandler(this.buttonReloadProcesses_Click);
            // 
            // checkBoxAutoRefresh
            // 
            this.checkBoxAutoRefresh.AutoSize = true;
            this.checkBoxAutoRefresh.Location = new System.Drawing.Point(12, 379);
            this.checkBoxAutoRefresh.Name = "checkBoxAutoRefresh";
            this.checkBoxAutoRefresh.Size = new System.Drawing.Size(150, 21);
            this.checkBoxAutoRefresh.TabIndex = 9;
            this.checkBoxAutoRefresh.Text = "自动刷新进程列表(1秒)";
            this.checkBoxAutoRefresh.UseVisualStyleBackColor = true;
            this.checkBoxAutoRefresh.CheckedChanged += new System.EventHandler(this.checkBoxAutoRefresh_CheckedChanged);
            // 
            // buttonRefreshModList
            // 
            this.buttonRefreshModList.Location = new System.Drawing.Point(667, 373);
            this.buttonRefreshModList.Name = "buttonRefreshModList";
            this.buttonRefreshModList.Size = new System.Drawing.Size(107, 31);
            this.buttonRefreshModList.TabIndex = 14;
            this.buttonRefreshModList.Text = "刷新 Mod 列表";
            this.buttonRefreshModList.UseVisualStyleBackColor = true;
            this.buttonRefreshModList.Click += new System.EventHandler(this.buttonRefreshModList_Click);
            // 
            // contextMenuStripIconRightClick
            // 
            this.contextMenuStripIconRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMainWindowToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.contextMenuStripIconRightClick.Name = "contextMenuStripIconRightClick";
            this.contextMenuStripIconRightClick.Size = new System.Drawing.Size(166, 54);
            // 
            // showMainWindowToolStripMenuItem
            // 
            this.showMainWindowToolStripMenuItem.Name = "showMainWindowToolStripMenuItem";
            this.showMainWindowToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.showMainWindowToolStripMenuItem.Text = "显示/隐藏主窗口";
            this.showMainWindowToolStripMenuItem.Click += new System.EventHandler(this.showMainWindowToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.quitToolStripMenuItem.Text = "退出";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.contextMenuStripIconRightClick;
            this.notifyIconMain.Text = "EVEModX Framework";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseDoubleClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 435);
            this.Controls.Add(this.buttonRefreshModList);
            this.Controls.Add(this.buttonReloadProcesses);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonDoInject);
            this.Controls.Add(this.checkBoxAutoRefresh);
            this.Controls.Add(this.groupBoxMod);
            this.Controls.Add(this.groupBoxProcesses);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "EVEModX Framework";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBoxProcesses.ResumeLayout(false);
            this.groupBoxMod.ResumeLayout(false);
            this.contextMenuStripMods.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStripIconRightClick.ResumeLayout(false);
            this.Icon = EVEModX.Properties.Resources.Injector;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxProcesses;
        private System.Windows.Forms.ListView listViewGameProcess;
        private System.Windows.Forms.ColumnHeader columnProcHeaderPid;
        private System.Windows.Forms.ColumnHeader columnProcHeaderCharName;
        private System.Windows.Forms.GroupBox groupBoxMod;
        private System.Windows.Forms.ListView listViewMod;
        private System.Windows.Forms.ColumnHeader columnModHeaderName;
        private System.Windows.Forms.Timer timerRefreshProcess;
        private System.Windows.Forms.ColumnHeader columnModHeaderDescription;
        private System.Windows.Forms.ColumnHeader columnModHeaderVersion;
        private System.Windows.Forms.ColumnHeader columnModHeaderAuthor;
        private System.Windows.Forms.Button buttonDoInject;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemModRepo;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOption;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDevMode;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMods;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemReloadMods;
        private System.Windows.Forms.Button buttonReloadProcesses;
        private System.Windows.Forms.CheckBox checkBoxAutoRefresh;
        private System.Windows.Forms.Button buttonRefreshModList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripIconRightClick;
        private System.Windows.Forms.ToolStripMenuItem showMainWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

