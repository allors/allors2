namespace Allors.Workspace.Client
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Workspace.Domain;

    using Protocol.Remote.Pull;


    public class Result
    {
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

        public Dictionary<string, SessionObject> Objects { get; }

        public Dictionary<string, SessionObject[]> Collections { get; }

        public Dictionary<string, object> Values { get; }

        public T[] GetCollection<T>(string key)
        {
            return this.Collections[key]?.Cast<T>().ToArray();
        }

        public void GetCollection<T>(string key, out T[] value)
        {
            value = this.Collections[key]?.Cast<T>().ToArray();
        }

        public T GetObject<T>(string key) where T : SessionObject
        {
            return (T)this.Objects[key];
        }

        public void GetObject<T>(string key, out T value) where T : SessionObject
        {
            value = (T)this.Objects[key];
        }

        public T GetValue<T>(string key)
        {
            return (T)this.Values[key];
        }

        public void GetValue<T>(string key, out T value)
        {
            value = (T)this.Values[key];
        }
    }
}