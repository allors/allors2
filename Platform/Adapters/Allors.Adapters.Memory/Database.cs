// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Allors.Meta;

    public class Database : IDatabase
    {
        public const long IntialVersion = 0;

        private readonly IObjectFactory objectFactory;
        private readonly Dictionary<IObjectType, object> concreteClassesByObjectType;

        private readonly string id;
        
        public Dictionary<string, object> Properties;

        private Session session;

        public Database(Configuration configuration)
        {
            this.objectFactory = configuration.ObjectFactory;
            if (this.objectFactory == null)
            {
                throw new Exception("Configuration.ObjectFactory is missing");
            }

            this.concreteClassesByObjectType = new Dictionary<IObjectType, object>();

            this.id = string.IsNullOrWhiteSpace(configuration.Id) ? Guid.NewGuid().ToString("N").ToLowerInvariant() : configuration.Id;
        }

        public event ObjectNotLoadedEventHandler ObjectNotLoaded;

        public event RelationNotLoadedEventHandler RelationNotLoaded;
        
        public string Id
        {
            get { return this.id; }
        }

        public bool IsDatabase => true;

        public bool IsWorkspace => false;

        public bool IsShared => false;

        public IObjectFactory ObjectFactory => this.objectFactory;

        public IMetaPopulation MetaPopulation => this.objectFactory.MetaPopulation;

        public IDatabase Serializable => null;

        internal bool IsLoading { get; private set; }

        protected virtual Memory.Session Session => this.session ?? (this.session = new Session(this));

        public object this[string name]
        {
            get
            {
                if (this.Properties == null)
                {
                    return null;
                }

                object value;
                this.Properties.TryGetValue(name, out value);
                return value;
            }

            set
            {
                if (this.Properties == null)
                {
                    this.Properties = new Dictionary<string, object>();
                }

                if (value == null)
                {
                    this.Properties.Remove(name);
                }
                else
                {
                    this.Properties[name] = value;
                }
            }
        }

        public ISession CreateSession()
        {
            return this.CreateDatabaseSession();
        }

        ISession IDatabase.CreateSession()
        {
            return this.CreateDatabaseSession();
        }

        public ISession CreateDatabaseSession()
        {
            return this.Session;
        }
        
        public void Load(XmlReader reader)
        {
            this.Init();

            try
            {
                this.IsLoading = true;

                var load = new Load(this.Session, reader);
                load.Execute();

                this.Session.Commit();
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        public void Save(XmlWriter writer)
        {
            this.Session.Save(writer);
        }

        public bool ContainsConcreteClass(IComposite objectType, IObjectType concreteClass)
        {
            object concreteClassOrClasses;
            if (!this.concreteClassesByObjectType.TryGetValue(objectType, out concreteClassOrClasses))
            {
                if (objectType.ExistExclusiveClass)
                {
                    concreteClassOrClasses = objectType.ExclusiveClass;
                    this.concreteClassesByObjectType[objectType] = concreteClassOrClasses;
                }
                else
                {
                    concreteClassOrClasses = new HashSet<IObjectType>(objectType.Classes);
                    this.concreteClassesByObjectType[objectType] = concreteClassOrClasses;
                }
            }

            if (concreteClassOrClasses is IObjectType)
            {
                return concreteClass.Equals(concreteClassOrClasses);
            }

            var concreteClasses = (HashSet<IObjectType>)concreteClassOrClasses;
            return concreteClasses.Contains(concreteClass);
        }

        public void UnitRoleChecks(IStrategy strategy, IRoleType roleType)
        {
            if (!this.ContainsConcreteClass(roleType.AssociationType.ObjectType, strategy.Class))
            {
                throw new ArgumentException(strategy.Class + " is not a valid association object type for " + roleType + ".");
            }

            if (roleType.ObjectType is IComposite)
            {
                throw new ArgumentException(roleType.ObjectType + " on roleType " + roleType + " is not a unit type.");
            }
        }

        public void CompositeRoleChecks(IStrategy strategy, IRoleType roleType)
        {
            this.CompositeSharedChecks(strategy, roleType, null);
        }

        public void CompositeRoleChecks(IStrategy strategy, IRoleType roleType, IObject role)
        {
            this.CompositeSharedChecks(strategy, roleType, role);
            if (!roleType.IsOne)
            {
                throw new ArgumentException("RelationType " + roleType + " has multiplicity many.");
            }
        }

        public void CompositeRolesChecks(IStrategy strategy, IRoleType roleType, IObject role)
        {
            this.CompositeSharedChecks(strategy, roleType, role);
            if (!roleType.IsMany)
            {
                throw new ArgumentException("RelationType " + roleType + " has multiplicity one.");
            }
        }

        internal void OnObjectNotLoaded(Guid metaTypeId, long allorsObjectId)
        {
            var args = new ObjectNotLoadedEventArgs(metaTypeId, allorsObjectId);
            if (this.ObjectNotLoaded != null)
            {
                this.ObjectNotLoaded(this, args);
            }
            else
            {
                throw new Exception("Object not loaded: " + args);
            }
        }

        internal void OnRelationNotLoaded(Guid relationTypeId, long associationObjectId, string roleContents)
        {
            var args = new RelationNotLoadedEventArgs(relationTypeId, associationObjectId, roleContents);
            if (this.RelationNotLoaded != null)
            {
                this.RelationNotLoaded(this, args);
            }
            else
            {
                throw new Exception("RelationType not loaded: " + args);
            }
        }

        private void CompositeSharedChecks(IStrategy strategy, IRoleType roleType, IObject role)
        {
            if (!this.ContainsConcreteClass(roleType.AssociationType.ObjectType, strategy.Class))
            {
                throw new ArgumentException(strategy.Class + " has no roleType with role " + roleType + ".");
            }

            if (role != null)
            {
                if (!strategy.Session.Equals(role.Strategy.Session))
                {
                    throw new ArgumentException(role + " is from different session");
                }

                if (role.Strategy.IsDeleted)
                {
                    throw new ArgumentException(roleType + " on object " + strategy + " is removed.");
                }

                var compositeType = roleType.ObjectType as IComposite;

                if (compositeType == null)
                {
                    throw new ArgumentException(role + " has no CompositeType");
                }

                if (!compositeType.IsAssignableFrom(role.Strategy.Class))
                {
                    throw new ArgumentException(role.Strategy.Class + " is not compatible with type " + roleType.ObjectType + " of role " + roleType + ".");
                }
            }
        }

        public virtual void Init()
        {
            this.Session.Init();

            this.session = null;
            this.Properties = null;
        }
    }
}