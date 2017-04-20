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

    public class SignInTests : ServerTest
    {
        public SignInTests()
        {
            var passwordHasher = new PasswordHasher<string>();

            new PersonBuilder(this.Session).WithUserName("John").Build();

            var hash = passwordHasher.HashPassword("Jane", "p@ssw0rd");
            new PersonBuilder(this.Session).WithUserName("Jane").WithUserPasswordHash(hash).Build();
            this.Session.Commit();
        }

        [Fact]
        public void CorrectUserAndPassword()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var args = new SignInRequest
                                       {
                                           UserName = "Jane",
                                           Password = "p@ssw0rd"
                                       };

                        var uri = new Uri("Authentication/SignIn", UriKind.Relative);
                        var response = await this.PostAsJsonAsync(uri, args);

                        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    });
        }


        [Fact]
        public void NonExistingUser()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var args = new SignInRequest
                                       {
                                           UserName = "Jeff",
                                           Password = "p@ssw0rd"
                                       };

                        var uri = new Uri("Authentication/SignIn", UriKind.Relative);
                        var response = await this.PostAsJsonAsync(uri, args);

                        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                    });
        }

        [Fact]
        public void EmptyStringPassword()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var args = new SignInRequest
                                       {
                                           UserName = "John",
                                           Password = ""
                                       };

                        var uri = new Uri("Authentication/SignIn", UriKind.Relative);
                        var response = await this.PostAsJsonAsync(uri, args);

                        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                    });
        }

        [Fact]
        public void NoPassword()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var args = new SignInRequest
                                       {
                                           UserName = "John"
                                       };

                        var uri = new Uri("Authentication/SignIn", UriKind.Relative);
                        var response = await this.PostAsJsonAsync(uri, args);

                        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                    });
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

                        var uri = new Uri("Authentication/SignIn", UriKind.Relative);
                        await this.PostAsJsonAsync(uri, args);

                        var rootUri = new Uri(ServerFixture.Url);
                        var cookies = this.HttpClientHandler.CookieContainer.GetCookies(rootUri);
                        var cookie = cookies[".AspNetCore.Allors"];

                        Assert.NotNull(cookie);
                    });
        }
    }
}