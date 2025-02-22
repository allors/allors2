namespace Allors.Workspace.Blazor.Bootstrap.Forms.Roles
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;

    public class ABSRadioGroupBase : RoleField
    {
        [Parameter]
        public ISessionObject[] Options { get; set; }

        [Parameter]
        public Func<ISessionObject, string> DisplayOption { get; set; } = o => o.ToString();

        public string OptionName(ISessionObject option) => this.Name + "_" + option.Id.ToString().Replace("-", "_");
    }
}
