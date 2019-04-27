// <copyright file="Expansion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public partial class Expansion : INode
    {
        public Expansion(JToken json, INode parent)
        {
            this.Json = json;
            this.Parent = parent;
        }

        public ExpansionCase[] ExpansionCases { get; set; }

        public JToken Json { get; }

        public INode Parent { get; set; }

        public string SwitchValue { get; set; }

        public void BaseLoad()
        {
            this.SwitchValue = this.Json["switchValue"]?.Value<string>();

            var jsonExpansionCases = this.Json["expansionCases"];
            this.ExpansionCases = jsonExpansionCases != null ? jsonExpansionCases.Select(v =>
                {
                    var expansionCase = new ExpansionCase(v, this);
                    expansionCase.BaseLoad();
                    return expansionCase;
                }).ToArray() : new ExpansionCase[0];
        }
    }
}