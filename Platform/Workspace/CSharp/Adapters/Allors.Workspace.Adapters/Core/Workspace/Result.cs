// <copyright file="Result.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Protocol.Remote.Pull;

    public class Result
    {
        public Result(Session session, PullResponse response)
        {
            this.Workspace = session.Workspace;

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

        public Dictionary<string, ISessionObject> Objects { get; }

        public Dictionary<string, ISessionObject[]> Collections { get; }

        public Dictionary<string, string> Values { get; }

        private IWorkspace Workspace { get; }

        public T[] GetCollection<T>()
        {
            var objectType = this.Workspace.ObjectFactory.GetObjectType<T>();
            var key = objectType.PluralName;
            return this.GetCollection<T>(key);
        }

        public T[] GetCollection<T>(string key) => this.Collections[key]?.Cast<T>().ToArray();

        public void GetCollection<T>(string key, out T[] value) => value = this.Collections[key]?.Cast<T>().ToArray();

        public T GetObject<T>() where T : SessionObject
        {
            var objectType = this.Workspace.ObjectFactory.GetObjectType<T>();
            var key = objectType.SingularName;
            return this.GetObject<T>(key);
        }

        public T GetObject<T>(string key) where T : SessionObject => (T)this.Objects[key];

        public void GetObject<T>(string key, out T value) where T : SessionObject => value = (T)this.Objects[key];

        public string GetValue(string key) => this.Values[key];

        public void GetValue(string key, out string value) => value = this.Values[key];
    }
}
