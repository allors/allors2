//------------------------------------------------------------------------------------------------- 
// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class Organisation
    {
        public void CustomToggleCanWrite(OrganisationToggleCanWrite method)
        {
            if (this.DeniedPermissions.Count != 0)
            {
                this.RemoveDeniedPermissions();
            }
            else
            {
                var permissions = new Permissions(this.strategy.Session);
                var deniedPermissions = new[]
                                            {
                                                permissions.Get(this.Meta.Class, this.Meta.Name, Operations.Write),
                                                permissions.Get(this.Meta.Class, this.Meta.Owner, Operations.Write),
                                                permissions.Get(this.Meta.Class, this.Meta.Employees, Operations.Write)
                                            };

                this.DeniedPermissions = deniedPermissions;
            }
        }

        public override string ToString() => this.Name;
    }
}
