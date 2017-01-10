//------------------------------------------------------------------------------------------------- 
// <copyright file="RoleType.cs" company="Allors bvba">
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
// <summary>Defines the RoleType type.</summary>
//-------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Allors.Meta
{
    using System;

    public sealed partial class RoleType : PropertyType, IRoleType, IComparable
    {
        /// <summary>
        /// The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

        public const string PluralSuffix = "s";

        private readonly RelationType relationType;

        private ObjectType objectType;

        private string derivedSingularPropertyName;

        private string derivedPluralPropertyName;

        private string assignedSingularName;

        private string assignedPluralName;

        private int? scale;

        private int? precision;

        private int? size;

        internal RoleType(RelationType relationType, Guid id)
            : base(relationType.MetaPopulation)
        {
            this.relationType = relationType;

            this.Id = id;

            relationType.MetaPopulation.OnRoleTypeCreated(this);
        }

        IObjectType IRoleType.ObjectType
        {
            get
            {
                return this.ObjectType;
            }
        }

        public ObjectType ObjectType
        {
            get
            {
                return this.objectType;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.objectType = value;
                this.MetaPopulation.Stale();
            }
        }

        public string AssignedSingularName
        {
            get
            {
                return this.assignedSingularName;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedSingularName = value;
                this.MetaPopulation.Stale();
            }
        }

        public string AssignedPluralName
        {
            get
            {
                return this.assignedPluralName;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedPluralName = value;
                this.MetaPopulation.Stale();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a multiplicity of one.
        /// </summary>
        /// <value><c>true</c> if this instance is one; otherwise, <c>false</c>.</value>
        public bool IsOne
        {
            get { return !this.IsMany; }
        }

        public bool IsMany
        {
            get
            {
                switch (this.relationType.Multiplicity)
                {
                    case Multiplicity.OneToMany:
                    case Multiplicity.ManyToMany:
                        return true;

                    default:
                        return false;
                }
            }
        }

        IRelationType IRoleType.RelationType
        {
            get
            {
                return this.RelationType;
            }
        }

        public RelationType RelationType
        {
            get
            {
                return this.relationType;
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return this.PropertyName;
            }
        }

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
        /// Gets the singular name.
        /// </summary>
        /// <value>The singular name.</value>
        public string SingularName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AssignedSingularName))
                {
                    return this.AssignedSingularName;
                }

                return this.ObjectType != null ? this.ObjectType.SingularName : this.IdAsString;
            }
        }

        /// <summary>
        /// Gets the plural name.
        /// </summary>
        /// <value>The plural name.</value>
        public string PluralName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AssignedPluralName))
                {
                    return this.AssignedPluralName;
                }

                return this.AssignedSingularName != null ? this.AssignedSingularName + PluralSuffix : this.IdAsString;
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get
            {
                if (this.IsMany)
                {
                    return this.PluralFullName;
                }

                return this.SingularFullName;
            }
        }

        /// <summary>
        /// Gets the full singular name.
        /// </summary>
        /// <value>The full singular name.</value>
        public string SingularFullName
        {
            get { return this.RelationType.AssociationType.SingularName + this.SingularName; }
        }

        /// <summary>
        /// Gets the full plural name.
        /// </summary>
        /// <value>The full plural name.</value>
        public string PluralFullName
        {
            get { return this.RelationType.AssociationType.SingularName + this.PluralName; }
        }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        /// <value>The full name</value>
        public string PropertyName
        {
            get
            {
                if (this.IsMany)
                {
                    return this.PluralPropertyName;
                }

                return this.SingularPropertyName;
            }
        }

        public string SingularPropertyName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSingularPropertyName;
            }
        }

        public string PluralPropertyName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedPluralPropertyName;
            }
        }

        IAssociationType IRoleType.AssociationType
        {
            get
            {
                return this.AssociationType;
            }
        }

        /// <summary>
        /// Gets the association.
        /// </summary>
        /// <value>The association.</value>
        public AssociationType AssociationType
        {
            get { return this.RelationType.AssociationType; }
        }
        
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
        protected internal override string ValidationName
        {
            get
            {
                return "role type " + RelationType.Name;
            }
        }

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
            var that = obj as RoleType;
            if (that != null)
            {
                return this.RelationType.Id.CompareTo(that.RelationType.Id);
            }

            return -1;
        }

        /// <summary>
        /// Get the value of the role on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The role value.
        /// </returns>
        public override object Get(IStrategy strategy)
        {
            return strategy.GetRole(this);
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
        public void Set(IStrategy strategy, object value)
        {
            strategy.SetRole(this, value);
        }

        /// <summary>
        /// Get the object type.
        /// </summary>
        /// <returns>
        /// The <see cref="ObjectType"/>.
        /// </returns>
        public override ObjectType GetObjectType()
        {
            return this.ObjectType;
        }

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
            var unitType = this.ObjectType as IUnit;
            if (unitType != null)
            {
                switch (unitType.UnitTag)
                {
                    case UnitTags.AllorsString:
                        if (!this.Size.HasValue)
                        {
                            this.Size = 256;
                        }

                        this.Scale = null;
                        this.Precision = null;

                        break;
                    case UnitTags.AllorsBinary:
                        if (!this.Size.HasValue)
                        {
                            this.Size = MaximumSize;
                        }

                        this.Scale = null;
                        this.Precision = null;

                        break;
                    case UnitTags.AllorsDecimal:
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

        internal void DeriveSingularPropertyName()
        {
            this.derivedSingularPropertyName = null;

            if (this.ObjectType != null && this.AssociationType.ObjectType != null)
            {
                this.derivedSingularPropertyName = this.SingularName;

                if (this.AssociationType.ObjectType.ExistClass)
                {
                    foreach (var @class in this.AssociationType.ObjectType.Classes)
                    {
                        foreach (var otherRole in @class.RoleTypes)
                        {
                            if (!Equals(otherRole))
                            {
                                if (otherRole.ObjectType != null)
                                {
                                    if (otherRole.SingularName.Equals(this.SingularName))
                                    {
                                        this.derivedSingularPropertyName = this.SingularFullName;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void DerivePluralPropertyName()
        {
            this.derivedPluralPropertyName = null;

            if (this.ObjectType != null && this.AssociationType.ObjectType != null)
            {
                this.derivedPluralPropertyName = this.PluralName;

                if (this.AssociationType.ObjectType.ExistClass)
                {
                    foreach (var @class in this.AssociationType.ObjectType.Classes)
                    {
                        foreach (var otherRole in @class.RoleTypes)
                        {
                            if (!Equals(otherRole))
                            {
                                if (otherRole.ObjectType != null)
                                {
                                    if (otherRole.PluralName.Equals(this.PluralName))
                                    {
                                        this.derivedPluralPropertyName = this.PluralFullName;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal void Validate(ValidationLog validationLog)
        {
            if (this.ObjectType == null)
            {
                var message = this.ValidationName + " has no IObjectType";
                validationLog.AddError(message, this, ValidationKind.Required, "RoleType.IObjectType");
            }

            if (!string.IsNullOrEmpty(this.AssignedSingularName) && this.AssignedSingularName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned singular name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.AssignedSingularName");
            }

            if (!string.IsNullOrEmpty(this.AssignedPluralName) && this.AssignedPluralName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned plural role name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.AssignedPluralName");
            }
        }
    }
}