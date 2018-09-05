namespace Server.Tests
{
    using System;

    using Allors.Domain;

    using Xunit;

    [Collection("Server")]
    public class UserServiceTests : ServerTest
    {
        [Fact]
        public async void SignedIn()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var uri = new Uri("TestSession/UserName", UriKind.Relative);
            var response = await this.HttpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Administrator", content);
        }

        [Fact]
        public async void SignedOut()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            this.SignOut();

            var uri = new Uri("TestSession/UserName", UriKind.Relative);
            var response = await this.HttpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Guest", content);
        }
    }
}