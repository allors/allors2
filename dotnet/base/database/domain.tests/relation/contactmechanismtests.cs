// <copyright file="ContactMechanismTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class ContactMechanismTests : DomainTest
    {
        [Fact]
        public void GivenTelecommunicationsNumber_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new TelecommunicationsNumberBuilder(this.Session);
            var contactMechanism = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithAreaCode("area");
            contactMechanism = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithContactNumber("number");
            contactMechanism = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
