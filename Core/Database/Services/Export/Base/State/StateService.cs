// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
