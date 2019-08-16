//------------------------------------------------------------------------------------------------- 
// <copyright file="Filter.cs" company="Allors bvba">
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

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Data;

    using Meta;
    using Protocol;

    public class Filter : IExtent, IPredicateContainer
    {
        public Filter(IComposite objectType)
        {
            this.ObjectType = objectType;
        }

        public IComposite ObjectType { get; set; }

        public IPredicate Predicate { get; set; }

        public Sort[] Sorting { get; set; }

        bool IExtent.HasMissingArguments(IReadOnlyDictionary<string, object> arguments)
        {
            return this.Predicate != null && this.Predicate.HasMissingArguments(arguments);
        }

        void IPredicateContainer.AddPredicate(IPredicate predicate)
        {
            this.Predicate = predicate;
        }

        public Extent ToJson()
        {
            return new Extent
            {
                Kind = ExtentKind.Filter,
                ObjectType = this.ObjectType?.Id,
                Predicate = this.Predicate?.ToJson(),
                Sorting = this.Sorting?.Select(v => new Protocol.Data.Sort { Descending = v.Descending, RoleType = v.RoleType?.Id }).ToArray()
            };

        }
    }
}
