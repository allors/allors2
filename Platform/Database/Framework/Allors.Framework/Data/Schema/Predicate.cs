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

using System;

namespace Allors.Data.Schema
{
    public class Predicate 
    {
        public string Kind { get; set; }

        public Predicate[] Operands { get; set; }

        public Guid PropertyType { get; set; }

        public Guid ObjectType { get; set; }

        public string[] Values { get; set; }

        public string[] Objects { get; set; }

        public string Parameter { get; set; }
    }
}