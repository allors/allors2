// <copyright file="MethodType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Linq;

    public sealed partial class MethodType : OperandType
    {
        private string name;
        private Composite objectType;

        private string xmlDoc;

        private bool workspace;

        public MethodType(MetaPopulation metaPopulation, Guid id)
            : base(metaPopulation)
        {
            this.Id = id;

            metaPopulation.OnMethodTypeCreated(this);
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

        public string FullName => this.ObjectType != null ? this.ObjectType.Name + this.name : this.Name;

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
        /// Gets the display name.
        /// </summary>
        public override string DisplayName => this.name;

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
