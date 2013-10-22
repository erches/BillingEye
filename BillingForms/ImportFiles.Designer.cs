namespace BillingForms
{
    partial class ImportFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ImportButton = new DevExpress.XtraEditors.SimpleButton();
            this.CancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit4 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(272, 24);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(75, 23);
            this.ImportButton.TabIndex = 0;
            this.ImportButton.Text = "Выполнить";
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(272, 53);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Отказ";
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(101, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Каталог с файлами:";
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(32, 27);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit1.Size = new System.Drawing.Size(196, 20);
            this.buttonEdit1.TabIndex = 3;
            this.buttonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit1_ButtonClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(32, 66);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(196, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "*.*";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(32, 51);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(76, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Маска файлов:";
            // 
            // checkEdit1
            // 
            this.checkEdit1.EditValue = true;
            this.checkEdit1.Location = new System.Drawing.Point(33, 111);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "приводить данные к единому виду";
            this.checkEdit1.Size = new System.Drawing.Size(204, 19);
            this.checkEdit1.TabIndex = 6;
            // 
            // checkEdit2
            // 
            this.checkEdit2.Location = new System.Drawing.Point(33, 136);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "выполнять обогащение";
            this.checkEdit2.Size = new System.Drawing.Size(149, 19);
            this.checkEdit2.TabIndex = 7;
            // 
            // checkEdit3
            // 
            this.checkEdit3.Location = new System.Drawing.Point(33, 164);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.Caption = "загружать исходные \"сырые\" строки";
            this.checkEdit3.Size = new System.Drawing.Size(222, 19);
            this.checkEdit3.TabIndex = 8;
            // 
            // checkEdit4
            // 
            this.checkEdit4.Location = new System.Drawing.Point(33, 189);
            this.checkEdit4.Name = "checkEdit4";
            this.checkEdit4.Properties.Caption = "строить индексы";
            this.checkEdit4.Size = new System.Drawing.Size(109, 19);
            this.checkEdit4.TabIndex = 9;
            // 
            // ImportFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 237);
            this.Controls.Add(this.checkEdit4);
            this.Controls.Add(this.checkEdit3);
            this.Controls.Add(this.checkEdit2);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonEdit1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ImportButton);
            this.MaximumSize = new System.Drawing.Size(379, 275);
            this.MinimumSize = new System.Drawing.Size(379, 275);
            this.Name = "ImportFiles";
            this.Text = "Импорт из файлов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportFiles_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton ImportButton;
        private DevExpress.XtraEditors.SimpleButton CancelButton;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private System.Windows.Forms.TextBox textBox1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraEditors.CheckEdit checkEdit3;
        private DevExpress.XtraEditors.CheckEdit checkEdit4;
    }
}