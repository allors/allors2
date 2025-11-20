// <copyright file="PreparedExtentTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Collections.Generic;
    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    [Collection("Api")]
    public class PreparedExtentTests : DomainTest
    {
        [Fact]
        public async void WithParameter()
        {
            var organisations = new Organisations(this.Session).Extent().ToArray();

            var extentService = this.Session.ServiceProvider.GetRequiredService<IExtentService>();
            var organizationByName = extentService.Get(PreparedExtents.ByName);

            var arguments = new Dictionary<string, string>
            {
                { "name", "Acme" },
            };

            Extent<Organisation> organizations = organizationByName.Build(this.Session, arguments).ToArray();

            Assert.Single(organizations);

            var organization = organizations[0];

            Assert.Equal("Acme", organization.Name);
        }
    }
}
