// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Path.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Data.Protocol
{
    using System;
  
    public class Path
    {
        public Guid PropertyType { get; set; }

        public Path Next { get; set; }

        public Tree Tree { get; set; }

        public Data.Path Load(ISession session)
        {
            return new Data.Path(session.Database.MetaPopulation, this.PropertyType)
                       {
                           Step = this.Next?.Load(session),
                           Tree = this.Tree?.Load(session)
                       };
        }
    }
}