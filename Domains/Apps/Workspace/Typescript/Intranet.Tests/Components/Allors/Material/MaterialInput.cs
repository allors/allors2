namespace Intranet.Tests
{
    using System.Threading.Tasks;

    using Allors.Meta;

    using PuppeteerSharp;

    public class MaterialInput
    {
        public MaterialInput(Page page, RoleType roleType)
        {
            this.Page = page;
            this.InputSelector = $"input[data-allors-roletype='{roleType.IdAsNumberString}']";
        }

        public Page Page { get; }

        public string InputSelector { get; }


        public async Task TypeAsync(string text)
        {
            await this.Page.TypeAsync(this.InputSelector, text);
            await this.Page.WaitForAngularAsync();
        }

        public async Task<string> Value()
        {
            var element = await this.Page.QuerySelectorAsync(this.InputSelector);
            var property = await element.GetPropertyAsync("value");
            return (string)await property.JsonValueAsync();
        }
    }
}
