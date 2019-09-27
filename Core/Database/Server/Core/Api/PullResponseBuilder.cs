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
    using Services;

    public class PullResponseBuilder
    {
        private readonly User user;
        private readonly ITreeService treeService;

        private readonly HashSet<IObject> objects = new HashSet<IObject>();
        private readonly Dictionary<string, IObject> objectByName = new Dictionary<string, IObject>();
        private readonly Dictionary<string, List<IObject>> collectionsByName = new Dictionary<string, List<IObject>>();
        private readonly Dictionary<string, string> valueByName = new Dictionary<string, string>();

        public PullResponseBuilder(User user, ITreeService treeService)
        {
            this.user = user;
            this.treeService = treeService;
        }

        public PullResponse Build() =>
            new PullResponse
            {
                UserSecurityHash = this.user.SecurityHash(),
                Objects = this.objects.Select(x => new[] { x.Id.ToString(), x.Strategy.ObjectVersion.ToString() }).ToArray(),
                NamedObjects = this.objectByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Id.ToString()),
                NamedCollections = this.collectionsByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(obj => obj.Id.ToString()).ToArray()),
                NamedValues = this.valueByName,
            };

        public void AddObject(string name, IObject @object, bool full = false)
        {
            if (@object != null)
            {
                TreeNode[] tree = null;
                if (full)
                {
                    tree = @object.Strategy.Session.Database.FullTree(@object.Strategy.Class, this.treeService);
                }

                this.AddObject(name, @object, tree);
            }
        }

        public void AddObject(string name, IObject @object, TreeNode[] tree)
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
                this.objectByName.Add(name, @object);
                tree?.Resolve(@object, this.objects);
            }
        }

        public void AddCollection(string name, IEnumerable<IObject> collection, bool full = false)
        {
            var inputList = (collection as IList<IObject>) ?? collection?.ToArray() ?? Array.Empty<IObject>();

            TreeNode[] tree = null;
            if (full && inputList.Count > 0)
            {
                var @object = inputList.FirstOrDefault();
                tree = @object?.Strategy.Session.Database.FullTree(@object.Strategy.Class, this.treeService);
            }

            this.AddCollection(name, inputList, tree);
        }

        public void AddCollection(string name, IEnumerable<IObject> collection, TreeNode[] tree)
        {
            if (collection != null)
            {
                if (!this.collectionsByName.TryGetValue(name, out var list))
                {
                    list = new List<IObject>();
                    this.collectionsByName.Add(name, list);
                }

                var inputList = collection as IList<IObject> ?? collection.ToArray();

                // Prefetch
                if (tree != null && inputList.Count > 0)
                {
                    var session = inputList[0].Strategy.Session;
                    var prefetcher = tree.BuildPrefetchPolicy();
                    session.Prefetch(prefetcher, inputList.ToArray());
                }

                list.AddRange(inputList);
                foreach (var namedObject in inputList)
                {
                    this.objects.Add(namedObject);
                    tree?.Resolve(namedObject, this.objects);
                }
            }
        }

        public void AddValue(string name, string value)
        {
            if (value != null)
            {
                this.valueByName.Add(name, value);
            }
        }
    }
}
