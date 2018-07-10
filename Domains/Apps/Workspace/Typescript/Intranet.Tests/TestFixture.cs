namespace Intranet.Tests
{
    using System;
    using System.Threading.Tasks;

    using PuppeteerSharp;

    using Xunit;

    public class TestFixture : IDisposable, IAsyncLifetime 
    {
        public async Task InitializeAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
        }

        public async Task DisposeAsync()
        {
        }

        public void Dispose()
        {
        }
    }
}