namespace Allors.Services
{
    using System;
    using System.Collections.Concurrent;

    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;

    public class PullService : IPullService
    {
        private readonly IDatabaseService databaseService;

        private readonly ConcurrentDictionary<Guid, Pull> pullById;

        public PullService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            this.pullById = new ConcurrentDictionary<Guid, Pull>();
        }

        public Pull Get(Guid id)
        {
            if (!this.pullById.TryGetValue(id, out var pull))
            {
                using (var session = this.databaseService.Database.CreateSession())
                {
                    var filter = new Filter(M.PreparedPull.Class)
                                     {
                                         Predicate = new Equals(M.PreparedPull.UniqueId.RoleType) { Value = id }
                                     };

                    var preparedPull = (PreparedPull)filter.Build(session).First;
                    if (preparedPull != null)
                    {
                        pull = preparedPull.Pull;
                        this.pullById[id] = pull;
                    }
                }
            }

            return pull;
        }
    }
}
