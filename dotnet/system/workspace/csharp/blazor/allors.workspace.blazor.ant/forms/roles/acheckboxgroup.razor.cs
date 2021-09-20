namespace Allors.Workspace.Blazor.Ant.Forms.Roles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Components;

    public class ACheckboxGroupBase : RoleField
    {
        [Parameter]
        public IObject[] Options { get; set; }

        [Parameter]
        public Func<IObject, string> DisplayOption { get; set; } = o => o.ToString();

        public string OptionName(IObject option) => this.Name + "_" + option.Id.ToString().Replace("-", "_");

        public object IsSelected(IObject option)
        {
            var model = (IEnumerable<IObject>)this.Model;
            return model.Contains(option);
        }

        public void OnChanged(object checkedValue, IObject option)
        {
            if ((bool)checkedValue)
            {
                this.Object.Strategy.AddCompositesRole(this.RoleType, option);
            }
            else
            {
                this.Object.Strategy.RemoveCompositesRole(this.RoleType, option);
            }
        }
    }
}
