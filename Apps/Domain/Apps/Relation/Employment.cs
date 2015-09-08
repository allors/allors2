// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Employment.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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
    using System;
    using Resources;

    public partial class Employment
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFromDate)
            {
                this.FromDate = DateTime.UtcNow;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistEmployer)
            {
                derivation.AddDependency(this, this.Employer);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistEmployee && this.ExistEmployer)
            {
                var employments = this.Employee.EmploymentsWhereEmployee;
                employments.Filter.AddNot().AddExists(Employments.Meta.ThroughDate);

                if (employments.Count > 1)
                {
                    derivation.Log.AddError(this, Employments.Meta.FromDate, ErrorMessages.ActiveDeploymentRegistered, this.Employer.Name);
                }
            }

            if (this.ExistEmployee)
            {
                this.Employee.OnDerive(x => x.WithDerivation(derivation));
            }

            if (this.ExistEmployer)
            {
                this.Employer.OnDerive(x => x.WithDerivation(derivation));
            }

            if (this.ExistEmployee && this.Employee.ExistSalesRepRelationshipsWhereSalesRepresentative)
            {
                foreach (SalesRepRelationship salesRepRelationship in this.Employee.SalesRepRelationshipsWhereSalesRepresentative)
                {
                    salesRepRelationship.OnDerive(x => x.WithDerivation(derivation));
                }
            }

            this.AppsOnDeriveInternalOrganisationCustomer(derivation);
        }

        public void AppsOnDeriveInternalOrganisationCustomer(IDerivation derivation)
        {
            if (this.ExistEmployee && this.ExistEmployer)
            {

                if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                {
                    if (!this.Employee.ExistInternalOrganisationWhereEmployee)
                    {
                        this.Employer.AddEmployee(this.Employee);
                    }
                }

                if (this.FromDate > DateTime.UtcNow || (this.ExistThroughDate && this.ThroughDate < DateTime.UtcNow))
                {
                    if (this.Employee.ExistInternalOrganisationWhereEmployee)
                    {
                        this.Employer.RemoveEmployee(this.Employee);
                    }
                }
            }
        }

    }
}