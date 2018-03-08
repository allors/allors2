//------------------------------------------------------------------------------------------------- 
// <copyright file="Repository.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Linq;

    public class Repository
    {
        protected Repository()
        {
            this.AssemblyByName = new Dictionary<string, Assembly>();
            this.UnitBySingularName = new Dictionary<string, Unit>();
            this.InterfaceBySingularName = new Dictionary<string, Interface>();
            this.ClassBySingularName = new Dictionary<string, Class>();
            this.CompositeByName = new Dictionary<string, Composite>();
            this.TypeBySingularName = new Dictionary<string, Type>();
        }

        public IEnumerable<Assembly> Assemblies => this.AssemblyByName.Values;

        public IEnumerable<Unit> Units => this.UnitBySingularName.Values;

        public IEnumerable<Interface> Interfaces => this.InterfaceBySingularName.Values;

        public IEnumerable<Class> Classes => this.ClassBySingularName.Values;

        public IEnumerable<Type> Types => this.Composites.Cast<Type>().Union(this.Units);

        public IEnumerable<Composite> Composites => this.ClassBySingularName.Values.Cast<Composite>().Union(this.InterfaceBySingularName.Values);

        public Dictionary<string, Assembly> AssemblyByName { get; }

        public Dictionary<string, Unit> UnitBySingularName { get; }

        public Dictionary<string, Interface> InterfaceBySingularName { get; }

        public Dictionary<string, Class> ClassBySingularName { get; }

        public Dictionary<string, Type> TypeBySingularName { get; }

        public Dictionary<string, Composite> CompositeByName { get; }

        public Assembly[] SortedAssemblies
        {
            get
            {
                var assemblies = this.Assemblies.ToList();
                assemblies.Sort((x, y) => x.Bases.Contains(y) ? 1 : -1);
                return assemblies.ToArray();
            }
        }
        
        public bool HasErrors { get; set; }
    }
}
