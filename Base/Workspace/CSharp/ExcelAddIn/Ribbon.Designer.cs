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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.baseTab = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.saveButton = this.Factory.CreateRibbonButton();
            this.refreshButton = this.Factory.CreateRibbonButton();
            this.buttonLogoff = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.customerInitializeButton = this.Factory.CreateRibbonButton();
            this.SalesInvoicesOverdueButton = this.Factory.CreateRibbonButton();
            this.groupPurchases = this.Factory.CreateRibbonGroup();
            this.purchaseInvoicesInitializeButton = this.Factory.CreateRibbonButton();
            this.groupInventory = this.Factory.CreateRibbonGroup();
            this.InventoryItemsButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.baseTab.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.groupPurchases.SuspendLayout();
            this.groupInventory.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // baseTab
            // 
            this.baseTab.Groups.Add(this.group1);
            this.baseTab.Groups.Add(this.group2);
            this.baseTab.Groups.Add(this.groupPurchases);
            this.baseTab.Groups.Add(this.groupInventory);
            this.baseTab.Label = "Base";
            this.baseTab.Name = "baseTab";
            // 
            // group1
            // 
            this.group1.Items.Add(this.saveButton);
            this.group1.Items.Add(this.refreshButton);
            this.group1.Items.Add(this.buttonLogoff);
            this.group1.Label = "General";
            this.group1.Name = "group1";
            // 
            // saveButton
            // 
            this.saveButton.Label = "Save";
            this.saveButton.Name = "saveButton";
            this.saveButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SaveButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Label = "Refresh";
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.RefreshButton_Click);
            // 
            // buttonLogoff
            // 
            this.buttonLogoff.Label = "Logoff";
            this.buttonLogoff.Name = "buttonLogoff";
            this.buttonLogoff.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonLogoff_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.customerInitializeButton);
            this.group2.Items.Add(this.SalesInvoicesOverdueButton);
            this.group2.Label = "Sales";
            this.group2.Name = "group2";
            // 
            // customerInitializeButton
            // 
            this.customerInitializeButton.Label = "Customers";
            this.customerInitializeButton.Name = "customerInitializeButton";
            this.customerInitializeButton.OfficeImageId = "DataFormSource";
            this.customerInitializeButton.ShowImage = true;
            this.customerInitializeButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CustomerInitializeButton_Click);
            // 
            // SalesInvoicesOverdueButton
            // 
            this.SalesInvoicesOverdueButton.Label = "Customers Overdue";
            this.SalesInvoicesOverdueButton.Name = "SalesInvoicesOverdueButton";
            this.SalesInvoicesOverdueButton.OfficeImageId = "DataFormSource";
            this.SalesInvoicesOverdueButton.ShowImage = true;
            this.SalesInvoicesOverdueButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SalesInvoicesOverdueButton_Click);
            // 
            // groupPurchases
            // 
            this.groupPurchases.Items.Add(this.purchaseInvoicesInitializeButton);
            this.groupPurchases.Label = "Purchases";
            this.groupPurchases.Name = "groupPurchases";
            // 
            // purchaseInvoicesInitializeButton
            // 
            this.purchaseInvoicesInitializeButton.Label = "Invoices";
            this.purchaseInvoicesInitializeButton.Name = "purchaseInvoicesInitializeButton";
            this.purchaseInvoicesInitializeButton.OfficeImageId = "DataFormSource";
            this.purchaseInvoicesInitializeButton.ShowImage = true;
            this.purchaseInvoicesInitializeButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PurchaseInvoicesInitializeButton_Click);
            // 
            // groupInventory
            // 
            this.groupInventory.Items.Add(this.InventoryItemsButton);
            this.groupInventory.Label = "Inventory";
            this.groupInventory.Name = "groupInventory";
            // 
            // InventoryItemsButton
            // 
            this.InventoryItemsButton.Label = "";
            this.InventoryItemsButton.Name = "InventoryItemsButton";
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Tabs.Add(this.baseTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.baseTab.ResumeLayout(false);
            this.baseTab.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.groupPurchases.ResumeLayout(false);
            this.groupPurchases.PerformLayout();
            this.groupInventory.ResumeLayout(false);
            this.groupInventory.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        private Microsoft.Office.Tools.Ribbon.RibbonTab baseTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton saveButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton refreshButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupPurchases;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton purchaseInvoicesInitializeButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton customerInitializeButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupInventory;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton InventoryItemsButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SalesInvoicesOverdueButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonLogoff;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
