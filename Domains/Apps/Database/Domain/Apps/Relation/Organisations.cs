// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisations.cs" company="Allors bvba">
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
    using Meta;

    public partial class Organisations
    {
        public Extent<Organisation> Suppliers
        {
            get
            {
                var suppliers = new Organisations(this.Session).Extent();
                suppliers.Filter.AddContains(M.Organisation.OrganisationRoles, new OrganisationRoles(this.Session).Supplier);
                return suppliers;
            }
        }

        public Extent<Organisation> Customers
        {
            get
            {
                var customers = new Organisations(this.Session).Extent();
                customers.Filter.AddContains(M.Organisation.OrganisationRoles, new OrganisationRoles(this.Session).Customer);
                return customers;
            }
        }
    }
}