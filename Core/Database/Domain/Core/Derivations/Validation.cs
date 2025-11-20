
// <copyright file="ValidationBase.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations
{
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;
    using Derivations.Errors;

    public partial class Validation : IValidation
    {
        private readonly List<IDerivationError> errors;

        internal Validation(IDerivation derivation)
        {
            this.Derivation = derivation;
            this.errors = new List<IDerivationError>();
        }

        public IDerivation Derivation { get; private set; }

        public bool HasErrors => this.errors.Count > 0;

        public IDerivationError[] Errors => this.errors.ToArray();

        public void AddError(IDerivationError derivationError) => this.errors.Add(derivationError);

        public void AddError(IObject association, RoleType roleType, string errorMessage, params object[] messageParam)
        {
            var error = new DerivationErrorGeneric(this, new DerivationRelation(association, roleType), errorMessage, messageParam);
            this.AddError(error);
        }

        public void AddError(IObject role, AssociationType associationType, string errorMessage, params object[] messageParam)
        {
            var error = new DerivationErrorGeneric(this, new DerivationRelation(role, associationType), errorMessage, messageParam);
            this.AddError(error);
        }

        public void AssertExists(IObject association, RoleType roleType)
        {
            if (!association.Strategy.ExistRole(roleType.RelationType))
            {
                this.AddError(new DerivationErrorRequired(this, association, roleType));
            }
        }

        public void AssertNotExists(IObject association, RoleType roleType)
        {
            if (association.Strategy.ExistRole(roleType.RelationType))
            {
                this.AddError(new DerivationErrorNotAllowed(this, association, roleType));
            }
        }

        public void AssertNonEmptyString(IObject association, RoleType roleType)
        {
            if (association.Strategy.ExistRole(roleType.RelationType))
            {
                if (association.Strategy.GetUnitRole(roleType.RelationType).Equals(string.Empty))
                {
                    this.AddError(new DerivationErrorRequired(this, association, roleType));
                }
            }
        }

        public void AssertExistsNonEmptyString(IObject association, RoleType roleType)
        {
            this.AssertExists(association, roleType);
            this.AssertNonEmptyString(association, roleType);
        }

        public void AssertIsUnique(IObject association, RoleType roleType)
        {
            if (this.Derivation.ChangeSet.RoleTypesByAssociation.TryGetValue(association.Id, out var roleTypes))
            {
                if (roleTypes.Contains(roleType))
                {
                    var objectType = roleType.AssociationType.ObjectType;
                    var role = association.Strategy.GetRole(roleType.RelationType);

                    if (role != null)
                    {
                        var session = association.Strategy.Session;
                        var extent = session.Extent(objectType);
                        extent.Filter.AddEquals(roleType, role);
                        if (extent.Count != 1)
                        {
                            this.AddError(new DerivationErrorUnique(this, association, roleType));
                        }
                    }
                }
            }
        }

        public void AssertAtLeastOne(IObject association, params RoleType[] roleTypes)
        {
            foreach (var roleType in roleTypes)
            {
                if (association.Strategy.ExistRole(roleType.RelationType))
                {
                    return;
                }
            }

            this.AddError(new DerivationErrorAtLeastOne(this, DerivationRelation.Create(association, roleTypes)));
        }

        public void AssertExistsAtMostOne(IObject association, params RoleType[] roleTypes)
        {
            var count = 0;
            foreach (var roleType in roleTypes)
            {
                if (association.Strategy.ExistRole(roleType.RelationType))
                {
                    ++count;
                }
            }

            if (count > 1)
            {
                this.AddError(new DerivationErrorAtMostOne(this, DerivationRelation.Create(association, roleTypes)));
            }
        }

        public void AssertAreEqual(IObject association, RoleType roleType, RoleType otherRoleType)
        {
            var value = association.Strategy.GetRole(roleType.RelationType);
            var otherValue = association.Strategy.GetRole(otherRoleType.RelationType);

            bool equal;
            if (value == null)
            {
                equal = otherValue == null;
            }
            else
            {
                equal = value.Equals(otherValue);
            }

            if (!equal)
            {
                this.AddError(new DerivationErrorEquals(this, DerivationRelation.Create(association, roleType, otherRoleType)));
            }
        }

        public void AssertExists(IObject role, AssociationType associationType)
        {
            if (!role.Strategy.ExistAssociation(associationType.RelationType))
            {
                this.AddError(new DerivationErrorRequired(this, role, associationType));
            }
        }
    }
}
