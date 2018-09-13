namespace Allors.Services
{
    using System;
    using System.Collections.Concurrent;

    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;

    public class PathService : IPathService
    {
        private readonly IDatabaseService databaseService;

        private readonly ConcurrentDictionary<Guid, Path> fetchById;

        public PathService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            this.fetchById = new ConcurrentDictionary<Guid, Path>();
        }

        public Path Get(Guid id)
        {
            if (!this.fetchById.TryGetValue(id, out var fetch))
            {
                using (var session = this.databaseService.Database.CreateSession())
                {
                    var filter = new Filter(M.PreparedPath.Class)
                                     {
                                         Predicate = new Equals(M.PreparedPath.UniqueId.RoleType) { Value = id }
                                     };

                    var preparedFetch = (PreparedPath)filter.Build(session).First;
                    if (preparedFetch != null)
                    {
                        fetch = preparedFetch.Path;
                        this.fetchById[id] = fetch;
                    }
                }
            }

            return fetch;
        }
    }
}
