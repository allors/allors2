// <copyright file="Result.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
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
                pair => session.Get(long.Parse(pair.Value)),
                StringComparer.OrdinalIgnoreCase);
            this.Collections = response.NamedCollections.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(v => session.Get(long.Parse(v))).ToArray(),
                StringComparer.OrdinalIgnoreCase);
            this.Values = response.NamedValues.ToDictionary(
                pair => pair.Key,
                pair => pair.Value,
                StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, ISessionObject> Objects { get; }

        public IDictionary<string, ISessionObject[]> Collections { get; }

        public IDictionary<string, object> Values { get; }

        private IWorkspace Workspace { get; }

        public T[] GetCollection<T>()
        {
            var objectType = this.Workspace.ObjectFactory.GetObjectType<T>();
            var key = objectType.PluralName;
            return this.GetCollection<T>(key);
        }

        public T[] GetCollection<T>(string key) => this.Collections.TryGetValue(key, out var collection) ? collection?.Cast<T>().ToArray() : null;

        public T GetObject<T>()
            where T : SessionObject
        {
            var objectType = this.Workspace.ObjectFactory.GetObjectType<T>();
            var key = objectType.SingularName;
            return this.GetObject<T>(key);
        }

        public T GetObject<T>(string key)
            where T : SessionObject => this.Objects.TryGetValue(key, out var @object) ? (T)@object : null;

        public object GetValue(string key) => this.Values[key];
    }
}
