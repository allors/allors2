// <copyright file="PrefetchRule.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectIdInteger type.</summary>

namespace Allors.Database
{
    using System;

    using Meta;

    public sealed class PrefetchRule
    {
        public PrefetchRule(IPropertyType propertyType, PrefetchPolicy prefetchPolicy)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException("propertyType");
            }

            if (prefetchPolicy != null && propertyType is IRoleType roleType && roleType.ObjectType.IsUnit)
            {
                throw new ArgumentException("prefetchPolicy");
            }

            this.PropertyType = propertyType;
            this.PrefetchPolicy = prefetchPolicy;
        }

        public IPropertyType PropertyType { get; private set; }

        public PrefetchPolicy PrefetchPolicy { get; private set; }

        public override string ToString() => this.PropertyType.ToString();
    }
}
