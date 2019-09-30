// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Meta
{
    using System;

    /// <summary>
    /// An <see cref="AssociationType"/> defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public sealed partial class AssociationType : PropertyType, IAssociationType, IComparable
    {
        /// <summary>
        /// Used to create property names.
        /// </summary>
        private const string Where = "Where";

        private Composite objectType;

        internal AssociationType(RelationType relationType, Guid id)
            : base(relationType.MetaPopulation)
        {
            this.RelationType = relationType;

            this.Id = id;

            relationType.MetaPopulation.OnAssociationTypeCreated(this);
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName => this.PropertyName;

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

        public bool IsMany
        {
            get
            {
                switch (this.RelationType.Multiplicity)
                {
                    case Multiplicity.ManyToOne:
                    case Multiplicity.ManyToMany:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a multiplicity of one.
        /// </summary>
        /// <value><c>true</c> if this instance is one; otherwise, <c>false</c>.</value>
        public bool IsOne => !this.IsMany;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public override string Name => this.FullName;

        IComposite IAssociationType.ObjectType => this.ObjectType;

        IObjectType IPropertyType.ObjectType => this.objectType;

        public Composite ObjectType
        {
            get => this.objectType;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.objectType = value;
                this.MetaPopulation.Stale();
            }
        }

        /// <summary>
        /// Gets the plural name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The plural name when using <see cref="Where"/>.</value>
        public string PluralFullName => this.RoleType.SingularName + this.PluralName;

        /// <summary>
        /// Gets the plural name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The plural name when using <see cref="Where"/>.</value>
        public string PluralName => this.ObjectType.PluralName;

        /// <summary>
        /// Gets the plural name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The plural name when using <see cref="Where"/>.</value>
        public string PluralPropertyName => this.ObjectType.PluralName + Where + this.RoleType.SingularName;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        /// <value>The full name.</value>
        public string PropertyName => this.IsMany ? this.PluralPropertyName : this.SingularPropertyName;

        IRelationType IAssociationType.RelationType => this.RelationType;

        public RelationType RelationType { get; private set; }

        IRoleType IAssociationType.RoleType => this.RoleType;

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>The role .</value>
        public RoleType RoleType => this.RelationType.RoleType;

        /// <summary>
        /// Gets the singular name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The singular name when using <see cref="Where"/>.</value>
        public string SingularFullName => this.RoleType.SingularName + this.SingularName;

        /// <summary>
        /// Gets the singular name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The singular name when using <see cref="Where"/>.</value>
        public string SingularName => this.ObjectType.SingularName;

        /// <summary>
        /// Gets the singular name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The singular name when using <see cref="Where"/>.</value>
        public string SingularPropertyName => this.ObjectType.SingularName + Where + this.RoleType.SingularName;

        public bool Workspace => this.RelationType.Workspace;

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The name of the validation.</value>
        protected internal override string ValidationName => "association type " + this.RelationType.Name;

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
            if (obj is AssociationType that)
            {
                return this.RelationType.Id.CompareTo(that.RelationType.Id);
            }

            return -1;
        }

        /// <summary>
        /// Get the value of the association on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The association value.
        /// </returns>
        public override object Get(IStrategy strategy) => strategy.GetAssociation(this.RelationType);

        /// <summary>
        /// Get the object type.
        /// </summary>
        /// <returns>
        /// The <see cref="ObjectType"/>.
        /// </returns>
        public override ObjectType GetObjectType() => this.ObjectType;

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
        /// Validates this object.
        /// </summary>
        /// <param name="validationLog">The validation information.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            if (this.ObjectType == null)
            {
                var message = this.ValidationName + " has no object type";
                validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.IObjectType");
            }

            if (this.RelationType == null)
            {
                var message = this.ValidationName + " has no relation type";
                validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.RelationType");
            }
        }
    }
}
