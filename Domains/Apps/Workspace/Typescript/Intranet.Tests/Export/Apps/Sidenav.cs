namespace Tests.Intranet
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Tests.Components;
    using Tests.Components.Html;
    using Tests.Intranet.CatalogueTests;
    using Tests.Intranet.PersonTests;
    using Tests.Intranet.ProductCategoryTest;
    using Tests.Intranet.ProductCharacteristicTest;
    using Tests.Intranet.ProductQuoteTest;
    using Tests.Intranet.ProductTest;
    using Tests.Intranet.ProductTypeTest;
    using Tests.Intranet.PurchaseInvoiceTest;
    using Tests.Intranet.RequestsForQuoteTest;
    using Tests.Intranet.SalesInvoicesOverviewTest;
    using Tests.Intranet.SalesOrderTest;
    using Tests.Intranet.WorkEffortOverviewTests;

    public class Sidenav : Page
    {
        public Sidenav(IWebDriver driver)
        : base(driver)
        {
            this.Selector = By.CssSelector("mat-sidenav");
        }

        public By Selector { get; }

        public Anchor Home => new Anchor(this.Driver, this.ByHref("/"));

        public Element ContactsGroup => this.Group("Contacts");

        public Anchor People => this.Link("/contacts/people");
        
        public Anchor Organisations => this.Link("/contacts/organisations");

        public Anchor CommunicationEvents => this.Link("/contacts/communicationevents");

        public Element SalesGroup => this.Group("Sales");

        public Anchor RequestsForQuote => this.Link("/sales/requestsforquote");

        public Anchor ProductQuotes => this.Link("/sales/productquotes");

        public Anchor SalesOrders => this.Link("/sales/salesorders");

        public Element ProductsGroup => this.Group("Products");

        public Anchor Goods => this.Link("/products/goods");

        public Anchor Parts => this.Link("/products/parts");

        public Anchor Catalogues => this.Link("/products/catalogues");

        public Anchor Categories => this.Link("/products/categories");

        public Anchor ProductCharacteristics => this.Link("/products/productcharacteristics");

        public Anchor ProductTypes => this.Link("/products/producttypes");

        public Element AccountingGroup => this.Group("Accounting");

        public Anchor PurchaseInvoices => this.Link("/accounting/purchaseinvoices");

        public Anchor SalesInvoices => this.Link("/accounting/salesinvoices");

        public Element WorkEffortsGroup => this.Group("Work Efforts");

        public Anchor WorkEfforts => this.Link("/workefforts/workefforts");

        public Button Toggle => new Button(this.Driver, By.CssSelector("button[aria-label='Toggle sidenav']"));

        public DashboardPage NavigateToHome()
        {
            this.Home.Click();

            return new DashboardPage(this.Driver);
        }

        public PersonListPage NavigateToPersonList()
        {
            this.Navigate(this.ContactsGroup, this.People);
            return new PersonListPage(this.Driver);
        }

        public RequestForQuoteListPage NavigateToRequestForQuoteList()
        {
            this.Navigate(this.SalesGroup, this.RequestsForQuote);
            return new RequestForQuoteListPage(this.Driver);
        }

        public ProductQuoteListPage NavigateToProductQuoteList()
        {
            this.Navigate(this.SalesGroup, this.ProductQuotes);
            return new ProductQuoteListPage(this.Driver);
        }

        public SalesOrderListPage NavigateToSalesOrderList()
        {
            this.Navigate(this.SalesGroup, this.SalesOrders);
            return new SalesOrderListPage(this.Driver);
        }

        public CatalogueListPage NavigateToCatalogueList()
        {
            this.Navigate(this.ProductsGroup, this.Catalogues);
            return new CatalogueListPage(this.Driver);
        }

        public ProductCategorieListPage NavigateToCategoryList()
        {
            this.Navigate(this.ProductsGroup, this.Categories);
            return new ProductCategorieListPage(this.Driver);
        }

        public ProductListPage NavigateToProductList()
        {
            this.Navigate(this.ProductsGroup, this.Goods);
            return new ProductListPage(this.Driver);
        }

        public ProductCharacteristicListPage NavigateToProductCharacteristicList()
        {
            this.Navigate(this.ProductsGroup, this.ProductCharacteristics);
            return new ProductCharacteristicListPage(this.Driver);
        }

        public ProductTypeListPage NavigateToProductTypeList()
        {
            this.Navigate(this.ProductsGroup, this.ProductTypes);
            return new ProductTypeListPage(this.Driver);
        }

        public PurchaseInvoiceListPage NavigateToPurchaseInvoiceList()
        {
            this.Navigate(this.AccountingGroup, this.PurchaseInvoices);
            return new PurchaseInvoiceListPage(this.Driver);
        }

        public SalesInvoiceListPage NavigateToSalesInvoiceList()
        {
            this.Navigate(this.AccountingGroup, this.SalesInvoices);
            return new SalesInvoiceListPage(this.Driver);
        }

        public WorkEffortListPage NavigateToWorkEffortList()
        {
            this.Navigate(this.WorkEffortsGroup, this.WorkEfforts);
            return new WorkEffortListPage(this.Driver);
        }

        private void Navigate(Element group, Anchor link)
        {
            this.Driver.WaitForAngular();

            if (!link.IsVisble)
            {
                if (!group.IsVisible)
                {
                    this.Toggle.Click();
                    this.Driver.WaitForCondition(driver => @group.IsVisible);
                }
               
                group.Click();
            }

            link.Click();
        }

        private Element Group(string name)
        {
            return new Element(this.Driver, new ByChained(this.Selector, By.XPath($"//span[contains(text(), '{name}')]")));
        }

        private Anchor Link(string href)
        {
            return new Anchor(this.Driver, this.ByHref(href));
        }

        private By ByHref(string href)
        {
            return new ByChained(this.Selector, By.CssSelector($"a[href='{href}']"));
        }
    }
}
