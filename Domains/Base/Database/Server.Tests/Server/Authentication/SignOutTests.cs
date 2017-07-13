namespace Tests
{
    using System;
    using Allors.Domain;
    using Allors.Server;

    using Microsoft.AspNetCore.Identity;

    using Xunit;

    [Collection("Server")]
    public class SignOutTests : ServerTest
    {
        public SignOutTests()
        {
            var passwordHasher = new PasswordHasher<string>();
            var hash = passwordHasher.HashPassword("Jane", "p@ssw0rd");
            new PersonBuilder(this.Session).WithUserName("Jane").WithUserPasswordHash(hash).Build();
            this.Session.Commit();
        }
                
        [Fact]
        public async void Cookies()
        {
            var args = new SignInRequest
                            {
                                UserName = "Jane",
                                Password = "p@ssw0rd"
                            };

            var signInUri = new Uri("Authentication/SignIn", UriKind.Relative);
            await this.PostAsJsonAsync(signInUri, args);

            var signOutUri = new Uri("Authentication/SignOut", UriKind.Relative);
            await this.PostAsJsonAsync(signOutUri, null);

            var rootUri = new Uri(ServerFixture.Url);
            var cookies = this.HttpClientHandler.CookieContainer.GetCookies(rootUri);
            var cookie = cookies[".AspNetCore.Allors"];

            Assert.Null(cookie);
        }
    }
}