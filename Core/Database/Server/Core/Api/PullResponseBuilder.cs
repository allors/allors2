// <copyright file="PullResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Protocol.Remote.Pull;
    using Protocol;
    using Allors.Services;

    public class PullResponseBuilder
    {
        private readonly AccessControlsWriter accessControlsWriter;
        private readonly IAccessControlLists acls;
        private readonly Dictionary<string, IList<IObject>> collectionsByName = new Dictionary<string, IList<IObject>>();
        private readonly PermissionsWriter permissionsWriter;
        private readonly Dictionary<string, IObject> objectByName = new Dictionary<string, IObject>();
        private readonly HashSet<IObject> objects;
        private readonly ITreeService treeService;
        private readonly Dictionary<string, object> valueByName = new Dictionary<string, object>();

        public PullResponseBuilder(User user, ITreeService treeService = null)
            : this(new WorkspaceAccessControlLists(user), treeService)
        {
        }

        public PullResponseBuilder(IAccessControlLists acls, ITreeService treeService)
        {
            this.acls = acls;
            this.treeService = treeService;

            this.objects = new HashSet<IObject>();
            this.accessControlsWriter = new AccessControlsWriter(this.acls);
            this.permissionsWriter = new PermissionsWriter(this.acls);
        }

        public void AddCollection(string name, IEnumerable<IObject> collection, bool full = false)
        {
            var inputList = (collection as IList<IObject>) ?? collection?.ToArray() ?? Array.Empty<IObject>();

            Node[] tree = null;
            if (full && inputList.Count > 0)
            {
                var @object = inputList.FirstOrDefault();
                tree = @object?.Strategy.Session.Database.FullTree(@object.Strategy.Class, this.treeService);
            }

            this.AddCollection(name, inputList, tree);
        }

        public void AddCollection(string name, IEnumerable<IObject> collection, Node[] tree)
        {
            if (collection != null)
            {
                var list = collection as IList<IObject> ?? collection.ToArray();

                // Prefetch
                if (tree != null && list.Count > 0)
                {
                    var session = list[0].Strategy.Session;
                    var prefetchPolicy = tree.BuildPrefetchPolicy();
                    session.Prefetch(prefetchPolicy, list);
                }

                if (this.collectionsByName.TryGetValue(name, out var existingIList))
                {
                    if (existingIList is List<IObject> existingList)
                    {
                        existingList.AddRange(list);
                    }
                    else
                    {
                        var newList = existingIList.Concat(list).ToArray();
                        this.collectionsByName[name] = newList;
                    }
                }
                else
                {
                    this.collectionsByName.Add(name, list);
                }

                foreach (var namedObject in list)
                {
                    this.objects.Add(namedObject);
                    tree?.Resolve(namedObject, this.acls, this.objects);
                }
            }
        }

        public void AddObject(string name, IObject @object, bool full = false)
        {
            if (@object != null)
            {
                Node[] tree = null;
                if (full)
                {
                    tree = @object.Strategy.Session.Database.FullTree(@object.Strategy.Class, this.treeService);
                }

                this.AddObject(name, @object, tree);
            }
        }

        public void AddObject(string name, IObject @object, Node[] tree)
        {
            if (@object != null)
            {
                if (tree != null)
                {
                    // Prefetch
                    var session = @object.Strategy.Session;
                    var prefetcher = tree.BuildPrefetchPolicy();
                    session.Prefetch(prefetcher, @object);
                }

                this.objects.Add(@object);
                this.objectByName[name] = @object;
                tree?.Resolve(@object, this.acls, this.objects);
            }
        }

        public void AddValue(string name, object value)
        {
            if (value != null)
            {
                this.valueByName.Add(name, value);
            }
        }

        public PullResponse Build()
        {
            var pullResponse = new PullResponse
            {
                Objects = this.objects.Select(v =>
                {
                    var strategy = v.Strategy;
                    var id = strategy.ObjectId.ToString();
                    var version = strategy.ObjectVersion.ToString();
                    var accessControls = this.accessControlsWriter.Write(v);
                    var deniedPermissions = this.permissionsWriter.Write(v);
                    return deniedPermissions != null
                        ? new[] { id, version, accessControls, deniedPermissions }
                        : new[] { id, version, accessControls };
                }).ToArray(),
                NamedObjects = this.objectByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Id.ToString()),
                NamedCollections = this.collectionsByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(obj => obj.Id.ToString()).ToArray()),
                NamedValues = this.valueByName,
            };

            pullResponse.AccessControls = this.acls.EffectivePermissionIdsByAccessControl.Keys
                .Select(v => new[]
                {
                    v.Strategy.ObjectId.ToString(),
                    v.Strategy.ObjectVersion.ToString(),
                })
                .ToArray();

            return pullResponse;
        }
    }
}
