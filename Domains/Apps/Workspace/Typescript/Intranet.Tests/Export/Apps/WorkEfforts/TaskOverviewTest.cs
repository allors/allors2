namespace Tests.Intranet.WorkEfforts
{
    using Xunit;

    [Collection("Test collection")]
    public class TaskOverviewTest : Test
    {
        public TaskOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToTasks();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Work Tasks", this.Driver.Title);
        }
    }
}
