namespace Intranet.Tests
{
    using System.Threading.Tasks;

    using PuppeteerSharp;
    using PuppeteerSharp.Input;

    public class Input
    {
        private Page page;

        private string selector;

        public Input(Page page, string selector)
        {
            this.page = page;
            this.selector = selector;
        }

        public async Task TypeAsync(string text)
        {
            await this.page.TypeAsync(this.selector, text);
            await this.page.WaitForAngularAsync();
        }

        public async Task<string> Value()
        {
            var element = await this.page.QuerySelectorAsync(this.selector);
            var property = await element.GetPropertyAsync("value");
            return (string) await property.JsonValueAsync();
        }
    }
}
