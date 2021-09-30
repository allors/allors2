// <copyright file="PullResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json;
    using Allors.Protocol.Json.Api.Pull;
    using Data;
    using Derivations;
    using Meta;
    using Ranges;
    using Security;
    using Services;

    public class PullResponseBuilder : IProcedureContext, IProcedureOutput
    {
        private readonly IUnitConvert unitConvert;
        private readonly IRanges<long> ranges;

        private readonly Dictionary<string, ISet<IObject>> collectionsByName = new Dictionary<string, ISet<IObject>>();
        private readonly Dictionary<string, IObject> objectByName = new Dictionary<string, IObject>();
        private readonly Dictionary<string, object> valueByName = new Dictionary<string, object>();

        private readonly HashSet<IObject> objects;

        private List<IValidation> errors;

        public PullResponseBuilder(ITransaction transaction, IAccessControl accessControl, ISet<IClass> allowedClasses, IPreparedSelects preparedSelects, IPreparedExtents preparedExtents, IUnitConvert unitConvert, IRanges<long> ranges)
        {
            this.unitConvert = unitConvert;
            this.ranges = ranges;
            this.Transaction = transaction;

            this.AccessControl = accessControl;
            this.AllowedClasses = allowedClasses;
            this.PreparedSelects = preparedSelects;
            this.PreparedExtents = preparedExtents;

            this.objects = new HashSet<IObject>();
        }

        public ITransaction Transaction { get; }

        public IAccessControl AccessControl { get; }

        public ISet<IClass> AllowedClasses { get; }

        public IPreparedSelects PreparedSelects { get; }

        public IPreparedExtents PreparedExtents { get; }

        public void AddError(IValidation validation)
        {
            this.errors ??= new List<IValidation>();
            this.errors.Add(validation);
        }

        public void AddCollection(string name, in IEnumerable<IObject> collection)
        {
            switch (collection)
            {
                case ICollection<IObject> asCollection:
                    this.AddCollectionInternal(name, asCollection, null);
                    break;
                default:
                    this.AddCollectionInternal(name, collection.ToArray(), null);
                    break;
            }
        }

        public void AddCollection(string name, in ICollection<IObject> collection) => this.AddCollectionInternal(name, collection, null);

        public void AddCollection(string name, in IEnumerable<IObject> collection, Node[] tree)
        {
            switch (collection)
            {
                case ICollection<IObject> list:
                    this.AddCollectionInternal(name, list, tree);
                    break;
                default:
                {
                    this.AddCollectionInternal(name, collection.ToArray(), tree);
                    break;
                }
            }
        }

        public void AddObject(string name, IObject @object)
        {
            if (@object != null)
            {
                this.AddObject(name, @object, null);
            }
        }

        public void AddObject(string name, IObject @object, Node[] tree)
        {
            if (@object != null && this.AllowedClasses?.Contains(@object.Strategy.Class) == true)
            {
                if (tree != null)
                {
                    // Prefetch
                    var transaction = @object.Strategy.Transaction;
                    var prefetcher = tree.BuildPrefetchPolicy();
                    transaction.Prefetch(prefetcher, @object);
                }

                this.objects.Add(@object);
                this.objectByName[name] = @object;
                tree?.Resolve(@object, this.AccessControl, this.objects);
            }
        }

        public void AddValue(string name, object value)
        {
            if (value != null)
            {
                this.valueByName.Add(name, value);
            }
        }

        private void AddCollectionInternal(string name, in ICollection<IObject> collection, Node[] tree)
        {
            if (collection?.Count > 0)
            {
                this.collectionsByName.TryGetValue(name, out var existingCollection);

                var filteredCollection = collection.Where(v => this.AllowedClasses != null && this.AllowedClasses.Contains(v.Strategy.Class));

                if (tree != null)
                {
                    var prefetchPolicy = tree.BuildPrefetchPolicy();

                    ICollection<IObject> newCollection;

                    if (existingCollection != null)
                    {
                        newCollection = filteredCollection.ToArray();
                        this.Transaction.Prefetch(prefetchPolicy, newCollection);
                        existingCollection.UnionWith(newCollection);
                    }
                    else
                    {
                        var newSet = new HashSet<IObject>(filteredCollection);
                        newCollection = newSet;
                        this.Transaction.Prefetch(prefetchPolicy, newCollection);
                        this.collectionsByName.Add(name, newSet);
                    }

                    this.objects.UnionWith(newCollection);

                    foreach (var newObject in newCollection)
                    {
                        tree.Resolve(newObject, this.AccessControl, this.objects);
                    }
                }
                else if (existingCollection != null)
                {
                    existingCollection.UnionWith(filteredCollection);
                }
                else
                {
                    var newWorkspaceCollection = new HashSet<IObject>(filteredCollection);
                    this.collectionsByName.Add(name, newWorkspaceCollection);
                    this.objects.UnionWith(newWorkspaceCollection);
                }
            }
        }

        public PullResponse Build(PullRequest pullRequest = null)
        {
            var pullResponse = new PullResponse();

            var procedure = pullRequest?.p?.FromJson(this.Transaction, this.unitConvert);
            if (procedure != null)
            {
                if (procedure.Pool != null)
                {
                    foreach (var kvp in procedure.Pool)
                    {
                        var @object = kvp.Key;
                        var version = kvp.Value;

                        if (!@object.Strategy.ObjectVersion.Equals(version))
                        {
                            pullResponse.AddVersionError(@object);
                        }
                    }

                    if (pullResponse.HasErrors)
                    {
                        return pullResponse;
                    }
                }

                var procedures = this.Transaction.Database.Services.Get<IProcedures>();
                var proc = procedures.Get(procedure.Name);
                if (proc == null)
                {
                    pullResponse._e = $"Missing procedure {procedure.Name}";
                    return pullResponse;
                }

                var input = new ProcedureInput(this.Transaction.Database.ObjectFactory, procedure);

                proc.Execute(this, input, this);

                if (this.errors?.Count > 0)
                {
                    foreach (var error in this.errors)
                    {
                        pullResponse.AddDerivationErrors(error);
                    }

                    return pullResponse;
                }
            }

            if (pullRequest?.l != null)
            {
                foreach (var p in pullRequest.l)
                {
                    var pull = p.FromJson(this.Transaction, this.unitConvert);

                    if (pull.Object != null)
                    {
                        var pullInstantiate = new PullInstantiate(this.Transaction, pull, this.AccessControl, this.PreparedSelects);
                        pullInstantiate.Execute(this);
                    }
                    else
                    {
                        var pullExtent = new PullExtent(this.Transaction, pull, this.AccessControl, this.PreparedSelects, this.PreparedExtents);
                        pullExtent.Execute(this);
                    }
                }
            }

            var grants = new HashSet<IGrant>();
            var revocations = new HashSet<IRevocation>();

            pullResponse = new PullResponse
            {
                p = this.objects.Select(v =>
                {
                    var accessControlList = this.AccessControl[v];

                    grants.UnionWith(accessControlList.Grants);
                    revocations.UnionWith(accessControlList.Revocations);

                    return new PullResponseObject
                    {
                        i = v.Strategy.ObjectId,
                        v = v.Strategy.ObjectVersion,
                        g = this.ranges.Import(accessControlList.Grants.Select(w => w.Strategy.ObjectId)).Save(),
                        r = this.ranges.Import(accessControlList.Revocations.Select(w => w.Strategy.ObjectId))
                            .Save(),
                    };
                }).ToArray(),
                o = this.objectByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Id),
                c = this.collectionsByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(obj => obj.Id).ToArray()),
                v = this.valueByName,
            };

            pullResponse.g = grants.Count > 0 ? grants.Select(v => new[] { v.Strategy.ObjectId, v.Strategy.ObjectVersion }).ToArray() : null;
            pullResponse.r = revocations.Count > 0 ? revocations.Select(v => new[] { v.Strategy.ObjectId, v.Strategy.ObjectVersion }).ToArray() : null;

            return pullResponse;
        }
    }
}
