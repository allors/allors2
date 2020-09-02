// <copyright file="FileTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Linq;
    using Allors.Domain;
    using Components;
    using libs.angular.material.custom.src.tests.form;
    using Xunit;

    [Collection("Test collection")]
    public class FileTest : Test
    {
        private readonly FormComponent page;

        public FileTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        public void Upload()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.File.Upload("logo.png");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.True(data.ExistFile);
        }

        [Fact]
        public void Remove()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.File.Upload("logo.png");

            this.page.SAVE.Click();

            this.page.File.Remove();

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.False(data.ExistFile);
        }
    }
}
