namespace src.app.auth
{
    using OpenQA.Selenium;

    using Angular;
    using Angular.Html;
    using Pages;

    public partial class LoginComponent
    {
        public Button Button => new Button(this.Driver, By.CssSelector("button"));

        public HomePage Login(string userName = "administrator")
        {
            this.UserName.Value = userName;
            this.Button.Click();

            this.Driver.WaitForAngular();

            return new HomePage(this.Driver);
        }
    }
}