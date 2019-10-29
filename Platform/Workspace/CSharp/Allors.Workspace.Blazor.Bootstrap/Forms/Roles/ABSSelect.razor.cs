namespace Allors.Workspace.Blazor.Bootstrap.Forms.Roles
{
    using System;
    using System.Linq;
    using System.Xml;
    using Microsoft.AspNetCore.Components;

    public class ABSSelectBase : RoleField
    {
        public long ModelId
        {
            get => ((ISessionObject)this.Model)?.Id ?? 0;
            set => this.Model = value != 0 ? this.Object.Session.Get(value) : null;
        }

        [Parameter]
        public ISessionObject[] Options { get; set; }

        [Parameter]
        public Func<ISessionObject, string> DisplayOption { get; set; } = o => o.ToString();
    }
}
