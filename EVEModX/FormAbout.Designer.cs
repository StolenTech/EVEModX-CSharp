namespace EVEModX {
    partial class FormAbout {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.labelH = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelP = new System.Windows.Forms.Label();
            this.labelDev = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelH.Location = new System.Drawing.Point(30, 29);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(94, 22);
            this.labelH.TabIndex = 1;
            this.labelH.Text = "EVEModX";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelVersion.Location = new System.Drawing.Point(30, 61);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(37, 20);
            this.labelVersion.TabIndex = 2;
            this.labelVersion.Text = "版本";
            // 
            // labelP
            // 
            this.labelP.Location = new System.Drawing.Point(31, 103);
            this.labelP.Name = "labelP";
            this.labelP.Size = new System.Drawing.Size(507, 120);
            this.labelP.TabIndex = 3;
            this.labelP.Text = "EVEModX 是一个不在修改本地文件的情况下，通过 Mod 改变 EVE Online 客户端的功能特性的框架。\r\n\r\n使用它意味着你已授权本框架以及 Mod " +
    "对你的 EVE Online 客户端程序进行 Monkey-Patch，此为严重违反 EULA 的行为，由此产生的后果均由你个人承担。\r\n\r\n用户群： 4942" +
    "45573";
            // 
            // labelDev
            // 
            this.labelDev.AutoSize = true;
            this.labelDev.Location = new System.Drawing.Point(31, 241);
            this.labelDev.Name = "labelDev";
            this.labelDev.Size = new System.Drawing.Size(224, 17);
            this.labelDev.TabIndex = 4;
            this.labelDev.Text = "对于 Mod 开发者，开发文档正在完善中";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(235, 298);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Licensed under the GNU General Public License v3.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(31, 268);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(131, 17);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://evemodx.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 337);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelDev);
            this.Controls.Add(this.labelP);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelH);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormAbout";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "关于";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelP;
        private System.Windows.Forms.Label labelDev;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}