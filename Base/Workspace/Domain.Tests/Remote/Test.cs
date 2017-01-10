namespace Tests.Remote
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Allors.Workspace;
    using Allors.Workspace.Client;

    using Nito.AsyncEx;

    using NUnit.Framework;

    public class Test
    {
        public static DirectoryInfo AppLocation => new DirectoryInfo("../../../Workspace.Web");
        
        public const string BaseAddress = "http://localhost:" + Fixture.Port;
        public const string InitUrl = "/Test/Init";
        public const string SetupUrl = "/Test/Setup";
        public const string LoginUrl = "/Test/Login";

        public Workspace Workspace { get; set; }

        public Database Database { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(BaseAddress),
            };

            this.Database = new Database(client);
            this.Workspace = new Workspace(Config.ObjectFactory);

            this.Init();
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

        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}