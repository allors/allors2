// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationContactKinds.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    public partial class OrganisationContactKinds
    {
        private UniquelyIdentifiableCache<OrganisationContactKind> cache;

        private UniquelyIdentifiableCache<OrganisationContactKind> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<OrganisationContactKind>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new OrganisationContactKindBuilder(this.Session).WithDescription("General contact").Build();
            new OrganisationContactKindBuilder(this.Session).WithDescription("Sales contact").Build();
            new OrganisationContactKindBuilder(this.Session).WithDescription("Support contact").Build();
            new OrganisationContactKindBuilder(this.Session).WithDescription("Supplier contact").Build();
        }
    }
}