// <copyright file="RemotePullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Remote
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api.Pull;

    public class PullResult : Result, IPullResultInternals
    {
        private IDictionary<string, IObject> objects;

        private IDictionary<string, IObject[]> collections;

        private IDictionary<string, object> values;

        private readonly PullResponse pullResponse;

        public PullResult(Adapters.Session session, PullResponse response) : base(session, response)
        {
            this.Workspace = session.Workspace;
            this.pullResponse = response;
        }

        private IWorkspace Workspace { get; }

        public IDictionary<string, IObject> Objects => this.objects ??= this.pullResponse.o.ToDictionary(pair => pair.Key.ToUpperInvariant(), pair => this.Session.Instantiate<IObject>(pair.Value));

        public IDictionary<string, IObject[]> Collections => this.collections ??= this.pullResponse.c.ToDictionary(pair => pair.Key.ToUpperInvariant(), pair => pair.Value.Select(this.Session.Instantiate<IObject>).ToArray());

        public IDictionary<string, object> Values => this.values ??= this.pullResponse.v.ToDictionary(pair => pair.Key.ToUpperInvariant(), pair => pair.Value);

        public T[] GetCollection<T>() where T : class, IObject
        {
            var objectType = this.Workspace.Configuration.ObjectFactory.GetObjectType<T>();
            var key = objectType.PluralName.ToUpperInvariant();
            return this.GetCollection<T>(key);
        }

        public T[] GetCollection<T>(string key) where T : class, IObject => this.Collections.TryGetValue(key.ToUpperInvariant(), out var collection) ? collection?.Cast<T>().ToArray() : null;

        public T GetObject<T>() where T : class, IObject
        {
            var objectType = this.Workspace.Configuration.ObjectFactory.GetObjectType<T>();
            var key = objectType.SingularName.ToUpperInvariant();
            return this.GetObject<T>(key);
        }

        public T GetObject<T>(string key) where T : class, IObject => this.Objects.TryGetValue(key.ToUpperInvariant(), out var @object) ? (T)@object : null;

        public object GetValue(string key) => this.Values[key.ToUpperInvariant()];

        public T GetValue<T>(string key) => (T)this.GetValue(key.ToUpperInvariant());
    }
}
