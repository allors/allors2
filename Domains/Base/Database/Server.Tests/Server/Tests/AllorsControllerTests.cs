namespace Tests.Remote
{
    using System;

    using Allors.Domain;
    using Allors.Server;

    using Domain;

    using Microsoft.AspNetCore.Identity;

    using Nito.AsyncEx;

    using Xunit;

    public class AllorsControllerTests : ServerTest
    {
        public AllorsControllerTests()
        {
            var passwordHasher = new PasswordHasher<string>();
            var hash = passwordHasher.HashPassword("Jane", "p@ssw0rd");
            var jane = new PersonBuilder(this.Session).WithUserName("Jane").WithUserPasswordHash(hash).Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(jane);
            this.Session.Commit();
        }
                
        [Fact]
        public void Test()
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

                        var uri = new Uri("TestAllors/UserInfo", UriKind.Relative);
                        var response = await this.HttpClient.PostAsync(uri, null);
                        var content = await response.Content.ReadAsStringAsync();

                        Assert.Equal("Jane", content);
                    });
        }
    }
}