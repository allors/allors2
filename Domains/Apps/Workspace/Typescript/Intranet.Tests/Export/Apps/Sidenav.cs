namespace Tests
{
    using Allors.Meta;

    using Angular;
    using Angular.Html;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Pages.ApplicationTests;
    using Pages.CatalogueTests;
    using Pages.CommunicationEventTests;
    using Pages.OrganisationTests;
    using Pages.PersonTests;
    using Pages.ProductCategoryTest;
    using Pages.ProductQuoteTest;
    using Pages.ProductTest;
    using Pages.ProductTypeTest;
    using Pages.PurchaseInvoiceTest;
    using Pages.RequestsForQuoteTest;
    using Pages.SalesInvoicesOverviewTest;
    using Pages.SalesOrderTest;
    using Pages.SerialisedItemCharacteristicTest;
    using Pages.WorkEffortOverviewTests;

    public class Sidenav : Component
    {
        public Sidenav(IWebDriver driver)
        : base(driver)
        {
            this.Selector = By.CssSelector("mat-sidenav");
        }

        public Element AccountingGroup => this.Group("Accounting");

        public Anchor Catalogues => this.Link(M.Catalogue.ObjectType);

        public Anchor Categories => this.Link(M.ProductCategory.ObjectType);

        public Anchor CommunicationEvents => this.Link(M.CommunicationEvent.ObjectType);

        public Element ContactsGroup => this.Group("Contacts");

        public Anchor Goods => this.Link(M.Good.ObjectType);

        public Anchor Home => this.Anchor(this.ByHref("/"));

        public Anchor Organisations => this.Link(M.Organisation.ObjectType);

        public Anchor Parts => this.Link(M.Part.ObjectType);

        public Anchor People => this.Link(M.Person.ObjectType);

        public Anchor ProductCharacteristics => this.Link(M.SerialisedItemCharacteristic.ObjectType);

        public Anchor ProductQuotes => this.Link(M.ProductQuote.ObjectType);

        public Element ProductsGroup => this.Group("Products");

        public Anchor ProductTypes => this.Link(M.ProductType.ObjectType);

        public Anchor PurchaseInvoices => this.Link(M.PurchaseInvoice.ObjectType);

        public Element PurchasingGroup => this.Group("Purchasing");

        public Anchor RequestsForQuote => this.Link(M.RequestForQuote.ObjectType);

        public Element SalesGroup => this.Group("Sales");

        public Anchor SalesInvoices => this.Link(M.SalesInvoice);

        public Anchor SalesOrders => this.Link(M.SalesOrder.ObjectType);

        public By Selector { get; }

        public Button Toggle => this.Button(By.CssSelector("button[aria-label='Toggle sidenav']"));

        public Anchor WorkEfforts => this.Link(M.WorkEffort.ObjectType);

        public Element WorkEffortsGroup => this.Group("WorkEfforts");

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

        public CommunicationEventListPage NavigateToCommunicationEventList()
        {
            this.Navigate(this.ContactsGroup, this.CommunicationEvents);
            return new CommunicationEventListPage(this.Driver);
        }

        public GoodListPage NavigateToGoodList()
        {
            this.Navigate(this.ProductsGroup, this.Goods);
            return new GoodListPage(this.Driver);
        }

        public DashboardPage NavigateToHome()
        {
            this.Home.Click();

            return new DashboardPage(this.Driver);
        }

        public OrganisationListPage NavigateToOrganisationList()
        {
            this.Navigate(this.ContactsGroup, this.Organisations);
            return new OrganisationListPage(this.Driver);
        }

        public PersonListPage NavigateToPersonList()
        {
            this.Navigate(this.ContactsGroup, this.People);
            return new PersonListPage(this.Driver);
        }

        public SerialisedItemCharacteristicListPage NavigateToProductCharacteristicList()
        {
            this.Navigate(this.ProductsGroup, this.ProductCharacteristics);
            return new SerialisedItemCharacteristicListPage(this.Driver);
        }

        public ProductQuoteListPage NavigateToProductQuoteList()
        {
            this.Navigate(this.SalesGroup, this.ProductQuotes);
            return new ProductQuoteListPage(this.Driver);
        }

        public ProductTypeListPage NavigateToProductTypeList()
        {
            this.Navigate(this.ProductsGroup, this.ProductTypes);
            return new ProductTypeListPage(this.Driver);
        }

        public PurchaseInvoiceListPage NavigateToPurchaseInvoiceList()
        {
            this.Navigate(this.PurchasingGroup, this.PurchaseInvoices);
            return new PurchaseInvoiceListPage(this.Driver);
        }

        public RequestForQuoteListPage NavigateToRequestForQuoteList()
        {
            this.Navigate(this.SalesGroup, this.RequestsForQuote);
            return new RequestForQuoteListPage(this.Driver);
        }

        public SalesInvoiceListPage NavigateToSalesInvoiceList()
        {
            this.Navigate(this.SalesGroup, this.SalesInvoices);
            return new SalesInvoiceListPage(this.Driver);
        }

        public SalesOrderListPage NavigateToSalesOrderList()
        {
            this.Navigate(this.SalesGroup, this.SalesOrders);
            return new SalesOrderListPage(this.Driver);
        }

        public WorkEffortListPage NavigateToWorkEffortList()
        {
            this.Navigate(this.WorkEffortsGroup, this.WorkEfforts);
            return new WorkEffortListPage(this.Driver);
        }

        private By ByHref(string href)
        {
            return new ByChained(this.Selector, By.CssSelector($"a[href='{href}']"));
        }

        private By ByObjectType(ObjectType objectType)
        {
            return new ByChained(this.Selector, By.CssSelector($"a[data-allors-id='{objectType.IdAsNumberString}']"));
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

        private void Navigate(Element group, Anchor link)
        {
            this.Driver.WaitForAngular();

            if (!link.IsVisible)
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
    }
}