// <copyright file="MethodType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using System;
    using System.Linq;

    public sealed partial class MethodType : OperandType, IMethodType
    {
        private string name;
        private Composite objectType;

        private bool workspace;
        private string xmlDoc;

        public MethodType(MetaPopulation metaPopulation, Guid id)
            : base(metaPopulation)
        {
            this.Id = id;

            metaPopulation.OnMethodTypeCreated(this);
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName => this.name;

        public string FullName => this.ObjectType != null ? this.ObjectType.Name + this.name : this.Name;

        public string Name
        {
            get => this.name;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.name = value;
                this.MetaPopulation.Stale();
            }
        }

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

        public bool Workspace
        {
            get => this.workspace;
            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.workspace = value;
                this.MetaPopulation.Stale();
            }
        }

        public string XmlDoc
        {
            get => this.xmlDoc;

            set => this.xmlDoc = !string.IsNullOrWhiteSpace(value) ? value : null;
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

        public string[] Tags { get; set; } = [];

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        protected internal override string ValidationName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.name))
                {
                    return "method type " + this.name;
                }

                return "unknown method type";
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() => this.name;

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (string.IsNullOrEmpty(this.name))
            {
                var message = this.ValidationName + " has no name";
                validationLog.AddError(message, this, ValidationKind.Required, "MethodType.Name");
            }
        }
    }
}
