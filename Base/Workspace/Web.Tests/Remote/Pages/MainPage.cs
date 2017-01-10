namespace Tests.Remote.Pages
{
    using System;
    using System.Threading;

    using Allors;
    using Allors.Domain;

    using OpenQA.Selenium;

    using Protractor;
    
    public abstract class MainPage
    {
        private const int WaitAttempts = 10;

        protected MainPage(NgWebDriver driver)
        {
            this.Driver = driver;
        }

        #region Location

        public bool IsOrganisationPage => this.Driver.Url.Contains("#/relation/organisation");

        #endregion

        public MainPage Login(Person person)
        {
            var url = Test.LoginUrl + "?user=" + Uri.EscapeDataString(person.UserName);
            this.Driver.Navigate().GoToUrl(url);
            return this;
        }

        public bool SaveSuccessful
        {
            get
            {
                var toasterMessage = this.WaitForToaster();
                return toasterMessage.Equals("Successfully saved.");
            }
        }

        protected NgWebDriver Driver { get; }

        #region Links
        private NgWebElement HomeLink => this.Driver.FindElement(By.Id("nav-home"));

        private NgWebElement RelationLink => this.Driver.FindElement(By.Id("nav-relation"));

        private NgWebElement OrganisationLink => this.Driver.FindElement(By.Id("nav-relation-organisations"));

        #endregion

        #region Navigation

        public HomePage GoToHome()
        {
            this.HomeLink.Click();

            return new HomePage(this.Driver);
        }

        public OrganisationEditPage GoToOrganisations()
        {
            this.RelationLink.Click();

            this.WaitTillDisplayed(this.OrganisationLink);
            this.OrganisationLink.Click();

            return new OrganisationEditPage(this.Driver);
        }

        #endregion

        protected NgWebElement GetObjectElement(IObject selection)
        {
            return this.Driver.FindElement(By.XPath("//*[@data-test='" + selection.Id + "']"));
        }

        protected void ScrollToElement(NgWebElement element)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)this.Driver.WrappedDriver;
            var scrollToCommand = @"arguments[0].scrollIntoView(true);";
            javaScriptExecutor.ExecuteScript(scrollToCommand, element);

            var checkVisibleCommand = @" 
var rect = arguments[0].getBoundingClientRect();

    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) && /*or $(window).height() */
        rect.right <= (window.innerWidth || document.documentElement.clientWidth) /*or $(window).width() */
    );";
            int attempt = 0;
            var result = (bool)javaScriptExecutor.ExecuteScript(checkVisibleCommand, element);

            while (!result && ++attempt < WaitAttempts)
            {
                Thread.Sleep(attempt * 100);
                result = (bool)javaScriptExecutor.ExecuteScript(checkVisibleCommand, element);
            }

            if (!result)
            {
                throw new Exception("TinyMCE not available");
            }
        }

        protected void SetValueOnTinyMceActiveEditor(string newText)
        {
            int attempt = 0;

            var javaScriptExecutor = (IJavaScriptExecutor)this.Driver.WrappedDriver;
            var waitForTinyMCE = @"return tinyMCE !== undefined;";

            var result = (bool)javaScriptExecutor.ExecuteScript(waitForTinyMCE);
            while (!result && ++attempt < WaitAttempts)
            {
                Thread.Sleep(attempt * 100);
                result = (bool)javaScriptExecutor.ExecuteScript(waitForTinyMCE);
            }

            if (!result)
            {
                throw new Exception("TinyMCE not available");
            }

            var setText = @"tinyMCE.activeEditor.setContent('" + newText + @"');";
            javaScriptExecutor.ExecuteScript(setText);

        }

        protected void CloseTinyMce(NgWebElement tinyMce)
        {
            var parent = tinyMce.FindElement(By.XPath("../.."));
            var close = parent.FindElement(By.LinkText("Close"));
            close.Click();
        }

        private string WaitForToaster()
        {
            int attempt = 0;
            var element = this.Driver.FindElement(By.ClassName("toast-message"));
            while (element == null  && ++attempt < WaitAttempts)
            {
                Thread.Sleep(attempt * 100);
                element = this.Driver.FindElement(By.ClassName("toast-message"));
            }

            if (element == null)
            {
                throw new Exception("Toaster not displayed");
            }
            else
            {
                return element.Text;
            }
        }

        protected void WaitTillDisplayed(NgWebElement element)
        {
            int attempt = 0;
            while (!element.Displayed && ++attempt < WaitAttempts)
            {
                Thread.Sleep(attempt * 100);
            }

            if (!element.Displayed)
            {
                throw new Exception(element.TagName + " element not displayed");
            }

        }
    }
}