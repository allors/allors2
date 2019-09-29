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
        private readonly AccessControlListFactory aclFactory;
        private readonly ITreeService treeService;

        private readonly Dictionary<IObject, IAccessControlList> aclByObject = new Dictionary<IObject, IAccessControlList>();

        private readonly Dictionary<string, IObject> objectByName = new Dictionary<string, IObject>();
        private readonly Dictionary<string, List<IObject>> collectionsByName = new Dictionary<string, List<IObject>>();
        private readonly Dictionary<string, string> valueByName = new Dictionary<string, string>();

        private readonly AccessControlsCompression accessControlsCompression;
        private readonly DeniedPermissionsCompression deniedPermissionsCompression;

        public PullResponseBuilder(User user, ITreeService treeService)
        {
            this.aclFactory = new AccessControlListFactory(user);
            this.aclByObject = new Dictionary<IObject, IAccessControlList>();
            this.treeService = treeService;

            this.accessControlsCompression = new AccessControlsCompression();
            this.deniedPermissionsCompression = new DeniedPermissionsCompression();
        }

        public PullResponse Build() =>
            new PullResponse
            {
                Objects = this.aclByObject.Select(kvp => new[]
                {
                    kvp.Key.Id.ToString(),
                    kvp.Value.Object.Id.ToString(),
                    this.accessControlsCompression.Write(kvp.Value),
                    this.deniedPermissionsCompression.Write(kvp.Value),
                }).ToArray(),
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

                this.objectByName.Add(name, @object);
                tree?.Resolve(@object, this.aclFactory, this.aclByObject);
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
                    tree?.Resolve(namedObject, this.aclFactory, this.aclByObject);
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
