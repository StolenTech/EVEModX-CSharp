using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EVEModX {
    public partial class FormAbout : Form {
        public FormAbout() {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e) {
            labelVersion.Text = labelVersion.Text + " " + FormMain.emxversion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://evemodx.com");
        }
    }
}
