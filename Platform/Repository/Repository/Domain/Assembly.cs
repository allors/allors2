//------------------------------------------------------------------------------------------------- 
// <copyright file="Assembly.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the IObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;

    public class Assembly
    {
        public Guid Id { get; set; }

        public string Name { get; }

        public Dictionary<string, PartialInterface> PartialInterfaceByName { get; }

        public Dictionary<string, PartialClass> PartialClassBySingularName { get; }

        public Dictionary<string, PartialType> PartialTypeBySingularName { get; }

        public HashSet<Assembly> Bases { get; set; }

        public Assembly(string name)
        {
            this.Name = name;
            this.Bases = new HashSet<Assembly>();
            this.PartialInterfaceByName = new Dictionary<string, PartialInterface>();
            this.PartialClassBySingularName = new Dictionary<string, PartialClass>();
            this.PartialTypeBySingularName = new Dictionary<string, PartialType>();
        }

        public void Extend(Assembly baseAssembly)
        {
            this.Bases.Add(baseAssembly);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}