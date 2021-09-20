// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Linq;

    /// <summary>
    /// An <see cref="AssociationType"/> defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public abstract class AssociationType : IAssociationTypeInternals
    {
        public MetaPopulation MetaPopulation { get; set; }

        internal IRelationTypeInternals RelationType { get; set; }
        internal IRoleTypeInternals RoleType => this.RelationType.RoleType;

        private ICompositeInternals ObjectType { get; set; }
        private string SingularName { get; set; }
        private string PluralName { get; set; }
        private string Name { get; set; }
        private bool IsMany { get; set; }
        private bool IsOne { get; set; }

        #region IComparable
        int IComparable<IPropertyType>.CompareTo(IPropertyType other) => string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);
        #endregion

        #region IOperandType
        string IOperandType.OperandTag => this.RelationType.Tag;
        #endregion

        #region IPropertyType
        Origin IPropertyType.Origin => this.RelationType.Origin;

        string IPropertyType.Name => this.Name;

        string IPropertyType.SingularName => this.SingularName;

        string IPropertyType.PluralName => this.PluralName;

        IObjectType IPropertyType.ObjectType => this.ObjectType;

        bool IPropertyType.IsOne => this.IsOne;

        bool IPropertyType.IsMany => this.IsMany;
        #endregion

        #region IAssociationType

        IRelationType IAssociationType.RelationType => this.RelationType;

        IComposite IAssociationType.ObjectType => this.ObjectType;

        IRoleType IAssociationType.RoleType => this.RoleType;

        #endregion

        #region IAssociationTypeInternals
        ICompositeInternals IAssociationTypeInternals.ObjectType { get => this.ObjectType; set => this.ObjectType = value; }

        IRelationTypeInternals IAssociationTypeInternals.RelationType { get => this.RelationType; set => this.RelationType = value; }
        #endregion

        public override string ToString() => $"{this.RoleType.ObjectType.SingularName}.{this.Name}";

        public void Init()
        {
            const string where = "Where";

            this.IsMany = this.RelationType.Multiplicity == Multiplicity.ManyToOne ||
                          this.RelationType.Multiplicity == Multiplicity.ManyToMany;
            this.IsOne = !this.IsMany;
            this.SingularName = this.ObjectType.SingularName + where + this.RoleType.SingularName;
            this.PluralName = this.ObjectType.PluralName + where + this.RoleType.SingularName;
            this.Name = this.IsMany ? this.PluralName : this.SingularName;
        }
    }
}
