//-------------------------------------------------------------------------------------------------
// <copyright file="Predicate.cs" company="Allors bvba">
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

namespace Allors.Protocol.Data
{
    using System;

    public class Predicate
    {
        public PredicateKind Kind { get; set; }

        public Guid? PropertyType { get; set; }

        public Guid? RoleType { get; set; }

        public Guid? ObjectType { get; set; }

        public string Parameter { get; set; }

        public Predicate Operand { get; set; }

        public Predicate[] Operands { get; set; }

        public string Object { get; set; }

        public string[] Objects { get; set; }

        public string Value { get; set; }

        public string[] Values { get; set; }

        public Extent Extent { get; set; }
    }
}
