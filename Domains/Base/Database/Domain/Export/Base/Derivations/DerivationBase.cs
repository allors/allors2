// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationBase.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Allors;
    using Allors.Adapters;
    using Allors.Meta;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Allors Object")]
    public abstract class DerivationBase : IDerivation
    {
        private static readonly EmptySet<IObject> EmptyObjectSet = new EmptySet<IObject>();

        private readonly HashSet<Object> derivedObjects;

        private readonly Dictionary<long, ISet<RelationType>> relationsByMarker;
        private readonly AccumulatedChangeSet accumulatedChangeSet;

        private Dictionary<string, object> properties;

        private IValidation validation;

        private int generation;

        private DerivationGraphBase derivationGraph;
        private HashSet<IObject> added;

        protected DerivationBase(ISession session, DerivationConfig config)
        {
            this.Config = config ?? new DerivationConfig();

            this.Id = Guid.NewGuid();
            this.TimeStamp = session.Now();

            this.Session = session;

            this.relationsByMarker = new Dictionary<long, ISet<RelationType>>();

            this.derivedObjects = new HashSet<Object>();

            this.accumulatedChangeSet = new AccumulatedChangeSet();
        
            this.generation = 0;
        }

        public DerivationConfig Config { get; }

        public Guid Id { get; }

        public DateTime TimeStamp { get; }

        public ISession Session { get; }

        public IValidation Validation
        {
            get => this.validation;

            protected set => this.validation = value;
        }

        public IChangeSet ChangeSet => this.accumulatedChangeSet;

        public ISet<Object> DerivedObjects => this.derivedObjects;

        public int Generation => this.generation;

        public object this[string name]
        {
            get
            {
                var lowerName = name.ToLowerInvariant();

                if (this.properties != null && this.properties.TryGetValue(lowerName, out var value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                var lowerName = name.ToLowerInvariant();

                if (value == null)
                {
                    if (this.properties != null)
                    {
                        this.properties.Remove(lowerName);
                        if (this.properties.Count == 0)
                        {
                            this.properties = null;
                        }
                    }
                }
                else
                {
                    if (this.properties == null)
                    {
                        this.properties = new Dictionary<string, object>();
                    }

                    this.properties[lowerName] = value;
                }
            }
        }

        public bool IsModified(Object @object)
        {
            return this.IsMarked(@object) || this.IsCreated(@object) || this.HasChangedRoles(@object);
        }

        public bool IsModified(Object @object, RelationKind kind)
        {
            return this.IsMarked(@object) || this.IsCreated(@object) || this.HasChangedRoles(@object, kind);
        }

        public bool IsCreated(Object derivable)
        {
            return this.ChangeSet.Created.Contains(derivable.Id);
        }

        public bool HasChangedRole(Object derivable, RoleType roleType)
        {
            this.ChangeSet.RoleTypesByAssociation.TryGetValue(derivable.Id, out var changedRoleTypes);
            return changedRoleTypes?.Contains(roleType) ?? false;
        }

        public bool HasChangedRoles(Object derivable, params RoleType[] roleTypes)
        {
            this.ChangeSet.RoleTypesByAssociation.TryGetValue(derivable.Id, out var changedRoleTypes);
            if (changedRoleTypes != null)
            {
                if (roleTypes.Length == 0 || roleTypes.Any(roleType => changedRoleTypes.Contains(roleType)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasChangedRoles(Object derivable, RelationKind relationKind)
        {
            Func<IRoleType, bool> check;
            switch (relationKind)
            {
                case RelationKind.Regular:
                    check = (roleType) => !roleType.RelationType.IsDerived && !roleType.RelationType.IsSynced;
                    break;

                case RelationKind.Derived:
                    check = (roleType) => roleType.RelationType.IsDerived;
                    break;

                case RelationKind.Synced:
                    check = (roleType) => roleType.RelationType.IsSynced;
                    break;

                default:
                    check = (roleType) => true;
                    break;
            }

            this.ChangeSet.RoleTypesByAssociation.TryGetValue(derivable.Id, out var changedRoleTypes);
            if (changedRoleTypes != null)
            {
                if (changedRoleTypes.Any(roleType => check(roleType)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasChangedAssociation(Object derivable, AssociationType associationType)
        {
            this.ChangeSet.AssociationTypesByRole.TryGetValue(derivable.Id, out var changedAssociationTypes);
            return changedAssociationTypes?.Contains(associationType) ?? false;
        }

        public bool HasChangedAssociations(Object derivable, params AssociationType[] associationTypes)
        {
            this.ChangeSet.AssociationTypesByRole.TryGetValue(derivable.Id, out var changedAssociationTypes);
            if (changedAssociationTypes != null)
            {
                if (associationTypes.Length == 0 || associationTypes.Any(associationType => changedAssociationTypes.Contains(associationType)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsMarked(Object derivable)
        {
            return this.relationsByMarker.ContainsKey(derivable.Id);
        }

        public void Mark(Object derivable, RelationType relationType = null)
        {
            if (derivable != null)
            {
                if (!this.relationsByMarker.TryGetValue(derivable.Id, out var relationTypes) && relationType == null)
                {
                    this.relationsByMarker.Add(derivable.Id, null);
                }

                if (relationType != null)
                {
                    if (relationTypes == null)
                    {
                        relationTypes = new HashSet<RelationType>();
                        this.relationsByMarker[derivable.Id] = relationTypes;
                    }

                    relationTypes.Add(relationType);
                }
            }
        }

        public void Mark(Object derivable, RoleType roleType)
        {
            this.Mark(derivable, roleType.RelationType);
        }

        public void Mark(IEnumerable<Object> derivables)
        {
            foreach (var derivable in derivables)
            {
                this.Mark(derivable);
            }
        }

        public ISet<RelationType> MarkedBy(Object marker)
        {
            this.relationsByMarker.TryGetValue(marker.Id, out var relationTypes);
            return relationTypes;
        }

        public void Add(Object derivable)
        {
            if (this.generation == 0)
            {
                throw new Exception("Add can only be called during a derivation. Use Mark() instead.");
            }

            if (derivable != null)
            {
                if (this.derivedObjects.Contains(derivable))
                {
                    return;
                }

                this.derivationGraph.Add(derivable);
                this.added.Add(derivable);

                this.OnAddedDerivable(derivable);
            }
        }

        public void Add(IEnumerable<Object> derivables)
        {
            foreach (var derivable in derivables)
            {
                this.Add(derivable);
            }
        }

        public void AddDependency(Object dependent, Object dependee)
        {
            if (this.generation == 0)
            {
                throw new Exception("AddDependency can only be called during a derivation.");
            }

            if (dependent != null && dependee != null)
            {
                if (this.derivedObjects.Contains(dependent) && this.derivedObjects.Contains(dependee))
                {
                    //return;
                }

                this.added.Add(dependent);
                this.added.Add(dependee);
                this.derivationGraph.AddDependency(dependent, dependee);

                this.OnAddedDependency(dependent, dependee);
            }
        }

        public IValidation Derive()
        {
            if (this.generation != 0)
            {
                throw new Exception("Derive can only be called once. Create a new Derivation object.");
            }

            var changeSet = this.Session.Checkpoint();
            this.accumulatedChangeSet.Add(changeSet);

            var changedObjectIds = new HashSet<long>(changeSet.Associations);
            changedObjectIds.UnionWith(changeSet.Roles);
            changedObjectIds.UnionWith(changeSet.Created);
            changedObjectIds.UnionWith(this.relationsByMarker.Keys);

            ISet<IObject> changedObjects = new HashSet<IObject>(this.Session.Instantiate(changedObjectIds));
            var preparedObjects = new HashSet<IObject>();

            var postDeriveObjects = new List<Object>();

            while (changedObjects.Count > 0)
            {
                this.generation++;
                this.OnStartedGeneration(this.generation);

                var newObjects = this.Session.Instantiate(changeSet.Created);
                foreach (var newObject in newObjects)
                {
                    ((Object)newObject).OnInit();
                }

                this.added = new HashSet<IObject>();

                var preparationRun = 1;

                this.OnStartedPreparation(preparationRun);

                this.derivationGraph = this.CreateDerivationGraph(this);
                foreach (var changedObject in changedObjects)
                {
                    if (this.Session.Instantiate(changedObject) is Object derivable)
                    {
                        this.OnPreDeriving(derivable);

                        derivable.OnPreDerive(x => x.WithDerivation(this));

                        this.OnPreDerived(derivable);

                        preparedObjects.Add(derivable);
                    }
                }

                while (this.added.Count > 0)
                {
                    preparationRun++;
                    this.OnStartedPreparation(preparationRun);

                    var objectsToPrepare = new HashSet<IObject>(this.added);
                    objectsToPrepare.ExceptWith(preparedObjects);

                    this.added = new HashSet<IObject>();

                    foreach (var o in objectsToPrepare)
                    {
                        var dependencyObject = (Object)o;

                        this.OnPreDeriving(dependencyObject);

                        dependencyObject.OnPreDerive(x => x.WithDerivation(this));

                        this.OnPreDerived(dependencyObject);

                        preparedObjects.Add(dependencyObject);
                    }
                }

                if (this.derivationGraph.Count == 0)
                {
                    changedObjects = EmptyObjectSet;
                }
                else
                {
                    // Derive
                    var generationPostDeriveObjects = new List<Object>();

                    this.derivationGraph.Derive(generationPostDeriveObjects);

                    // Keep derivation order within a generation,
                    // but post derive generations backwards
                    postDeriveObjects.InsertRange(0, generationPostDeriveObjects);

                    changeSet = this.Session.Checkpoint();
                    this.accumulatedChangeSet.Add(changeSet);

                    changedObjectIds = new HashSet<long>(changeSet.Associations);
                    changedObjectIds.UnionWith(changeSet.Roles);
                    changedObjectIds.UnionWith(changeSet.Created);

                    changedObjects = new HashSet<IObject>(this.Session.Instantiate(changedObjectIds));
                    changedObjects.ExceptWith(this.derivedObjects);
                }

                // PostDerive
                if (changedObjects.Count == 0)
                {
                    foreach (var derivable in postDeriveObjects)
                    {
                        if (!derivable.Strategy.IsDeleted)
                        {
                            this.OnPostDeriving(derivable);
                            derivable.OnPostDerive(x => x.WithDerivation(this));
                            this.OnPostDerived(derivable);
                        }
                    }

                    postDeriveObjects = new List<Object>();

                    changeSet = this.Session.Checkpoint();
                    this.accumulatedChangeSet.Add(changeSet);

                    changedObjectIds = new HashSet<long>(changeSet.Associations);
                    changedObjectIds.UnionWith(changeSet.Roles);
                    changedObjectIds.UnionWith(changeSet.Created);

                    changedObjects = new HashSet<IObject>(this.Session.Instantiate(changedObjectIds));
                    changedObjects.ExceptWith(this.derivedObjects);
                }

                this.derivationGraph = null;
            }

            return this.validation;
        }

        internal void AddDerivedObject(Object derivable)
        {
            this.derivedObjects.Add(derivable);
        }

        protected abstract DerivationGraphBase CreateDerivationGraph(DerivationBase derivation);

        protected abstract void OnAddedDerivable(Object derivable);

        protected abstract void OnAddedDependency(Object dependent, Object dependee);

        protected abstract void OnStartedGeneration(int generation);

        protected abstract void OnStartedPreparation(int preparationRun);

        protected abstract void OnPreDeriving(Object derivable);

        protected abstract void OnPreDerived(Object derivable);

        protected abstract void OnPostDeriving(Object derivable);

        protected abstract void OnPostDerived(Object derivable);
    }
}