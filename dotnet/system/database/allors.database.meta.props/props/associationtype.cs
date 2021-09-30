// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An <see cref="AssociationType"/> defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public abstract partial class AssociationType : IAssociationTypeBase, IComparable
    {
        /// <summary>
        /// Used to create property names.
        /// </summary>
        private const string Where = "Where";

        private readonly IMetaPopulationBase metaPopulation;
        private readonly IRelationTypeBase relationType;
        private ICompositeBase objectType;

        private AssociationTypeProps props;

        protected AssociationType(IRelationTypeBase relationType)
        {
            this.metaPopulation = relationType.MetaPopulation;
            this.relationType = relationType;
            relationType.MetaPopulation.OnAssociationTypeCreated(this);
        }

        public MetaPopulation M => (MetaPopulation)this.metaPopulation;

        public AssociationTypeProps _ => this.props ??= new AssociationTypeProps(this);

        #region IMetaObject & IMetaObjectBase
        IMetaPopulation IMetaObject.MetaPopulation => this.metaPopulation;

        Origin IMetaObject.Origin => this.relationType.Origin;

        IMetaPopulationBase IMetaObjectBase.MetaPopulation => this.metaPopulation;

        string IMetaObjectBase.ValidationName => this.ValidationName;
        #endregion

        #region IOperandType & IOperandTypeBase
        string IOperandTypeBase.DisplayName => this.DisplayName;

        string[] IOperandType.WorkspaceNames => this.relationType.WorkspaceNames;

        #endregion

        #region IPropertyType & IPropertyTypeBase
        string IPropertyType.Name => this.Name;

        string IPropertyType.SingularName => this.SingularName;

        string IPropertyType.SingularFullName => this.SingularName;

        string IPropertyType.PluralName => this.PluralName;

        string IPropertyType.PluralFullName => this.PluralName;

        IObjectType IPropertyType.ObjectType => this.objectType;

        bool IPropertyType.IsOne => !this.IsMany;

        bool IPropertyType.IsMany => this.IsMany;

        object IPropertyType.Get(IStrategy strategy, IComposite ofType)
        {
            var association = strategy.GetAssociation(this);

            if (ofType == null || association == null)
            {
                return association;
            }

            if (this.IsMany)
            {
                var extent = (IEnumerable<IObject>)association;
                return extent.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
            }

            return !ofType.IsAssignableFrom(((IObject)association).Strategy.Class) ? null : association;
        }

        IObjectTypeBase IPropertyTypeBase.ObjectType => this.objectType;

        #endregion

        #region IAssociationType & IAssociationTypeBase
        IRelationType IAssociationType.RelationType => this.relationType;

        IRoleType IAssociationType.RoleType => this.RoleType;

        IComposite IAssociationType.ObjectType => this.objectType;

        ICompositeBase IAssociationTypeBase.ObjectType
        {
            get => this.objectType;

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.objectType = value;
                this.metaPopulation.Stale();
            }
        }

        IRelationTypeBase IAssociationTypeBase.RelationType => this.relationType;

        IRoleTypeBase IAssociationTypeBase.RoleType => this.RoleType;

        void IAssociationTypeBase.Validate(ValidationLog validationLog)
        {
            if (this.objectType == null)
            {
                var message = this.ValidationName + " has no object type";
                validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.IObjectType");
            }

            if (this.relationType == null)
            {
                var message = this.ValidationName + " has no relation type";
                validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.RelationType");
            }
        }

        #endregion

        public IRoleTypeBase RoleType => this.relationType.RoleType;

        private bool IsMany
        {
            get
            {
                switch (this.relationType.Multiplicity)
                {
                    case Multiplicity.ManyToOne:
                    case Multiplicity.ManyToMany:
                        return true;

                    default:
                        return false;
                }
            }
        }

        private string Name => this.IsMany ? this.PluralName : this.SingularName;

        private string SingularName => this.objectType.SingularName + Where + this.RoleType.SingularName;

        private string PluralName => this.objectType.PluralName + Where + this.RoleType.SingularName;

        private string DisplayName => this.Name;

        private string ValidationName => "association type " + this.Name;

        public override bool Equals(object other) => this.relationType.Id.Equals((other as AssociationType)?.relationType.Id);

        public override int GetHashCode() => this.relationType.Id.GetHashCode();

        public int CompareTo(object other) => this.relationType.Id.CompareTo((other as AssociationType)?.relationType.Id);

        public override string ToString() => $"{this.RoleType.ObjectType.Name}.{this.DisplayName}";
    }
}
