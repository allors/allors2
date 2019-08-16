// <copyright file="FetchService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
