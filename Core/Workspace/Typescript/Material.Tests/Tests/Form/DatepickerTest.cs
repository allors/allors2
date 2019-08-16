// <copyright file="DatepickerTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using src.allors.material.custom.tests.form;

namespace Tests
{
    using System.Globalization;
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Components;


    using Xunit;

    [Collection("Test collection")]
    public class DatepickerTest : Test
    {
        private readonly FormComponent page;

        public DatepickerTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        public void Initial()
        {
            CultureInfo.CurrentCulture = new CultureInfo("nl-BE");

            var before = new Datas(this.Session).Extent().ToArray();

            var date = this.Session.Now();
            this.page.Date.Value = date;

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(before.Length + 1, after.Length);

            var data = after.Except(before).First();

            Assert.True(data.ExistDate);
        }
    }
}
