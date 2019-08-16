// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultDerivationLogTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;
    using Allors.Domain;

    using Xunit;

    public class DefaultDerivationLogTests : DomainTest
    {
        [Fact]
        public void DeletedUserinterfaceable()
        {
            var organisation = new OrganisationBuilder(this.Session).Build();

            var validation = this.Session.Derive(false);
            Assert.Equal(1, validation.Errors.Length);

            var error = validation.Errors[0];
            Assert.Equal("Organisation.Name is required", error.Message);
        }
    }
}
