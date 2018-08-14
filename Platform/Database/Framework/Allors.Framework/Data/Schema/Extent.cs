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

namespace Allors.Data.Schema
{
    using System;
    using System.Linq;

    using Allors.Meta;

    public class Extent 
    {
        public string Kind { get; set; }

        public Extent[] Operands { get; set; }

        public Guid? ObjectType { get; set; }

        public Predicate Predicate { get; set; }

        /// <summary>
        /// Loads an <see cref="Allors.Extent"/> based on this <see cref="Extent"/>.
        /// </summary>
        /// <param name="session">
        /// The database to resolve information from.
        /// </param>
        /// <returns>
        /// The loaded <see cref="Extent"/>.
        /// </returns>
        public IExtent Load(ISession session)
        {
            Func<IExtent[]> operands = () => this.Operands.Select(v => v.Load(session)).ToArray();

            switch (this.Kind)
            {
                case ExtentKind.Predicate:
                    if (!this.ObjectType.HasValue)
                    {
                        return null;
                    }

                    var objectType = (IComposite)session.Database.ObjectFactory.MetaPopulation.Find(this.ObjectType.Value);
                    var extent = new Data.Extent(objectType)
                    {
                        Predicate = this.Predicate?.Load(session)
                    };

                    return extent;

                case ExtentKind.Union:
                    return new Union(operands());

                case ExtentKind.Except:
                    return new Except(operands());

                case ExtentKind.Intersect:
                    return new Intersect(operands());

                default:
                    throw new Exception("Unknown extent kind " + this.Kind);
            }
        }
    }
}