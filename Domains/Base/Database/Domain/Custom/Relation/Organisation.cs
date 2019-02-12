//------------------------------------------------------------------------------------------------- 
// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
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

        public override string ToString()
        {
            return this.Name;
        }
    }
}
