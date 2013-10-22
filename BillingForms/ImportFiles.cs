using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace BillingForms
{
    public partial class ImportFiles : DevExpress.XtraEditors.XtraForm
    {
        public ImportFiles()
        {
            InitializeComponent();
            run = false;
            //this.FormClosed += (this.Parent as Ribbon2007Style).ImportRun(this);
        }

        private bool run { get; set; }

        public string GetFolderRoot()
        {
            return buttonEdit1.Text;
        }

        public bool GetCheck1()
        {
            return checkEdit1.Checked;
        }

        public bool GetCheck2()
        {
            return checkEdit2.Checked;
        }

        public bool GetCheck3()
        {
            return checkEdit3.Checked;
        }

        public bool GetCheck4()
        {
            return checkEdit4.Checked;
        }

        public string GetMaskFiles()
        {
            return textBox1.Text;
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            //folderDialog.FileName = string.Empty;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                buttonEdit1.Text = folderDialog.SelectedPath;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            run = true;
            this.Close();
        }

        private void ImportFiles_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!run) return;
            var ribbon2007Style = this.Parent as Ribbon2007Style;
            if (ribbon2007Style != null) ribbon2007Style.ImportRun(this);
        }
    }
}