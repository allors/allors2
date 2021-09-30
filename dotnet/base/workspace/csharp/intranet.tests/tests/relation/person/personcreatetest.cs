// <copyright file="PersonCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PersonTests
{
    using System.Linq;
    using Allors.Domain;
    using Components;
    using src.allors.material.@base.objects.person.list;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PersonCreateTest : Test
    {
        private readonly PersonListComponent people;

        public PersonCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new People(this.Session).Extent().ToArray();

            var personCreate = this.people.CreatePerson();

            personCreate
                .Salutation.Select(new Salutations(this.Session).Mr)
                .FirstName.Set("Jos")
                .MiddleName.Set("de")
                .LastName.Set("Smos")
                .Function.Set("CEO")
                .Gender.Select(new GenderTypes(this.Session).Male)
                .Locale.Select(this.Session.GetSingleton().AdditionalLocales.First)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var person = after.Except(before).First();

            Assert.Equal(new Salutations(this.Session).Mr, person.Salutation);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("de", person.MiddleName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("CEO", person.Function);
            Assert.Equal(new GenderTypes(this.Session).Male, person.Gender);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First, person.Locale);
        }
    }
}
