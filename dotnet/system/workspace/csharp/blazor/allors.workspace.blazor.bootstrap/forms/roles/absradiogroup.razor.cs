namespace Allors.Workspace.Blazor.Bootstrap.Forms.Roles
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;

    public class ABSRadioGroupBase : RoleField
    {
        [Parameter]
        public IObject[] Options { get; set; }

        [Parameter]
        public Func<IObject, string> DisplayOption { get; set; } = o => o.ToString();

        public string OptionName(IObject option) => this.Name + "_" + option.Id.ToString().Replace("-", "_");
    }
}
