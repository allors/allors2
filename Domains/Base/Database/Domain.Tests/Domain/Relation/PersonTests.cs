// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    
    public class PersonTests : DomainTest
    {
        [Fact]
        public void LastNameIsRequired()
        {
            var rainer = new PersonBuilder(this.Session).WithFirstName("Rainer").Build();
            var log = this.Session.Derive(false);

            Assert.Equal(log.Errors.Length, 1);
            var error = log.Errors[0];
            Assert.Equal(error.RoleTypes[0], M.Person.LastName);
        }


        [Fact]
        public void Fullname()
        {
            var john = new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();
            this.Session.Derive();

            Assert.Equal(john.FullName, "John Doe");
        }
    }
}
