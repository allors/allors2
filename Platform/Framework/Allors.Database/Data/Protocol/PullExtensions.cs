//-------------------------------------------------------------------------------------------------
// <copyright file="PullExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
