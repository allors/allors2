// <copyright file="MetaPopulationProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MetaPopulation type.</summary>

namespace Allors.Database.Meta
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed partial class MetaPopulationProps
    {
        private readonly IMetaPopulationBase metaPopulation;

        internal MetaPopulationProps(IMetaPopulationBase relationClass) => this.metaPopulation = relationClass;

        public IEnumerable<IDomain> Domains => this.metaPopulation.Domains;

        public IEnumerable<IUnit> Units => this.metaPopulation.Units;

        public IEnumerable<IComposite> Composites => this.metaPopulation.Composites;

        public IEnumerable<IInterface> Interfaces => this.metaPopulation.Interfaces;

        public IEnumerable<IClass> Classes => this.metaPopulation.Classes;

        public IEnumerable<IRelationType> RelationTypes => this.metaPopulation.RelationTypes;

        public IEnumerable<IMethodType> MethodTypes => this.metaPopulation.MethodTypes;

        public IEnumerable<IInheritance> Inheritances => this.metaPopulation.Inheritances;

        public IEnumerable<IComposite> DatabaseComposites => this.metaPopulation.DatabaseComposites;

        public IEnumerable<IInterface> DatabaseInterfaces => this.metaPopulation.DatabaseInterfaces;

        public IEnumerable<IClass> DatabaseClasses => this.metaPopulation.DatabaseClasses;

        public IEnumerable<IRelationType> DatabaseRelationTypes => this.metaPopulation.DatabaseRelationTypes;

        public IEnumerable<string> WorkspaceNames => this.metaPopulation.WorkspaceNames;

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceOriginTagsByWorkspaceName(Origin origin) =>
            this.WorkspaceNames
                .ToDictionary(v => v, v =>
                    this.Composites.Where(w => w.Origin == origin && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).Union(
                    this.RelationTypes.Where(w => w.Origin == origin && w.WorkspaceNames.Contains(v)).Select(w => w.Tag)).Union(
                    this.MethodTypes.Where(w => w.Origin == origin && w.WorkspaceNames.Contains(v)).Select(w => w.Tag))
                        .OrderBy(w => w));

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceWorkspaceOriginTagsByWorkspaceName => this.WorkspaceOriginTagsByWorkspaceName(Origin.Workspace);

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceSessionOriginTagsByWorkspaceName => this.WorkspaceOriginTagsByWorkspaceName(Origin.Session);

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity multiplicity) =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.RoleType.ObjectType.IsComposite && w.Multiplicity == multiplicity && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceOneToOneTagsByWorkspaceName => this.WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity.OneToOne);

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceOneToManyTagsByWorkspaceName => this.WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity.OneToMany);

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceManyToManyTagsByWorkspaceName => this.WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity.ManyToMany);

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceDerivedTagsByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.IsDerived && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceRequiredTagsByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.RoleType.IsRequired && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

        public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceUniqueTagsByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.RoleType.IsUnique && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

        public IReadOnlyDictionary<string, Dictionary<string, IOrderedEnumerable<string>>> WorkspaceMediaTagsByMediaTypeNameByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v =>
                    this.RelationTypes.Where(w => !string.IsNullOrWhiteSpace(w.RoleType.MediaType) && w.WorkspaceNames.Contains(v))
                        .GroupBy(w => w.RoleType.MediaType, w => w.Tag)
                            .ToDictionary(w => w.Key, w => w.OrderBy(x => x)));

        public IReadOnlyDictionary<string, IOrderedEnumerable<IComposite>> WorkspaceCompositesByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.Composites.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

        public IReadOnlyDictionary<string, IOrderedEnumerable<IInterface>> WorkspaceInterfacesByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.Interfaces.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

        public IReadOnlyDictionary<string, IOrderedEnumerable<IClass>> WorkspaceClassesByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.Classes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRelationType>> WorkspaceRelationTypesByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

        public IReadOnlyDictionary<string, IOrderedEnumerable<IMethodType>> WorkspaceMethodTypesByWorkspaceName =>
            this.WorkspaceNames
                .ToDictionary(v => v, v => this.MethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

        public bool IsValid => this.metaPopulation.IsValid;
    }
}
