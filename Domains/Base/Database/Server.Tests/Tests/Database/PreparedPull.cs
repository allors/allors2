namespace Tests
{
    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    [Collection("Server")]
    public class PreparedPullTests : ServerTest
    {
        [Fact]
        public async void WithParameter()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            this.Session.Commit();

            var pullService = this.Session.ServiceProvider.GetRequiredService<IPullService>();
            var organizationByName = pullService.Get(Organisations.PullByName);

            organizationByName.Arguments = new Arguments(new { name = "Acme" });

            Extent<Organisation> organizations = organizationByName.Extent.Build(this.Session, organizationByName.Arguments).ToArray();

            Assert.Single(organizations);

            var organization = organizations[0];

            Assert.Equal("Acme", organization.Name);
        }
    }
}