namespace Allors.Workspace.Client
{
    using System.Collections.Generic;
    using System.Linq;

    using Server;

    public class Result
    {
        public Dictionary<string, SessionObject> Objects { get; }

        public Dictionary<string, SessionObject[]> Collections { get; }

        public Dictionary<string, object> Values { get; }

        public Result(Session session, PullResponse response)
        {
            this.Objects = response.NamedObjects.ToDictionary(
                pair => pair.Key,
                pair => session.Get(long.Parse(pair.Value)));
            this.Collections = response.NamedCollections.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(v => session.Get(long.Parse(v))).ToArray());
            this.Values = response.NamedValues.ToDictionary(
                pair => pair.Key, 
                pair => pair.Value);
        }
    }
}