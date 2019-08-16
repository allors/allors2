//------------------------------------------------------------------------------------------------- 
// <copyright file="PullExtensions.cs" company="Allors bvba">
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
    using System.Linq;

    using Allors.Data;
    using Allors.Meta;

    public static class PullExtensions
    {
        public static Allors.Data.Pull Load(this Pull @this, ISession session)
        {
            var pull = new Allors.Data.Pull
            {
                ExtentRef = @this.ExtentRef,
                Extent = @this.Extent?.Load(session),
                ObjectType = @this.ObjectType.HasValue ? (IObjectType)session.Database.MetaPopulation.Find(@this.ObjectType.Value) : null,
                Object = @this.Object != null ? session.Instantiate(@this.Object) : null,
                Results = @this.Results?.Select(v => v.Load(session)).ToArray(),
                Arguments = @this.Arguments != null ? new Arguments(@this.Arguments) : null
            };

            return pull;
        }
    }
}
