// <copyright file="ResultExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
                FetchRef = @this.FetchRef,
                Fetch = @this.Fetch?.Load(session),
                Name = @this.Name,
                Skip = @this.Skip,
                Take = @this.Take,
            };

            return result;
        }
    }
}
