namespace Intranet.Tests
{
    using System.Threading.Tasks;

    using PuppeteerSharp;

    public class Button
    {
        public Button(Page page, string selector = null)
        {
            this.Page = page;
            if (!string.IsNullOrWhiteSpace(selector))
            {
                this.Selector = selector;
            }
        }

        public Page Page { get; }

        public string Selector { get; }
        
        public async Task ClickAsync()
        {
            await this.Page.ClickAsync(this.Selector);
            await this.Page.WaitForAngularAsync();
        }
    }
}
