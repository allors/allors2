// <copyright file="PrefetchPolicy.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectIdInteger type.</summary>

namespace Allors.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Meta;

    public sealed class PrefetchPolicy : IEnumerable<PrefetchRule>
    {
        internal PrefetchPolicy(PrefetchRule[] rules)
        {
            if (rules == null)
            {
                throw new ArgumentNullException("rules");
            }

            this.PrefetchRules = new List<PrefetchRule>(rules).ToArray();
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public PrefetchRule[] PrefetchRules { get; }

        public bool AllowCompilation { get; set; }

        public static implicit operator PrefetchPolicy(PrefetchRule prefetchRule)
        {
            var rules = new[] { prefetchRule };
            return new PrefetchPolicy(rules)
            {
                AllowCompilation = false,
            };
        }

        public static implicit operator PrefetchPolicy(IPropertyType[] propertyTypes)
        {
            var rules = propertyTypes.Select(x => new PrefetchRule(x, null)).ToArray();
            return new PrefetchPolicy(rules)
            {
                AllowCompilation = false,
            };
        }

        public IEnumerator<PrefetchRule> GetEnumerator() => ((IEnumerable<PrefetchRule>)this.PrefetchRules).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.PrefetchRules.GetEnumerator();

        public string DebugView
        {
            get
            {
                var toString = new StringBuilder();
                this.DebugRuleView(toString, this.PrefetchRules, 1);
                return toString.ToString();
            }
        }

        private void DebugRuleView(StringBuilder toString, PrefetchRule[] rules, int level)
        {
            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    var indent = new string(' ', level * 2);
                    toString.Append(indent + "- " + rule.PropertyType + "\n");

                    if (rule.PrefetchPolicy != null)
                    {
                        this.DebugRuleView(toString, rule.PrefetchPolicy.PrefetchRules, level + 1);
                    }
                }
            }
        }
    }
}
