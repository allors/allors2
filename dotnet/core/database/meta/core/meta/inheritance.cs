// <copyright file="Inheritance.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Inheritance type.</summary>

namespace Allors.Meta
{
    using System;
    using System.Linq;

    public sealed partial class Inheritance : DomainObject
    {
        private Composite subtype;

        private Interface supertype;

        internal Inheritance(MetaPopulation metaPopulation)
            : base(metaPopulation)
        {
            this.Id = Guid.NewGuid();

            metaPopulation.OnInheritanceCreated(this);
        }

        public Composite Subtype
        {
            get => this.subtype;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.subtype = value;
                this.MetaPopulation.Stale();
            }
        }

        public Interface Supertype
        {
            get => this.supertype;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.supertype = value;
                this.MetaPopulation.Stale();
            }
        }

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        protected internal override string ValidationName
        {
            get
            {
                if (this.Supertype != null && this.Subtype != null)
                {
                    return "inheritance " + this.Subtype + "::" + this.Supertype;
                }

                return "unknown inheritance";
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() => (this.Subtype != null ? this.Subtype.Name : string.Empty) + "::" + (this.Supertype != null ? this.Supertype.Name : string.Empty);

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (this.Subtype != null && this.Supertype != null)
            {
                if (this.MetaPopulation.Inheritances.Count(inheritance => this.Subtype.Equals(inheritance.Subtype) && this.Supertype.Equals(inheritance.Supertype)) != 1)
                {
                    var message = "name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, "Inheritance.Supertype");
                }

                IObjectType tempQualifier = this.Supertype;
                if (tempQualifier is IClass)
                {
                    var message = this.ValidationName + " can not have a concrete superclass";
                    validationLog.AddError(message, this, ValidationKind.Hierarchy, "Inheritance.Supertype");
                }
            }
            else
            {
                if (this.Supertype == null)
                {
                    var message = this.ValidationName + " has a missing Supertype";
                    validationLog.AddError(message, this, ValidationKind.Unique, "Inheritance.Supertype");
                }
                else
                {
                    var message = this.ValidationName + " has a missing Subtype";
                    validationLog.AddError(message, this, ValidationKind.Unique, "Inheritance.Supertype");
                }
            }
        }
    }
}
