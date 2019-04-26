// <copyright file="Expansion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Expansion : INode
    {
        public Expansion(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public string SwitchValue { get; set; }

        public ExpansionCase[] ExpansionCases { get; set; }

        public void BaseLoad()
        {
            this.SwitchValue = this.Json["switchValue"]?.Value<string>();

            var jsonExpansionCases = this.Json["expansionCases"];
            this.ExpansionCases = jsonExpansionCases != null ? jsonExpansionCases.Select(v =>
                {
                    var expansionCase = new ExpansionCase(v);
                    expansionCase.BaseLoad();
                    return expansionCase;
                }).ToArray() : new ExpansionCase[0];
        }
    }
}