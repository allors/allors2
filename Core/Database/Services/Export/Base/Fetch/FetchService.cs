// <copyright file="FetchService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;
    using System.Collections.Concurrent;

    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;

    public class FetchService : IFetchService
    {
        private readonly IDatabaseService databaseService;

        private readonly ConcurrentDictionary<Guid, Fetch> fetchById;

        public FetchService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            this.fetchById = new ConcurrentDictionary<Guid, Fetch>();
        }

        public Fetch Get(Guid id)
        {
            if (!this.fetchById.TryGetValue(id, out var fetch))
            {
                using (var session = this.databaseService.Database.CreateSession())
                {
                    var filter = new Filter(M.PreparedFetch.Class)
                    {
                        Predicate = new Equals(M.PreparedFetch.UniqueId.RoleType) { Value = id }
                    };

                    var preparedFetch = (PreparedFetch)filter.Build(session).First;
                    if (preparedFetch != null)
                    {
                        fetch = preparedFetch.Fetch;
                        this.fetchById[id] = fetch;
                    }
                }
            }

            return fetch;
        }
    }
}
