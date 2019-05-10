// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuilderTest.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
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
