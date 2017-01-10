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

namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class PersonTests : DomainTest
    {
        [Test]
        public void LastNameIsRequired()
        {
            var rainer = new PersonBuilder(this.Session).WithFirstName("Rainer").Build();
            var log = this.Session.Derive();

            log.Errors.Length.ShouldEqual(1);
            var error = log.Errors[0];
            error.RoleTypes[0].ShouldEqual(M.Person.LastName);
        }


        [Test]
        public void Fullname()
        {
            var john = new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();
            this.Session.Derive();

            john.FullName.ShouldEqual("John Doe");
        }

    }
}
