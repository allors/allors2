namespace Tests.Local
{
    using System;
    using Allors.Workspace;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Allors.Meta;
    using Allors.Workspace.Domain;

    public class Test : IDisposable
    {
        public Workspace Workspace { get; set; }

        public Test()
        {
            var config = new Config();
            this.Workspace = new Workspace(config.ObjectFactory);
        }

        public void Dispose()
        {
        }

    }
}