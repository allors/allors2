namespace Intranet.Tests
{
    using System.Threading.Tasks;

    using Allors.Meta;

    using PuppeteerSharp;
    using PuppeteerSharp.Input;

    public class MaterialSelect
    {
        public MaterialSelect(Page page, RoleType roleType)
        {
            this.Page = page;
            this.ArrowSelector = $"mat-select[data-allors-roletype='{roleType.IdAsNumberString}'] .mat-select-arrow";
        }

        public Page Page { get; }

        public string ArrowSelector { get; }

        public async Task SelectAsyn(string text)
        {
            await this.Page.ClickAsync(this.ArrowSelector);
            await this.Page.WaitForAngularAsync();

            var optionSelector = $"mat-option[data-allors-option-display='{text}']";
            await this.Page.ClickAsync(optionSelector);
            await this.Page.WaitForAngularAsync();
        }

        public async Task<string> Value()
        {
            var element = await this.Page.QuerySelectorAsync(this.ArrowSelector);
            var property = await element.GetPropertyAsync("value");
            return (string)await property.JsonValueAsync();
        }
    }
}
