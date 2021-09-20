// <copyright file="MetaPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public sealed partial class MetaPopulation : IMetaPopulation
    {
        public static readonly MetaPopulation Instance;
        private readonly Dictionary<Guid, MetaObjectBase> metaObjectById;

        private Dictionary<string, Class> derivedClassByLowercaseName;
        private Composite[] derivedComposites;
        private bool isStale;
        private bool isDeriving;

        private IList<Domain> domains;
        private IList<Unit> units;
        private IList<Interface> interfaces;
        private IList<Class> classes;
        private IList<Inheritance> inheritances;
        private IList<RelationType> relationTypes;
        private IList<AssociationType> associationTypes;
        private IList<RoleType> roleTypes;
        private IList<MethodType> methodTypes;

        static MetaPopulation()
        {
            Instance = new MetaPopulation();
            var metaBuilder = new MetaBuilder(Instance);
            metaBuilder.BuildDomains();
            metaBuilder.BuildDomainInheritances();
            metaBuilder.BuildUnits();
            metaBuilder.BuildInterfaces();
            metaBuilder.BuildClasses();
            metaBuilder.BuildInheritances();
            metaBuilder.BuildRoles();
            metaBuilder.BuildInheritedRoles();
            metaBuilder.BuildImplementedRoles();
            metaBuilder.BuildAssociations();
            metaBuilder.BuildInheritedAssociations();
            metaBuilder.BuildDefinedMethods();
            metaBuilder.BuildInheritedMethods();
            metaBuilder.ExtendInterfaces();
            metaBuilder.ExtendClasses();
        }

        private MetaPopulation()
        {
            this.isStale = true;
            this.isDeriving = false;

            this.domains = new List<Domain>();
            this.units = new List<Unit>();
            this.interfaces = new List<Interface>();
            this.classes = new List<Class>();
            this.inheritances = new List<Inheritance>();
            this.relationTypes = new List<RelationType>();
            this.associationTypes = new List<AssociationType>();
            this.roleTypes = new List<RoleType>();
            this.methodTypes = new List<MethodType>();

            this.metaObjectById = new Dictionary<Guid, MetaObjectBase>();
        }

        public bool IsBound { get; private set; }

        public IEnumerable<Domain> Domains => this.domains;

        public IEnumerable<Domain> SortedDomains
        {
            get
            {
                var sortedDomains = new List<Domain>(this.domains);
                sortedDomains.Sort((x, y) => x.Superdomains.Contains(y) ? -1 : 1);
                return sortedDomains.ToArray();
            }
        }

        IEnumerable<IUnit> IMetaPopulation.Units => this.Units;

        public IEnumerable<Unit> Units => this.units;

        IEnumerable<IInterface> IMetaPopulation.Interfaces => this.Interfaces;

        public IEnumerable<Interface> Interfaces => this.interfaces;

        IEnumerable<IClass> IMetaPopulation.Classes => this.Classes;

        public IEnumerable<Class> Classes => this.classes;

        public IEnumerable<Inheritance> Inheritances => this.inheritances;

        IEnumerable<IRelationType> IMetaPopulation.RelationTypes => this.RelationTypes;

        public IEnumerable<RelationType> RelationTypes => this.relationTypes;

        public IEnumerable<AssociationType> AssociationTypes => this.associationTypes;

        public IEnumerable<RoleType> RoleTypes => this.roleTypes;

        public IEnumerable<MethodType> MethodTypes => this.methodTypes;

        IEnumerable<IComposite> IMetaPopulation.Composites => this.Composites;

        public IEnumerable<Composite> Composites
        {
            get
            {
                this.Derive();
                return this.derivedComposites;
            }
        }

        public IEnumerable<Composite> SortedComposites => this.Composites.OrderBy(v => v.Name);

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                var validation = this.Validate();
                if (validation.ContainsErrors)
                {
                    return false;
                }

                return true;
            }
        }

        // TODO: Added for Workspace.Meta
        public IEnumerable<Composite> WorkspaceComposites
        {
            get
            {
                this.Derive();
                return this.Composites.Where(m => m.Workspace);
            }
        }

        public IEnumerable<Interface> WorkspaceInterfaces
        {
            get
            {
                this.Derive();
                return this.Interfaces.Where(m => m.Workspace);
            }
        }

        public IEnumerable<Class> WorkspaceClasses
        {
            get
            {
                this.Derive();
                return this.Classes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<RelationType> WorkspaceRelationTypes
        {
            get
            {
                this.Derive();
                return this.RelationTypes.Where(m => m.Workspace);
            }
        }

        public IEnumerable<MethodType> WorkspaceMethodTypes
        {
            get
            {
                this.Derive();
                return this.MethodTypes.Where(m => m.Workspace);
            }
        }

        IMetaObject IMetaPopulation.Find(Guid id) => this.Find(id);

        /// <summary>
        /// Find a meta object by meta object id.
        /// </summary>
        /// <param name="id">
        /// The meta object id.
        /// </param>
        /// <returns>
        /// The <see cref="IMetaObject"/>.
        /// </returns>
        public MetaObjectBase Find(Guid id)
        {
            this.metaObjectById.TryGetValue(id, out var metaObject);

            return metaObject;
        }

        IClass IMetaPopulation.FindClassByName(string name) => this.FindByName(name);

        /// <summary>
        /// Find a meta object by name.
        /// </summary>
        /// <param name="name">
        /// The meta object id.
        /// </param>
        /// <returns>
        /// The <see cref="IMetaObject"/>.
        /// </returns>
        public Class FindByName(string name)
        {
            this.Derive();

            this.derivedClassByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var cls);

            return cls;
        }

        IValidationLog IMetaPopulation.Validate() => this.Validate();

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>The Validate.</returns>
        public ValidationLog Validate()
        {
            var log = new ValidationLog();

            foreach (var domain in this.Domains)
            {
                domain.Validate(log);
            }

            foreach (var unitType in this.Units)
            {
                unitType.Validate(log);
            }

            foreach (var @interface in this.Interfaces)
            {
                @interface.Validate(log);
            }

            foreach (var @class in this.Classes)
            {
                @class.Validate(log);
            }

            foreach (var inheritance in this.Inheritances)
            {
                inheritance.Validate(log);
            }

            foreach (var relationType in this.RelationTypes)
            {
                relationType.Validate(log);
            }

            foreach (var methodType in this.MethodTypes)
            {
                methodType.Validate(log);
            }

            var inheritancesBySubtype = new Dictionary<Composite, List<Inheritance>>();
            foreach (var inheritance in this.Inheritances)
            {
                var subtype = inheritance.Subtype;
                if (subtype != null)
                {
                    if (!inheritancesBySubtype.TryGetValue(subtype, out var inheritanceList))
                    {
                        inheritanceList = new List<Inheritance>();
                        inheritancesBySubtype[subtype] = inheritanceList;
                    }

                    inheritanceList.Add(inheritance);
                }
            }

            var supertypes = new HashSet<Interface>();
            foreach (var subtype in inheritancesBySubtype.Keys)
            {
                supertypes.Clear();
                if (this.HasCycle(subtype, supertypes, inheritancesBySubtype))
                {
                    var message = subtype.ValidationName + " has a cycle in its inheritance hierarchy";
                    log.AddError(message, subtype, ValidationKind.Cyclic, "IComposite.Supertypes");
                }
            }

            return log;
        }

        public void Bind(Type[] types, MethodInfo[] methods)
        {
            if (!this.IsBound)
            {
                this.Derive();

                this.IsBound = true;

                this.domains = this.domains.ToArray();
                this.units = this.units.ToArray();
                this.interfaces = this.interfaces.ToArray();
                this.classes = this.classes.ToArray();
                this.inheritances = this.inheritances.ToArray();
                this.relationTypes = this.relationTypes.ToArray();
                this.associationTypes = this.associationTypes.ToArray();
                this.roleTypes = this.roleTypes.ToArray();
                this.methodTypes = this.methodTypes.ToArray();

                foreach (var domain in this.domains)
                {
                    domain.Bind();
                }

                var typeByName = types.ToDictionary(type => type.Name, type => type);

                foreach (var unit in this.Units)
                {
                    unit.Bind();
                }

                foreach (var @interface in this.interfaces)
                {
                    @interface.Bind(typeByName);
                }

                foreach (var @class in this.classes)
                {
                    @class.Bind(typeByName);
                }

                var sortedDomains = new List<Domain>(this.domains);
                sortedDomains.Sort((a, b) => a.Superdomains.Contains(b) ? -1 : 1);

                var actionByMethodInfoByType = new Dictionary<Type, Dictionary<MethodInfo, Action<object, object>>>();

                foreach (var @class in this.Classes)
                {
                    foreach (var concreteMethodType in @class.ConcreteMethodTypes)
                    {
                        concreteMethodType.Bind(sortedDomains, methods, actionByMethodInfoByType);
                    }
                }
            }
        }

        internal void AssertUnlocked()
        {
            if (this.IsBound)
            {
                throw new Exception("Environment is locked");
            }
        }

        internal void Derive()
        {
            if (this.isStale && !this.isDeriving)
            {
                try
                {
                    this.isDeriving = true;

                    var sharedDomains = new HashSet<Domain>();
                    var sharedCompositeTypes = new HashSet<Composite>();
                    var sharedInterfaces = new HashSet<Interface>();
                    var sharedClasses = new HashSet<Class>();
                    var sharedAssociationTypes = new HashSet<AssociationType>();
                    var sharedRoleTypes = new HashSet<RoleType>();
                    var sharedMethodTypes = new HashSet<MethodType>();

                    // Domains
                    foreach (var domain in this.domains)
                    {
                        domain.DeriveSuperdomains(sharedDomains);
                    }

                    // Unit & IComposite ObjectTypes
                    var compositeTypes = new List<Composite>(this.Interfaces);
                    compositeTypes.AddRange(this.Classes);
                    this.derivedComposites = compositeTypes.ToArray();

                    // DirectSupertypes
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveDirectSupertypes(sharedInterfaces);
                    }

                    // DirectSubtypes
                    foreach (var type in this.Interfaces)
                    {
                        type.DeriveDirectSubtypes(sharedCompositeTypes);
                    }

                    // Supertypes
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveSupertypes(sharedInterfaces);
                    }

                    // isSynced
                    foreach (var composite in this.Composites)
                    {
                        composite.DeriveIsSynced();
                    }

                    // Subtypes
                    foreach (var type in this.Interfaces)
                    {
                        type.DeriveSubtypes(sharedCompositeTypes);
                    }

                    // Subclasses
                    foreach (var type in this.Interfaces)
                    {
                        type.DeriveSubclasses(sharedClasses);
                    }

                    // Exclusive Subclass
                    foreach (var type in this.Interfaces)
                    {
                        type.DeriveExclusiveSubclass();
                    }

                    var roleTypesByAssociationObjectType = new Dictionary<ObjectType, HashSet<RoleType>>();
                    var associationTypesByRoleObjectType = new Dictionary<ObjectType, HashSet<AssociationType>>();
                    foreach (var relationType in this.RelationTypes)
                    {
                        {
                            var associationObjectType = relationType.AssociationType.ObjectType;
                            if (!roleTypesByAssociationObjectType.TryGetValue(associationObjectType, out var roles))
                            {
                                roles = new HashSet<RoleType>();
                                roleTypesByAssociationObjectType[associationObjectType] = roles;
                            }

                            roles.Add(relationType.RoleType);
                        }

                        {
                            var roleObjectType = relationType.RoleType.ObjectType;
                            if (!associationTypesByRoleObjectType.TryGetValue(roleObjectType, out var associations))
                            {
                                associations = new HashSet<AssociationType>();
                                associationTypesByRoleObjectType[roleObjectType] = associations;
                            }

                            associations.Add(relationType.AssociationType);
                        }
                    }

                    // RoleTypes
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveRoleTypes(sharedRoleTypes, roleTypesByAssociationObjectType);
                    }

                    // AssociationTypes
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveAssociationTypes(sharedAssociationTypes, associationTypesByRoleObjectType);
                    }

                    // RoleType
                    foreach (var relationType in this.RelationTypes)
                    {
                        relationType.RoleType.DeriveScaleAndSize();
                    }

                    // RelationType Multiplicity
                    foreach (var relationType in this.RelationTypes)
                    {
                        relationType.DeriveMultiplicity();
                    }

                    var sharedMethodTypeList = new HashSet<MethodType>();

                    // MethodTypes
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveMethodTypes(sharedMethodTypeList);
                    }

                    // ConcreteRoleType
                    foreach (var @class in this.classes)
                    {
                        @class.DeriveConcreteRoleTypes(sharedRoleTypes);
                    }

                    // ConcreteMethodType
                    foreach (var @class in this.classes)
                    {
                        @class.DeriveConcreteMethodTypes(sharedMethodTypes);
                    }

                    // workspace composites
                    foreach (var composite in this.Composites)
                    {
                        composite.DeriveWorkspace();
                    }

                    // MetaPopulation
                    this.derivedClassByLowercaseName = new Dictionary<string, Class>();
                    foreach (var cls in this.classes)
                    {
                        this.derivedClassByLowercaseName[cls.Name.ToLowerInvariant()] = cls;
                    }
                }
                finally
                {
                    // Ignore stale requests during a derivation
                    this.isStale = false;
                    this.isDeriving = false;
                }
            }
        }

        internal void OnDomainCreated(Domain domain)
        {
            this.domains.Add(domain);
            this.metaObjectById.Add(domain.Id, domain);

            this.Stale();
        }

        internal void OnUnitCreated(Unit unit)
        {
            this.units.Add(unit);
            this.metaObjectById.Add(unit.Id, unit);

            this.Stale();
        }

        internal void OnInterfaceCreated(Interface @interface)
        {
            this.interfaces.Add(@interface);
            this.metaObjectById.Add(@interface.Id, @interface);

            this.Stale();
        }

        internal void OnClassCreated(Class @class)
        {
            this.classes.Add(@class);
            this.metaObjectById.Add(@class.Id, @class);

            this.Stale();
        }

        internal void OnInheritanceCreated(Inheritance inheritance)
        {
            this.inheritances.Add(inheritance);
            this.metaObjectById.Add(inheritance.Id, inheritance);

            this.Stale();
        }

        internal void OnRelationTypeCreated(RelationType relationType)
        {
            this.relationTypes.Add(relationType);
            this.metaObjectById.Add(relationType.Id, relationType);

            this.Stale();
        }

        internal void OnAssociationTypeCreated(AssociationType associationType)
        {
            this.associationTypes.Add(associationType);
            this.metaObjectById.Add(associationType.Id, associationType);

            this.Stale();
        }

        internal void OnRoleTypeCreated(RoleType roleType)
        {
            this.roleTypes.Add(roleType);
            this.metaObjectById.Add(roleType.Id, roleType);

            this.Stale();
        }

        internal void OnMethodTypeCreated(MethodType methodType)
        {
            this.methodTypes.Add(methodType);
            this.metaObjectById.Add(methodType.Id, methodType);

            this.Stale();
        }

        internal void Stale() => this.isStale = true;

        private bool HasCycle(Composite subtype, HashSet<Interface> supertypes, Dictionary<Composite, List<Inheritance>> inheritancesBySubtype)
        {
            foreach (var inheritance in inheritancesBySubtype[subtype])
            {
                var supertype = inheritance.Supertype;
                if (supertype != null)
                {
                    if (this.HasCycle(subtype, supertype, supertypes, inheritancesBySubtype))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasCycle(Composite originalSubtype, Interface currentSupertype, HashSet<Interface> supertypes, Dictionary<Composite, List<Inheritance>> inheritancesBySubtype)
        {
            if (originalSubtype is Interface && supertypes.Contains((Interface)originalSubtype))
            {
                return true;
            }

            if (!supertypes.Contains(currentSupertype))
            {
                supertypes.Add(currentSupertype);

                if (inheritancesBySubtype.TryGetValue(currentSupertype, out var currentSuperInheritances))
                {
                    foreach (var inheritance in currentSuperInheritances)
                    {
                        var newSupertype = inheritance.Supertype;
                        if (newSupertype != null)
                        {
                            if (this.HasCycle(originalSubtype, newSupertype, supertypes, inheritancesBySubtype))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
