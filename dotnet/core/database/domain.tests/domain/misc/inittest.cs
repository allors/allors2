// <copyright file="InitTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors;

    using Allors.Domain;

    using Xunit;

    public class InitTest : DomainTest
    {
        [Fact]
        public void Init()
        {
            var allors = new OrganisationBuilder(this.Session).WithName("Allors").Build();
            var acme = new OrganisationBuilder(this.Session).WithName("Acme").Build();
            var person = new PersonBuilder(this.Session).WithLastName("Hesius").Build();

            allors.Manager = person;

            var derivation = this.Session.Derive();

            Assert.Contains(person, allors.Employees);
            Assert.DoesNotContain(person, acme.Employees);

            allors.RemoveManager();
            acme.Manager = person;

            derivation = this.Session.Derive();

            Assert.Contains(person, allors.Employees);
            Assert.DoesNotContain(person, acme.Employees);
        }
    }
}
