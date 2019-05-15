//------------------------------------------------------------------------------------------------- 
// <copyright file="FetchExtensions.cs" company="Allors bvba">
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
    public static class FetchExtensions 
    {
        public static Allors.Data.Fetch Load(this Fetch @this, ISession session)
        {
            return new Allors.Data.Fetch(session.Database.MetaPopulation)
                       {
                           Step = @this.Step?.Load(session),
                           Include = @this.Include?.Load(session)
                       };
        }
    }
}