namespace Tests.Material
{
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Tests.Components;
    using Tests.Material.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class SliderTest : Test
    {
        private readonly FormPage page;

        public SliderTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToForm();
        }
        
        [Fact]
        public void Initial()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Slider.Select(1, 20, 10);

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Equal(10, data.Slider);
        }
    }
}
