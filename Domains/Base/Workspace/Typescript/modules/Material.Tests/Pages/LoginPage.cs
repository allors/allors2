namespace Pages
{
    using OpenQA.Selenium;

    using Angular;
    using Angular.Html;

    public class LoginPage : Page
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public Input UserName => new Input(this.Driver, formControlName: "userName");

        public Button Button => new Button(this.Driver, By.CssSelector("button"));

        public DashboardPage Login(string userName = "administrator")
        {
            this.UserName.Value = userName;
            this.Button.Click();

            this.Driver.WaitForAngular();

            return new DashboardPage(this.Driver);
        }
    }
}
