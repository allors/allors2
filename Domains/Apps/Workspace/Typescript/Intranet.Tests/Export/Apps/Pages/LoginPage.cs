namespace Pages.ApplicationTests
{
    using Components;
    using OpenQA.Selenium;

    public class LoginPage : Component
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public Button<LoginPage> Button => this.Button(By.CssSelector("button"));

        public Input<LoginPage> UserName => this.Input(formControlName: "userName");

        public void Login(string userName = "administrator")
        {
            this.UserName.Set(userName);
            this.Button.Click();

            this.Driver.WaitForAngular();
        }
    }
}