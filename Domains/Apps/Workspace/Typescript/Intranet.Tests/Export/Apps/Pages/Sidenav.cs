namespace Intranet.Tests
{
    using System;
    using System.Threading;

    using Intranet.Pages;
    using Intranet.Pages.Orders;
    using Intranet.Pages.Relations;
    using Intranet.Pages.WorkEfforts;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using OpenQA.Selenium.Support.UI;

    public class Sidenav : Page
    {
        public Sidenav(IWebDriver driver)
        : base(driver)
        {
            this.Selector = By.CssSelector("mat-sidenav");
        }

        public By Selector { get; }

        public Anchor Home => new Anchor(this.Driver, this.ByHref("/"));

        public Element RelationsGroup => this.Group("Relations");

        public Anchor PersonList => this.Link("/relations/people");
        
        public Anchor Organisations => this.Link("/relations/organisations");

        public Anchor Communications => this.Link("/relations/communicationevents");

        public Element OrdersGroup => this.Group("Orders");

        public Anchor Requests => this.Link("/orders/requests");

        public Anchor Quotes => this.Link("/orders/productQuotes");

        public Anchor Orders => this.Link("/orders/salesOrders");

        public Element CataloguesGroup => this.Group("Catalogues");

        public Anchor Catalogues => this.Link("/catalogues/catalogues");

        public Anchor Categories => this.Link("/catalogues/categories");

        public Anchor Products => this.Link("/catalogues/goods");

        public Anchor ProductCharacteristics => this.Link("/catalogues/productCharacteristics");

        public Anchor ProductTypes => this.Link("/catalogues/productTypes");

        public Element AccountsPayableGroup => this.Group("Accounts Payable");

        public Anchor AccountsPayableInvoices => this.Link("/accountspayable/invoices");

        public Element AccountsReceivableGroup => this.Group("Accounts Receivable");

        public Anchor AccountsReceivableInvoices => this.Link("/accountsreceivable/invoices");

        public Element WorkEffortsGroup => this.Group("Work Efforts");

        public Anchor Tasks => this.Link("/workefforts/worktasks");

        public Button Toggle => new Button(this.Driver, By.CssSelector("a-mat-sidenavtoggle button"));

        public DashboardPage NavigateToHome()
        {
            this.Home.Click();

            return new DashboardPage(this.Driver);
        }

        public PersonListPage NavigateToPersonList()
        {
            var button = new Button(this.Driver, By.CssSelector("button[aria-label='Toggle sidenav']"));
            button.Click();
            this.Driver.WaitForAngular();

            this.Navigate(this.RelationsGroup, this.PersonList);
            return new PersonListPage(this.Driver);
        }

        public OrganisationsOverviewPage NavigateToOrganisations()
        {
            this.Navigate(this.RelationsGroup, this.Organisations);
            return new OrganisationsOverviewPage(this.Driver);
        }

        public CommunicationsOverviewPage NavigateToCommunications()
        {
            this.Navigate(this.RelationsGroup, this.Communications);
            return new CommunicationsOverviewPage(this.Driver);
        }

        public RequestsOverviewPage NavigateToRequests()
        {
            this.Navigate(this.OrdersGroup, this.Requests);
            return new RequestsOverviewPage(this.Driver);
        }

        public QuotesOverviewPage NavigateToQuotes()
        {
            this.Navigate(this.OrdersGroup, this.Quotes);
            return new QuotesOverviewPage(this.Driver);
        }

        public OrdersOverviewPage NavigateToOrders()
        {
            this.Navigate(this.OrdersGroup, this.Orders);
            return new OrdersOverviewPage(this.Driver);
        }

        public CataloguesOverviewPage NavigateToCatalogues()
        {
            this.Navigate(this.CataloguesGroup, this.Catalogues);
            return new CataloguesOverviewPage(this.Driver);
        }

        public CategoriesOverviewPage NavigateToCategories()
        {
            this.Navigate(this.CataloguesGroup, this.Categories);
            return new CategoriesOverviewPage(this.Driver);
        }

        public ProductsOverviewPage NavigateToProducts()
        {
            this.Navigate(this.CataloguesGroup, this.Products);
            return new ProductsOverviewPage(this.Driver);
        }

        public ProductCharacteristicsOverviewPage NavigateToProductCharacteristics()
        {
            this.Navigate(this.CataloguesGroup, this.ProductCharacteristics);
            return new ProductCharacteristicsOverviewPage(this.Driver);
        }

        public ProductTypesOverviewPage NavigateToProductTypes()
        {
            this.Navigate(this.CataloguesGroup, this.ProductTypes);
            return new ProductTypesOverviewPage(this.Driver);
        }

        public Pages.AccountsPayable.InvoicesOverviewPage NavigateToAccountsPayableInvoices()
        {
            this.Navigate(this.AccountsPayableGroup, this.AccountsPayableInvoices);
            return new Pages.AccountsPayable.InvoicesOverviewPage(this.Driver);
        }

        public Pages.AccountsReceivable.InvoicesOverviewPage NavigateToAccountsReceivableInvoices()
        {
            this.Navigate(this.AccountsReceivableGroup, this.AccountsReceivableInvoices);
            return new Pages.AccountsReceivable.InvoicesOverviewPage(this.Driver);
        }

        public TasksOverviewPage NavigateToTasks()
        {
            this.Navigate(this.WorkEffortsGroup, this.Tasks);
            return new TasksOverviewPage(this.Driver);
        }

        private void Navigate(Element group, Anchor link)
        {
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
