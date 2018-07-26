namespace Intranet.Tests
{
    using System;
    using Intranet.Pages;
    using Intranet.Pages.Relations;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

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

        public Anchor People => this.Link("/relations/people");
        
        public Anchor Organisations => this.Link("/relations/organisations");

        public Element TestsGroup => this.Group("Tests");

        public Anchor Form => this.Link("/tests/form");


        public Button Toggle => new Button(this.Driver, By.CssSelector("a-mat-sidenavtoggle button"));

        public DashboardPage NavigateToHome()
        {
            this.Driver.WaitForAngular();
            this.Home.Click();

            return new DashboardPage(this.Driver);
        }

        public PeopleOverviewPage NavigateToPeople()
        {
            this.Navigate(this.RelationsGroup, this.People);
            return new PeopleOverviewPage(this.Driver);
        }

        public OrganisationsOverviewPage NavigateToOrganisations()
        {
            this.Navigate(this.RelationsGroup, this.Organisations);
            return new OrganisationsOverviewPage(this.Driver);
        }

        public FormPage NavigateToForm()
        {
            this.Navigate(this.TestsGroup, this.Form);
            return new FormPage(this.Driver);
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
