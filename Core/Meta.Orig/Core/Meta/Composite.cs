//------------------------------------------------------------------------------------------------- 
// <copyright file="Composite.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract partial class Composite : ObjectType, IComposite
    {
        private LazySet<Interface> derivedDirectSupertypes;
        private LazySet<Interface> derivedSupertypes;

        private LazySet<AssociationType> derivedAssociationTypes;
        private LazySet<RoleType> derivedRoleTypes;
        private LazySet<MethodType> derivedMethodTypes;

        private Dictionary<string, IList<Interface>> derivedDirectSupertypesByGroup;
        private Dictionary<string, IList<RoleType>> derivedExclusiveRoleTypesByGroup;
        private Dictionary<string, IList<RoleType>> derivedRoleTypesByGroup;
        private Dictionary<string, IList<AssociationType>> derivedAssociationTypesByGroup;
        private Dictionary<string, IList<MethodType>> derivedMethodTypesByGroup;

        protected Composite(MetaPopulation metaPopulation)
            : base(metaPopulation)
        {
        }

        public bool ExistExclusiveClass
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ExclusiveSubclass != null;
            }
        }

        public abstract bool ExistClass { get; }

        public bool ExistDirectSupertypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedDirectSupertypes.Count > 0;
            }
        }

        public bool ExistSupertypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSupertypes.Count > 0;
            }
        }

        public bool ExistAssociationTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedAssociationTypes.Count > 0;
            }
        }

        public bool ExistRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedAssociationTypes.Count > 0;
            }
        }

        public bool ExistMethodTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedMethodTypes.Count > 0;
            }
        }

        IClass IComposite.ExclusiveClass
        {
            get
            {
                return this.ExclusiveSubclass;
            }
        }

        /// <summary>
        /// Gets the exclusive concrete subclass.
        /// </summary>
        /// <value>The exclusive concrete subclass.</value>
        public abstract Class ExclusiveSubclass { get; }

        IEnumerable<IClass> IComposite.Classes
        {
            get
            {
                return this.Classes;
            }
        } 

        /// <summary>
        /// Gets the root classes.
        /// </summary>
        /// <value>The root classes.</value>
        public abstract IEnumerable<Class> Classes { get; }

        IEnumerable<IInterface> IComposite.DirectSupertypes
        {
            get
            {
                return this.DirectSupertypes;
            }
        }

        /// <summary>
        /// Gets the direct super types.
        /// </summary>
        /// <value>The super types.</value>
        public IEnumerable<Interface> DirectSupertypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedDirectSupertypes;
            }
        }

        IEnumerable<IInterface> IComposite.Supertypes
        {
            get
            {
                return this.Supertypes;
            }
        }

        /// <summary>
        /// Gets the super types.
        /// </summary>
        /// <value>The super types.</value>
        public IEnumerable<Interface> Supertypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSupertypes;
            }
        }
        
        IEnumerable<IAssociationType> IComposite.AssociationTypes 
        {
            get
            {
                return this.AssociationTypes;
            }
        }

        /// <summary>
        /// Gets the associations.
        /// </summary>
        /// <value>The associations.</value>
        public IEnumerable<AssociationType> AssociationTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedAssociationTypes;
            }
        }

        public IEnumerable<AssociationType> ExclusiveAssociationTypes
        {
            get
            {
                return this.AssociationTypes.Where(associationType => this.Equals(associationType.RoleType.ObjectType)).ToArray();
            }
        }

        IEnumerable<IRoleType> IComposite.RoleTypes
        {
            get
            {
                return this.RoleTypes;
            }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public IEnumerable<RoleType> RoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedRoleTypes;
            }
        }

        /// <summary>
        /// Gets the direct supertypes by group.
        /// </summary>
        /// <value>The grouped direct supertypes.</value>
        public Dictionary<string, IList<Interface>> DirectSupertypesByGroup
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedDirectSupertypesByGroup;
            }
        }

        /// <summary>
        /// Gets the exclusive roles by group.
        /// </summary>
        /// <value>The grouped exclusive roles.</value>
        public Dictionary<string, IList<RoleType>> ExclusiveRoleTypesByGroup
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedExclusiveRoleTypesByGroup;
            }
        }

        /// <summary>
        /// Gets the roles by group.
        /// </summary>
        /// <value>The grouped roles.</value>
        public Dictionary<string, IList<RoleType>> RoleTypesByGroup
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedRoleTypesByGroup;
            }
        }

        /// <summary>
        /// Gets the associations by group.
        /// </summary>
        /// <value>The grouped associations.</value>
        public Dictionary<string, IList<AssociationType>> AssociationTypesByGroup
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedAssociationTypesByGroup;
            }
        }

        /// <summary>
        /// Gets the methods by group.
        /// </summary>
        /// <value>The grouped methods.</value>
        public Dictionary<string, IList<MethodType>> MethodTypesByGroup
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedMethodTypesByGroup;
            }
        }

        public IEnumerable<RoleType> UnitRoleTypes
        {
            get
            {
                return this.RoleTypes.Where(roleType => roleType.ObjectType.IsUnit).ToArray();
            }
        }

        public IEnumerable<RoleType> CompositeRoleTypes
        {
            get
            {
                return this.RoleTypes.Where(roleType => roleType.ObjectType.IsComposite).ToArray();
            }
        }

        public IEnumerable<RoleType> ExclusiveRoleTypes
        {
            get
            {
                return this.RoleTypes.Where(roleType => this.Equals(roleType.AssociationType.ObjectType)).ToArray();
            }
        }

        /// <summary>
        /// Gets the method types.
        /// </summary>
        /// <value>The method types.</value>
        public IEnumerable<MethodType> MethodTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedMethodTypes;
            }
        }

        public IEnumerable<MethodType> ExclusiveMethodTypes
        {
            get
            {
                return this.MethodTypes.Where(methodType => this.Equals(methodType.ObjectType)).ToArray();
            }
        }

        public bool ExistSupertype(IInterface @interface)
        {
            this.MetaPopulation.Derive();
            return this.derivedSupertypes.Contains(@interface);
        }

        public bool ExistAssociationType(IAssociationType associationType)
        {
            this.MetaPopulation.Derive();
            return this.derivedAssociationTypes.Contains(associationType);
        }

        public bool ExistRoleType(IRoleType roleType)
        {
            this.MetaPopulation.Derive();
            return this.derivedRoleTypes.Contains(roleType);
        }
        
        /// <summary>
        /// Contains this concrete class.
        /// </summary>
        /// <param name="objectType">
        /// The concrete class.
        /// </param>
        /// <returns>
        /// True if this contains the concrete class.
        /// </returns>
        public abstract bool IsAssignableFrom(IClass objectType);

        /// <summary>
/// Derive direct super type derivations.
/// </summary>
/// <param name="directSupertypes">The direct super types.</param>
        internal void DeriveDirectSupertypes(HashSet<Interface> directSupertypes)
        {
            directSupertypes.Clear();
            foreach (var inheritance in this.MetaPopulation.Inheritances.Where(inheritance => this.Equals(inheritance.Subtype)))
            {
                directSupertypes.Add(inheritance.Supertype);
            }

            this.derivedDirectSupertypes = new LazySet<Interface>(directSupertypes);
        }

        /// <summary>
        /// Derive super types.
        /// </summary>
        /// <param name="superTypes">The super types.</param>
        internal void DeriveSupertypes(HashSet<Interface> superTypes)
        {
            superTypes.Clear();

            this.DeriveSupertypesRecursively(this, superTypes);

            this.derivedSupertypes = new LazySet<Interface>(superTypes);
        }

        /// <summary>
        /// Derive method types.
        /// </summary>
        /// <param name="methodTypes">
        /// The method types.
        /// </param>
        internal void DeriveMethodTypes(HashSet<MethodType> methodTypes)
        {
            methodTypes.Clear();
            foreach (var methodType in this.MetaPopulation.MethodTypes.Where(m => this.Equals(m.ObjectType)))
            {
                methodTypes.Add(methodType);
            }

            foreach (var superType in this.Supertypes)
            {
                var type = superType;
                foreach (var methodType in this.MetaPopulation.MethodTypes.Where(m => type.Equals(m.ObjectType)))
                {
                    methodTypes.Add(methodType);
                }
            }

            this.derivedMethodTypes = new LazySet<MethodType>(methodTypes);
        }

        /// <summary>
        /// Derive method types by group.
        /// </summary>
        /// <param name="methodTypes">The grouped method types.</param>
        internal void DeriveMethodTypesByGroup()
        {
            var methodTypesByGroup = new Dictionary<string, IList<MethodType>>();

            foreach (var methodType in this.MethodTypes)
            {
                foreach (var group in methodType.Groups)
                {
                    IList<MethodType> groupedMethodTypes;
                    if (!methodTypesByGroup.TryGetValue(group, out groupedMethodTypes))
                    {
                        groupedMethodTypes = new List<MethodType>();
                        methodTypesByGroup[group] = groupedMethodTypes;
                    }

                    groupedMethodTypes.Add(methodType);
                }
            }

            foreach (var group in methodTypesByGroup.Keys.ToArray())
            {
                methodTypesByGroup[group] = methodTypesByGroup[group].ToArray();
            }

            this.derivedMethodTypesByGroup = methodTypesByGroup;
        }

        /// <summary>
        /// Derive role types.
        /// </summary>
        /// <param name="roleTypes">The role types.</param>
        /// <param name="roleTypesByAssociationObjectType">RoleTypes grouped by the ObjectType of the Association</param>
        internal void DeriveRoleTypes(HashSet<RoleType> roleTypes, Dictionary<ObjectType, HashSet<RoleType>> roleTypesByAssociationObjectType)
        {
            roleTypes.Clear();

            HashSet<RoleType> classRoleTypes;
            if (roleTypesByAssociationObjectType.TryGetValue(this, out classRoleTypes))
            {
                roleTypes.UnionWith(classRoleTypes);
            }

            foreach (var superType in this.Supertypes)
            {
                HashSet<RoleType> superTypeRoleTypes;
                if (roleTypesByAssociationObjectType.TryGetValue(superType, out superTypeRoleTypes))
                {
                    roleTypes.UnionWith(superTypeRoleTypes);
                }
            }

            this.derivedRoleTypes = new LazySet<RoleType>(roleTypes);
        }

        /// <summary>
        /// Derive direct supertypes by group.
        /// </summary>
        /// <param name="inerfaces">The grouped direct supertypes.</param>
        internal void DeriveDirectSupertypesByGroup()
        {
            var directSupertypesByGroup = new Dictionary<string, IList<Interface>>();

            foreach (var directSupertype in this.DirectSupertypes)
            {
                var groups = directSupertype.RoleTypesByGroup.Keys.Union(directSupertype.AssociationTypesByGroup.Keys);
                foreach (var group in groups)
                {
                    IList<Interface> groupedDiresctSupertypes;
                    if (!directSupertypesByGroup.TryGetValue(group, out groupedDiresctSupertypes))
                    {
                        groupedDiresctSupertypes = new List<Interface>();
                        directSupertypesByGroup[group] = groupedDiresctSupertypes;
                    }

                    groupedDiresctSupertypes.Add(directSupertype);
                }
            }

            foreach (var group in directSupertypesByGroup.Keys.ToArray())
            {
                directSupertypesByGroup[group] = directSupertypesByGroup[group].ToArray();
            }

            this.derivedDirectSupertypesByGroup = directSupertypesByGroup;
        }

        /// <summary>
        /// Derive exclusive role types by group.
        /// </summary>
        /// <param name="roleTypes">The exclusive grouped role types.</param>
        internal void DeriveExclusiveRoleTypesByGroup()
        {
            var roleTypesByGroup = new Dictionary<string, IList<RoleType>>();

            foreach (var roleType in this.ExclusiveRoleTypes)
            {
                foreach (var group in roleType.RelationType.Groups)
                {
                    IList<RoleType> groupedExclusiveRoleTypes;
                    if (!roleTypesByGroup.TryGetValue(group, out groupedExclusiveRoleTypes))
                    {
                        groupedExclusiveRoleTypes = new List<RoleType>();
                        roleTypesByGroup[group] = groupedExclusiveRoleTypes;
                    }

                    groupedExclusiveRoleTypes.Add(roleType);
                }
            }

            foreach (var group in roleTypesByGroup.Keys.ToArray())
            {
                roleTypesByGroup[group] = roleTypesByGroup[group].ToArray();
            }

            this.derivedExclusiveRoleTypesByGroup = roleTypesByGroup;
        }

        /// <summary>
        /// Derive role types by group.
        /// </summary>
        /// <param name="roleTypes">The grouped role types.</param>
        internal void DeriveRoleTypesByGroup()
        {
            var roleTypesByGroup = new Dictionary<string, IList<RoleType>>();

            foreach (var roleType in this.RoleTypes)
            {
                foreach (var group in roleType.RelationType.Groups)
                {
                    IList<RoleType> groupedRoleTypes;
                    if (!roleTypesByGroup.TryGetValue(group, out groupedRoleTypes))
                    {
                        groupedRoleTypes = new List<RoleType>();
                        roleTypesByGroup[group] = groupedRoleTypes;
                    }

                    groupedRoleTypes.Add(roleType);
                }   
            }
            
            foreach (var group in roleTypesByGroup.Keys.ToArray())
            {
                roleTypesByGroup[group] = roleTypesByGroup[group].ToArray();
            }

            this.derivedRoleTypesByGroup = roleTypesByGroup;
        }

        /// <summary>
        /// Derive association types by group.
        /// </summary>
        /// <param name="associationTypes">The grouped association types.</param>
        internal void DeriveAssociationTypesByGroup()
        {
            var associationTypesByGroup = new Dictionary<string, IList<AssociationType>>();

            foreach (var associationType in this.AssociationTypes)
            {
                foreach (var group in associationType.RelationType.Groups)
                {
                    IList<AssociationType> groupedAssociationTypes;
                    if (!associationTypesByGroup.TryGetValue(group, out groupedAssociationTypes))
                    {
                        groupedAssociationTypes = new List<AssociationType>();
                        associationTypesByGroup[group] = groupedAssociationTypes;
                    }

                    groupedAssociationTypes.Add(associationType);
                }
            }

            foreach (var group in associationTypesByGroup.Keys.ToArray())
            {
                associationTypesByGroup[group] = associationTypesByGroup[group].ToArray();
            }

            this.derivedAssociationTypesByGroup = associationTypesByGroup;
        }

        /// <summary>
        /// Derive association types.
        /// </summary>
        /// <param name="associations">The associations.</param>
        /// <param name="relationTypesByRoleObjectType">AssociationTypes grouped by the ObjectType of the Role</param>
        internal void DeriveAssociationTypes(HashSet<AssociationType> associations, Dictionary<ObjectType, HashSet<AssociationType>> relationTypesByRoleObjectType)
        {
            associations.Clear();
            
            HashSet<AssociationType> classAssociationTypes;
            if (relationTypesByRoleObjectType.TryGetValue(this, out classAssociationTypes))
            {
                associations.UnionWith(classAssociationTypes);
            }

            foreach (var superType in this.Supertypes)
            {
                HashSet<AssociationType> interfaceAssociationTypes;
                if (relationTypesByRoleObjectType.TryGetValue(superType, out interfaceAssociationTypes))
                {
                    associations.UnionWith(interfaceAssociationTypes);
                }
            }

            this.derivedAssociationTypes = new LazySet<AssociationType>(associations);
        }
        
        /// <summary>
        /// Derive super types recursively.
        /// </summary>
        /// <param name="type">The type .</param>
        /// <param name="superTypes">The super types.</param>
        private void DeriveSupertypesRecursively(ObjectType type, HashSet<Interface> superTypes)
        {
            foreach (var directSupertype in this.derivedDirectSupertypes)
            {
                if (!Equals(directSupertype, type))
                {
                    superTypes.Add(directSupertype);
                    directSupertype.DeriveSupertypesRecursively(type, superTypes);
                }
            }
        }

    }
}