namespace Allors.Workspace.Blazor.Ant.Forms.Roles
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;

    public class ARadioGroupBase : RoleField
    {
        [Parameter]
        public IObject[] Options { get; set; }

        [Parameter]
        public Func<IObject, string> DisplayOption { get; set; } = o => o.ToString();

        public string OptionName(IObject option) => this.Name + "_" + option.Id.ToString().Replace("-", "_");
    }
}
