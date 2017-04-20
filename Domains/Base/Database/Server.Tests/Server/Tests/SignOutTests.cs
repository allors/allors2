namespace Tests.Remote
{
    using System;
    using System.Net;

    using Allors.Domain;
    using Allors.Server;

    using Domain;

    using Microsoft.AspNetCore.Identity;

    using Nito.AsyncEx;

    using Xunit;

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
        public void Cookies()
        {
            AsyncContext.Run(
                async () =>
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
                    });
        }
    }
}