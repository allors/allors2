// <copyright file="RoleType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;

    public sealed partial class RoleType : PropertyType, IRoleType, IComparable
    {
        /// <summary>
        /// The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

        public const string PluralSuffix = "s";

        public bool IsRequired { get; set; }

        public bool IsUnique { get; set; }

        public string MediaType { get; set; }

        private ObjectType objectType;

        private string singularName;

        private string pluralName;

        private int? scale;

        private int? precision;

        private int? size;

        internal RoleType(RelationType relationType, Guid id)
            : base(relationType.MetaPopulation)
        {
            this.RelationType = relationType;

            this.Id = id;

            relationType.MetaPopulation.OnRoleTypeCreated(this);
        }

        public bool Workspace => this.RelationType.Workspace;

        public IObjectType ObjectType
        {
            get => this.objectType;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.objectType = (ObjectType)value;
                this.MetaPopulation.Stale();
            }
        }

        public string SingularName
        {
            get => this.singularName;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.singularName = value;
                this.MetaPopulation.Stale();
            }
        }

        public string PluralName
        {
            get => this.pluralName;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.pluralName = value;
                this.MetaPopulation.Stale();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a multiplicity of one.
        /// </summary>
        /// <value><c>true</c> if this instance is one; otherwise, <c>false</c>.</value>
        public bool IsOne => !this.IsMany;

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

        IRelationType IRoleType.RelationType => this.RelationType;

        public RelationType RelationType { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName => this.PropertyName?.Humanize();

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public override string Name
        {
            get
            {
                if (this.IsMany)
                {
                    return this.PluralName;
                }

                return this.SingularName;
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

        /// <summary>
        /// Gets the full singular name.
        /// </summary>
        /// <value>The full singular name.</value>
        public string SingularFullName => this.RelationType.AssociationType.SingularName + this.SingularName;

        /// <summary>
        /// Gets the full plural name.
        /// </summary>
        /// <value>The full plural name.</value>
        public string PluralFullName => this.RelationType.AssociationType.SingularName + this.PluralName;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        /// <value>The full name.</value>
        public string PropertyName => this.IsMany ? this.PluralPropertyName : this.SingularPropertyName;

        public string SingularPropertyName => this.SingularName;

        public string PluralPropertyName => this.PluralName;

        IAssociationType IRoleType.AssociationType => this.AssociationType;

        /// <summary>
        /// Gets the association.
        /// </summary>
        /// <value>The association.</value>
        public AssociationType AssociationType => this.RelationType.AssociationType;

        public int? Scale
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.scale;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.scale = value;
                this.MetaPopulation.Stale();
            }
        }

        public int? Precision
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.precision;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.precision = value;
                this.MetaPopulation.Stale();
            }
        }

        public int? Size
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.size;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.size = value;
                this.MetaPopulation.Stale();
            }
        }

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        protected internal override string ValidationName => "RoleType: " + this.RelationType.Name;

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj is RoleType that)
            {
                return this.RelationType.Id.CompareTo(that.RelationType.Id);
            }

            return -1;
        }

        /// <summary>
        /// Get the object type.
        /// </summary>
        /// <returns>
        /// The <see cref="ObjectType"/>.
        /// </returns>
        public override ObjectType GetObjectType() => (ObjectType)this.ObjectType;

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            try
            {
                return this.RelationType.ToString();
            }
            catch
            {
                return base.ToString();
            }
        }

        /// <summary>
        /// Derive multiplicity, scale and size.
        /// </summary>
        internal void DeriveScaleAndSize()
        {
            if (this.ObjectType is IUnit unitType)
            {
                switch (unitType.UnitTag)
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
        /// Validates the instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

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
