//------------------------------------------------------------------------------------------------- 
// <copyright file="PrefetchPolicyBuilder.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ObjectIdInteger type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using System.Collections.Generic;

    using Allors.Meta;

    public sealed class PrefetchPolicyBuilder
    {
        private List<PrefetchRule> rules;

        private bool allowCompilation = true;

        public PrefetchPolicyBuilder()
        {
            this.rules = new List<PrefetchRule>();
        }

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
