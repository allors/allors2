namespace Allors.Workspace.Blazor.Ant.Forms.Roles
{
    using System;
    using Microsoft.AspNetCore.Components;

    public class ASelectBase : RoleField
    {
        public long ModelId
        {
            get => ((IObject)this.Model)?.Id ?? 0;
            set => this.Model = value != 0 ? this.Object.Strategy.Session.Instantiate<IObject>(value) : null;
        }

        [Parameter]
        public IObject[] Options { get; set; }

        [Parameter]
        public Func<IObject, string> DisplayOption { get; set; } = o => o.ToString();

        [Parameter]
        public string EmptyValue { get; set; }
    }
}
