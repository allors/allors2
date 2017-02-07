//------------------------------------------------------------------------------------------------- 
// <copyright file="MethodType.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
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

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.name = value;
                this.MetaPopulation.Stale();
            }
        }

        public string FullName
        {
            get
            {
                return this.ObjectType != null ? this.ObjectType.Name + this.name : this.Name;
            }
        }

        public Composite ObjectType
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

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return this.name;
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