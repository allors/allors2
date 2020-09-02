// <copyright file="ChipsTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;
    using Components;
    using libs.angular.material.custom.src.tests.form;
    using Xunit;

    [Collection("Test collection")]
    public class ChipsTest : Test
    {
        private readonly FormComponent page;

        public ChipsTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        public void AddOne()
        {
            var jane = new Users(this.Session).GetUser("jane@example.com");
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Chips.Add("jane", "jane@example.com");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Contains(jane, data.Chips);
        }

        [Fact]
        public void AddTwo()
        {
            var jane = new Users(this.Session).GetUser("jane@example.com");
            var john = new Users(this.Session).GetUser("john@example.com");
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Chips.Add("jane", "jane@example.com");

            this.page.Chips.Add("john", "john@example.com");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Contains(jane, data.Chips);
            Assert.Contains(john, data.Chips);
        }

        [Fact]
        public void RemoveOne()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Chips.Add("jane", "jane@example.com");

            this.page.SAVE.Click();

            this.page.Chips.Remove("jane@example.com");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Empty(data.Chips);
        }
    }
}
