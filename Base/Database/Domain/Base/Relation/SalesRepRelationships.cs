// <copyright file="SalesRepRelationships.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class SalesRepRelationships
    {
        public static Person SalesRep(Party customer, ProductCategory productCategory, DateTime date)
        {
            Person salesRep = null;
            var salesRepRelationships = customer.SalesRepRelationshipsWhereCustomer;

            if (productCategory != null)
            {
                salesRep = SalesRep(salesRepRelationships, productCategory, date);
            }

            return salesRep ?? SalesRep(salesRepRelationships, date);
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

                        foreach (ProductCategory parent in productCategory.ProductCategoriesWhereDescendant)
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
