// <copyright file="MetaPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class MetaPopulation : IMetaPopulation
    {
        private IUnitInternals[] Units { get; set; }
        private IInterfaceInternals[] Interfaces { get; set; }
        private IClassInternals[] Classes { get; set; }
        private IRelationTypeInternals[] RelationTypes { get; set; }
        private IMethodTypeInternals[] MethodTypes { get; set; }

        private Dictionary<string, IMetaObject> MetaObjectByTag { get; set; }
        private ICompositeInternals[] Composites { get; set; }
        private Dictionary<string, ICompositeInternals> CompositeByLowercaseName { get; set; }

        #region IMetaPopulation
        IEnumerable<IUnit> IMetaPopulation.Units => this.Units;
        IEnumerable<IInterface> IMetaPopulation.Interfaces => this.Interfaces;
        IEnumerable<IClass> IMetaPopulation.Classes => this.Classes;
        IEnumerable<IRelationType> IMetaPopulation.RelationTypes => this.RelationTypes;
        IEnumerable<IMethodType> IMetaPopulation.MethodTypes => this.MethodTypes;
        IEnumerable<IComposite> IMetaPopulation.Composites => this.Composites;
        IMetaObject IMetaPopulation.FindByTag(string tag)
        {
            this.MetaObjectByTag.TryGetValue(tag, out var metaObject);
            return metaObject;
        }
        IComposite IMetaPopulation.FindByName(string name)
        {
            this.CompositeByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var composite);
            return composite;
        }
        void IMetaPopulation.Bind(Type[] types)
        {
            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var unit in this.Units)
            {
                unit.Bind();
            }

            foreach (var @interface in this.Interfaces)
            {
                @interface.Bind(typeByName);
            }

            foreach (var @class in this.Classes)
            {
                @class.Bind(typeByName);
            }
        }

        #endregion

        public void Init(IUnitInternals[] units, IInterfaceInternals[] interfaces, IClassInternals[] classes, Inheritance[] inheritances, IRelationTypeInternals[] relationTypes, IMethodTypeInternals[] methodTypes)
        {
            this.Units = units;
            this.Interfaces = interfaces;
            this.Classes = classes;
            this.RelationTypes = relationTypes;
            this.MethodTypes = methodTypes;

            this.MetaObjectByTag =
                this.Units.Cast<IMetaObject>()
                .Union(this.Classes)
                .Union(this.RelationTypes)
                .Union(this.MethodTypes)
                .ToDictionary(v => v.Tag, v => v);

            this.Composites = this.Interfaces.Cast<ICompositeInternals>().Union(this.Classes).ToArray();
            this.CompositeByLowercaseName = this.Composites.ToDictionary(v => v.SingularName.ToLowerInvariant());

            foreach (var composite in this.Composites)
            {
                composite.MetaPopulation = this;
            }

            foreach (var unit in this.Units)
            {
                unit.MetaPopulation = this;
            }

            foreach (var methodType in this.MethodTypes)
            {
                methodType.MetaPopulation = this;
            }

            // DirectSupertypes
            foreach (var grouping in inheritances.GroupBy(v => v.Subtype, v => v.Supertype))
            {
                var composite = grouping.Key;
                composite.DirectSupertypes = new HashSet<IInterfaceInternals>(grouping);
            }

            // DirectSubtypes
            foreach (var grouping in inheritances.GroupBy(v => v.Supertype, v => v.Subtype))
            {
                var @interface = grouping.Key;
                @interface.DirectSubtypes = new HashSet<ICompositeInternals>(grouping);
            }

            // Supertypes
            foreach (var composite in this.Composites)
            {
                static IEnumerable<IInterfaceInternals> RecurseDirectSupertypes(ICompositeInternals composite)
                {
                    if (composite.DirectSupertypes != null)
                    {
                        foreach (var directSupertype in composite.DirectSupertypes)
                        {
                            yield return directSupertype;

                            foreach (var directSuperSupertype in RecurseDirectSupertypes(directSupertype))
                            {
                                yield return directSuperSupertype;
                            }
                        }
                    }
                }

                composite.Supertypes = new HashSet<IInterfaceInternals>(RecurseDirectSupertypes(composite));
            }

            // Subtypes
            foreach (var @interface in this.Interfaces)
            {
                static IEnumerable<ICompositeInternals> RecurseDirectSubtypes(IInterfaceInternals @interface)
                {
                    if (@interface.DirectSubtypes != null)
                    {
                        foreach (var directSubtype in @interface.DirectSubtypes)
                        {
                            yield return directSubtype;

                            if (directSubtype is Interface directSubinterface)
                            {
                                foreach (var directSubSubtype in RecurseDirectSubtypes(directSubinterface))
                                {
                                    yield return directSubSubtype;
                                }
                            }
                        }
                    }
                }

                @interface.Subtypes = new HashSet<ICompositeInternals>(RecurseDirectSubtypes(@interface));
                @interface.Classes = new HashSet<IClassInternals>(@interface.Subtypes.Where(v => v.IsClass).Cast<Class>());
            }

            // RoleTypes
            {
                var exclusiveRoleTypesObjectType = this.RelationTypes
                    .GroupBy(v => v.AssociationType.ObjectType)
                    .ToDictionary(g => g.Key, g => g.Select(v => v.RoleType).ToArray());

                foreach (var objectType in this.Composites)
                {
                    exclusiveRoleTypesObjectType.TryGetValue(objectType, out var exclusiveRoleTypes);
                    objectType.ExclusiveRoleTypes = exclusiveRoleTypes ?? Array.Empty<IRoleTypeInternals>();
                }
            }

            // AssociationTypes
            {
                var exclusiveAssociationTypesByObjectType = this.RelationTypes
                   .GroupBy(v => v.RoleType.ObjectType)
                   .ToDictionary(g => g.Key, g => g.Select(v => v.AssociationType).ToArray());

                foreach (var objectType in this.Composites)
                {
                    exclusiveAssociationTypesByObjectType.TryGetValue(objectType, out var exclusiveAssociationTypes);
                    objectType.ExclusiveAssociationTypes = exclusiveAssociationTypes ?? Array.Empty<IAssociationTypeInternals>();
                }
            }

            // MethodTypes
            {
                var exclusiveMethodTypeByObjectType = this.MethodTypes
                    .GroupBy(v => v.ObjectType)
                    .ToDictionary(g => g.Key, g => g.ToArray());

                foreach (var objectType in this.Composites)
                {
                    exclusiveMethodTypeByObjectType.TryGetValue(objectType, out var exclusiveMethodTypes);
                    objectType.ExclusiveMethodTypes = exclusiveMethodTypes ?? Array.Empty<IMethodTypeInternals>();
                }
            }
        }
    }
}
