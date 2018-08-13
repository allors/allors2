//------------------------------------------------------------------------------------------------- 
// <copyright file="Extent.cs" company="Allors bvba">
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

    public class Union : IExtentOperator
    {
        public Union(params IExtent[] operands)
        {
            this.Operands = operands;
        }

        public IExtent[] Operands { get; set; }

        Allors.Extent IExtent.Build(ISession session, IReadOnlyDictionary<string, object> arguments)
        {
            return session.Union(Operands[0].Build(session, arguments), Operands[1].Build(session, arguments));
        }

        public Schema.Extent Save()
        {
            throw new System.NotImplementedException();
        }
    }
}