namespace Tests.Material
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Material.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class SlideToggleTest : Test
    {
        private readonly FormPage page;

        public SlideToggleTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToForm();
        }

        [Fact]
        public void Initial()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.SlideToggle.Value = true;

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.True(data.SlideToggle);
        }
    }
}
