
// <copyright file="StateService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;
    using System.Collections.Concurrent;

    public class StateService : IStateService
    {
        private readonly ConcurrentBag<WeakReference<IStateful>> statefulRefs;

        public StateService() => this.statefulRefs = new ConcurrentBag<WeakReference<IStateful>>();

        public void Register(IStateful stateful) => this.statefulRefs.Add(new WeakReference<IStateful>(stateful));

        public void Clear()
        {
            foreach (var statefulRef in this.statefulRefs.ToArray())
            {
                if (statefulRef.TryGetTarget(out var stateful))
                {
                    stateful.Clear();
                }
                else
                {
                    this.statefulRefs.TryTake(out var taken);
                }
            }
        }
    }
}
