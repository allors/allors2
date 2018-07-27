namespace Intranet.Tests
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Intranet.Pages;
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class FileTest : Test
    {
        private readonly Sidenav sidenav;

        private readonly FormPage page;

        public FileTest(TestFixture fixture)
            : base(fixture)
        {
            this.sidenav = this.Login().Sidenav;
            this.page = this.sidenav.NavigateToForm();
        }

        [Fact]
        public void Upload()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.File.Upload("logo.png");

            this.page.Save.Click();

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

            this.page.Save.Click();
            
            this.page.File.Remove();

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.False(data.ExistFile);
        }
    }
}
