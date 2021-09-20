// <copyright file="Expansion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Html
{
    using System.Linq;
    using Autotest.Angular;
    using Newtonsoft.Json.Linq;

    public partial class Expansion : INode
    {
        public Expansion(JToken json, Template template, INode parent)
        {
            this.Json = json;
            this.Template = template;
            this.Parent = parent;
        }

        public ExpansionCase[] ExpansionCases { get; set; }

        public JToken Json { get; }

        public Template Template { get; }

        public INode Parent { get; set; }

        public Scope InScope { get; set; }

        public string SwitchValue { get; set; }

        public void BaseLoad()
        {
            this.SwitchValue = this.Json["switchValue"]?.Value<string>();

            var jsonExpansionCases = this.Json["expansionCases"];
            this.ExpansionCases = jsonExpansionCases != null ? jsonExpansionCases.Select(v =>
                {
                    var expansionCase = new ExpansionCase(v, this.Template, this);
                    expansionCase.BaseLoad();
                    return expansionCase;
                }).ToArray() : new ExpansionCase[0];
        }

        public void SetInScope(Scope scope)
        {
            this.InScope = scope;
            scope.Nodes.Add(this);

            foreach (var expansionCase in this.ExpansionCases)
            {
                expansionCase.SetInScope(scope);
            }
        }
    }
}
