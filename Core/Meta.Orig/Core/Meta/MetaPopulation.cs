//------------------------------------------------------------------------------------------------- 
// <copyright file="MetaPopulation.cs" company="Allors bvba">
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
// <summary>Defines the Domain type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public sealed partial class MetaPopulation : IMetaPopulation
    {
        private const string NamespaceName = "Allors.Domain";

        public static readonly MetaPopulation Instance;

        private readonly Dictionary<Guid, MetaObject> metaObjectById;

        private Dictionary<string, Class> derivedClassByLowercaseName;
        private Composite[] derivedComposites;

        private bool isBound;

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

            var assembly = Assembly.GetExecutingAssembly();
            var classes = new List<Type>(assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract));

            // Domains
            foreach (var domainType in classes.Where(type => type.GetInterfaces().Contains(typeof(IDomain))))
            {
                var constructor = domainType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new []{Instance.GetType()}, null);
                var domain = (Domain)constructor.Invoke(new object[] { Instance });

                domain.Name = domainType.Name.Substring(0, domainType.Name.Length-"Domain".Length);

                var instanceProperty = domainType.GetProperty("Instance");
                instanceProperty.SetMethod.Invoke(null, new[] { domain });
            }

            // Domain Inheritance
            foreach (var domain in Instance.Domains)
            {
                var type = domain.GetType();

                // Always inherit from Object
                if (!domain.Equals(CoreDomain.Instance))
                {
                    domain.AddDirectSuperdomain(CoreDomain.Instance);
                }

                // Create Inheritance objects
                foreach (var attribute in type.GetCustomAttributes(typeof(InheritAttribute)))
                {
                    var inheritanceAttribute = (InheritAttribute)attribute;
                    var idAttribute = (IdAttribute)Attribute.GetCustomAttribute(inheritanceAttribute.Value, typeof(IdAttribute));
                    var id = new Guid(idAttribute.Value);
                    var superdomain = (Domain)Instance.Find(id);

                    domain.AddDirectSuperdomain(superdomain);
                }
            }

            // ObjectTypes
            foreach (var objectType in classes.Where(type => type.GetInterfaces().Contains(typeof(IUnit)) || type.GetInterfaces().Contains(typeof(IInterface)) || type.GetInterfaces().Contains(typeof(IClass))))
            {
                var constructor = objectType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                var instance = (ObjectType)constructor.Invoke(null);

                if (instance is IUnit)
                {
                    instance.SingularName = objectType.Name.Substring(0, objectType.Name.Length - "Unit".Length);
                }
                else if (instance is IClass)
                {
                    instance.SingularName = objectType.Name.Substring(0, objectType.Name.Length - "Class".Length);
                }
                else if (instance is IInterface)
                {
                    instance.SingularName = objectType.Name.Substring(0, objectType.Name.Length - "Interface".Length);
                }

                var pluralAttribute = (PluralAttribute)Attribute.GetCustomAttribute(objectType, typeof(PluralAttribute));
                if (pluralAttribute != null)
                {
                    instance.PluralName = pluralAttribute.Value;
                }
                else
                {
                    instance.PluralName = instance.SingularName + "s";
                }


                var instanceProperty = objectType.GetProperty("Instance");
                instanceProperty.SetMethod.Invoke(null, new[] { instance });
            }

            // ObjectType Inheritance
            foreach (var composite in Instance.Composites)
            {
                var type = composite.GetType();

                // Always inherit from Object
                if (!composite.Equals(ObjectInterface.Instance))
                {
                    new Inheritance(Instance)
                    {
                        Subtype = composite,
                        Supertype = ObjectInterface.Instance
                    };
                }

                // Create Inheritance objects
                foreach (var attribute in type.GetCustomAttributes(typeof(InheritAttribute)))
                {
                    var inheritanceAttribute = (InheritAttribute)attribute;
                    var idAttribute = (IdAttribute)Attribute.GetCustomAttribute(inheritanceAttribute.Value, typeof(IdAttribute));
                    var id = new Guid(idAttribute.Value);
                    var supertype = (Interface)Instance.Find(id);

                    new Inheritance(Instance)
                    {
                        Subtype = composite,
                        Supertype = supertype
                    };
                }
            }
            
            foreach (var composite in Instance.Composites)
            {
                var type = composite.GetType();

                // Create RelationType objects
                var relationTypeFields = type
                    .GetFields()
                    .Where(field => field.FieldType == typeof(RelationType));

                foreach (var relationTypeField in relationTypeFields)
                {
                    var idAttribute = (IdAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(IdAttribute));
                    var associationIdAttribute = (AssociationIdAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(AssociationIdAttribute));
                    var roleIdAttribute = (RoleIdAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(RoleIdAttribute));
                    var id = new Guid(idAttribute.Value);
                    var associationId = new Guid(associationIdAttribute.Value);
                    var roleId = new Guid(roleIdAttribute.Value);
                    var relationType = (RelationType)Activator.CreateInstance(typeof(RelationType), new object[] { Instance, id , associationId, roleId });

                    relationType.AssociationType.ObjectType = composite;

                    var multiplicityTypeAttribute = (MultiplicityAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(MultiplicityAttribute));
                    if (multiplicityTypeAttribute != null)
                    {
                        relationType.AssignedMultiplicity = multiplicityTypeAttribute.Value;
                    }

                    var derivedAttribute = (DerivedAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(DerivedAttribute));
                    if (derivedAttribute != null)
                    {
                        relationType.IsDerived = derivedAttribute.Value;
                    }

                    var indexedAttribute = (IndexedAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(IndexedAttribute));
                    if (indexedAttribute != null)
                    {
                        relationType.IsIndexed = indexedAttribute.Value;
                    }

                    relationType.AssociationType.ObjectType = composite;

                    relationType.RoleType.AssignedSingularName = relationTypeField.Name;

                    var roleTypeAttribute = (TypeAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(TypeAttribute));
                    var roleTypeIdAttribute = (IdAttribute)Attribute.GetCustomAttribute(roleTypeAttribute.Value, typeof(IdAttribute));
                    var roleTypeId = new Guid(roleTypeIdAttribute.Value);
                    var roleType = (ObjectType)Instance.Find(roleTypeId);

                    relationType.RoleType.ObjectType = roleType;

                    var scaleAttribute = (ScaleAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(ScaleAttribute));
                    if (scaleAttribute != null)
                    {
                        relationType.RoleType.Scale = scaleAttribute.Value;
                    }

                    var precisionAttribute = (PrecisionAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(PrecisionAttribute));
                    if (precisionAttribute != null)
                    {
                        relationType.RoleType.Precision = precisionAttribute.Value;
                    }

                    var sizeAttribute = (SizeAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(SizeAttribute));
                    if (sizeAttribute != null)
                    {
                        relationType.RoleType.Size = sizeAttribute.Value;
                    }

                    var pluralAttribute = (PluralAttribute)Attribute.GetCustomAttribute(relationTypeField, typeof(PluralAttribute));
                    if (pluralAttribute != null)
                    {
                        relationType.RoleType.AssignedPluralName = pluralAttribute.Value;
                    }

                    foreach (var groupAttribute in Attribute.GetCustomAttributes(relationTypeField, typeof(GroupAttribute)).Cast<GroupAttribute>())
                    {
                        relationType.AddGroup(groupAttribute.Value);
                    }

                    relationTypeField.SetValue(composite, relationType);
                }
            }

            foreach (var composite in Instance.Composites)
            {
                var type = composite.GetType();

                // Create MethodType objects
                var methodTypeFields = type
                    .GetFields()
                    .Where(field => field.FieldType == typeof(MethodType));

                foreach (var methodTypeField in methodTypeFields)
                {
                    var idAttribute = (IdAttribute)Attribute.GetCustomAttribute(methodTypeField, typeof(IdAttribute));
                    var id = new Guid(idAttribute.Value);
                    var methodType = (MethodType)Activator.CreateInstance(typeof(MethodType), new object[] { Instance, id });
                    methodType.Name = methodTypeField.Name;
                    methodType.ObjectType = composite;
                    methodTypeField.SetValue(composite, methodType);
                    
                    foreach (var groupAttribute in Attribute.GetCustomAttributes(methodTypeField, typeof(GroupAttribute)).Cast<GroupAttribute>())
                    {
                        methodType.AddGroup(groupAttribute.Value);
                    }
                }
            }

            Instance.Extend();
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

            this.metaObjectById = new Dictionary<Guid, MetaObject>();
        }

        public bool IsBound
        {
            get
            {
                return this.isBound;
            }
        }

        public IEnumerable<Domain> Domains
        {
            get
            {
                return this.domains;
            }
        }

        public IEnumerable<Domain> SortedDomains
        {
            get
            {
                var sortedDomains = new List<Domain>(this.domains);
                sortedDomains.Sort((x, y) => x.Superdomains.Contains(y) ? -1 : 1);
                return sortedDomains.ToArray();
            }
        }

        IEnumerable<IUnit> IMetaPopulation.Units 
        {
            get
            {
                return this.Units;
            }
        }

        public IEnumerable<Unit> Units
        {
            get
            {
                return this.units;
            }
        }

        IEnumerable<IInterface> IMetaPopulation.Interfaces
        {
            get
            {
                return this.Interfaces;
            }
        }

        public IEnumerable<Interface> Interfaces
        {
            get
            {
                return this.interfaces;
            }
        }

        IEnumerable<IClass> IMetaPopulation.Classes
        {
            get
            {
                return this.Classes;
            }
        }

        public IEnumerable<Class> Classes
        {
            get
            {
                return this.classes;
            }
        }

        public IEnumerable<Inheritance> Inheritances
        {
            get
            {
                return this.inheritances;
            }
        }

        IEnumerable<IRelationType> IMetaPopulation.RelationTypes
        {
            get
            {
                return this.RelationTypes;
            }
        }

        public IEnumerable<RelationType> RelationTypes
        {
            get
            {
                return this.relationTypes;
            }
        }

        public IEnumerable<AssociationType> AssociationTypes
        {
            get
            {
                return this.associationTypes;
            }
        }

        public IEnumerable<RoleType> RoleTypes
        {
            get
            {
                return this.roleTypes;
            }
        }

        public IEnumerable<MethodType> MethodTypes
        {
            get
            {
                return this.methodTypes;
            }
        }

        IEnumerable<IComposite> IMetaPopulation.Composites
        {
            get
            {
                return this.Composites;
            }
        }

        public IEnumerable<Composite> Composites
        {
            get
            {
                this.Derive();
                return this.derivedComposites;
            }
        }
        
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

        IMetaObject IMetaPopulation.Find(Guid id)
        {
            return this.Find(id);
        }

        /// <summary>
        /// Find a meta object by meta object id.
        /// </summary>
        /// <param name="id">
        /// The meta object id.
        /// </param>
        /// <returns>
        /// The <see cref="IMetaObject"/>.
        /// </returns>
        public MetaObject Find(Guid id)
        {
            MetaObject metaObject;
            this.metaObjectById.TryGetValue(id, out metaObject);

            return metaObject;
        }
        
        IClass IMetaPopulation.FindClassByName(string name)
        {
            return this.FindByName(name);
        }

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

            Class cls;
            this.derivedClassByLowercaseName.TryGetValue(name.ToLowerInvariant(), out cls);

            return cls;
        }

        IValidationLog IMetaPopulation.Validate()
        {
            return this.Validate();
        }

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
                    List<Inheritance> inheritanceList;
                    if (!inheritancesBySubtype.TryGetValue(subtype, out inheritanceList))
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

        public void Bind(Assembly assembly)
        {
            if (!this.IsBound)
            {
                this.Derive();

                this.isBound = true;

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
                    domain.Bind(assembly);
                }

                var types = assembly.GetTypes().Where(type =>
                    type.Namespace != null &&
                    type.Namespace.Equals(NamespaceName) &&
                    type.GetInterfaces().Contains(typeof(IObject)));

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

                var extensionMethods = (from type in assembly.GetTypes()
                            where type.IsSealed && !type.IsGenericType && !type.IsNested
                            from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                            where method.IsDefined(typeof(ExtensionAttribute), false)
                            select method).ToArray();

                var actionByMethodInfoByType = new Dictionary<Type, Dictionary<MethodInfo, Action<object, object>>>();

                foreach (var @class in this.Classes)
                {
                    foreach (var concreteMethodType in @class.ConcreteMethodTypes)
                    {
                        concreteMethodType.Bind(assembly, sortedDomains, extensionMethods, actionByMethodInfoByType);
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
                            HashSet<RoleType> roles;
                            if (!roleTypesByAssociationObjectType.TryGetValue(associationObjectType, out roles))
                            {
                                roles = new HashSet<RoleType>();
                                roleTypesByAssociationObjectType[associationObjectType] = roles;
                            }

                            roles.Add(relationType.RoleType);
                        }

                        {
                            var roleObjectType = relationType.RoleType.ObjectType;
                            HashSet<AssociationType> associations;
                            if (!associationTypesByRoleObjectType.TryGetValue(roleObjectType, out associations))
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

                    // RoleType Property Names
                    foreach (var relationType in this.RelationTypes)
                    {
                        relationType.RoleType.DeriveSingularPropertyName();
                        relationType.RoleType.DerivePluralPropertyName();
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

                    // DirectSupertypesByGroup
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveDirectSupertypesByGroup();
                    }

                    // AssociationTypesByGroup
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveAssociationTypesByGroup();
                    }

                    // RoleTypesByGroup
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveRoleTypesByGroup();
                    }

                    // ExclusiveRoleTypesByGroup
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveExclusiveRoleTypesByGroup();
                    }

                    // MethodTypesByGroup
                    foreach (var type in this.derivedComposites)
                    {
                        type.DeriveMethodTypesByGroup();
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

        internal void Stale()
        {
            this.isStale = true;
        }

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

                List<Inheritance> currentSuperInheritances;
                if (inheritancesBySubtype.TryGetValue(currentSupertype, out currentSuperInheritances))
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