// <copyright file="RoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Linq;
    using Text;

    public abstract partial class RoleType : IRoleTypeBase, IComparable
    {
        /// <summary>
        /// The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

        private readonly IMetaPopulationBase metaPopulation;
        private IObjectTypeBase objectType;

        private string singularName;
        private string pluralName;
        private int? precision;
        private int? scale;
        private int? size;
        private bool? isRequired;
        private bool? isUnique;

        private RoleTypeProps props;

        protected RoleType(IRelationTypeBase relationType)
        {
            this.metaPopulation = relationType.MetaPopulation;
            this.RelationType = relationType;

            this.metaPopulation.OnRoleTypeCreated(this);
        }

        public MetaPopulation M => (MetaPopulation)this.metaPopulation;

        public RoleTypeProps _ => this.props ??= new RoleTypeProps(this);

        IMetaPopulationBase IMetaObjectBase.MetaPopulation => this.metaPopulation;
        IMetaPopulation IMetaObject.MetaPopulation => this.metaPopulation;
        Origin IMetaObject.Origin => this.RelationType.Origin;

        public IRelationTypeBase RelationType { get; }
        IRelationType IRoleType.RelationType => this.RelationType;

        /// <summary>
        /// Gets the association.
        /// </summary>
        /// <value>The association.</value>
        public IAssociationTypeBase AssociationType => this.RelationType.AssociationType;
        IAssociationType IRoleType.AssociationType => this.AssociationType;

        public IObjectTypeBase ObjectType
        {
            get => this.objectType;

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.objectType = value;
                this.metaPopulation.Stale();
            }
        }

        IObjectType IPropertyType.ObjectType => this.ObjectType;

        string[] IOperandType.WorkspaceNames => this.RelationType.WorkspaceNames;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public string Name => this.IsMany ? this.PluralName : this.SingularName;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName => this.Name;

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        public string ValidationName => "RoleType: " + this.RelationType.Name;

        public string SingularName
        {
            get => !string.IsNullOrEmpty(this.singularName) ? this.singularName : this.ObjectType.SingularName;

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.singularName = value;
                this.metaPopulation.Stale();
            }
        }

        public bool ExistAssignedSingularName => !this.SingularName.Equals(this.ObjectType.SingularName);

        /// <summary>
        /// Gets the full singular name.
        /// </summary>
        /// <value>The full singular name.</value>
        public string SingularFullName => this.RelationType.AssociationType.ObjectType + this.SingularName;

        public string PluralName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.pluralName))
                {
                    return this.pluralName;
                }

                if (!string.IsNullOrEmpty(this.singularName))
                {
                    return Pluralizer.Pluralize(this.singularName);
                }

                return this.ObjectType.PluralName;
            }

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.pluralName = value;
                this.metaPopulation.Stale();
            }
        }

        public bool ExistAssignedPluralName => !this.PluralName.Equals(Pluralizer.Pluralize(this.SingularName));

        /// <summary>
        /// Gets the full plural name.
        /// </summary>
        /// <value>The full plural name.</value>
        public string PluralFullName => this.RelationType.AssociationType.ObjectType + this.PluralName;

        public bool IsMany
        {
            get
            {
                switch (this.RelationType.Multiplicity)
                {
                    case Multiplicity.OneToMany:
                    case Multiplicity.ManyToMany:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this state has a multiplicity of one.
        /// </summary>
        /// <value><c>true</c> if this state is one; otherwise, <c>false</c>.</value>
        public bool IsOne => !this.IsMany;

        public int? Size
        {
            get
            {
                this.metaPopulation.Derive();
                return this.size;
            }

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.size = value;
                this.metaPopulation.Stale();
            }
        }

        public int? Precision
        {
            get
            {
                this.metaPopulation.Derive();
                return this.precision;
            }

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.precision = value;
                this.metaPopulation.Stale();
            }
        }

        public int? Scale
        {
            get
            {
                this.metaPopulation.Derive();
                return this.scale;
            }

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.scale = value;
                this.metaPopulation.Stale();
            }
        }

        public bool IsRequired
        {
            get => this.isRequired ?? false;
            set => this.isRequired = value;
        }

        public bool IsUnique
        {
            get => this.isUnique ?? false;
            set => this.isUnique = value;
        }

        public string MediaType { get; set; }

        /// <summary>
        /// Get the value of the role on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The role value.
        /// </returns>
        public object Get(IStrategy strategy, IComposite ofType)
        {
            var role = strategy.GetRole(this);

            if (ofType == null || role == null || !this.ObjectType.IsComposite)
            {
                return role;
            }

            if (this.IsOne)
            {
                return ofType.IsAssignableFrom(((IObject)role).Strategy.Class) ? role : null;
            }

            var extent = (Extent)role;
            return extent.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
        }

        /// <summary>
        /// Set the value of the role on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <param name="value">
        /// The role value.
        /// </param>
        public void Set(IStrategy strategy, object value) => strategy.SetRole(this, value);

        public override bool Equals(object other) => this.RelationType.Id.Equals((other as RoleType)?.RelationType.Id);

        public override int GetHashCode() => this.RelationType.Id.GetHashCode();

        /// <summary>
        /// Compares the current state with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this state.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This state is less than <paramref name="obj"/>. Zero This state is equal to <paramref name="obj"/>. Greater than zero This state is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="other"/> is not the same type as this state. </exception>
        public int CompareTo(object other) => this.RelationType.Id.CompareTo((other as RoleType)?.RelationType.Id);

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() => $"{this.AssociationType.ObjectType.Name}.{this.Name}";

        /// <summary>
        /// Derive multiplicity, scale and size.
        /// </summary>
        public void DeriveScaleAndSize()
        {
            if (this.ObjectType is IUnit unitType)
            {
                switch (unitType.Tag)
                {
                    case UnitTags.String:
                        if (!this.Size.HasValue)
                        {
                            this.Size = 256;
                        }

                        this.Scale = null;
                        this.Precision = null;

                        break;

                    case UnitTags.Binary:
                        if (!this.Size.HasValue)
                        {
                            this.Size = MaximumSize;
                        }

                        this.Scale = null;
                        this.Precision = null;

                        break;

                    case UnitTags.Decimal:
                        if (!this.Precision.HasValue)
                        {
                            this.Precision = 19;
                        }

                        if (!this.Scale.HasValue)
                        {
                            this.Scale = 2;
                        }

                        this.Size = null;

                        break;

                    default:
                        this.Size = null;
                        this.Scale = null;
                        this.Precision = null;

                        break;
                }
            }
            else
            {
                this.Size = null;
                this.Scale = null;
                this.Precision = null;
            }
        }

        /// <summary>
        /// Validates the state.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        public void Validate(ValidationLog validationLog)
        {
            if (this.ObjectType == null)
            {
                var message = this.ValidationName + " has no IObjectType";
                validationLog.AddError(message, this, ValidationKind.Required, "RoleType.IObjectType");
            }

            if (!string.IsNullOrEmpty(this.SingularName) && this.SingularName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned singular name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.SingularName");
            }

            if (!string.IsNullOrEmpty(this.PluralName) && this.PluralName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned plural role name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.PluralName");
            }
        }
    }
}
