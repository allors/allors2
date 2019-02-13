namespace Pages.ApplicationTests
{
    using Angular;
    using Angular.Html;

    using OpenQA.Selenium;

    public class LoginPage : Page
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public Input<LoginPage> UserName => this.Input(formControlName: "userName");

        public Button<LoginPage> Button => this.Button(By.CssSelector("button"));

        public DashboardPage Login(string userName = "administrator")
        {
            this.UserName.Set(userName);
            this.Button.Click();

            this.Driver.WaitForAngular();

            return new DashboardPage(this.Driver);
        }
    }
}
