// <copyright file="RadioGroupTest.cs" company="Allors bvba">
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
    public class RadioGroupTest : Test
    {
        private readonly FormComponent page;

        public RadioGroupTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        public void Initial()
        {
            this.Driver.WaitForAngular();

            var before = new Datas(this.Session).Extent().ToArray();

            this.page.RadioGroup.Select("one");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(before.Length + 1, after.Length);

            var data = after.Except(before).First();

            Assert.Equal("one", data.RadioGroup);
        }

        [Fact]
        public void Set()
        {
            this.Driver.WaitForAngular();

            var before = new Datas(this.Session).Extent().ToArray();

            this.page.RadioGroup.Select("two");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(before.Length + 1, after.Length);

            var data = after.Except(before).First();

            Assert.Equal("two", data.RadioGroup);
        }
    }
}
