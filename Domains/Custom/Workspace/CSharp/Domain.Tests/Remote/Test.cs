using Tests.Local;

namespace Tests.Remote
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Allors.Workspace;
    using Allors.Workspace.Client;

    using Nito.AsyncEx;

    public class Test : IDisposable
    {
        public static DirectoryInfo AppLocation => new DirectoryInfo("../../../Workspace.Web");
        
        public const string BaseAddress = "http://localhost:" + Fixture.Port;
        public const string InitUrl = "/Test/Init";
        public const string SetupUrl = "/Test/Setup";
        public const string LoginUrl = "/Test/Login";

        public Workspace Workspace { get; set; }

        public Database Database { get; set; }

        public Test()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(BaseAddress),
            };

            this.Database = new Database(client);

            var config = new Config();
            this.Workspace = new Workspace(config.ObjectFactory);

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
                        var init = await this.Database.HttpClient.GetAsync(InitUrl);
                        var setup = await this.Database.HttpClient.GetAsync(SetupUrl);
                        var login = await this.Database.HttpClient.GetAsync(LoginUrl);
                    });
        }

    }
}