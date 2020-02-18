// <copyright file="Strategy.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Allors;
    using Adapters;
    using Allors.Meta;

    public sealed class Strategy : IStrategy
    {
        private readonly Dictionary<IRoleType, object> unitRoleByRoleType;
        private readonly Dictionary<IRoleType, Strategy> compositeRoleByRoleType;
        private readonly Dictionary<IRoleType, HashSet<Strategy>> compositesRoleByRoleType;
        private readonly Dictionary<IAssociationType, Strategy> compositeAssociationByAssociationType;
        private readonly Dictionary<IAssociationType, HashSet<Strategy>> compositesAssociationByAssociationType;

        private Dictionary<IRoleType, object> rollbackUnitRoleByRoleType;
        private Dictionary<IRoleType, Strategy> rollbackCompositeRoleByRoleType;
        private Dictionary<IRoleType, HashSet<Strategy>> rollbackCompositesRoleByRoleType;
        private Dictionary<IAssociationType, Strategy> rollbackCompositeAssociationByAssociationType;
        private Dictionary<IAssociationType, HashSet<Strategy>> rollbackCompositesAssociationByAssociationType;
        private bool isDeletedOnRollback;
        private WeakReference allorizedObjectWeakReference;

        internal Strategy(Session session, IClass objectType, long objectId, long version)
        {
            this.MemorySession = session;
            this.UncheckedObjectType = objectType;
            this.ObjectId = objectId;

            this.IsDeleted = false;
            this.isDeletedOnRollback = true;
            this.IsNewInSession = true;

            this.ObjectVersion = version;

            this.unitRoleByRoleType = new Dictionary<IRoleType, object>();
            this.compositeRoleByRoleType = new Dictionary<IRoleType, Strategy>();
            this.compositesRoleByRoleType = new Dictionary<IRoleType, HashSet<Strategy>>();
            this.compositeAssociationByAssociationType = new Dictionary<IAssociationType, Strategy>();
            this.compositesAssociationByAssociationType = new Dictionary<IAssociationType, HashSet<Strategy>>();

            this.rollbackUnitRoleByRoleType = null;
            this.rollbackCompositeRoleByRoleType = null;
            this.rollbackCompositesRoleByRoleType = null;
            this.rollbackCompositeAssociationByAssociationType = null;
            this.rollbackCompositesAssociationByAssociationType = null;
        }

        public bool IsDeleted { get; private set; }

        public bool IsNewInSession { get; private set; }

        public long ObjectId { get; }

        public long ObjectVersion { get; private set; }

        public IClass Class
        {
            get
            {
                this.AssertNotDeleted();
                return this.UncheckedObjectType;
            }
        }

        public ISession Session => this.MemorySession;

        internal IClass UncheckedObjectType { get; }

        internal Session MemorySession { get; }

        private ChangeSet ChangeSet => this.MemorySession.MemoryChangeSet;

        private Dictionary<IRoleType, object> RollbackUnitRoleByRoleType => this.rollbackUnitRoleByRoleType ?? (this.rollbackUnitRoleByRoleType = new Dictionary<IRoleType, object>());

        private Dictionary<IRoleType, Strategy> RollbackCompositeRoleByRoleType => this.rollbackCompositeRoleByRoleType ?? (this.rollbackCompositeRoleByRoleType = new Dictionary<IRoleType, Strategy>());

        private Dictionary<IRoleType, HashSet<Strategy>> RollbackCompositesRoleByRoleType => this.rollbackCompositesRoleByRoleType ?? (this.rollbackCompositesRoleByRoleType = new Dictionary<IRoleType, HashSet<Strategy>>());

        private Dictionary<IAssociationType, Strategy> RollbackCompositeAssociationByAssociationType => this.rollbackCompositeAssociationByAssociationType ?? (this.rollbackCompositeAssociationByAssociationType = new Dictionary<IAssociationType, Strategy>());

        private Dictionary<IAssociationType, HashSet<Strategy>> RollbackCompositesAssociationByAssociationType => this.rollbackCompositesAssociationByAssociationType ?? (this.rollbackCompositesAssociationByAssociationType = new Dictionary<IAssociationType, HashSet<Strategy>>());

        public override string ToString() => this.UncheckedObjectType.Name + " " + this.ObjectId;

        public object GetRole(IRelationType relationType)
        {
            var roleType = relationType.RoleType;

            if (roleType.ObjectType is IUnit)
            {
                return this.GetUnitRole(relationType);
            }

            return roleType.IsMany ? (object)this.GetCompositeRoles(relationType) : this.GetCompositeRole(relationType);
        }

        public void SetRole(IRelationType relationType, object value)
        {
            if (relationType.RoleType.ObjectType is IUnit)
            {
                this.SetUnitRole(relationType, value);
            }
            else
            {
                if (relationType.RoleType.IsMany)
                {
                    if (!(value is Allors.Extent roles))
                    {
                        var roleList = new List<IObject>((IEnumerable<IObject>)value);
                        roles = roleList.ToArray();
                    }

                    this.SetCompositeRoles(relationType, roles);
                }
                else
                {
                    this.SetCompositeRole(relationType, (IObject)value);
                }
            }
        }

        public void RemoveRole(IRelationType relationType)
        {
            if (relationType.RoleType.ObjectType is IUnit)
            {
                this.RemoveUnitRole(relationType);
            }
            else
            {
                if (relationType.RoleType.IsMany)
                {
                    this.RemoveCompositeRoles(relationType);
                }
                else
                {
                    this.RemoveCompositeRole(relationType);
                }
            }
        }

        public bool ExistRole(IRelationType relationType)
        {
            if (relationType.RoleType.ObjectType is IUnit)
            {
                return this.ExistUnitRole(relationType);
            }

            return relationType.RoleType.IsMany ? this.ExistCompositeRoles(relationType) : this.ExistCompositeRole(relationType);
        }

        public object GetUnitRole(IRelationType relationType)
        {
            this.AssertNotDeleted();

            var roleType = relationType.RoleType;
            this.MemorySession.MemoryDatabase.UnitRoleChecks(this, roleType);

            return this.GetInternalizedUnitRole(roleType);
        }

        public void SetUnitRole(IRelationType relationType, object role)
        {
            this.AssertNotDeleted();
            var roleType = relationType.RoleType;
            this.MemorySession.MemoryDatabase.UnitRoleChecks(this, roleType);

            var previousRole = this.GetInternalizedUnitRole(roleType);
            role = roleType.Normalize(role);

            if (role != previousRole)
            {
                if (!this.RollbackUnitRoleByRoleType.ContainsKey(roleType))
                {
                    this.RollbackUnitRoleByRoleType[roleType] = this.GetInternalizedUnitRole(roleType);
                }

                this.ChangeSet.OnChangingUnitRole(this.ObjectId, roleType);

                if (role == null)
                {
                    this.unitRoleByRoleType.Remove(roleType);
                }
                else
                {
                    this.unitRoleByRoleType[roleType] = role;
                }
            }
        }

        public void RemoveUnitRole(IRelationType relationType) => this.SetUnitRole(relationType, null);

        public bool ExistUnitRole(IRelationType relationType)
        {
            this.AssertNotDeleted();
            var roleType = relationType.RoleType;
            this.MemorySession.MemoryDatabase.UnitRoleChecks(this, roleType);
            return this.unitRoleByRoleType.ContainsKey(roleType);
        }

        public IObject GetCompositeRole(IRelationType relationType)
        {
            this.AssertNotDeleted();
            var roleType = relationType.RoleType;

            this.compositeRoleByRoleType.TryGetValue(roleType, out var strategy);

            return strategy?.GetObject();
        }

        public void SetCompositeRole(IRelationType relationType, IObject newRole)
        {
            if (newRole == null)
            {
                this.RemoveCompositeRole(relationType);
            }
            else
            {
                if (relationType.AssociationType.IsOne)
                {
                    // 1-1
                    this.SetCompositeRoleOne2One(relationType.RoleType, newRole);
                }
                else
                {
                    // *-1
                    this.SetCompositeRoleMany2One(relationType.RoleType, newRole);
                }
            }
        }

        public void RemoveCompositeRole(IRelationType relationType)
        {
            if (relationType.AssociationType.IsOne)
            {
                // 1-1
                this.RemoveCompositeRoleOne2One(relationType.RoleType);
            }
            else
            {
                // *-1
                this.RemoveCompositeRoleMany2One(relationType.RoleType);
            }
        }

        public bool ExistCompositeRole(IRelationType relationType)
        {
            this.AssertNotDeleted();
            return this.compositeRoleByRoleType.ContainsKey(relationType.RoleType);
        }

        public Allors.Extent GetCompositeRoles(IRelationType relationType)
        {
            this.AssertNotDeleted();

            return new ExtentSwitch(this, relationType.RoleType);
        }

        public void SetCompositeRoles(IRelationType relationType, Allors.Extent roles)
        {
            if (roles == null || roles.Count == 0)
            {
                this.RemoveCompositeRoles(relationType);
            }
            else
            {
                this.AssertNotDeleted();

                var newStrategies = new HashSet<Strategy>();
                foreach (IObject role in roles)
                {
                    if (role != null)
                    {
                        this.MemorySession.MemoryDatabase.CompositeRolesChecks(this, relationType.RoleType, role);

                        var roleStrategy = this.MemorySession.GetStrategy(role.Strategy.ObjectId);
                        newStrategies.Add(roleStrategy);
                    }
                }

                if (relationType.AssociationType.IsMany)
                {
                    this.SetCompositeRolesMany2Many(relationType.RoleType, newStrategies);
                }
                else
                {
                    this.SetCompositesRoleOne2Many(relationType.RoleType, newStrategies);
                }
            }
        }

        public void AddCompositeRole(IRelationType relationType, IObject role)
        {
            this.AssertNotDeleted();
            if (role != null)
            {
                this.MemorySession.MemoryDatabase.CompositeRolesChecks(this, relationType.RoleType, role);

                var roleStrategy = this.MemorySession.GetStrategy(role.Strategy.ObjectId);

                if (relationType.AssociationType.IsMany)
                {
                    this.AddCompositeRoleMany2Many(relationType.RoleType, roleStrategy);
                }
                else
                {
                    this.AddCompositeRoleOne2Many(relationType.RoleType, roleStrategy);
                }
            }
        }

        public void RemoveCompositeRole(IRelationType relationType, IObject role)
        {
            this.AssertNotDeleted();
            if (role != null)
            {
                this.MemorySession.MemoryDatabase.CompositeRolesChecks(this, relationType.RoleType, role);

                var roleStrategy = this.MemorySession.GetStrategy(role.Strategy.ObjectId);

                if (relationType.AssociationType.IsMany)
                {
                    this.RemoveCompositeRoleMany2Many(relationType.RoleType, roleStrategy);
                }
                else
                {
                    this.RemoveCompositeRoleOne2Many(relationType.RoleType, roleStrategy);
                }
            }
        }

        public void RemoveCompositeRoles(IRelationType relationType)
        {
            this.AssertNotDeleted();
            if (relationType.AssociationType.IsMany)
            {
                this.RemoveCompositeRolesMany2Many(relationType.RoleType);
            }
            else
            {
                this.RemoveCompositeRolesOne2Many(relationType.RoleType);
            }
        }

        public bool ExistCompositeRoles(IRelationType relationType)
        {
            this.AssertNotDeleted();
            this.compositesRoleByRoleType.TryGetValue(relationType.RoleType, out var roleStrategies);
            return roleStrategies != null;
        }

        public object GetAssociation(IRelationType relationType) => relationType.AssociationType.IsMany ? (object)this.GetCompositeAssociations(relationType) : this.GetCompositeAssociation(relationType);

        public bool ExistAssociation(IRelationType relationType) => relationType.AssociationType.IsMany ? this.ExistCompositeAssociations(relationType) : this.ExistCompositeAssociation(relationType);

        public IObject GetCompositeAssociation(IRelationType relationType)
        {
            this.AssertNotDeleted();
            this.compositeAssociationByAssociationType.TryGetValue(relationType.AssociationType, out var strategy);
            return strategy?.GetObject();
        }

        public bool ExistCompositeAssociation(IRelationType relationType) => this.GetCompositeAssociation(relationType) != null;

        public Allors.Extent GetCompositeAssociations(IRelationType relationType)
        {
            this.AssertNotDeleted();

            return new ExtentSwitch(this, relationType.AssociationType);
        }

        public bool ExistCompositeAssociations(IRelationType relationType)
        {
            this.AssertNotDeleted();
            this.compositesAssociationByAssociationType.TryGetValue(relationType.AssociationType, out var strategies);

            return strategies != null;
        }

        public void Delete()
        {
            this.AssertNotDeleted();

            // Roles
            foreach (var roleType in this.UncheckedObjectType.RoleTypes)
            {
                var relationType = roleType.RelationType;

                if (this.ExistRole(relationType))
                {
                    if (roleType.ObjectType is IUnit)
                    {
                        this.RemoveUnitRole(relationType);
                    }
                    else
                    {
                        var associationType = roleType.AssociationType;
                        if (associationType.IsMany)
                        {
                            if (roleType.IsMany)
                            {
                                this.RemoveCompositeRolesMany2Many(roleType);
                            }
                            else
                            {
                                this.RemoveCompositeRoleMany2One(roleType);
                            }
                        }
                        else
                        {
                            if (roleType.IsMany)
                            {
                                this.RemoveCompositeRolesOne2Many(roleType);
                            }
                            else
                            {
                                this.RemoveCompositeRoleOne2One(roleType);
                            }
                        }
                    }
                }
            }

            // Associations
            foreach (var associationType in this.UncheckedObjectType.AssociationTypes)
            {
                var relationType = associationType.RelationType;
                var roleType = relationType.RoleType;

                if (this.ExistAssociation(relationType))
                {
                    if (associationType.IsMany)
                    {
                        this.compositesAssociationByAssociationType.TryGetValue(associationType, out var associationStrategies);

                        // TODO: Optimize
                        if (associationStrategies != null)
                        {
                            foreach (var associationStrategy in new HashSet<Strategy>(associationStrategies))
                            {
                                if (roleType.IsMany)
                                {
                                    associationStrategy.RemoveCompositeRoleMany2Many(roleType, this);
                                }
                                else
                                {
                                    associationStrategy.RemoveCompositeRoleMany2One(roleType);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.compositeAssociationByAssociationType.TryGetValue(associationType, out var associationStrategy);

                        if (associationStrategy != null)
                        {
                            if (roleType.IsMany)
                            {
                                associationStrategy.RemoveCompositeRoleOne2Many(roleType, this);
                            }
                            else
                            {
                                associationStrategy.RemoveCompositeRoleOne2One(roleType);
                            }
                        }
                    }
                }
            }

            this.IsDeleted = true;

            this.ChangeSet.OnDeleted(this.ObjectId);
        }

        public IObject GetObject()
        {
            IObject allorsObject;
            if (this.allorizedObjectWeakReference == null)
            {
                allorsObject = this.MemorySession.Database.ObjectFactory.Create(this);
                this.allorizedObjectWeakReference = new WeakReference(allorsObject);
            }
            else
            {
                allorsObject = (IObject)this.allorizedObjectWeakReference.Target;
                if (allorsObject == null)
                {
                    allorsObject = this.MemorySession.Database.ObjectFactory.Create(this);
                    this.allorizedObjectWeakReference.Target = allorsObject;
                }
            }

            return allorsObject;
        }

        internal void Commit()
        {
            if (!this.IsDeleted && !this.MemorySession.MemoryDatabase.IsLoading)
            {
                if (this.rollbackUnitRoleByRoleType != null ||
                    this.rollbackCompositeRoleByRoleType != null ||
                    this.rollbackCompositeRoleByRoleType != null ||
                    this.rollbackCompositeRoleByRoleType != null ||
                    this.rollbackCompositeRoleByRoleType != null ||
                    this.rollbackCompositeRoleByRoleType != null ||
                    this.IsNewInSession)
                {
                    ++this.ObjectVersion;
                }
            }

            this.rollbackUnitRoleByRoleType = null;
            this.rollbackCompositeRoleByRoleType = null;
            this.rollbackCompositesRoleByRoleType = null;
            this.rollbackCompositeAssociationByAssociationType = null;
            this.rollbackCompositesAssociationByAssociationType = null;

            this.isDeletedOnRollback = this.IsDeleted;
            this.IsNewInSession = false;
        }

        internal void Rollback()
        {
            foreach (var dictionaryItem in this.RollbackUnitRoleByRoleType)
            {
                var roleType = dictionaryItem.Key;
                var role = dictionaryItem.Value;

                if (role != null)
                {
                    this.unitRoleByRoleType[roleType] = role;
                }
                else
                {
                    this.unitRoleByRoleType.Remove(roleType);
                }
            }

            foreach (var dictionaryItem in this.RollbackCompositeRoleByRoleType)
            {
                var roleType = dictionaryItem.Key;
                var role = dictionaryItem.Value;

                if (role != null)
                {
                    this.compositeRoleByRoleType[roleType] = role;
                }
                else
                {
                    this.compositeRoleByRoleType.Remove(roleType);
                }
            }

            foreach (var dictionaryItem in this.RollbackCompositesRoleByRoleType)
            {
                var roleType = dictionaryItem.Key;
                var role = dictionaryItem.Value;

                if (role != null)
                {
                    this.compositesRoleByRoleType[roleType] = role;
                }
                else
                {
                    this.compositesRoleByRoleType.Remove(roleType);
                }
            }

            foreach (var dictionaryItem in this.RollbackCompositeAssociationByAssociationType)
            {
                var associationType = dictionaryItem.Key;
                var association = dictionaryItem.Value;

                if (association != null)
                {
                    this.compositeAssociationByAssociationType[associationType] = association;
                }
                else
                {
                    this.compositeAssociationByAssociationType.Remove(associationType);
                }
            }

            foreach (var dictionaryItem in this.RollbackCompositesAssociationByAssociationType)
            {
                var associationType = dictionaryItem.Key;
                var association = dictionaryItem.Value;

                if (association != null)
                {
                    this.compositesAssociationByAssociationType[associationType] = association;
                }
                else
                {
                    this.compositesAssociationByAssociationType.Remove(associationType);
                }
            }

            this.rollbackUnitRoleByRoleType = null;
            this.rollbackCompositeRoleByRoleType = null;
            this.rollbackCompositesRoleByRoleType = null;
            this.rollbackCompositeAssociationByAssociationType = null;
            this.rollbackCompositesAssociationByAssociationType = null;

            this.IsDeleted = this.isDeletedOnRollback;
            this.IsNewInSession = false;
        }

        internal object GetInternalizedUnitRole(IRoleType roleType)
        {
            this.unitRoleByRoleType.TryGetValue(roleType, out var unitRole);
            return unitRole;
        }

        internal List<Strategy> GetStrategies(IAssociationType associationType)
        {
            this.compositesAssociationByAssociationType.TryGetValue(associationType, out var strategies);
            if (strategies == null)
            {
                return new List<Strategy>();
            }

            return strategies.ToList();
        }

        internal List<Strategy> GetStrategies(IRoleType roleType)
        {
            this.compositesRoleByRoleType.TryGetValue(roleType, out var strategies);
            if (strategies == null)
            {
                return new List<Strategy>();
            }

            return strategies.ToList();
        }

        internal void SetCompositeRoleOne2One(IRoleType roleType, IObject newRole)
        {
            this.AssertNotDeleted();
            this.MemorySession.MemoryDatabase.CompositeRoleChecks(this, roleType, newRole);

            var previousRole = this.GetCompositeRole(roleType.RelationType);

            if (!newRole.Equals(previousRole))
            {
                this.ChangeSet.OnChangingCompositeRole(this.ObjectId, roleType, previousRole?.Id, newRole?.Id);

                var newRoleStrategy = this.MemorySession.GetStrategy(newRole);

                if (previousRole != null)
                {
                    // previous role
                    var previousRoleStrategy = this.MemorySession.GetStrategy(previousRole);
                    var associationType = roleType.AssociationType;
                    previousRoleStrategy.Backup(associationType);
                    previousRoleStrategy.compositeAssociationByAssociationType.Remove(associationType);
                }

                // previous association of newRole
                var newRolePreviousAssociation = newRoleStrategy.GetCompositeAssociation(roleType.RelationType);
                if (newRolePreviousAssociation != null)
                {
                    var newRolePreviousAssociationStrategy = this.MemorySession.GetStrategy(newRolePreviousAssociation);
                    if (!this.Equals(newRolePreviousAssociationStrategy))
                    {
                        this.ChangeSet.OnChangingCompositeRole(newRolePreviousAssociationStrategy.ObjectId, roleType, previousRole?.Id, null);

                        newRolePreviousAssociationStrategy.Backup(roleType);
                        newRolePreviousAssociationStrategy.compositeRoleByRoleType.Remove(roleType);
                    }
                }

                // Set new role
                this.Backup(roleType);
                this.compositeRoleByRoleType[roleType] = newRoleStrategy;

                // Set new role's association
                var associationType1 = roleType.AssociationType;
                newRoleStrategy.Backup(associationType1);
                newRoleStrategy.compositeAssociationByAssociationType[associationType1] = this;
            }
        }

        internal void SetCompositeRoleMany2One(IRoleType roleType, IObject newRole)
        {
            this.AssertNotDeleted();
            this.MemorySession.MemoryDatabase.CompositeRoleChecks(this, roleType, newRole);

            var previousRole = this.GetCompositeRole(roleType.RelationType);

            if (!newRole.Equals(previousRole))
            {
                this.ChangeSet.OnChangingCompositeRole(this.ObjectId, roleType, previousRole?.Id, newRole?.Id);

                var associationType = roleType.AssociationType;

                // Update association of previous role
                if (previousRole != null)
                {
                    var previousRoleStrategy = this.MemorySession.GetStrategy(previousRole);
                    previousRoleStrategy.compositesAssociationByAssociationType.TryGetValue(associationType, out var previousRoleStrategies);

                    previousRoleStrategy.Backup(associationType);
                    previousRoleStrategies.Remove(this);
                    if (previousRoleStrategies.Count == 0)
                    {
                        previousRoleStrategy.compositesAssociationByAssociationType.Remove(associationType);
                    }
                }

                var newRoleStrategy = this.MemorySession.GetStrategy(newRole);

                this.Backup(roleType);
                this.compositeRoleByRoleType[roleType] = newRoleStrategy;

                newRoleStrategy.compositesAssociationByAssociationType.TryGetValue(associationType, out var strategies);

                newRoleStrategy.Backup(associationType);
                if (strategies == null)
                {
                    strategies = new HashSet<Strategy>();
                    newRoleStrategy.compositesAssociationByAssociationType[associationType] = strategies;
                }

                strategies.Add(this);
            }
        }

        internal void SetCompositesRoleOne2Many(IRoleType roleType, HashSet<Strategy> roles)
        {
            this.AssertNotDeleted();

            var originalRoles = this.GetStrategies(roleType);

            if (!roles.SetEquals(originalRoles))
            {
                // TODO: Optimize
                this.RemoveCompositeRolesOne2Many(roleType);

                // TODO: Optimize this
                foreach (var strategy in roles)
                {
                    this.AddCompositeRoleOne2Many(roleType, strategy);
                }
            }
        }

        internal void SetCompositeRolesMany2Many(IRoleType roleType, HashSet<Strategy> roles)
        {
            this.AssertNotDeleted();

            var originalRoles = this.GetStrategies(roleType);

            if (!roles.SetEquals(originalRoles))
            {
                this.RemoveCompositeRolesMany2Many(roleType);

                // TODO: Optimize this
                foreach (var strategy in roles)
                {
                    this.AddCompositeRoleMany2Many(roleType, strategy);
                }
            }
        }

        internal void FillRoleForSave(Dictionary<IRoleType, List<Strategy>> strategiesByRoleType)
        {
            if (this.IsDeleted)
            {
                return;
            }

            if (this.unitRoleByRoleType != null)
            {
                foreach (var dictionaryEntry in this.unitRoleByRoleType)
                {
                    var roleType = dictionaryEntry.Key;

                    if (!strategiesByRoleType.TryGetValue(roleType, out var strategies))
                    {
                        strategies = new List<Strategy>();
                        strategiesByRoleType.Add(roleType, strategies);
                    }

                    strategies.Add(this);
                }
            }

            if (this.compositeRoleByRoleType != null)
            {
                foreach (var dictionaryEntry in this.compositeRoleByRoleType)
                {
                    var roleType = dictionaryEntry.Key;

                    if (!strategiesByRoleType.TryGetValue(roleType, out var strategies))
                    {
                        strategies = new List<Strategy>();
                        strategiesByRoleType.Add(roleType, strategies);
                    }

                    strategies.Add(this);
                }
            }

            if (this.compositesRoleByRoleType != null)
            {
                foreach (var dictionaryEntry in this.compositesRoleByRoleType)
                {
                    var roleType = dictionaryEntry.Key;

                    if (!strategiesByRoleType.TryGetValue(roleType, out var strategies))
                    {
                        strategies = new List<Strategy>();
                        strategiesByRoleType.Add(roleType, strategies);
                    }

                    strategies.Add(this);
                }
            }
        }

        internal void SaveUnit(XmlWriter writer, IRoleType roleType)
        {
            var unitType = (IUnit)roleType.ObjectType;
            var value = Serialization.WriteString(unitType.UnitTag, this.unitRoleByRoleType[roleType]);

            writer.WriteStartElement(Serialization.Relation);
            writer.WriteAttributeString(Serialization.Association, this.ObjectId.ToString());
            writer.WriteString(value);
            writer.WriteEndElement();
        }

        internal void SaveComposites(XmlWriter writer, IRoleType roleType)
        {
            writer.WriteStartElement(Serialization.Relation);
            writer.WriteAttributeString(Serialization.Association, this.ObjectId.ToString());

            var roleStragies = this.compositesRoleByRoleType[roleType];
            var i = 0;
            foreach (var roleStrategy in roleStragies)
            {
                if (i > 0)
                {
                    writer.WriteString(Serialization.ObjectsSplitter);
                }

                writer.WriteString(roleStrategy.ObjectId.ToString());
                ++i;
            }

            writer.WriteEndElement();
        }

        internal void SaveComposite(XmlWriter writer, IRoleType roleType)
        {
            writer.WriteStartElement(Serialization.Relation);
            writer.WriteAttributeString(Serialization.Association, this.ObjectId.ToString());

            var roleStragy = this.compositeRoleByRoleType[roleType];
            writer.WriteString(roleStragy.ObjectId.ToString());

            writer.WriteEndElement();
        }

        private void AssertNotDeleted()
        {
            if (this.IsDeleted)
            {
                throw new Exception("Object of class " + this.UncheckedObjectType.Name + " with id " + this.ObjectId +
                                    " has been deleted");
            }
        }

        private void Backup(IRoleType roleType)
        {
            if (roleType.IsMany)
            {
                if (!this.RollbackCompositesRoleByRoleType.ContainsKey(roleType))
                {
                    this.compositesRoleByRoleType.TryGetValue(roleType, out var strategies);

                    if (strategies == null)
                    {
                        this.RollbackCompositesRoleByRoleType[roleType] = null;
                    }
                    else
                    {
                        this.RollbackCompositesRoleByRoleType[roleType] = new HashSet<Strategy>(strategies);
                    }
                }
            }
            else
            {
                if (!this.RollbackCompositeRoleByRoleType.ContainsKey(roleType))
                {
                    this.compositeRoleByRoleType.TryGetValue(roleType, out var strategy);

                    if (strategy == null)
                    {
                        this.RollbackCompositeRoleByRoleType[roleType] = null;
                    }
                    else
                    {
                        this.RollbackCompositeRoleByRoleType[roleType] = strategy;
                    }
                }
            }
        }

        private void Backup(IAssociationType associationType)
        {
            if (associationType.IsMany)
            {
                if (!this.RollbackCompositesAssociationByAssociationType.ContainsKey(associationType))
                {
                    this.compositesAssociationByAssociationType.TryGetValue(associationType, out var strategies);

                    if (strategies == null)
                    {
                        this.RollbackCompositesAssociationByAssociationType[associationType] = null;
                    }
                    else
                    {
                        this.RollbackCompositesAssociationByAssociationType[associationType] = new HashSet<Strategy>(strategies);
                    }
                }
            }
            else
            {
                if (!this.RollbackCompositeAssociationByAssociationType.ContainsKey(associationType))
                {
                    this.compositeAssociationByAssociationType.TryGetValue(associationType, out var strategy);

                    if (strategy == null)
                    {
                        this.RollbackCompositeAssociationByAssociationType[associationType] = null;
                    }
                    else
                    {
                        this.RollbackCompositeAssociationByAssociationType[associationType] = strategy;
                    }
                }
            }
        }

        private void RemoveCompositeRoleOne2One(IRoleType roleType)
        {
            this.AssertNotDeleted();
            this.MemorySession.MemoryDatabase.CompositeRoleChecks(this, roleType);

            var previousRole = this.GetCompositeRole(roleType.RelationType);
            if (previousRole != null)
            {
                this.ChangeSet.OnChangingCompositeRole(this.ObjectId, roleType, previousRole.Id, null);

                var previousRoleStrategy = this.MemorySession.GetStrategy(previousRole);
                var associationType = roleType.AssociationType;
                previousRoleStrategy.Backup(associationType);
                previousRoleStrategy.compositeAssociationByAssociationType.Remove(associationType);

                // remove role
                this.Backup(roleType);
                this.compositeRoleByRoleType.Remove(roleType);
            }
        }

        private void RemoveCompositeRoleMany2One(IRoleType roleType)
        {
            this.AssertNotDeleted();
            this.MemorySession.MemoryDatabase.CompositeRoleChecks(this, roleType);

            var previousRole = this.GetCompositeRole(roleType.RelationType);

            if (previousRole != null)
            {
                this.ChangeSet.OnChangingCompositeRole(this.ObjectId, roleType, previousRole.Id, null);

                var previousRoleStrategy = this.MemorySession.GetStrategy(previousRole);
                var associationType = roleType.AssociationType;

                previousRoleStrategy.compositesAssociationByAssociationType.TryGetValue(associationType, out var previousRoleStrategyAssociations);

                previousRoleStrategy.Backup(associationType);
                previousRoleStrategyAssociations.Remove(this);

                if (previousRoleStrategyAssociations.Count == 0)
                {
                    previousRoleStrategy.compositesAssociationByAssociationType.Remove(associationType);
                }

                // remove role
                this.Backup(roleType);
                this.compositeRoleByRoleType.Remove(roleType);
            }
        }

        private void AddCompositeRoleMany2Many(IRoleType roleType, Strategy newRoleStrategy)
        {
            this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRoleStrategies);
            if (previousRoleStrategies != null && previousRoleStrategies.Contains(newRoleStrategy))
            {
                return;
            }

            this.ChangeSet.OnChangingCompositesRole(this.ObjectId, roleType, newRoleStrategy);

            // Add the new role
            this.Backup(roleType);
            this.compositesRoleByRoleType.TryGetValue(roleType, out var roleStrategies);
            if (roleStrategies == null)
            {
                roleStrategies = new HashSet<Strategy>();
                this.compositesRoleByRoleType[roleType] = roleStrategies;
            }

            roleStrategies.Add(newRoleStrategy);

            // Add the new association
            newRoleStrategy.Backup(roleType.AssociationType);
            newRoleStrategy.compositesAssociationByAssociationType.TryGetValue(roleType.AssociationType, out var newRoleStrategiesAssociationStrategies);
            if (newRoleStrategiesAssociationStrategies == null)
            {
                newRoleStrategiesAssociationStrategies = new HashSet<Strategy>();
                newRoleStrategy.compositesAssociationByAssociationType[roleType.AssociationType] = newRoleStrategiesAssociationStrategies;
            }

            newRoleStrategiesAssociationStrategies.Add(this);
        }

        private void AddCompositeRoleOne2Many(IRoleType roleType, Strategy newRoleStrategy)
        {
            this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRoleStrategies);
            if (previousRoleStrategies != null && previousRoleStrategies.Contains(newRoleStrategy))
            {
                return;
            }

            this.ChangeSet.OnChangingCompositesRole(this.ObjectId, roleType, newRoleStrategy);

            // 1-...
            newRoleStrategy.compositeAssociationByAssociationType.TryGetValue(roleType.AssociationType, out var newRolePreviousAssociationStrategy);
            if (newRolePreviousAssociationStrategy != null)
            {
                this.ChangeSet.OnChangingCompositesRole(newRolePreviousAssociationStrategy.ObjectId, roleType, null);

                // Remove obsolete role
                newRolePreviousAssociationStrategy.Backup(roleType);
                newRolePreviousAssociationStrategy.compositesRoleByRoleType.TryGetValue(roleType, out var newRolePreviousAssociationStrategyRoleStrategies);
                if (newRolePreviousAssociationStrategyRoleStrategies == null)
                {
                    newRolePreviousAssociationStrategyRoleStrategies = new HashSet<Strategy>();
                    newRolePreviousAssociationStrategy.compositesRoleByRoleType[roleType] = newRolePreviousAssociationStrategyRoleStrategies;
                }

                newRolePreviousAssociationStrategyRoleStrategies.Remove(newRoleStrategy);
                if (newRolePreviousAssociationStrategyRoleStrategies.Count == 0)
                {
                    newRolePreviousAssociationStrategy.compositesRoleByRoleType.Remove(roleType);
                }
            }

            // Add the new role
            this.Backup(roleType);
            this.compositesRoleByRoleType.TryGetValue(roleType, out var roleStrategies);
            if (roleStrategies == null)
            {
                roleStrategies = new HashSet<Strategy>();
                this.compositesRoleByRoleType[roleType] = roleStrategies;
            }

            roleStrategies.Add(newRoleStrategy);

            // Set new association
            newRoleStrategy.Backup(roleType.AssociationType);
            newRoleStrategy.compositeAssociationByAssociationType[roleType.AssociationType] = this;
        }

        private void RemoveCompositeRoleMany2Many(IRoleType roleType, Strategy roleStrategy)
        {
            this.compositesRoleByRoleType.TryGetValue(roleType, out var roleStrategies);
            if (roleStrategies == null || !roleStrategies.Contains(roleStrategy))
            {
                return;
            }

            this.ChangeSet.OnChangingCompositesRole(this.ObjectId, roleType, roleStrategy);

            // Remove role
            this.Backup(roleType);
            roleStrategies.Remove(roleStrategy);
            if (roleStrategies.Count == 0)
            {
                this.compositesRoleByRoleType.Remove(roleType);
            }

            // Remove association
            roleStrategy.Backup(roleType.AssociationType);
            roleStrategy.compositesAssociationByAssociationType.TryGetValue(roleType.AssociationType, out var roleStrategiesAssociationStrategies);
            roleStrategiesAssociationStrategies.Remove(this);

            if (roleStrategiesAssociationStrategies.Count == 0)
            {
                roleStrategy.compositesAssociationByAssociationType.Remove(roleType.AssociationType);
            }
        }

        private void RemoveCompositeRoleOne2Many(IRoleType roleType, Strategy roleStrategy)
        {
            this.compositesRoleByRoleType.TryGetValue(roleType, out var roleStrategies);
            if (roleStrategies == null || !roleStrategies.Contains(roleStrategy))
            {
                return;
            }

            this.ChangeSet.OnChangingCompositesRole(this.ObjectId, roleType, roleStrategy);

            this.Backup(roleType);

            // Remove role
            roleStrategies.Remove(roleStrategy);
            if (roleStrategies.Count == 0)
            {
                this.compositesRoleByRoleType.Remove(roleType);
            }

            // Remove association
            roleStrategy.Backup(roleType.AssociationType);
            roleStrategy.compositeAssociationByAssociationType.Remove(roleType.AssociationType);
        }

        private void RemoveCompositeRolesMany2Many(IRoleType roleType)
        {
            this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRoleStrategies);
            if (previousRoleStrategies != null)
            {
                foreach (var previousRoleStrategy in previousRoleStrategies)
                {
                    this.ChangeSet.OnChangingCompositesRole(this.ObjectId, roleType, previousRoleStrategy);
                }

                foreach (var strategy in new List<Strategy>(previousRoleStrategies))
                {
                    this.RemoveCompositeRoleMany2Many(roleType, strategy);
                }
            }
        }

        private void RemoveCompositeRolesOne2Many(IRoleType roleType)
        {
            // TODO: Optimize
            this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRoleStrategies);
            if (previousRoleStrategies != null)
            {
                foreach (var strategy in new List<Strategy>(previousRoleStrategies))
                {
                    this.RemoveCompositeRoleOne2Many(roleType, strategy);
                }
            }
        }

        public class ObjectIdComparer : IComparer<Strategy>
        {
            public int Compare(Strategy x, Strategy y) => x.ObjectId.CompareTo(y.ObjectId);
        }
    }
}
