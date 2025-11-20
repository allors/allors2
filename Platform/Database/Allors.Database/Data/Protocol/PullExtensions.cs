// <copyright file="PullExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
                ExtentRef = @this.extentRef,
                Extent = @this.extent?.Load(session),
                ObjectType = @this.objectType.HasValue ? (IObjectType)session.Database.MetaPopulation.Find(@this.objectType.Value) : null,
                Object = @this.@object != null ? session.Instantiate(@this.@object) : null,
                Results = @this.results?.Select(v => v.Load(session)).ToArray(),
                Parameters = @this.parameters,
            };

            return pull;
        }
    }
}
