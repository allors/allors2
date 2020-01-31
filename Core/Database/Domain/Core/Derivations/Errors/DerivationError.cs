// <copyright file="DerivationError.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Errors
{
    using System.Collections.Generic;

    using Allors.Meta;

    public abstract class DerivationError : IDerivationError
    {
        private readonly string message;

        protected DerivationError(IValidation validation, DerivationRelation[] relations, string errorMessage)
            : this(validation, relations, errorMessage, new object[] { DerivationRelation.ToString(relations) })
        {
        }

        protected DerivationError(IValidation validation, DerivationRelation[] relations, string errorMessage, object[] errorMessageParameters)
        {
            this.Validation = validation;
            this.Relations = relations;

            try
            {
                if (errorMessageParameters != null && errorMessageParameters.Length > 0)
                {
                    this.message = string.Format(errorMessage, errorMessageParameters);
                }
                else
                {
                    this.message = string.Format(errorMessage, new object[] { DerivationRelation.ToString(relations) });
                }
            }
            catch
            {
                this.message = this.GetType() + ": " + DerivationRelation.ToString(this.Relations);
            }
        }

        public IValidation Validation { get; }

        public DerivationRelation[] Relations { get; }

        public RoleType[] RoleTypes
        {
            get
            {
                var roleTypes = new List<RoleType>();
                foreach (var derivationRole in this.Relations)
                {
                    var roleType = derivationRole.RoleType;
                    if (!roleTypes.Contains(roleType))
                    {
                        roleTypes.Add(roleType);
                    }
                }

                return roleTypes.ToArray();
            }
        }

        public virtual string Message => this.message;

        public override string ToString() => this.message;
    }
}
