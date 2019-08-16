//------------------------------------------------------------------------------------------------- 
// <copyright file="Method.cs" company="Allors bvba">
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System.Linq;

    public abstract partial class Method
    {
        private readonly IObject @object;

        private string xmlDoc;

        protected Method(IObject @object)
        {
            this.Executed = false;
            this.@object = @object;
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

        public abstract MethodInvocation MethodInvocation { get; }

        public IObject Object => this.@object;

        public bool Executed { get; set; }

        public virtual void Execute()
        {
            this.MethodInvocation.Execute(this);
        }
    }
}
