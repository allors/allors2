namespace AppsExcelAddIn
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.appsTab = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.saveButton = this.Factory.CreateRibbonButton();
            this.refreshButton = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.peopleInitializeButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.appsTab.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // appsTab
            // 
            this.appsTab.Groups.Add(this.group1);
            this.appsTab.Groups.Add(this.group2);
            this.appsTab.Label = "Apps";
            this.appsTab.Name = "appsTab";
            // 
            // group1
            // 
            this.group1.Items.Add(this.saveButton);
            this.group1.Items.Add(this.refreshButton);
            this.group1.Label = "General";
            this.group1.Name = "group1";
            // 
            // saveButton
            // 
            this.saveButton.Label = "Save";
            this.saveButton.Name = "saveButton";
            this.saveButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.saveButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Label = "Refresh";
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.refreshButton_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.peopleInitializeButton);
            this.group2.Label = "Relations";
            this.group2.Name = "group2";
            // 
            // peopleInitializeButton
            // 
            this.peopleInitializeButton.Label = "Initialize People";
            this.peopleInitializeButton.Name = "peopleInitializeButton";
            this.peopleInitializeButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.peopleInitializeButton_Click);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Tabs.Add(this.appsTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.appsTab.ResumeLayout(false);
            this.appsTab.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        private Microsoft.Office.Tools.Ribbon.RibbonTab appsTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton saveButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton refreshButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton peopleInitializeButton;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
