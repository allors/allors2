namespace Tests.Remote
{
    using System;
    using System.IO;
    using System.Net.Http;

    using Allors.Workspace;
    using Allors.Workspace.Client;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;

    using Nito.AsyncEx;

    using Xunit;

    [Collection("Remote")]
    public class RemoteTest : IDisposable
    {
        public const string Url = "http://localhost:5000";

        public const string InitUrl = "/Test/Init";
        public const string SetupUrl = "/Test/Setup";
        public const string LoginUrl = "/Test/Login";

        public Workspace Workspace { get; set; }

        public Database Database { get; set; }

        public RemoteTest()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(Url),
            };

            this.Database = new Database(client);

            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
            this.Workspace = new Workspace(objectFactory);

            this.Init();
        }

        public void Dispose()
        {
        }

        private void Init()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var setup = await this.Database.HttpClient.GetAsync(SetupUrl);
                        var login = await this.Database.HttpClient.GetAsync(LoginUrl);
                    });
        }

    }
}