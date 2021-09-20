// <copyright file="RelationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Linq;

    /// <summary>
    /// A <see cref="RelationType"/> defines the state and behavior for
    /// a set of <see cref="AssociationType"/>s and <see cref="RoleType"/>s.
    /// </summary>
    public sealed partial class RelationType : IRelationTypeBase
    {
        private readonly IMetaPopulationBase metaPopulation;

        private Multiplicity assignedMultiplicity;
        private Multiplicity multiplicity;

        private bool isDerived;
        private bool isIndexed;

        private string[] assignedWorkspaceNames;
        private string[] derivedWorkspaceNames;

        private RelationTypeProps props;

        public RelationType(ICompositeBase associationTypeComposite, Guid id, Func<IRelationTypeBase, IAssociationTypeBase> associationTypeFactory, Func<IRelationTypeBase, IRoleTypeBase> roleTypeFactory, string tag = null)
        {
            this.metaPopulation = associationTypeComposite.MetaPopulation;
            this.Id = id;
            this.Tag = tag ?? id.Tag();
            this.AssignedOrigin = Origin.Database;

            this.AssociationType = associationTypeFactory(this);
            this.AssociationType.ObjectType = associationTypeComposite;

            this.RoleType = roleTypeFactory(this);

            this.metaPopulation.OnRelationTypeCreated(this);
        }

        public RelationTypeProps _ => this.props ??= new RelationTypeProps(this);

        public Guid Id { get; }

        public string Tag { get; }

        public string[] AssignedWorkspaceNames
        {
            get => this.assignedWorkspaceNames;

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.assignedWorkspaceNames = value;
                this.metaPopulation.Stale();
            }
        }

        public string[] WorkspaceNames
        {
            get
            {
                this.metaPopulation.Derive();
                return this.derivedWorkspaceNames;
            }
        }

        IMetaPopulationBase IMetaObjectBase.MetaPopulation => this.metaPopulation;
        IMetaPopulation IMetaObject.MetaPopulation => this.metaPopulation;
        Origin IMetaObject.Origin => this.AssignedOrigin;

        public Origin AssignedOrigin { get; set; }

        public bool IsDerived
        {
            get => this.isDerived;

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.isDerived = value;
                this.metaPopulation.Stale();
            }
        }

        public Multiplicity AssignedMultiplicity
        {
            get => this.assignedMultiplicity;

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.assignedMultiplicity = value;
                this.metaPopulation.Stale();
            }
        }

        public Multiplicity Multiplicity
        {
            get
            {
                this.metaPopulation.Derive();
                return this.multiplicity;
            }
        }

        public bool ExistExclusiveDatabaseClasses
        {
            get
            {
                if (this.AssociationType?.ObjectType != null && this.RoleType?.ObjectType != null)
                {
                    return this.AssociationType.ObjectType.ExistExclusiveDatabaseClass && this.RoleType.ObjectType is Composite roleCompositeType && roleCompositeType.ExistExclusiveDatabaseClass;
                }

                return false;
            }

        }

        public bool IsIndexed
        {
            get => this.isIndexed;

            set
            {
                this.metaPopulation.AssertUnlocked();
                this.isIndexed = value;
                this.metaPopulation.Stale();
            }
        }

        IAssociationType IRelationType.AssociationType => this.AssociationType;
        public IAssociationTypeBase AssociationType { get; }

        IRoleType IRelationType.RoleType => this.RoleType;
        public IRoleTypeBase RoleType { get; }

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
                    return this.AssociationType.ObjectType.ExistExclusiveClass && this.RoleType.ObjectType is Composite roleCompositeType && roleCompositeType.ExistExclusiveClass;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this state is many to many.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this state is many to many; otherwise, <c>false</c>.
        /// </value>
        public bool IsManyToMany => this.AssociationType.IsMany && this.RoleType.IsMany;

        /// <summary>
        /// Gets a value indicating whether this state is many to one.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this state is many to one; otherwise, <c>false</c>.
        /// </value>
        public bool IsManyToOne => this.AssociationType.IsMany && !this.RoleType.IsMany;

        /// <summary>
        /// Gets a value indicating whether this state is one to many.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this state is one to many; otherwise, <c>false</c>.
        /// </value>
        public bool IsOneToMany => this.AssociationType.IsOne && this.RoleType.IsMany;

        /// <summary>
        /// Gets a value indicating whether this state is one to one.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this state is one to one; otherwise, <c>false</c>.
        /// </value>
        public bool IsOneToOne => this.AssociationType.IsOne && !this.RoleType.IsMany;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name  .</value>
        public string Name => this.AssociationType.ObjectType + this.RoleType.SingularName;

        /// <summary>
        /// Gets the name of the reverse.
        /// </summary>
        /// <value>The name of the reverse.</value>
        public string ReverseName => this.RoleType.SingularName + this.AssociationType.ObjectType;

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        public string ValidationName => "relation type" + this.Name;

        public override bool Equals(object other) => this.Id.Equals((other as RelationType)?.Id);

        public override int GetHashCode() => this.Id.GetHashCode();

        /// <summary>
        /// Compares the current state with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this state.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This state is less than <paramref name="obj"/>. Zero This state is equal to <paramref name="obj"/>. Greater than zero This state is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="other"/> is not the same type as this state. </exception>
        public int CompareTo(object other) => this.Id.CompareTo((other as RelationType)?.Id);

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
                return this.Tag.ToString();
            }
        }

        public void DeriveMultiplicity()
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

        public void DeriveWorkspaceNames() =>
            this.derivedWorkspaceNames = this.assignedWorkspaceNames != null ?
                this.assignedWorkspaceNames.Intersect(this.AssociationType.ObjectType switch
                {
                    Interface @interface => @interface.Classes.SelectMany(v => v.WorkspaceNames),
                    Class @class => @class.WorkspaceNames,
                    _ => Array.Empty<string>()
                }).Intersect(this.RoleType.ObjectType switch
                {
                    Unit unit => unit.WorkspaceNames,
                    Interface @interface => @interface.Classes.SelectMany(v => v.WorkspaceNames),
                    Class @class => @class.WorkspaceNames,
                    _ => Array.Empty<string>()
                }).ToArray() :
                Array.Empty<string>();

        /// <summary>
        /// Validates this. state.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        public void Validate(ValidationLog validationLog)
        {
            this.ValidateIdentity(validationLog);

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
            else if (this.AssociationType == null)
            {
                var message = this.ValidationName + " has no association type";
                validationLog.AddError(message, this, ValidationKind.Required, "RelationType.AssociationType");
            }
            else
            {
                var message = this.ValidationName + " has no role type";
                validationLog.AddError(message, this, ValidationKind.Required, "RelationType.RoleType");
            }

            this.AssociationType?.Validate(validationLog);

            this.RoleType?.Validate(validationLog);
        }
    }
}
