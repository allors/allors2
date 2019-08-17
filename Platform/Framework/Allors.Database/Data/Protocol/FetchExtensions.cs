//-------------------------------------------------------------------------------------------------
// <copyright file="FetchExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Protocol.Data
{
    public static class FetchExtensions
    {
        public static Allors.Data.Fetch Load(this Fetch @this, ISession session) =>
            new Allors.Data.Fetch(session.Database.MetaPopulation)
            {
                Step = @this.Step?.Load(session),
                Include = @this.Include?.Load(session),
            };
    }
}
