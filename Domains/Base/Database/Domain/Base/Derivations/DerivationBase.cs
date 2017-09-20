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
    using System.Linq;

    using Allors;
    using Allors.Meta;

    public abstract class DerivationBase : IDerivation
    {
        private readonly HashSet<long> markedAsModified;
        private readonly HashSet<Object> derivedObjects;

        private Dictionary<string, object> properties;

        private IValidation validation;

        private int generation;
        private DerivationGraphBase derivationGraph;
        private HashSet<IObject> addedDerivables;

        protected DerivationBase(ISession session)
        {
            this.Id = Guid.NewGuid();
            this.TimeStamp = session.Now();

            this.Session = session;

            this.markedAsModified = new HashSet<long>();
            this.derivedObjects = new HashSet<Object>();

            this.ChangeSet = session.Checkpoint();

            this.generation = 0;
        }

        protected DerivationBase(ISession session, IEnumerable<long> markedAsModified)
            : this(session)
        {
            this.markedAsModified.UnionWith(markedAsModified);
        }

        protected DerivationBase(ISession session, IEnumerable<IObject> markedAsModified)
            : this(session)
        {
            this.markedAsModified.UnionWith(markedAsModified.Where(v => v != null).Select(v => v.Id));
        }

        public Guid Id { get; }

        public DateTime TimeStamp { get; }

        public ISession Session { get; }

        public IValidation Validation
        {
            get
            {
                return this.validation;
            }

            protected set
            {
                this.validation = value;
            }
        }

        public IChangeSet ChangeSet { get; private set; }

        public ISet<Object> DerivedObjects => this.derivedObjects;

        public int Generation => this.generation;

        public object this[string name]
        {
            get
            {
                var lowerName = name.ToLowerInvariant();

                object value;
                if (this.properties != null && this.properties.TryGetValue(lowerName, out value))
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
            var id = @object.Id;
            return this.markedAsModified.Contains(id) || this.ChangeSet.Associations.Contains(id) || this.ChangeSet.Created.Contains(id);
        }

        public bool IsCreated(Object derivable)
        {
            return this.ChangeSet.Created.Contains(derivable.Id);
        }

        public bool HasChangedRole(Object derivable, RoleType roleType)
        {
            ISet<IRoleType> changedRoleTypes;
            this.ChangeSet.RoleTypesByAssociation.TryGetValue(derivable.Id, out changedRoleTypes);
            return changedRoleTypes?.Contains(roleType) ?? false;
        }

        public bool HasChangedRoles(Object derivable, params RoleType[] roleTypes)
        {
            ISet<IRoleType> changedRoleTypes;
            this.ChangeSet.RoleTypesByAssociation.TryGetValue(derivable.Id, out changedRoleTypes);
            if (changedRoleTypes != null)
            {
                if (roleTypes.Any(roleType => changedRoleTypes.Contains(roleType)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsMarkedAsModified(Object derivable)
        {
            return this.markedAsModified.Contains(derivable.Id);
        }

        public void MarkAsModified(Object derivable)
        {
            this.markedAsModified.Add(derivable.Id);
        }

        public void AddDerivable(Object derivable)
        {
            if (derivable != null)
            {
                if (this.DerivedObjects.Contains(derivable))
                {
                    throw new ArgumentException("Object has alreadry been derived.");
                }
                
                this.derivationGraph.Add(derivable);
                this.addedDerivables.Add(derivable);

                this.OnAddedDerivable(derivable);
            }
        }

        public void AddDerivables(IEnumerable<Object> derivables)
        {
            foreach (var derivable in derivables)
            {
                this.AddDerivable(derivable);
            }
        }

        public void AddDependency(Object dependent, Object dependee)
        {
            if (dependent != null && dependee != null)
            {
                // TODO: add additional methods in case dependent/dependee is already derived and should not be derived again.
                this.derivationGraph.AddDependency(dependent, dependee);

                this.addedDerivables.Add(dependent);
                this.addedDerivables.Add(dependee);

                this.OnAddedDependency(dependent, dependee);
            }
        }

        public IValidation Derive()
        {
            var changedObjectIds = new HashSet<long>(this.ChangeSet.Associations);
            changedObjectIds.UnionWith(this.ChangeSet.Roles);
            changedObjectIds.UnionWith(this.ChangeSet.Created);
            changedObjectIds.UnionWith(this.markedAsModified);

            var preparedObjects = new HashSet<IObject>();
            var changedObjects = new HashSet<IObject>(this.Session.Instantiate(changedObjectIds.ToArray()));

            while (changedObjects.Count > 0)
            {
                this.generation++;

                this.OnStartedGeneration(this.generation);

                this.addedDerivables = new HashSet<IObject>();

                var preparationRun = 1;
                
                this.OnStartedPreparation(preparationRun);

                this.derivationGraph = this.CreateDerivationGraph(this);
                foreach (var changedObject in changedObjects)
                {
                    var derivable = this.Session.Instantiate(changedObject) as Object;

                    if (derivable != null)
                    {
                        this.OnPreDeriving(derivable);

                        derivable.OnPreDerive(x => x.WithDerivation(this));

                        this.OnPreDerived(derivable);

                        preparedObjects.Add(derivable);
                    }
                }

                while (this.addedDerivables.Count > 0)
                {
                    preparationRun++;
                    this.OnStartedPreparation(preparationRun);
 
                    var dependencyObjectsToPrepare = new HashSet<IObject>(this.addedDerivables);
                    dependencyObjectsToPrepare.ExceptWith(preparedObjects);

                    this.addedDerivables = new HashSet<IObject>();

                    foreach (var o in dependencyObjectsToPrepare)
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
                    break;
                }

                this.derivationGraph.Derive();

                this.ChangeSet = this.Session.Checkpoint();

                changedObjectIds = new HashSet<long>(this.ChangeSet.Associations);
                changedObjectIds.UnionWith(this.ChangeSet.Roles);
                changedObjectIds.UnionWith(this.ChangeSet.Created);

                changedObjects = new HashSet<IObject>(this.Session.Instantiate(changedObjectIds.ToArray()));
                changedObjects.ExceptWith(this.derivedObjects);

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
    }
}