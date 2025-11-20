// <copyright file="ResultExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    public static class ResultExtensions
    {
        public static Allors.Data.Result Load(this Result @this, ISession session)
        {
            var result = new Allors.Data.Result
            {
                FetchRef = @this.fetchRef,
                Fetch = @this.fetch?.Load(session),
                Name = @this.name,
                Skip = @this.skip,
                Take = @this.take,
            };

            return result;
        }
    }
}
