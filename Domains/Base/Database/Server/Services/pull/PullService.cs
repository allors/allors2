namespace Allors.Server
{
    using System;
    using System.Collections.Generic;

    using Allors.Data;
    using Allors.Meta;
    using Allors.Services;

    public class PullService : IPullService
    {
        private readonly Dictionary<Guid, Pull> pullById;

        public PullService()
        {
            this.pullById = new Dictionary<Guid, Pull>
            {
                [new Guid("5E2BEC46-E2EB-4761-BB27-8508AFB42E72")] =
                                        new Pull
                                        {
                                            Extent = new Filter(M.Person.Class)
                                            {
                                                Predicate = new Equals
                                                {
                                                    PropertyType = M.Person.FirstName,
                                                    Parameter = "firstname"
                                                }
                                            }
                                        }
            };

        }

        public Pull Get(Guid id)
        {
            this.pullById.TryGetValue(id, out var pull);
            return pull;
        }
    }
}
