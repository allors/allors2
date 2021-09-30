// <copyright file="OrganisationContactKindTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Domain
{
    using Xunit;

    public class OrganisationContactKindTests : DomainTest
    {
        [Fact]
        public void GivenOrganisationContactKind_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrganisationContactKindBuilder(this.Session);
            var contactKind = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("contactkind");
            contactKind = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
