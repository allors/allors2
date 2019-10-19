namespace Allors.Workspace.Blazor.Bootstrap.Forms.Roles
{
    using System;
    using Microsoft.AspNetCore.Components.Forms;

    public class ABSInputBase : RoleField
    {
        public ISessionObject DerivedObject
        {
            get
            {
                return this.Object ?? (ISessionObject)this.EditContext.Model;
            }
        }

        public string DerivedLabel
        {
            get
            {
                return this.Label ?? this.RoleType.DisplayName;
            }
        }

        public string Model
        {
            get
            {
                return (string)this.DerivedObject.Get(this.RoleType);
            }

            set
            {
                this.DerivedObject.Set(this.RoleType, value);
            }
        }

     
        
        

    }
}
