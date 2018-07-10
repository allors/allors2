namespace Intranet.Pages
{
    using System.Threading.Tasks;

    using Intranet.Tests;

    public abstract class BasePage
    {
        protected readonly PuppeteerSharp.Page Page;

        protected BasePage(PuppeteerSharp.Page page)
        {
            this.Page = page;
        }

        public async Task ClickAsync(string selector)
        {
            await this.Page.ClickAsync(selector);
            await this.Page.WaitForAngularAsync();
        }

        public async Task TypeAsync(string selector, string userName)
        {
            await this.Page.TypeAsync(selector, userName);
            await this.Page.WaitForAngularAsync();
        }
    }
}
