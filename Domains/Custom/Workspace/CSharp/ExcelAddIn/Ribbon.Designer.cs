namespace ExcelAddIn
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
            this.baseTab = this.Factory.CreateRibbonTab();
            this.generalGroup = this.Factory.CreateRibbonGroup();
            this.saveButton = this.Factory.CreateRibbonButton();
            this.refreshButton = this.Factory.CreateRibbonButton();
            this.peopleGroup = this.Factory.CreateRibbonGroup();
            this.peopleInitializeButton = this.Factory.CreateRibbonButton();
            this.baseTab.SuspendLayout();
            this.generalGroup.SuspendLayout();
            this.peopleGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseTab
            // 
            this.baseTab.Groups.Add(this.generalGroup);
            this.baseTab.Groups.Add(this.peopleGroup);
            this.baseTab.Label = "Base";
            this.baseTab.Name = "baseTab";
            // 
            // generalGroup
            // 
            this.generalGroup.Items.Add(this.saveButton);
            this.generalGroup.Items.Add(this.refreshButton);
            this.generalGroup.Label = "General";
            this.generalGroup.Name = "generalGroup";
            // 
            // saveButton
            // 
            this.saveButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.saveButton.Label = "Save";
            this.saveButton.Name = "saveButton";
            this.saveButton.OfficeImageId = "FileSave";
            this.saveButton.ShowImage = true;
            this.saveButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SaveButtonClick);
            // 
            // refreshButton
            // 
            this.refreshButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.refreshButton.Label = "Refresh";
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.OfficeImageId = "ImportMoreMenu";
            this.refreshButton.ShowImage = true;
            this.refreshButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.RefreshButtonClick);
            // 
            // peopleGroup
            // 
            this.peopleGroup.Items.Add(this.peopleInitializeButton);
            this.peopleGroup.Label = "People";
            this.peopleGroup.Name = "peopleGroup";
            // 
            // peopleInitializeButton
            // 
            this.peopleInitializeButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.peopleInitializeButton.Label = "Initialize";
            this.peopleInitializeButton.Name = "peopleInitializeButton";
            this.peopleInitializeButton.OfficeImageId = "TableInsertExcel";
            this.peopleInitializeButton.ShowImage = true;
            this.peopleInitializeButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PeopleInitializeButtonClick);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.baseTab);
            this.baseTab.ResumeLayout(false);
            this.baseTab.PerformLayout();
            this.generalGroup.ResumeLayout(false);
            this.generalGroup.PerformLayout();
            this.peopleGroup.ResumeLayout(false);
            this.peopleGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Office.Tools.Ribbon.RibbonTab baseTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup generalGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton saveButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton refreshButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup peopleGroup;
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
