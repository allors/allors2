// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Ranges;

    public abstract class Strategy : IStrategy, IComparable<Strategy>
    {
        private readonly long rangeId;

        private IObject @object;

        protected Strategy(Session session, IClass @class, long id)
        {
            this.Session = session;
            this.Id = id;
            this.rangeId = this.Id;
            this.Class = @class;
            this.Ranges = this.Session.Workspace.StrategyRanges;

            if (this.Class.Origin != Origin.Session)
            {
                this.WorkspaceOriginState = new WorkspaceOriginState(this, this.Session.Workspace.GetRecord(this.Id));
            }
        }

        protected Strategy(Session session, DatabaseRecord databaseRecord)
        {
            this.Session = session;
            this.Id = databaseRecord.Id;
            this.rangeId = this.Id;
            this.Class = databaseRecord.Class;
            this.Ranges = this.Session.Workspace.StrategyRanges;

            this.WorkspaceOriginState = new WorkspaceOriginState(this, this.Session.Workspace.GetRecord(this.Id));
        }

        public long Version =>
            this.Class.Origin switch
            {
                Origin.Session => Allors.Version.WorkspaceInitial.Value,
                Origin.Workspace => this.WorkspaceOriginState.Version,
                Origin.Database => this.DatabaseOriginState.Version,
                _ => throw new Exception()
            };

        public Session Session { get; }

        public DatabaseOriginState DatabaseOriginState { get; protected set; }

        public WorkspaceOriginState WorkspaceOriginState { get; }

        public IRanges<Strategy> Ranges { get; }

        ISession IStrategy.Session => this.Session;

        public IClass Class { get; }

        public long Id { get; private set; }

        public bool IsNew => Session.IsNewId(this.Id);

        public IObject Object => this.@object ??= this.Session.Workspace.DatabaseConnection.Configuration.ObjectFactory.Create(this);

        public void Reset()
        {
            this.WorkspaceOriginState?.Reset();
            this.DatabaseOriginState?.Reset();
        }

        public IReadOnlyList<IDiff> Diff()
        {
            var diffs = new List<IDiff>();

            this.WorkspaceOriginState.Diff(diffs);
            this.DatabaseOriginState.Diff(diffs);

            return diffs.ToArray();
        }

        public bool ExistRole(IRoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType) != null;
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole<IObject>(roleType) != null;
            }

            return this.GetCompositesRole<IObject>(roleType).Any();
        }

        public object GetRole(IRoleType roleType)
        {
            if (roleType == null)
            {
                throw new ArgumentNullException(nameof(roleType));
            }

            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType);
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole<IObject>(roleType);
            }

            return this.GetCompositesRole<IObject>(roleType);
        }

        public object GetUnitRole(IRoleType roleType) =>
            roleType.Origin switch
            {
                Origin.Session => this.Session.SessionOriginState.GetUnitRole(this, roleType),
                Origin.Workspace => this.WorkspaceOriginState?.GetUnitRole(roleType),
                Origin.Database => this.CanRead(roleType) ? this.DatabaseOriginState?.GetUnitRole(roleType) : null,
                _ => throw new ArgumentException("Unsupported Origin")
            };

        public T GetCompositeRole<T>(IRoleType roleType) where T : class, IObject =>
            roleType.Origin switch
            {
                Origin.Session => (T)this.Session.SessionOriginState.GetCompositeRole(this, roleType)?.Object,
                Origin.Workspace => (T)this.WorkspaceOriginState?.GetCompositeRole(roleType)?.Object,
                Origin.Database => this.CanRead(roleType) ? (T)this.DatabaseOriginState.GetCompositeRole(roleType)?.Object : null,
                _ => throw new ArgumentException("Unsupported Origin")
            };

        public IEnumerable<T> GetCompositesRole<T>(IRoleType roleType) where T : class, IObject =>
            roleType.Origin switch
            {
                Origin.Session => this.Session.SessionOriginState.GetCompositesRole(this, roleType).Select(v => (T)v.Object),
                Origin.Workspace => this.WorkspaceOriginState?.GetCompositesRole(roleType).Select(v => (T)v.Object),
                Origin.Database => this.CanRead(roleType) ? this.DatabaseOriginState?.GetCompositesRole(roleType).Select(v => (T)v.Object) : null,
                _ => throw new ArgumentException("Unsupported Origin")
            };

        public void SetRole(IRoleType roleType, object value)
        {
            if (roleType.ObjectType.IsUnit)
            {
                this.SetUnitRole(roleType, value);
            }
            else if (roleType.IsOne)
            {
                this.SetCompositeRole(roleType, (IObject)value);
            }
            else
            {
                this.SetCompositesRole(roleType, (IEnumerable<IObject>)value);
            }
        }

        public void SetUnitRole(IRoleType roleType, object value)
        {
            AssertUnit(roleType, value);

            switch (roleType.Origin)
            {
                case Origin.Session:
                    this.Session.SessionOriginState.SetUnitRole(this, roleType, value);
                    break;

                case Origin.Workspace:
                    this.WorkspaceOriginState?.SetUnitRole(roleType, value);
                    break;

                case Origin.Database:
                    if (this.CanWrite(roleType))
                    {
                        this.DatabaseOriginState?.SetUnitRole(roleType, value);
                    }

                    break;
                default:
                    throw new ArgumentException("Unsupported Origin");
            }
        }

        public void SetCompositeRole<T>(IRoleType roleType, T value) where T : class, IObject
        {
            this.AssertComposite(value);

            if (value != null)
            {
                this.AssertSameType(roleType, value);
                this.AssertSameSession(value);
            }

            if (roleType.IsMany)
            {
                throw new ArgumentException($"Given {nameof(roleType)} is the wrong multiplicity");
            }

            switch (roleType.Origin)
            {
                case Origin.Session:
                    this.Session.SessionOriginState.SetCompositeRole(this, roleType, (Strategy)value?.Strategy);
                    break;

                case Origin.Workspace:
                    this.WorkspaceOriginState.SetCompositeRole(roleType, (Strategy)value?.Strategy);

                    break;

                case Origin.Database:
                    if (this.CanWrite(roleType))
                    {
                        this.DatabaseOriginState.SetCompositeRole(roleType, (Strategy)value?.Strategy);
                    }

                    break;
                default:
                    throw new ArgumentException("Unsupported Origin");
            }
        }

        public void SetCompositesRole<T>(IRoleType roleType, in IEnumerable<T> role) where T : class, IObject
        {
            this.AssertComposites(role);

            var roleStrategies = this.Ranges.Load(role?.Select(v => (Strategy)v.Strategy));

            switch (roleType.Origin)
            {
                case Origin.Session:
                    this.Session.SessionOriginState.SetCompositesRole(this, roleType, roleStrategies);
                    break;

                case Origin.Workspace:
                    this.WorkspaceOriginState?.SetCompositesRole(roleType, roleStrategies);

                    break;

                case Origin.Database:
                    if (this.CanWrite(roleType))
                    {
                        this.DatabaseOriginState?.SetCompositesRole(roleType, roleStrategies);
                    }

                    break;
                default:
                    throw new ArgumentException("Unsupported Origin");
            }
        }

        public void AddCompositesRole<T>(IRoleType roleType, T value) where T : class, IObject
        {
            if (value == null)
            {
                return;
            }

            this.AssertComposite(value);

            this.AssertSameType(roleType, value);

            if (roleType.IsOne)
            {
                throw new ArgumentException($"Given {nameof(roleType)} is the wrong multiplicity");
            }

            switch (roleType.Origin)
            {
                case Origin.Session:
                    this.Session.SessionOriginState.AddCompositesRole(this, roleType, (Strategy)value.Strategy);
                    break;

                case Origin.Workspace:
                    this.WorkspaceOriginState.AddCompositesRole(roleType, (Strategy)value.Strategy);
                    break;

                case Origin.Database:
                    if (this.CanWrite(roleType))
                    {
                        this.DatabaseOriginState.AddCompositesRole(roleType, (Strategy)value.Strategy);
                    }

                    break;
                default:
                    throw new ArgumentException("Unsupported Origin");
            }
        }

        public void RemoveCompositesRole<T>(IRoleType roleType, T value) where T : class, IObject
        {
            if (value == null)
            {
                return;
            }

            this.AssertComposite(value);

            switch (roleType.Origin)
            {
                case Origin.Session:
                    this.Session.SessionOriginState.RemoveCompositesRole(this, roleType, (Strategy)value.Strategy);
                    break;

                case Origin.Workspace:
                    this.WorkspaceOriginState.RemoveCompositesRole(roleType, (Strategy)value.Strategy);

                    break;

                case Origin.Database:
                    if (this.CanWrite(roleType))
                    {
                        this.DatabaseOriginState.RemoveCompositesRole(roleType, (Strategy)value.Strategy);
                    }

                    break;
                default:
                    throw new ArgumentException("Unsupported Origin");
            }
        }

        public void RemoveRole(IRoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                this.SetUnitRole(roleType, null);
            }
            else if (roleType.IsOne)
            {
                this.SetCompositeRole(roleType, (IObject)null);
            }
            else
            {
                this.SetCompositesRole(roleType, (IEnumerable<IObject>)null);
            }
        }

        public T GetCompositeAssociation<T>(IAssociationType associationType) where T : class, IObject
        {
            if (associationType.Origin != Origin.Session)
            {
                return (T)this.Session.GetCompositeAssociation(this, associationType)?.Object;
            }

            var association = this.Session.SessionOriginState.GetCompositeAssociation(this, associationType);
            return (T)association?.Object;
        }

        public IEnumerable<T> GetCompositesAssociation<T>(IAssociationType associationType) where T : class, IObject
        {
            if (associationType.Origin != Origin.Session)
            {
                return this.Session.GetCompositesAssociation(this, associationType).Select(v => v.@object).Cast<T>();
            }

            var association = this.Session.SessionOriginState.GetCompositesAssociation(this, associationType);
            return association?.Select(v => (T)v.Object) ?? Array.Empty<T>();
        }

        public bool CanRead(IRoleType roleType) => roleType.RelationType.Origin != Origin.Database || (this.DatabaseOriginState?.CanRead(roleType) ?? true);

        public bool CanWrite(IRoleType roleType) => roleType.RelationType.Origin != Origin.Database || (this.DatabaseOriginState?.CanWrite(roleType) ?? true);

        public bool CanExecute(IMethodType methodType) => methodType.Origin == Origin.Database && (this.DatabaseOriginState?.CanExecute(methodType) ?? false);

        public bool IsCompositeAssociationForRole(IRoleType roleType, Strategy forRole) =>
            roleType.Origin switch
            {
                Origin.Session => Equals(this.Session.SessionOriginState.GetCompositeRole(this, roleType), forRole),
                Origin.Workspace => this.WorkspaceOriginState?.IsAssociationForRole(roleType, forRole) ?? false,
                Origin.Database => this.DatabaseOriginState?.IsAssociationForRole(roleType, forRole) ?? false,
                _ => throw new ArgumentException("Unsupported Origin")
            };

        public bool IsCompositesAssociationForRole(IRoleType roleType, Strategy forRoleId) =>
            roleType.Origin switch
            {
                Origin.Session => this.Session.SessionOriginState.GetCompositesRole(this, roleType).Contains(forRoleId),
                Origin.Workspace => this.WorkspaceOriginState?.IsAssociationForRole(roleType, forRoleId) ?? false,
                Origin.Database => this.DatabaseOriginState?.IsAssociationForRole(roleType, forRoleId) ?? false,
                _ => throw new ArgumentException("Unsupported Origin")
            };

        public void OnDatabasePushNewId(long newId) => this.Id = newId;

        public void OnDatabasePushed() => this.DatabaseOriginState.OnPushed();

        private void AssertSameType<T>(IRoleType roleType, T value) where T : class, IObject
        {
            if (!((IComposite)roleType.ObjectType).IsAssignableFrom(value.Strategy.Class))
            {
                throw new ArgumentException($"Types do not match: {nameof(roleType)}: {roleType.ObjectType.ClrType} and {nameof(value)}: {value.GetType()}");
            }
        }

        private void AssertSameSession(IObject value)
        {
            if (this.Session != value.Strategy.Session)
            {
                throw new ArgumentException($"Session do not match");
            }
        }

        private static void AssertUnit(IRoleType roleType, object value)
        {
            if (value == null)
            {
                return;
            }

            switch (roleType.ObjectType.Tag)
            {
                case UnitTags.Binary:
                    if (!(value is byte[]))
                    {
                        throw new ArgumentException($"{nameof(value)} is not a Binary");
                    }
                    break;
                case UnitTags.Boolean:
                    if (!(value is bool))
                    {
                        throw new ArgumentException($"{nameof(value)} is not an Bool");
                    }
                    break;
                case UnitTags.DateTime:
                    if (!(value is DateTime))
                    {
                        throw new ArgumentException($"{nameof(value)} is not an DateTime");
                    }
                    break;
                case UnitTags.Decimal:
                    if (!(value is decimal))
                    {
                        throw new ArgumentException($"{nameof(value)} is not an Decimal");
                    }
                    break;
                case UnitTags.Float:
                    if (!(value is double))
                    {
                        throw new ArgumentException($"{nameof(value)} is not an Float");
                    }
                    break;
                case UnitTags.Integer:
                    if (!(value is int))
                    {
                        throw new ArgumentException($"{nameof(value)} is not an Integer");
                    }
                    break;
                case UnitTags.String:
                    if (!(value is string))
                    {
                        throw new ArgumentException($"{nameof(value)} is not an String");
                    }
                    break;
                case UnitTags.Unique:
                    if (!(value is Guid))
                    {
                        throw new ArgumentException($"{nameof(value)} is not an Unique");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roleType));
            }
        }

        private void AssertComposite(IObject value)
        {
            if (value == null)
            {
                return;
            }

            if (this.Session != value.Strategy.Session)
            {
                throw new ArgumentException("Strategy is from a different session");
            }
        }

        private void AssertComposites(IEnumerable<IObject> inputs)
        {
            if (inputs == null)
            {
                return;
            }

            foreach (var input in inputs)
            {
                this.AssertComposite(input);
            }
        }

        int IComparable<Strategy>.CompareTo(Strategy other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : this.rangeId.CompareTo(other.rangeId);
        }
    }
}
