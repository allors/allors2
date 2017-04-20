namespace Allors.Server
{
    public class SignInRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}