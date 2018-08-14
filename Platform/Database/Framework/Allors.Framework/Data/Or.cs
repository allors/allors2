//------------------------------------------------------------------------------------------------- 
// <copyright file="Or.cs" company="Allors bvba">
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

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Data.Schema;

    public class Or : ICompositePredicate
    {
        public Or(params IPredicate[] operands)
        {
            this.Operands = operands;
        }

        public IPredicate[] Operands { get; set; }

        void IPredicateContainer.AddPredicate(IPredicate predicate)
        {
            this.Operands = this.Operands.Append(predicate).ToArray();
        }

        public Predicate Save()
        {
            return new Predicate()
                       {
                           Kind = PredicateKind.Or,
                           Operands = this.Operands.Select(v => v.Save()).ToArray()
                       };
        }


        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var or = compositePredicate.AddOr();
            foreach (var predicate in this.Operands)
            {
                predicate.Build(session, arguments, or);
            }
        }
    }
}