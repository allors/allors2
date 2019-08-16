
// <copyright file="Employment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Resources;

    public partial class Employment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistEmployee)
            {
                derivation.AddDependency(this.Employee, this);
            }

            if (this.ExistEmployer)
            {
                derivation.AddDependency(this.Employer, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
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
                if (this.FromDate <= this.strategy.Session.Now() && (!this.ExistThroughDate || this.ThroughDate >= this.strategy.Session.Now()))
                {
                    this.Employer.AddActiveEmployee(this.Employee);
                    new UserGroups(this.Strategy.Session).Employees.AddMember(this.Employee);
                }

                if (this.FromDate > this.strategy.Session.Now() || (this.ExistThroughDate && this.ThroughDate < this.strategy.Session.Now()))
                {
                    this.Employer.RemoveActiveEmployee(this.Employee);
                    new UserGroups(this.Strategy.Session).Employees.RemoveMember(this.Employee);
                }
            }

            this.Parties = new Party[] { this.Employee, this.Employer };
        }
    }
}
