namespace Tests.Local
{
    using System;

    using Allors.Workspace;
    using Allors.Workspace.Domain;
    using Allors.Meta;

    public class LocalTest : IDisposable
    {
        public Workspace Workspace { get; set; }

        public LocalTest()
        {
            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
            this.Workspace = new Workspace(objectFactory);
        }

        public void Dispose()
        {
        }

    }
}