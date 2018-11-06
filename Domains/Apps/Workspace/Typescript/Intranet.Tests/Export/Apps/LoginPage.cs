namespace Tests.Intranet
{
    using OpenQA.Selenium;

    using Tests.Components;
    using Tests.Components.Html;

    public class LoginPage : Page
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public Input UserName => new Input(this.Driver, formControlName: "userName");

        public Button Button => new Button(this.Driver, By.CssSelector("button"));

        public DashboardPage Login(string userName = "administrator")
        {
            this.UserName.Text = userName;
            this.Button.Click();

            this.Driver.WaitForAngular();

            return new DashboardPage(this.Driver);
        }
    }
}
