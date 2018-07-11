namespace Intranet.Tests
{
    using System.Threading.Tasks;

    using PuppeteerSharp;
    using PuppeteerSharp.Input;

    public class Input
    {
        public Input(Page page, string selector = null, string formControlName = null)
        {
            this.Page = page;
            if (!string.IsNullOrWhiteSpace(selector))
            {
                this.Selector = selector;
            }
            else if (formControlName != null)
            {
                this.Selector = $"input[formcontrolname='{formControlName}']";
            }
        }

        public Page Page { get; }

        public string Selector { get; }


        public async Task TypeAsync(string text)
        {
            await this.Page.TypeAsync(this.Selector, text);
            await this.Page.WaitForAngularAsync();
        }

        public async Task<string> Value()
        {
            var element = await this.Page.QuerySelectorAsync(this.Selector);
            var property = await element.GetPropertyAsync("value");
            return (string) await property.JsonValueAsync();
        }
    }
}
