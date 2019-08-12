namespace src.app.auth
{
    using Components;

    public partial class LoginComponent 
    {
        public void Login(string userName = "administrator")
        {
            this.Username.Value = userName;
            this.SignIn.Click();

            this.Driver.WaitForAngular();
        }
    }
}