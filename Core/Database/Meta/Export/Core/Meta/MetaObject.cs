// <copyright file="MetaObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using System;

    /// <summary>
    /// Base class for Meta objects.
    /// </summary>
    public abstract partial class MetaObjectBase : IMetaObject
    {
        private Guid id;

        protected MetaObjectBase(MetaPopulation metaPopulation) => this.MetaPopulation = metaPopulation;

        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

        public MetaPopulation MetaPopulation { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The meta object id.</value>
        public Guid Id
        {
            get => this.id;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.id = value;
                this.MetaPopulation.Stale();
            }
        }

        /// <summary>
        /// Gets the id as a number only string.
        /// </summary>
        /// <value>The id as a number only string.</value>
        public string IdAsNumberString => this.Id.ToString("N").ToLower();

        /// <summary>
        /// Gets the id as a string.
        /// </summary>
        /// <value>The id as a string.</value>
        public string IdAsString => this.Id.ToString("D").ToLower();

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        protected internal abstract string ValidationName { get; }

        /// <summary>
        /// Validate this object.
        /// </summary>
        /// <param name="validationLog">
        /// The validation log.
        /// </param>
        protected internal virtual void Validate(ValidationLog validationLog)
        {
            if (this.Id == Guid.Empty)
            {
                var message = "id on " + this.ValidationName + " is required";
                validationLog.AddError(message, this, ValidationKind.Unique, "IMetaObject.Id");
            }
            else
            {
                if (validationLog.ExistId(this.Id))
                {
                    var message = "id " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, "IMetaObject.Id");
                }
                else
                {
                    validationLog.AddId(this.Id);
                }
            }
        }
    }
}
