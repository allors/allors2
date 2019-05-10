namespace Tests.Remote
{
    using Allors.Workspace.Meta;
    using Allors.Protocol.Remote.Pull;
    using Allors.Workspace.Client;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;

    using Xunit;

    public class PullTests : RemoteTest
    {
        [Fact]
        public void Pull()
        {
            var context = new Context(this.Database, this.Workspace);

            var pull = new Pull
            {
                Extent = new Filter(M.Person.ObjectType)
            };

            var result = context.Load(pull).Result;

            var people = result.GetCollection<Person>("People");
        }
    }
}