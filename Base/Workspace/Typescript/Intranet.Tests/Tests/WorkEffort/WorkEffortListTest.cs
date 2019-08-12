namespace Tests.WorkEffortOverviewTests
{
    using Xunit;

    [Collection("Test collection")]
    public class WorkEffortListTest : Test
    {
        public WorkEffortListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToWorkEfforts();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Work Orders", this.Driver.Title);
        }
    }
}
