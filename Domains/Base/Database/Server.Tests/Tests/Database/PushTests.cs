namespace Tests
{
    using System;

    using Allors.Domain;
    using Allors.Server;

    using Xunit;

    [Collection("Server")]
    public class PushTests : ServerTest
    {
        [Fact]
        public async void OnBuild()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var uri = new Uri(@"Database/Push", UriKind.Relative);

            var pushRequest = new PushRequest { NewObjects = new[] { new PushRequestNewObject{ T = "Build", NI = "-1" }, } };
            var response = await this.PostAsJsonAsync(uri, pushRequest);
            var pushResponse = await this.ReadAsAsync<PushResponse>(response);

            this.Session.Rollback();

            var build = (Build)this.Session.Instantiate(pushResponse.NewObjects[0].I);

            Assert.Equal(new Guid("DCE649A4-7CF6-48FA-93E4-CDE222DA2A94"), build.Guid);
            Assert.Equal("Exist", build.String);
        }
    }
}