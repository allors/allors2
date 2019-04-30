namespace src.app.auth
{
    using Angular;
    using src.app.dashboard;

    public partial class LoginComponent
    {
        public DashboardComponent Login(string userName = "administrator")
        {
            this.UserName.Value = userName;
            this.SignIn.Click();

            this.Driver.WaitForAngular();

            return new DashboardComponent(this.Driver);
        }
    }
}