// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StickyService.cs" company="Allors bvba">
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class StickyService : IStickyService
    {
        private ConcurrentDictionary<string, object> stickies;

        public StickyService(IStateService stateService)
        {
            this.Clear();
            stateService.Register(this);
        }

        public IDictionary<T, long> Get<T>(string key)
        {
            if (this.stickies.TryGetValue(key, out var cache))
            {
                return (IDictionary<T, long>)cache;
            }

            return null;
        }

        public void Set<T>(string key, IDictionary<T, long> value)
        {
            this.stickies[key] = value;
        }

        public void Clear()
        {
            this.stickies = new ConcurrentDictionary<string, object>();
        }
    }
}
