namespace Tests.Intranet
{
    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Tests.Components;
    using Tests.Components.Html;
    using Tests.Intranet.CatalogueTests;
    using Tests.Intranet.PersonTests;
    using Tests.Intranet.ProductCategoryTest;
    using Tests.Intranet.SerialisedItemCharacteristicTest;
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

        public Anchor People => this.Link(M.Person.ObjectType);
        
        public Anchor Organisations => this.Link(M.Organisation.ObjectType);

        public Anchor CommunicationEvents => this.Link(M.CommunicationEvent.ObjectType);

        public Element SalesGroup => this.Group("Sales");

        public Anchor RequestsForQuote => this.Link(M.RequestForQuote.ObjectType);

        public Anchor ProductQuotes => this.Link(M.ProductQuote.ObjectType);

        public Anchor SalesOrders => this.Link(M.SalesOrder.ObjectType);

        public Element ProductsGroup => this.Group("Products");

        public Anchor Goods => this.Link(M.Good.ObjectType);

        public Anchor Parts => this.Link(M.Part.ObjectType);

        public Anchor Catalogues => this.Link(M.Catalogue.ObjectType);

        public Anchor Categories => this.Link(M.ProductCategory.ObjectType);

        public Anchor ProductCharacteristics => this.Link(M.SerialisedItemCharacteristic.ObjectType);

        public Anchor ProductTypes => this.Link(M.ProductType.ObjectType);

        public Element AccountingGroup => this.Group("Accounting");

        public Anchor PurchaseInvoices => this.Link(M.PurchaseInvoice.ObjectType);

        public Anchor SalesInvoices => this.Link(M.SalesInvoice);

        public Element WorkEffortsGroup => this.Group("WorkEfforts");

        public Anchor WorkEfforts => this.Link(M.WorkEffort.ObjectType);

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

        public SerialisedItemCharacteristicListPage NavigateToProductCharacteristicList()
        {
            this.Navigate(this.ProductsGroup, this.ProductCharacteristics);
            return new SerialisedItemCharacteristicListPage(this.Driver);
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

        private Anchor Link(ObjectType objectType)
        {
            return new Anchor(this.Driver, this.ByObjectType(objectType));
        }

        private By ByHref(string href)
        {
            return new ByChained(this.Selector, By.CssSelector($"a[href='{href}']"));
        }

        private By ByObjectType(ObjectType objectType)
        {
            return new ByChained(this.Selector, By.CssSelector($"a[data-allors-id='{objectType.IdAsNumberString}']"));
        }
    }
}
