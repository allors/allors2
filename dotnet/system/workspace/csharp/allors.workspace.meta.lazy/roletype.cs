// <copyright file="RoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Linq;
    using Text;

    public abstract class RoleType : IRoleTypeInternals
    {
        /// <summary>
        /// The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

        public MetaPopulation MetaPopulation { get; set; }

        private IRelationTypeInternals RelationType { get; set; }
        private IAssociationTypeInternals AssociationType => this.RelationType.AssociationType;

        private IObjectType ObjectType { get; set; }
        private string SingularName { get; set; }
        private string PluralName { get; set; }
        private string Name { get; set; }
        private bool IsMany { get; set; }
        private bool IsOne { get; set; }
        private int? Size { get; set; }
        private int? Precision { get; set; }
        private int? Scale { get; set; }
        private bool IsRequired { get; set; }
        private bool IsUnique { get; set; }
        private string MediaType { get; set; }

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

        #region IRoleType
        IAssociationType IRoleType.AssociationType => this.AssociationType;

        IRelationType IRoleType.RelationType => this.RelationType;

        int? IRoleType.Size => this.Size;

        int? IRoleType.Precision => this.Precision;

        int? IRoleType.Scale => this.Scale;

        bool IRoleType.IsRequired => this.IsRequired;

        bool IRoleType.IsUnique => this.IsUnique;

        string IRoleType.MediaType => this.MediaType;
        #endregion

        #region IRoleTypeInternals
        IObjectType IRoleTypeInternals.ObjectType { get => this.ObjectType; set => this.ObjectType = value; }

        IRelationTypeInternals IRoleTypeInternals.RelationType { get => this.RelationType; set => this.RelationType = value; }
        #endregion
        
        public override string ToString() => $"{this.AssociationType.ObjectType.SingularName}.{this.Name}";

        public void Init(string singularName = null, string pluralName = null, int? size = null, int? precision = null, int? scale = null, bool isRequired = false, bool isUnique = false, string mediaType = null)
        {
            this.SingularName = singularName ?? this.ObjectType.SingularName;
            this.PluralName = pluralName ?? Pluralizer.Pluralize(this.SingularName);

            this.IsMany = this.RelationType.Multiplicity == Multiplicity.OneToMany ||
                          this.RelationType.Multiplicity == Multiplicity.ManyToMany;
            this.IsOne = !this.IsMany;
            this.Name = this.IsMany ? this.PluralName : this.SingularName;

            if (this.ObjectType is IUnit unitType)
            {
                switch (unitType.Tag)
                {
                    case UnitTags.String:
                        this.Size = size ?? 256;
                        break;

                    case UnitTags.Binary:
                        this.Size = size ?? MaximumSize;
                        break;

                    case UnitTags.Decimal:
                        this.Precision = precision ?? 19;
                        this.Scale = scale ?? 2;
                        break;
                }
            }

            this.IsRequired = isRequired;
            this.IsUnique = isUnique;
            this.MediaType = mediaType;
        }
    }
}
