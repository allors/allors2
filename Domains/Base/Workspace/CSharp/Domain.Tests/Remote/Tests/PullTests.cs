namespace Tests.Remote
{
    using Allors.Meta;
    using Allors.Server;
    using Allors.Workspace.Client;
    using Allors.Workspace.Data;

    using Nito.AsyncEx;

    using Xunit;

    public class PullTests : RemoteTest
    {
        [Fact]
        public void Pull()
        {
            var context = new Context("Pull", this.Database, this.Workspace);

            var pull = new Pull
            {
                ObjectType = M.Person.ObjectType,
            };



            context.Load(new PullRequest{ new[] { pull.Save() }).Wait();}

            var people = context.Collections["People"];
        }
    }
}