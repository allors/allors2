// <copyright file="LocalPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Derivations;
    using Database.Meta;
    using Database.Security;
    using Database.Services;
    using Ranges;

    public class Push : Result, IPushResult
    {
        internal Push(Session session) : base(session)
        {
            this.Workspace = session.Workspace;
            this.Transaction = this.Workspace.DatabaseConnection.Database.CreateTransaction();

            var metaCache = this.Transaction.Database.Services.Get<IMetaCache>();

            this.AccessControl = this.Transaction.Services.Get<IWorkspaceAclsService>().Create(this.Workspace.DatabaseConnection.Configuration.Name);
            this.AllowedClasses = metaCache.GetWorkspaceClasses(this.Workspace.DatabaseConnection.Configuration.Name);
            this.M = this.Transaction.Database.MetaPopulation;
            this.Build = @class => this.Transaction.Services.Get<IObjectBuilderService>().Build(@class);
            this.Derive = () => this.Transaction.Database.Services.Get<IDerivationService>().CreateDerivation(this.Transaction).Derive();

            this.Objects = new HashSet<IObject>();
        }

        internal Dictionary<long, IObject> ObjectByNewId { get; private set; }

        internal IAccessControl AccessControl { get; }

        internal ISet<IObject> Objects { get; }

        private Workspace Workspace { get; }

        private ITransaction Transaction { get; }

        private ISet<IClass> AllowedClasses { get; }

        private IMetaPopulation M { get; }

        private Func<IClass, IObject> Build { get; }

        private Func<IValidation> Derive { get; }

        internal void Execute(PushToDatabaseTracker tracker)
        {
            var metaPopulation = this.Workspace.DatabaseConnection.Database.MetaPopulation;

            if (tracker.Created != null)
            {
                this.ObjectByNewId = tracker.Created.ToDictionary(
                    k => k.Id,
                    v =>
                    {
                        var local = (Strategy)v;
                        var cls = (IClass)metaPopulation.FindByTag(v.Class.Tag);
                        if (this.AllowedClasses?.Contains(cls) == true)
                        {
                            var newObject = this.Build(cls);
                            this.Objects.Add(newObject);
                            return newObject;
                        }

                        this.AddAccessError(local);

                        return null;
                    });


                if (this.HasErrors)
                {
                    return;
                }

                foreach (var local in tracker.Created)
                {
                    this.PushRequestRoles((Strategy)local, this.ObjectByNewId[local.Id]);
                }
            }


            if (tracker.Changed != null)
            {
                // bulk load all objects
                var objectIds = tracker.Changed.Select(v => v.Strategy.Id).ToArray();
                var objects = this.Transaction.Instantiate(objectIds);
                this.Objects.UnionWith(objects);

                if (objectIds.Length != objects.Length)
                {
                    var existingIds = objects.Select(v => v.Id);
                    foreach (var missingId in objectIds.Where(v => !existingIds.Contains(v)))
                    {
                        this.AddMissingId(missingId);
                    }
                }

                if (!this.HasErrors)
                {
                    foreach (var state in tracker.Changed)
                    {
                        var strategy = (Strategy)state.Strategy;
                        var obj = this.Transaction.Instantiate(strategy.Id);
                        if (!strategy.DatabaseOriginState.Version.Equals(obj.Strategy.ObjectVersion))
                        {
                            this.AddVersionError(obj.Id);
                        }
                        else if (this.AllowedClasses?.Contains(obj.Strategy.Class) == true)
                        {
                            this.PushRequestRoles(strategy, obj);
                        }
                        else
                        {
                            this.AddAccessError(strategy);
                        }
                    }
                }
            }

            var validation = this.Derive();
            if (validation.HasErrors)
            {
                this.AddDerivationErrors(validation.Errors);
            }

            if (!this.HasErrors)
            {
                this.Transaction.Commit();
            }
        }

        private void PushRequestRoles(Strategy local, IObject obj)
        {
            if (local.DatabaseOriginState.ChangedRoleByRelationType == null)
            {
                return;
            }

            // TODO: Cache and filter for workspace
            var acl = this.AccessControl[obj];

            var ranges = this.Workspace.RecordRanges;

            foreach (var keyValuePair in local.DatabaseOriginState.ChangedRoleByRelationType)
            {
                var relationType = keyValuePair.Key;
                var roleType = ((IRelationType)this.M.FindByTag(keyValuePair.Key.Tag)).RoleType;

                if (acl.CanWrite(roleType))
                {
                    var roleValue = keyValuePair.Value;

                    if (roleValue == null)
                    {
                        obj.Strategy.RemoveRole(roleType);
                    }
                    else if (roleType.ObjectType.IsUnit)
                    {
                        obj.Strategy.SetUnitRole(roleType, roleValue);
                    }
                    else if (relationType.RoleType.IsOne)
                    {
                        var workspaceRole = (Adapters.Strategy)roleValue;
                        IObject role;

                        if (workspaceRole.Id < 0)
                        {
                            this.ObjectByNewId.TryGetValue(workspaceRole.Id, out role);
                        }
                        else
                        {
                            role = this.Transaction.Instantiate(workspaceRole.Id);
                        }

                        obj.Strategy.SetCompositeRole(roleType, role);
                    }
                    else
                    {
                        var workspaceRole = (IRange<Adapters.Strategy>)roleValue;

                        if (this.ObjectByNewId != null)
                        {
                            obj.Strategy.SetCompositesRole(roleType, this.GetRoles(workspaceRole));
                        }
                        else
                        {
                            var role = this.Transaction.Instantiate(workspaceRole.Select(v => v.Id));
                            obj.Strategy.SetCompositesRole(roleType, role);
                        }
                    }
                }
                else
                {
                    this.AddAccessError(local);
                }
            }
        }

        private IEnumerable<IObject> GetRoles(IRange<Adapters.Strategy> strategies)
        {
            foreach (var v in strategies.Select(v => v.Id).Where(v => v < 0))
            {
                this.ObjectByNewId.TryGetValue(v, out var role);
                yield return role;
            }

            var existingIds = strategies.Select(v => v.Id).Where(v => v > 0);
            foreach (var role in this.Transaction.Instantiate(existingIds))
            {
                yield return role;
            }
        }
    }
}
