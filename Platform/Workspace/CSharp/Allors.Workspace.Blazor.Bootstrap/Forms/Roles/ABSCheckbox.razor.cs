namespace Allors.Workspace.Blazor.Bootstrap.Forms.Roles
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Components;

    public class ABSCheckboxBase : RoleField
    {
        public bool? IntModel { get => (bool?)this.Model; set => this.Model = value; }
    }
}
