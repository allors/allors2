// <copyright file="Prefetchers.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters
{
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    public class Prefetchers
    {
        private readonly Dictionary<IClass, PrefetchPolicy> prefetchPolicyByClass;

        public Prefetchers() => this.prefetchPolicyByClass = new Dictionary<IClass, PrefetchPolicy>();

        public PrefetchPolicy this[IClass @class] // Indexer declaration
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
