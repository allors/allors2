namespace Tests
{
    using System;

    using Allors.Domain;

    using Xunit;

    [Collection("Server")]

    public class AllorsContextTests : ServerTest
    {
        [Fact]
        public async void Test()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var uri = new Uri("TestAllorsContext/UserName", UriKind.Relative);
            var response = await this.HttpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Administrator", content);
        }
    }
}