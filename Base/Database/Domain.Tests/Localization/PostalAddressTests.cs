// <copyright file="PostalAddressTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class PostalAddressGeographicBoundaryTests : DomainTest
    {
        [Fact]
        public void GivenGeographicBoundary_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var country = new Countries(this.Session).CountryByIsoCode["BE"];

            new PostalAddressBuilder(this.Session).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithAddress1("address1").Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(country).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithAddress1("address1").WithPostalAddressBoundary(country).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
