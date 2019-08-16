// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
