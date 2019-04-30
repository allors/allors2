//------------------------------------------------------------------------------------------------- 
// <copyright file="RelationType.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the RelationType type.</summary>
//-------------------------------------------------------------------------------------------------

using System.Linq;

namespace Allors.Meta
{
    using System;

    /// <summary>
    /// A <see cref="RelationType"/> defines the state and behavior for
    /// a set of <see cref="AssociationType"/>s and <see cref="RoleType"/>s.
    /// </summary>
    public sealed partial class RelationType : DomainObject, IRelationType, IComparable
    {
        private Multiplicity assignedMultiplicity;
        private Multiplicity multiplicity;

        private bool isDerived;
        private bool isSynced;
        private bool isIndexed;

        private string xmlDoc;

        private bool workspace;

        public RelationType(MetaPopulation metaPopulation, Guid id, Guid associationTypeId, Guid roleTypdId)
            : base(metaPopulation)
        {
            this.Id = id;

            this.AssociationType = new AssociationType(this, associationTypeId);
            this.RoleType = new RoleType(this, roleTypdId);

            metaPopulation.OnRelationTypeCreated(this);
        }

        public bool Workspace
        {
            get
            {
                return this.workspace;
            }
            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.workspace = value;
                this.MetaPopulation.Stale();
            }
        }

        public string XmlDoc
        {
            get
            {
                return this.xmlDoc;
            }

            set
            {
                this.xmlDoc = !string.IsNullOrWhiteSpace(value) ? value : null;
            }
        }

        public string XmlDocComment
        {
            get
            {
                var lines = this.xmlDoc?.Split('\n').Select(v => "   /// " + v).ToArray();
                if (lines != null && lines.Any())
                {
                    return string.Join("\n", lines);
                }

                return null;
            }
        }

        public bool IsDerived
        {
            get 
            {
                return this.isDerived;
            }

            set 
            {
                this.MetaPopulation.AssertUnlocked();
                this.isDerived = value;
                this.MetaPopulation.Stale();
            }
        }
        
        public bool IsSynced
        {
            get
            {
                return this.isSynced;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.isSynced = value;
                this.MetaPopulation.Stale();
            }
        }

        public Multiplicity AssignedMultiplicity 
        {
            get
            {
                return this.assignedMultiplicity;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedMultiplicity = value;
                this.MetaPopulation.Stale();
            }
        }

        public Multiplicity Multiplicity 
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.multiplicity;
            }
        }

        public bool IsIndexed
        {
            get
            {
                return this.isIndexed;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.isIndexed = value;
                this.MetaPopulation.Stale();
            }
        }

        IAssociationType IRelationType.AssociationType => this.AssociationType;

        public AssociationType AssociationType { get; }

        IRoleType IRelationType.RoleType => this.RoleType;

        public RoleType RoleType { get; }

        /// <summary>
        /// Gets a value indicating whether there exist exclusive classes.
        /// </summary>
        /// <value>
        ///  <c>true</c> if [exist exclusive classes]; otherwise, <c>false</c>.
        /// </value>
        public bool ExistExclusiveClasses
        {
            get
            {
                if (this.AssociationType?.ObjectType != null && this.RoleType?.ObjectType != null)
                {
                    var roleCompositeType = this.RoleType.ObjectType as Composite;
                    return ((Composite)this.AssociationType.ObjectType).ExistExclusiveClass && roleCompositeType != null && roleCompositeType.ExistExclusiveClass;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is many to many.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is many to many; otherwise, <c>false</c>.
        /// </value>
        public bool IsManyToMany => this.AssociationType.IsMany && this.RoleType.IsMany;

        /// <summary>
        /// Gets a value indicating whether this instance is many to one.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is many to one; otherwise, <c>false</c>.
        /// </value>
        public bool IsManyToOne => this.AssociationType.IsMany && !this.RoleType.IsMany;

        /// <summary>
        /// Gets a value indicating whether this instance is one to many.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is one to many; otherwise, <c>false</c>.
        /// </value>
        public bool IsOneToMany => this.AssociationType.IsOne && this.RoleType.IsMany;

        /// <summary>
        /// Gets a value indicating whether this instance is one to one.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is one to one; otherwise, <c>false</c>.
        /// </value>
        public bool IsOneToOne => this.AssociationType.IsOne && !this.RoleType.IsMany;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name  .</value>
        public string Name => this.AssociationType.SingularName + this.RoleType.SingularName;

        /// <summary>
        /// Gets the name of the reverse.
        /// </summary>
        /// <value>The name of the reverse.</value>
        public string ReverseName => this.RoleType.SingularName + this.AssociationType.SingularName;
        
        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        protected internal override string ValidationName => "relation type" + this.Name;

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
            var that = obj as RelationType;
            return that != null ? this.Id.CompareTo(that.Id) : -1;
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
                return this.Name;
            }
            catch
            {
                return this.IdAsString;
            }
        }

        internal void DeriveMultiplicity()
        {
            if (this.RoleType?.ObjectType != null && this.RoleType.ObjectType.IsUnit)
            {
                this.multiplicity = Multiplicity.OneToOne;
            }
            else
            {
                this.multiplicity = this.AssignedMultiplicity;
            }
        }

        /// <summary>
        /// Validates this. instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (this.AssociationType != null && this.RoleType != null)
            {
                if (validationLog.ExistRelationName(this.Name))
                {
                    var message = "name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }
                else
                {
                    validationLog.AddRelationTypeName(this.Name);
                }

                if (validationLog.ExistRelationName(this.ReverseName))
                {
                    var message = "reversed name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }
                else
                {
                    validationLog.AddRelationTypeName(this.ReverseName);
                }

                if (validationLog.ExistObjectTypeName(this.Name))
                {
                    var message = "name of " + this.ValidationName + " is in conflict with object type " + this.Name;
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }

                if (validationLog.ExistObjectTypeName(this.ReverseName))
                {
                    var message = "reversed name of " + this.ValidationName + " is in conflict with object type " + this.Name;
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }
            }
            else
            {
                if (this.AssociationType == null)
                {
                    var message = this.ValidationName + " has no association type";
                    validationLog.AddError(message, this, ValidationKind.Required, "RelationType.AssociationType");
                }
                else
                {
                    var message = this.ValidationName + " has no role type";
                    validationLog.AddError(message, this, ValidationKind.Required, "RelationType.RoleType");
                }
            }

            this.AssociationType?.Validate(validationLog);

            this.RoleType?.Validate(validationLog);
        }
    }
}