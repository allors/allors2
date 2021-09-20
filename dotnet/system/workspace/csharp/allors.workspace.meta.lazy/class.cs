// <copyright file="Class.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Text;

    public abstract class Class : IClassInternals
    {
        public MetaPopulation MetaPopulation { get; set; }

        private HashSet<IAssociationTypeInternals> lazyAssociationTypes;
        private HashSet<IRoleTypeInternals> lazyRoleTypes;
        private HashSet<IRoleTypeInternals> lazyWorkspaceRoleTypes;
        private HashSet<IRoleTypeInternals> lazyDatabaseRoleTypes;
        private HashSet<IMethodTypeInternals> lazyMethodTypes;

        private HashSet<IAssociationTypeInternals> LazyAssociationTypes => this.lazyAssociationTypes ??= new HashSet<IAssociationTypeInternals>(this.ExclusiveAssociationTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveAssociationTypes)));
        private HashSet<IRoleTypeInternals> LazyRoleTypes => this.lazyRoleTypes ??= new HashSet<IRoleTypeInternals>(this.ExclusiveRoleTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveRoleTypes)));
        private HashSet<IRoleTypeInternals> LazyWorkspaceRoleTypes => this.lazyWorkspaceRoleTypes ??= new HashSet<IRoleTypeInternals>(this.LazyRoleTypes.Where(v => v.RelationType.Origin == Origin.Workspace));
        private HashSet<IRoleTypeInternals> LazyDatabaseRoleTypes => this.lazyDatabaseRoleTypes ??= new HashSet<IRoleTypeInternals>(this.LazyRoleTypes.Where(v => v.RelationType.Origin == Origin.Database));
        private HashSet<IMethodTypeInternals> LazyMethodTypes => this.lazyMethodTypes ??= new HashSet<IMethodTypeInternals>(this.ExclusiveMethodTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveMethodTypes)));

        private Origin Origin { get; set; }
        private string Tag { get; set; }
        private string SingularName { get; set; }
        private string PluralName { get; set; }
        private Type ClrType { get; set; }
        private Class[] Classes { get; set; }

        private ISet<IInterfaceInternals> DirectSupertypes { get; set; }
        private ISet<IInterfaceInternals> Supertypes { get; set; }
        private IRoleTypeInternals[] ExclusiveRoleTypes { get; set; }
        private IAssociationTypeInternals[] ExclusiveAssociationTypes { get; set; }
        private IMethodTypeInternals[] ExclusiveMethodTypes { get; set; }

        #region IComparable
        int IComparable<IObjectType>.CompareTo(IObjectType other) => string.Compare(this.SingularName, other.SingularName, StringComparison.InvariantCulture);
        #endregion

        #region IMetaObject
        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

        Origin IMetaObject.Origin => this.Origin;
        #endregion

        #region IMetaIdentifiableObject
        string IMetaObject.Tag => this.Tag;
        #endregion

        #region IObjectType
        bool IObjectType.IsUnit => false;

        bool IObjectType.IsComposite => true;

        bool IObjectType.IsInterface => false;

        bool IObjectType.IsClass => true;

        string IObjectType.SingularName => this.SingularName;

        string IObjectType.PluralName => this.PluralName;

        Type IObjectType.ClrType => this.ClrType;
        #endregion

        #region IComposite

        IEnumerable<IInterface> IComposite.DirectSupertypes => this.DirectSupertypes;

        IEnumerable<IInterface> IComposite.Supertypes => this.Supertypes;

        IEnumerable<IClass> IComposite.Classes => this.Classes;

        IEnumerable<IAssociationType> IComposite.AssociationTypes => this.LazyAssociationTypes;

        IEnumerable<IRoleType> IComposite.RoleTypes => this.LazyRoleTypes ?? new HashSet<IRoleTypeInternals>(this.ExclusiveRoleTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveRoleTypes)));

        IEnumerable<IRoleType> IComposite.WorkspaceOriginRoleTypes => this.LazyWorkspaceRoleTypes;

        IEnumerable<IRoleType> IComposite.DatabaseOriginRoleTypes => this.LazyDatabaseRoleTypes;

        IEnumerable<IMethodType> IComposite.MethodTypes => this.LazyMethodTypes;

        bool IComposite.IsAssignableFrom(IComposite objectType) => this.Equals(objectType);
        #endregion

        #region ICompositeInternals
        ISet<IInterfaceInternals> ICompositeInternals.DirectSupertypes { get => this.DirectSupertypes; set => this.DirectSupertypes = value; }
        ISet<IInterfaceInternals> ICompositeInternals.Supertypes { get => this.Supertypes; set => this.Supertypes = value; }
        IRoleTypeInternals[] ICompositeInternals.ExclusiveRoleTypes { get => this.ExclusiveRoleTypes; set => this.ExclusiveRoleTypes = value; }
        IAssociationTypeInternals[] ICompositeInternals.ExclusiveAssociationTypes { get => this.ExclusiveAssociationTypes; set => this.ExclusiveAssociationTypes = value; }
        IMethodTypeInternals[] ICompositeInternals.ExclusiveMethodTypes { get => this.ExclusiveMethodTypes; set => this.ExclusiveMethodTypes = value; }
        #endregion

        public void Init(string tag, string singularName, string pluralName = null, Origin origin = Origin.Database)
        {
            this.Tag = tag;
            this.SingularName = singularName;
            this.PluralName = pluralName ?? Pluralizer.Pluralize(singularName);
            this.Classes = new[] { this };
            this.Origin = origin;
        }

        void ICompositeInternals.Bind(Dictionary<string, Type> typeByTypeName) => this.ClrType = typeByTypeName[this.SingularName];
    }
}
