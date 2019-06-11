//------------------------------------------------------------------------------------------------- 
// <copyright file="Composite.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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

namespace Allors.Workspace.Meta
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract partial class Composite : ObjectType, IComposite
    {
        private bool derivedWorkspace;

        private bool assignedIsSynced;
        private bool isSynced;

        private LazySet<Interface> derivedDirectSupertypes;
        private LazySet<Interface> derivedSupertypes;

        private LazySet<AssociationType> derivedAssociationTypes;
        private LazySet<RoleType> derivedRoleTypes;
        private LazySet<MethodType> derivedMethodTypes;

        private string xmlDoc;

        public bool Workspace => this.derivedWorkspace;

        public string XmlDoc
        {
            get
            {
                return this.xmlDoc;
            }

            set
            {
                this.xmlDoc = !string.IsNullOrWhiteSpace(value) ? value : null;
            }
        }

        public string XmlDocComment
        {
            get
            {
                var lines = this.xmlDoc?.Split('\n').Select(v => "   /// " + v).ToArray();
                if (lines != null && lines.Any())
                {
                    return string.Join("\n", lines);
                }

                return null;
            }
        }
        
        public bool AssignedIsSynced 
        {
            get
            {
                return this.assignedIsSynced;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedIsSynced = value;
                this.MetaPopulation.Stale();
            }
        }

        public bool IsSynced 
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.isSynced;
            }
        }

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

        IClass IComposite.ExclusiveClass => this.ExclusiveSubclass;

        /// <summary>
        /// Gets the exclusive concrete subclass.
        /// </summary>
        /// <value>The exclusive concrete subclass.</value>
        public abstract Class ExclusiveSubclass { get; }

        IEnumerable<IClass> IComposite.Classes => this.Classes;

        /// <summary>
        /// Gets the root classes.
        /// </summary>
        /// <value>The root classes.</value>
        public abstract IEnumerable<Class> Classes { get; }

        IEnumerable<IInterface> IComposite.DirectSupertypes => this.DirectSupertypes;

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

        IEnumerable<IInterface> IComposite.Supertypes => this.Supertypes;

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
        
        IEnumerable<IAssociationType> IComposite.AssociationTypes => this.AssociationTypes;

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

        IEnumerable<IRoleType> IComposite.RoleTypes => this.RoleTypes;

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

        public IEnumerable<RoleType> UnitRoleTypes => this.RoleTypes.Where(roleType => roleType.ObjectType.IsUnit).ToArray();

        public IEnumerable<RoleType> CompositeRoleTypes => this.RoleTypes.Where(roleType => roleType.ObjectType.IsComposite).ToArray();

        public IEnumerable<RoleType> ExclusiveRoleTypes => this.RoleTypes.Where(roleType => this.Equals(roleType.AssociationType.ObjectType)).ToArray();

        public IEnumerable<RoleType> SortedExclusiveRoleTypes => this.ExclusiveRoleTypes.OrderBy(v => v.Name);

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

        public IEnumerable<MethodType> ExclusiveMethodTypes => this.MethodTypes.Where(methodType => this.Equals(methodType.ObjectType)).ToArray();

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
        public abstract bool IsAssignableFrom(IComposite objectType);
        
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
        /// Derive role types.
        /// </summary>
        /// <param name="roleTypes">The role types.</param>
        /// <param name="roleTypesByAssociationObjectType">RoleTypes grouped by the ObjectType of the Association</param>
        internal void DeriveRoleTypes(HashSet<RoleType> roleTypes, Dictionary<IObjectType, HashSet<RoleType>> roleTypesByAssociationObjectType)
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
        /// Derive association types.
        /// </summary>
        /// <param name="associations">The associations.</param>
        /// <param name="relationTypesByRoleObjectType">AssociationTypes grouped by the ObjectType of the Role</param>
        internal void DeriveAssociationTypes(HashSet<AssociationType> associations, Dictionary<IObjectType, HashSet<AssociationType>> relationTypesByRoleObjectType)
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

        internal void DeriveWorkspace()
        {
            this.derivedWorkspace = this.RoleTypes.Any(v => v.Workspace) || this.AssociationTypes.Any(v => v.Workspace) || this.MethodTypes.Any(v => v.Workspace);
        }

        internal void DeriveIsSynced()
        {
            this.isSynced = this.assignedIsSynced || this.derivedSupertypes.Any(v => v.assignedIsSynced);
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

        // TODO: Added for Workspace.Meta
        public IEnumerable<MethodType> DefinedMethods
        {
            get
            {
                return this.MetaPopulation.MethodTypes.Where(m => this.Equals(m.ObjectType));
            }
        }

        public IEnumerable<MethodType> InheritedMethods => this.MethodTypes.Except(this.DefinedMethods);

        public IEnumerable<RoleType> InheritedRoles => this.RoleTypes.Except(this.ExclusiveRoleTypes);

        public IEnumerable<AssociationType> InheritedAssociations => this.AssociationTypes.Except(this.ExclusiveAssociationTypes);

        // Workspace
        public IEnumerable<Interface> WorkspaceSupertypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.Supertypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<Interface> WorkspaceDirectSupertypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.DirectSupertypes.Where(m => m.Workspace);
            }
        }

        // TODO: Derive
        public IEnumerable<RoleType> WorkspaceRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.RoleTypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<RoleType> WorkspaceExclusiveRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ExclusiveRoleTypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<MethodType> WorkspaceExclusiveMethodTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ExclusiveMethodTypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<RoleType> WorkspaceUnitRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.UnitRoleTypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<RoleType> WorkspaceCompositeRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.CompositeRoleTypes.Where(m => m.Workspace);
            }
        }

        // TODO: Derive
        public IEnumerable<MethodType> WorkspaceMethodTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.MethodTypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<MethodType> WorkspaceInheritedMethods
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.InheritedMethods.Where(m => m.Workspace);
            }
        }

        public IEnumerable<RoleType> WorkspaceInheritedRoles
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.InheritedRoles.Where(m => m.Workspace);
            }
        }

        public IEnumerable<AssociationType> WorkspaceInheritedAssociations
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.InheritedAssociations.Where(m => m.Workspace);
            }
        }

        public IEnumerable<AssociationType> WorkspaceExclusiveAssociationTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ExclusiveAssociationTypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<MethodType> WorkspaceDefinedMethods
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.DefinedMethods.Where(m => m.Workspace);
            }
        }

        public IEnumerable<Composite> WorkspaceRelatedComposites
        {
            get
            {
                this.MetaPopulation.Derive();
                return this
                    .Supertypes.Where(m => m.Workspace)
                    .Union(this.RoleTypes.Where(m => m.Workspace && m.ObjectType.IsComposite).Select(v => (Composite)v.ObjectType))
                    .Union(this.AssociationTypes.Where(m => m.Workspace).Select(v => (Composite)v.ObjectType)).Distinct()
                    .Except(new[] { this }).ToArray();
            }
        }
    }
}