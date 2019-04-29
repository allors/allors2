namespace Pages.ApplicationTests
{
    using Angular;
    using Angular.Html;

    using OpenQA.Selenium;

    public class LoginPage : Component
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public Button<LoginPage> Button => this.Button(By.CssSelector("button"));

        public Input<LoginPage> UserName => this.Input(formControlName: "userName");

        public DashboardPage Login(string userName = "administrator")
        {
            this.UserName.Set(userName);
            this.Button.Click();

            this.Driver.WaitForAngular();

            return new DashboardPage(this.Driver);
        }
    }
}