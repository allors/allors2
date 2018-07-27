namespace Intranet.Tests
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Intranet.Pages;
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class FilesTest : Test
    {
        private readonly Sidenav sidenav;

        private readonly FormPage page;

        public FilesTest(TestFixture fixture)
            : base(fixture)
        {
            this.sidenav = this.Login().Sidenav;
            this.page = this.sidenav.NavigateToForm();
        }

        [Fact]
        public void UploadOne()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Files.Upload("logo.png");

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.True(data.ExistMultipleFiles);
        }

        [Fact]
        public void UploadTwo()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Files.Upload("logo.png");

            this.page.Files.Upload("logo2.png");

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.True(data.ExistMultipleFiles);
            Assert.Equal(2, data.MultipleFiles.Count);
        }


        [Fact]
        public void Remove()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Files.Upload("logo.png");

            this.page.Save.Click();
            
            this.page.Files.Remove();

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.False(data.ExistMultipleFiles);
        }
    }
}
