// <copyright file="AutoCompleteFilterTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Components;
    using libs.angular.material.custom.src.tests.form;
    using Xunit;

    [Collection("Test collection")]
    public class AutoCompleteDerivedFilterTest : Test
    {
        private readonly FormComponent page;

        private readonly Person john;
        private readonly Person jane;

        public AutoCompleteDerivedFilterTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();

            this.john = (Person)new Users(this.Session).GetUser("john@example.com");
            this.jane = (Person)new Users(this.Session).GetUser("jane@example.com");

            var singleton = this.Session.GetSingleton();
            singleton.AutocompleteDefault = john;

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void Empty()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Null(data.AutocompleteAssignedFilter);
            Assert.Equal(this.john, data.AutocompleteDerivedFilter);
        }

        [Fact]
        public void Full()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.AutocompleteDerivedFilter.Select("jane@example.com");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Equal(this.jane, data.AutocompleteAssignedFilter);
            Assert.Equal(this.jane, data.AutocompleteDerivedFilter);
        }
        
        [Fact]
        public void PartialWithSelection()
        {
            var jane = new Users(this.Session).GetUser("jane@example.com");
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.AutocompleteDerivedFilter.Select("jane", "jane@example.com");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Equal(this.jane, data.AutocompleteAssignedFilter);
            Assert.Equal(this.jane, data.AutocompleteDerivedFilter);
        }
    }
}
