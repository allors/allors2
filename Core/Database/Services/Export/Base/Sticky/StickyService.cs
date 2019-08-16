// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StickyService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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

        public void Set<T>(string key, IDictionary<T, long> value) => this.stickies[key] = value;

        public void Clear() => this.stickies = new ConcurrentDictionary<string, object>();
    }
}
