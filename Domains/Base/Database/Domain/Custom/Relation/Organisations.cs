// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisations.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class Organisations
    {
        private UniquelyIdentifiableSticky<Organisation> sticky;

        public UniquelyIdentifiableSticky<Organisation> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<Organisation>(this.Session));

        protected override void CustomSecure(Security config)
        {
            base.CustomSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };
            config.GrantAdministrator(this.ObjectType, full);
            config.GrantCreator(this.ObjectType, full);

            config.GrantGuest(this.ObjectType, Operations.Read);
        }
    }
}
