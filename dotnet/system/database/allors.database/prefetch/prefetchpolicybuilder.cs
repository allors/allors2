// <copyright file="PrefetchPolicyBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectIdInteger type.</summary>

namespace Allors.Database
{
    using System.Collections.Generic;

    using Meta;

    public sealed class PrefetchPolicyBuilder
    {
        private List<PrefetchRule> rules;

        private bool allowCompilation = true;

        public PrefetchPolicyBuilder() => this.rules = new List<PrefetchRule>();

        public PrefetchPolicyBuilder WithRule(IPropertyType propertyType)
        {
            var rule = new PrefetchRule(propertyType, null);
            this.rules.Add(rule);
            return this;
        }

        public PrefetchPolicyBuilder WithRule(IPropertyType propertyType, PrefetchPolicy prefetch)
        {
            var rule = new PrefetchRule(propertyType, prefetch);
            this.rules.Add(rule);
            return this;
        }

        public PrefetchPolicyBuilder WithAllowCompilation(bool allowCompilation)
        {
            this.allowCompilation = allowCompilation;
            return this;
        }

        public PrefetchPolicy Build()
        {
            try
            {
                return new PrefetchPolicy(this.rules.ToArray()) { AllowCompilation = this.allowCompilation };
            }
            finally
            {
                this.rules = null;
            }
        }
    }
}
