// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepRelationships.cs" company="Allors bvba">
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
    using System.Collections.Generic;


    public partial class SalesRepRelationships
    {
        public static void DeriveCommissions(ISession session)
        {
            foreach (SalesRepRelationship salesRepRelationship in session.Extent<SalesRepRelationship>())
            {
                salesRepRelationship.DeriveCommission();
            }
        }

        public static Person SalesRep(Organisation customer, ProductCategory productCategory, DateTime date)
        {
            Person salesRep = null;
            var salesRepRelationships = customer.SalesRepRelationshipsWhereCustomer;

            if (productCategory != null)
            {
                salesRep = SalesRep(salesRepRelationships, productCategory, date);
            }

            return salesRep ?? SalesRep(salesRepRelationships, date);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static Person SalesRep(IList<SalesRepRelationship> salesRepRelationships, ProductCategory productCategory, DateTime date)
        {
            if (salesRepRelationships != null)
            {
                foreach (var salesRepRelationship in salesRepRelationships)
                {
                    if (salesRepRelationship.FromDate <= date && (!salesRepRelationship.ExistThroughDate || salesRepRelationship.ThroughDate >= date))
                    {
                        if (salesRepRelationship.ProductCategories.Contains(productCategory))
                        {
                            return salesRepRelationship.SalesRepresentative;
                        }

                        foreach (ProductCategory parent in productCategory.Parents)
                        {
                            var salesRep = SalesRep(salesRepRelationships, parent, date);
                            if (salesRep != null)
                            {
                                return salesRep;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static Person SalesRep(IEnumerable<SalesRepRelationship> salesRepRelationships, DateTime date)
        {
            foreach (var salesRepRelationship in salesRepRelationships)
            {
                if (salesRepRelationship.FromDate <= date && (!salesRepRelationship.ExistThroughDate || salesRepRelationship.ThroughDate >= date))
                {
                    if (!salesRepRelationship.ExistProductCategories)
                    {
                        return salesRepRelationship.SalesRepresentative;
                    }
                }
            }

            return null;
        }
    }
}