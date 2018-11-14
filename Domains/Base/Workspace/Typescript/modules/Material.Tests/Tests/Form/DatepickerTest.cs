namespace Tests.Material
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Allors;
    using Allors.Domain;

    using Tests.Components;
    using Tests.Material.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class DatepickerTest : Test
    {
        private readonly FormPage page;

        public DatepickerTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToForm();
        }

        [Fact]
        public void Initial()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            var date = this.Session.Now();
            this.page.Date.Value = date;

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(before.Length + 1, after.Length);

            var data = after.Except(before).First();

            Assert.True(data.ExistDate);
        }
    }
}
