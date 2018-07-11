namespace Intranet.Pages
{
    using System.Threading.Tasks;

    using Intranet.Tests;

    using OpenQA.Selenium;

    public class LoginPage : Page
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public Input UserName => new Input(this.Driver, By.CssSelector("input[formcontrolname='userName']"));

        public Button Button => new Button(this.Driver, By.CssSelector("button"));

        public void Login(string userName = "administrator")
        {
            this.UserName.Text = userName;
            this.Button.Click();

            this.Driver.WaitForAngular();
        }
    }
}
