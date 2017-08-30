namespace Allors.Server
{
    public class SignInResponse
    {
        public bool Authenticated { get; set; }

        public string Token { get; set; }
    }
}