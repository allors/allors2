namespace Tests.Remote
{
    using Allors.Meta;
    using Allors.Protocol.Remote.Pull;
    using Allors.Workspace.Client;
    using Allors.Workspace.Data;

    using Xunit;

    public class PullTests : RemoteTest
    {
        [Fact]
        public void Pull()
        {
            var context = new Context("Pull", this.Database, this.Workspace);

            var pull = new Pull
            {
                Extent = new Filter(M.Person.ObjectType)
            }.Save();

            context.Load(new PullRequest
            {
                P = new[] { pull }
            }).Wait();

            var people = context.Collections["People"];
        }
    }
}