namespace Tests.WorkEffortOverviewTests
{
    using Xunit;

    [Collection("Test collection")]
    public class WorkEffortListTest : Test
    {
        public WorkEffortListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToWorkEffortList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Work Efforts", this.Driver.Title);
        }
    }
}
