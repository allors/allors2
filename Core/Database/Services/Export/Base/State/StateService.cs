// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using System;
    using System.Collections.Concurrent;

    public class StateService : IStateService
    {
        private readonly ConcurrentBag<WeakReference<IStateful>> statefulRefs;

        public StateService()
        {
            this.statefulRefs = new ConcurrentBag<WeakReference<IStateful>>();
        }

        public void Register(IStateful stateful)
        {
            this.statefulRefs.Add(new WeakReference<IStateful>(stateful));
        }

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
