namespace Tests.Remote
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Allors.Server;

    using Microsoft.AspNetCore.Hosting;

    public class RemoteFixture : IDisposable
    {
        public const string Url = "http://localhost:5050";

        public RemoteFixture()
        {
            var current = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(current + @"\..\..\..\..\..\..\Database\Server");
            var directory = directoryInfo.FullName;

            if (!directoryInfo.Exists)
            {
                throw new Exception($"Content root {directory} doesn't exist");
            }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(directory)
                .UseStartup<Startup>()
                .UseUrls(Url)
                .Build();

            Task.Run(() => host.Run());
        }

        public void Dispose()
        {
        }
    }
}