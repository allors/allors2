// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Prefetchers.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    public class Prefetchers
    {
        private readonly Dictionary<IClass, PrefetchPolicy> prefetchPolicyByClass;

        public Prefetchers() => this.prefetchPolicyByClass = new Dictionary<IClass, PrefetchPolicy>();

        public PrefetchPolicy this[IClass @class]    // Indexer declaration
        {
            get
            {
                if (!this.prefetchPolicyByClass.TryGetValue(@class, out var prefetchPolicy))
                {
                    var prefetchPolicyBuilder = new PrefetchPolicyBuilder();

                    foreach (var roleType in @class.RoleTypes)
                    {
                        prefetchPolicyBuilder.WithRule(roleType);
                    }

                    foreach (var associationType in @class.AssociationTypes)
                    {
                        prefetchPolicyBuilder.WithRule(associationType);
                    }

                    prefetchPolicy = prefetchPolicyBuilder.Build();
                    this.prefetchPolicyByClass[@class] = prefetchPolicy;
                }

                return prefetchPolicy;
            }
        }
    }
}
