// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;

    using global::Allors.Domain;

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
