namespace src.app.auth
{
    using Angular;
    using Pages;

    public partial class LoginComponent
    {
        public HomePage Login(string userName = "administrator")
        {
            this.UserName.Value = userName;
            this.SignIn.Click();

            this.Driver.WaitForAngular();

            return new HomePage(this.Driver);
        }
    }
}