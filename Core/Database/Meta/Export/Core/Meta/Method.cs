//-------------------------------------------------------------------------------------------------
// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System.Linq;

    public abstract partial class Method
    {
        private string xmlDoc;

        protected Method(IObject @object)
        {
            this.Executed = false;
            this.Object = @object;
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

        public abstract MethodInvocation MethodInvocation { get; }

        public IObject Object { get; private set; }

        public bool Executed { get; set; }

        public virtual void Execute() => this.MethodInvocation.Execute(this);
    }
}
