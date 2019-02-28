// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Employment.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Resources;

    public partial class Employment
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistEmployee)
            {
                derivation.AddDependency(this.Employee, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistEmployer && internalOrganisations.Count() == 1)
            {
                this.Employer = internalOrganisations.First();
            }

            if (this.ExistEmployee && this.Employee.ExistSalesRepRelationshipsWhereSalesRepresentative)
            {
                foreach (SalesRepRelationship salesRepRelationship in this.Employee.SalesRepRelationshipsWhereSalesRepresentative)
                {
                    salesRepRelationship.OnDerive(x => x.WithDerivation(derivation));
                }
            }

            if (this.ExistEmployee)
            {
                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    this.Employer.AddActiveEmployee(this.Employee);
                    new UserGroups(this.Strategy.Session).Creators.AddMember(this.Employee);
                }

                if (this.FromDate > DateTime.UtcNow || (this.ExistThroughDate && this.ThroughDate < DateTime.UtcNow))
                {
                    this.Employer.RemoveActiveEmployee(this.Employee);
                    new UserGroups(this.Strategy.Session).Creators.RemoveMember(this.Employee);
                }
            }

            this.Parties = new Party[] { this.Employee, this.Employer};
        }
    }
}