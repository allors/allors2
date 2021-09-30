namespace Allors.Workspace.Blazor.Bootstrap.Forms.Roles
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Components;

    public class ABSCheckboxGroupBase : RoleField
    {
        [Parameter]
        public ISessionObject[] Options { get; set; }

        [Parameter]
        public Func<ISessionObject, string> DisplayOption { get; set; } = o => o.ToString();

        public string OptionName(ISessionObject option) => this.Name + "_" + option.Id.ToString().Replace("-", "_");

        public object IsSelected(ISessionObject option)
        {
            var model = (IEnumerable<ISessionObject>)this.Model;
            return model.Contains(option);
        }

        public void OnChanged(object checkedValue, ISessionObject option)
        {
            if ((bool)checkedValue)
            {
                this.Object.Add(this.RoleType, option);
            }
            else
            {
                this.Object.Remove(this.RoleType, option);
            }
        }
    }
}
