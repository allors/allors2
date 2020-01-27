// <copyright file="SalesRepRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesRepRelationship
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistCustomer && this.ExistSalesRepresentative)
            {
                var customer = this.Customer;

                // HACK: DerivedRoles
                var customerDerivedRoles = (PartyDerivedRoles)customer;

                customerDerivedRoles.RemoveCurrentSalesReps();

                foreach (SalesRepRelationship salesRepRelationship in customer.SalesRepRelationshipsWhereCustomer)
                {
                    if (salesRepRelationship.FromDate <= this.Session().Now() &&
                        (!salesRepRelationship.ExistThroughDate || salesRepRelationship.ThroughDate >= this.Session().Now()))
                    {
                        customerDerivedRoles.AddCurrentSalesRep(salesRepRelationship.SalesRepresentative);
                    }
                }

                this.SalesRepresentative.OnDerive(x => x.WithDerivation(derivation));
            }

            this.Parties = new[] { this.Customer };

            if (!this.ExistCustomer | !this.ExistSalesRepresentative)
            {
                this.Delete();
            }
        }
    }
}
