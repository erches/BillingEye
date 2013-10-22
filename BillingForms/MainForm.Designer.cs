using System.Threading;

namespace BillingForms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager2 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::BillingForms.SplashScreen1), true, true);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::BillingForms.WaitForm2), true, true);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.newDocumentButton = new DevExpress.XtraBars.BarButtonItem();
            this.openDocumentButton = new DevExpress.XtraBars.BarButtonItem();
            this.saveDocumentButton = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.closeVisibleDocument = new DevExpress.XtraBars.BarButtonItem();
            this.exitButton = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem7 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem5 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem6 = new DevExpress.XtraBars.BarSubItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.xrDesignMdiController1 = new DevExpress.XtraReports.UserDesigner.XRDesignMdiController(this.components);
            this.xrTabbedMdiManager1 = new DevExpress.XtraReports.UserDesigner.XRTabbedMdiManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.newDocumentButton,
            this.openDocumentButton,
            this.saveDocumentButton,
            this.closeVisibleDocument,
            this.barSubItem2,
            this.barSubItem3,
            this.barSubItem4,
            this.barSubItem5,
            this.barSubItem6,
            this.exitButton,
            this.barSubItem7,
            this.barButtonItem4,
            this.barButtonItem5});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 21;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.newDocumentButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.openDocumentButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.saveDocumentButton)});
            this.bar1.Text = "Tools";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Id = 3;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // newDocumentButton
            // 
            this.newDocumentButton.Caption = "Новый";
            this.newDocumentButton.Id = 4;
            this.newDocumentButton.ImageIndex = 0;
            this.newDocumentButton.Name = "newDocumentButton";
            this.newDocumentButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.newDocumentButton_ItemClick);
            // 
            // openDocumentButton
            // 
            this.openDocumentButton.Caption = "Открыть";
            this.openDocumentButton.Id = 5;
            this.openDocumentButton.ImageIndex = 2;
            this.openDocumentButton.Name = "openDocumentButton";
            this.openDocumentButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.openDocumentButton_ItemClick);
            // 
            // saveDocumentButton
            // 
            this.saveDocumentButton.Caption = "Сохранить";
            this.saveDocumentButton.Id = 7;
            this.saveDocumentButton.ImageIndex = 1;
            this.saveDocumentButton.Name = "saveDocumentButton";
            this.saveDocumentButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.saveDocumentButton_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem6)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Файл";
            this.barSubItem1.Id = 0;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.closeVisibleDocument),
            new DevExpress.XtraBars.LinkPersistInfo(this.exitButton, true)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Создать";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.ImageIndex = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.newDocumentButton_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Открыть";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.ImageIndex = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.openDocumentButton_ItemClick);
            // 
            // closeVisibleDocument
            // 
            this.closeVisibleDocument.Caption = "Закрыть";
            this.closeVisibleDocument.Id = 8;
            this.closeVisibleDocument.Name = "closeVisibleDocument";
            this.closeVisibleDocument.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.closeVisibleDocument_ItemClick);
            // 
            // exitButton
            // 
            this.exitButton.Caption = "Выход";
            this.exitButton.Id = 17;
            this.exitButton.Name = "exitButton";
            this.exitButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.exitButton_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Правка";
            this.barSubItem2.Id = 11;
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "Вид";
            this.barSubItem3.Id = 13;
            this.barSubItem3.Name = "barSubItem3";
            // 
            // barSubItem7
            // 
            this.barSubItem7.Caption = "Данные";
            this.barSubItem7.Id = 18;
            this.barSubItem7.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4)});
            this.barSubItem7.Name = "barSubItem7";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Объеденить активные документы";
            this.barButtonItem4.Id = 19;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barSubItem5
            // 
            this.barSubItem5.Caption = "Окно";
            this.barSubItem5.Id = 15;
            this.barSubItem5.Name = "barSubItem5";
            // 
            // barSubItem6
            // 
            this.barSubItem6.Caption = "Справка";
            this.barSubItem6.Id = 16;
            this.barSubItem6.Name = "barSubItem6";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(703, 53);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 599);
            this.barDockControlBottom.Size = new System.Drawing.Size(703, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 53);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 546);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(703, 53);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 546);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "new+.png");
            this.imageCollection1.Images.SetKeyName(1, "save1.png");
            this.imageCollection1.Images.SetKeyName(2, "open.png");
            // 
            // barSubItem4
            // 
            this.barSubItem4.Caption = "Данные";
            this.barSubItem4.Id = 14;
            this.barSubItem4.Name = "barSubItem4";
            // 
            // xrDesignMdiController1
            // 
            this.xrDesignMdiController1.Form = this;
            this.xrDesignMdiController1.XtraTabbedMdiManager = this.xrTabbedMdiManager1;
            // 
            // xrTabbedMdiManager1
            // 
            this.xrTabbedMdiManager1.MdiParent = this;
            this.xrTabbedMdiManager1.MenuManager = this.barManager1;
            this.xrTabbedMdiManager1.View = this.tabbedView1;
            this.xrTabbedMdiManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "Сохранить";
            this.barButtonItem5.Id = 20;
            this.barButtonItem5.ImageIndex = 1;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.saveDocumentButton_ItemClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 599);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "GSM Eye";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem newDocumentButton;
        private DevExpress.XtraBars.BarButtonItem openDocumentButton;
        private DevExpress.XtraReports.UserDesigner.XRDesignMdiController xrDesignMdiController1;
        private DevExpress.XtraReports.UserDesigner.XRTabbedMdiManager xrTabbedMdiManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.BarButtonItem saveDocumentButton;
        private DevExpress.XtraBars.BarButtonItem closeVisibleDocument;
        private DevExpress.XtraBars.BarButtonItem exitButton;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraBars.BarSubItem barSubItem5;
        private DevExpress.XtraBars.BarSubItem barSubItem6;


        //splash screen manager
        private DevExpress.XtraBars.BarSubItem barSubItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
    }
}