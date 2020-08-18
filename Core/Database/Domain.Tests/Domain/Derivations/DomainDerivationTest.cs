// <copyright file="DerivationNodesTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Xunit;

    public class DomainDerivationTest : DomainTest
    {
        [Fact]
        public void Derive()
        {
            this.Session.Database.CoreRegisterDerivations();

            var person = new PersonBuilder(this.Session)
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            this.Session.Derive();

            Assert.Equal("Jane Doe", person.DomainFullName);
            Assert.Equal("Hello Jane Doe!", person.DomainGreeting);

        }
    }
}
