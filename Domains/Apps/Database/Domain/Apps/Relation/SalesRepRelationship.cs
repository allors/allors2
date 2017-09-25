// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepRelationship.cs" company="Allors bvba">
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
    using System;
    using Meta;

    public partial class SalesRepRelationship
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistCustomer && this.ExistSalesRepresentative)
            {
                this.Customer.AppsOnDeriveCurrentSalesReps(derivation);
                this.SalesRepresentative.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Parties = new[] { this.Customer, this.InternalOrganisation };
    
            if (!this.ExistCustomer | !this.ExistSalesRepresentative)
            {
                this.Delete();
            }
        }

        public void AppsOnDeriveCommission()
        {
            this.YTDCommission = 0;
            this.LastYearsCommission = 0;

            foreach (SalesRepCommission salesRepCommission in this.SalesRepresentative.SalesRepCommissionsWhereSalesRep)
            {
                if (salesRepCommission.InternalOrganisation.Equals(this.InternalOrganisation))
                {
                    if (salesRepCommission.Year == DateTime.UtcNow.Year)
                    {
                        this.YTDCommission += salesRepCommission.Year;
                    }

                    if (salesRepCommission.Year == DateTime.UtcNow.AddYears(-1).Year)
                    {
                        this.LastYearsCommission += salesRepCommission.Year;
                    }
                }
            }
        }
    }
}