// <copyright file="PreparedExtentTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests
{
    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    [Collection("Server")]
    public class PreparedExtentTests : DomainTest
    {
        [Fact]
        public async void WithParameter()
        {
            var extentService = this.Session.ServiceProvider.GetRequiredService<IExtentService>();
            var organizationByName = extentService.Get(Organisations.ExtentByName);

            var arguments = new Arguments(new { name = "Acme" });

            Extent<Organisation> organizations = organizationByName.Build(this.Session, arguments).ToArray();

            Assert.Single(organizations);

            var organization = organizations[0];

            Assert.Equal("Acme", organization.Name);
        }
    }
}
