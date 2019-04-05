namespace Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Angular;
    using Angular.Html;
    using Pages;
    using Pages.Relations;

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

        public Element TestsGroup => this.Group("Tests");

        public Anchor Form => this.Link("/tests/form");


        public Button Toggle => new Button(this.Driver, By.CssSelector("a-mat-sidenavtoggle button"));

        public DashboardPage NavigateToHome()
        {
            this.Driver.WaitForAngular();

            if (!this.Home.IsVisible)
            {
                this.Toggle.Click();
                this.Driver.WaitForCondition(driver => this.Home.IsVisible);
            }

            this.Home.Click();

            return new DashboardPage(this.Driver);
        }

        public PersonListPage NavigateToPersonList()
        {
            this.Navigate(this.ContactsGroup, this.People);
            return new PersonListPage(this.Driver);
        }

        public OrganisationListPage NavigateToOrganisations()
        {
            this.Navigate(this.ContactsGroup, this.Organisations);
            return new OrganisationListPage(this.Driver);
        }

        public FormPage NavigateToForm()
        {
            this.Navigate(this.TestsGroup, this.Form);
            return new FormPage(this.Driver);
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

                if (!link.IsVisible)
                {
                    group.Click();
                }
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
