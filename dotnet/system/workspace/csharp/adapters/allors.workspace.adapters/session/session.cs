// <copyright file="RemoteSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public abstract class Session : ISession
    {
        private readonly Dictionary<IClass, ISet<Strategy>> strategiesByClass;

        protected Session(Workspace workspace, ISessionServices sessionServices)
        {
            this.Workspace = workspace;
            this.Services = sessionServices;

            this.StrategyByWorkspaceId = new Dictionary<long, Strategy>();
            this.strategiesByClass = new Dictionary<IClass, ISet<Strategy>>();
            this.SessionOriginState = new SessionOriginState(workspace.StrategyRanges);

            this.ChangeSetTracker = new ChangeSetTracker();
            this.PushToDatabaseTracker = new PushToDatabaseTracker();
            this.PushToWorkspaceTracker = new PushToWorkspaceTracker();

            this.Services.OnInit(this);
        }

        public ISessionServices Services { get; }

        IWorkspace ISession.Workspace => this.Workspace;
        public Workspace Workspace { get; }

        public ChangeSetTracker ChangeSetTracker { get; }

        public PushToDatabaseTracker PushToDatabaseTracker { get; }

        public PushToWorkspaceTracker PushToWorkspaceTracker { get; }

        public SessionOriginState SessionOriginState { get; }

        protected Dictionary<long, Strategy> StrategyByWorkspaceId { get; }

        public override string ToString() => $"session: {base.ToString()}";

        internal static bool IsNewId(long id) => id < 0;

        public abstract T Create<T>(IClass @class) where T : class, IObject;

        public T Create<T>() where T : class, IObject => this.Create<T>((IClass)this.Workspace.DatabaseConnection.Configuration.ObjectFactory.GetObjectType<T>());

        public IWorkspaceResult PullFromWorkspace()
        {
            var result = new WorkspaceResult();

            // TODO: Optimize
            foreach (var kvp in this.StrategyByWorkspaceId)
            {
                var strategy = kvp.Value;
                if (strategy.Class.Origin != Origin.Session)
                {
                    strategy.WorkspaceOriginState.OnPulled(result);
                }
            }

            return result;
        }

        public IWorkspaceResult PushToWorkspace()
        {
            var pushResult = new WorkspaceResult();

            if (this.PushToWorkspaceTracker.Created != null)
            {
                foreach (var strategy in this.PushToWorkspaceTracker.Created)
                {
                    strategy.WorkspaceOriginState.Push();
                }
            }

            if (this.PushToWorkspaceTracker.Changed != null)
            {
                foreach (var state in this.PushToWorkspaceTracker.Changed)
                {
                    if (this.PushToWorkspaceTracker.Created?.Contains(state.Strategy) == true)
                    {
                        continue;
                    }

                    state.Push();
                }
            }

            this.PushToWorkspaceTracker.Created = null;
            this.PushToWorkspaceTracker.Changed = null;

            return pushResult;
        }

        public IChangeSet Checkpoint()
        {
            var changeSet = new ChangeSet(this, this.ChangeSetTracker.Created, this.ChangeSetTracker.Instantiated);

            if (this.ChangeSetTracker.DatabaseOriginStates != null)
            {
                foreach (var databaseOriginState in this.ChangeSetTracker.DatabaseOriginStates)
                {
                    databaseOriginState.Checkpoint(changeSet);
                }
            }

            if (this.ChangeSetTracker.WorkspaceOriginStates != null)
            {
                foreach (var workspaceOriginState in this.ChangeSetTracker.WorkspaceOriginStates)
                {
                    workspaceOriginState.Checkpoint(changeSet);
                }
            }

            this.SessionOriginState.Checkpoint(changeSet);

            this.ChangeSetTracker.Created = null;
            this.ChangeSetTracker.Instantiated = null;
            this.ChangeSetTracker.DatabaseOriginStates = null;
            this.ChangeSetTracker.WorkspaceOriginStates = null;

            return changeSet;
        }

        #region Instantiate
        public T Instantiate<T>(IObject @object) where T : class, IObject => this.Instantiate<T>(@object.Id);

        public T Instantiate<T>(T @object) where T : class, IObject => this.Instantiate<T>(@object.Id);

        public T Instantiate<T>(long? id) where T : class, IObject => id.HasValue ? this.Instantiate<T>((long)id) : default;

        public T Instantiate<T>(long id) where T : class, IObject => (T)this.GetStrategy(id)?.Object;

        public T Instantiate<T>(string idAsString) where T : class, IObject => long.TryParse(idAsString, out var id) ? (T)this.GetStrategy(id)?.Object : default;

        public IEnumerable<T> Instantiate<T>(IEnumerable<IObject> objects) where T : class, IObject => objects.Select(this.Instantiate<T>);

        public IEnumerable<T> Instantiate<T>(IEnumerable<T> objects) where T : class, IObject => objects.Select(this.Instantiate);

        public IEnumerable<T> Instantiate<T>(IEnumerable<long> ids) where T : class, IObject => ids.Select(this.Instantiate<T>);

        public IEnumerable<T> Instantiate<T>(IEnumerable<string> ids) where T : class, IObject => this.Instantiate<T>(ids.Select(
            v =>
            {
                long.TryParse(v, out var id);
                return id;
            }));

        public IEnumerable<T> Instantiate<T>() where T : class, IObject
        {
            var objectType = (IComposite)this.Workspace.DatabaseConnection.Configuration.ObjectFactory.GetObjectType<T>();
            return this.Instantiate<T>(objectType);
        }

        public IEnumerable<T> Instantiate<T>(IComposite objectType) where T : class, IObject
        {
            foreach (var @class in objectType.Classes)
            {
                switch (@class.Origin)
                {
                    case Origin.Workspace:
                        if (this.Workspace.WorkspaceIdsByWorkspaceClass.TryGetValue(@class, out var ids))
                        {
                            foreach (var id in ids)
                            {
                                if (this.StrategyByWorkspaceId.TryGetValue(id, out var strategy))
                                {
                                    yield return (T)strategy.Object;
                                }
                                else
                                {
                                    strategy = this.InstantiateWorkspaceStrategy(id);
                                    yield return (T)strategy.Object;
                                }
                            }
                        }

                        break;
                    case Origin.Database:
                    case Origin.Session:
                        if (this.strategiesByClass.TryGetValue(@class, out var strategies))
                        {
                            foreach (var strategy in strategies)
                            {
                                yield return (T)strategy.Object;
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Origin {@class.Origin}");
                }
            }
        }
        #endregion

        public Strategy GetStrategy(long id)
        {
            if (id == 0)
            {
                return null;
            }

            if (this.StrategyByWorkspaceId.TryGetValue(id, out var sessionStrategy))
            {
                return sessionStrategy;
            }

            return IsNewId(id) ? this.InstantiateWorkspaceStrategy(id) : null;
        }

        public Strategy GetCompositeAssociation(Strategy role, IAssociationType associationType)
        {
            var roleType = associationType.RoleType;

            foreach (var association in this.StrategiesForClass(associationType.ObjectType))
            {
                if (!association.CanRead(roleType))
                {
                    continue;
                }

                if (association.IsCompositeAssociationForRole(roleType, role))
                {
                    return association;
                }
            }

            return null;
        }

        public IEnumerable<Strategy> GetCompositesAssociation(Strategy role, IAssociationType associationType)
        {
            var roleType = associationType.RoleType;

            foreach (var association in this.StrategiesForClass(associationType.ObjectType))
            {
                if (!association.CanRead(roleType))
                {
                    continue;
                }

                if (association.IsCompositesAssociationForRole(roleType, role))
                {
                    yield return association;
                }
            }
        }

        protected void AddStrategy(Strategy strategy)
        {
            this.StrategyByWorkspaceId.Add(strategy.Id, strategy);

            var @class = strategy.Class;
            if (!this.strategiesByClass.TryGetValue(@class, out var strategies))
            {
                this.strategiesByClass[@class] = new HashSet<Strategy> { strategy };
            }
            else
            {
                strategies.Add(strategy);
            }
        }

        public void OnDatabasePushResponseNew(long workspaceId, long databaseId)
        {
            var strategy = this.StrategyByWorkspaceId[workspaceId];
            this.PushToDatabaseTracker.Created.Remove(strategy);
            strategy.OnDatabasePushNewId(databaseId);
            this.AddStrategy(strategy);
            strategy.OnDatabasePushed();
        }

        private IEnumerable<Strategy> StrategiesForClass(IComposite objectType)
        {
            // TODO: Optimize
            var classes = new HashSet<IClass>(objectType.Classes);
            return this.StrategyByWorkspaceId.Where(v => classes.Contains(v.Value.Class)).Select(v => v.Value).Distinct();
        }

        protected abstract Strategy InstantiateWorkspaceStrategy(long id);

    }
}
