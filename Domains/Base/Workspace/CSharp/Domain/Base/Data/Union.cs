//------------------------------------------------------------------------------------------------- 
// <copyright file="Union.cs" company="Allors bvba">
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

    using Allors.Meta;
    using Allors.Workspace.Data.Protocol;

    public class Union : IExtentOperator
    {
        public Union(params IExtent[] operands)
        {
            this.Operands = operands;
        }

        public IComposite ObjectType => this.Operands?[0].ObjectType;

        public IExtent[] Operands { get; set; }

        public Sort[] Sorting { get; set; }

        bool IExtent.HasMissingArguments(IReadOnlyDictionary<string, object> arguments)
        {
            return this.Operands.Any(v => v.HasMissingArguments(arguments));
        }

        public Protocol.Extent Save()
        {
            return new Protocol.Extent
                       {
                           Kind = ExtentKind.Union,
                           Operands = this.Operands.Select(v => v.Save()).ToArray(),
                           Sorting = this.Sorting.Select(v => new Protocol.Sort { Descending = v.Descending, RoleType = v.RoleType?.Id }).ToArray()
            };
        }
    }
}