//------------------------------------------------------------------------------------------------- 
// <copyright file="PrefetchRule.cs" company="Allors bvba">
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
// <summary>Defines the ObjectIdInteger type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Allors.Meta;

    public sealed class PrefetchPolicy : IEnumerable<PrefetchRule>
    {
        private readonly Guid id;

        internal PrefetchPolicy(PrefetchRule[] rules)
        {
            if (rules == null)
            {
                throw new ArgumentNullException("rules");
            }

            this.PrefetchRules = new List<PrefetchRule>(rules).ToArray();
            this.id = Guid.NewGuid();
        }

        public Guid Id => this.id;

        public PrefetchRule[] PrefetchRules { get; }

        public bool AllowCompilation { get; set; }

        public static implicit operator PrefetchPolicy(PrefetchRule prefetchRule)
        {
            var rules = new[] { prefetchRule };
            return new PrefetchPolicy(rules)
            {
                AllowCompilation = false
            };
        }

        public static implicit operator PrefetchPolicy(IPropertyType[] propertyTypes)
        {
            var rules = propertyTypes.Select(x => new PrefetchRule(x, null)).ToArray();
            return new PrefetchPolicy(rules)
            {
                AllowCompilation = false
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
