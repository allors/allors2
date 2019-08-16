// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniquelyIdentifiableSticky.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    using Allors;
    using Allors.Meta;

    public class UniquelyIdentifiableSticky<TObject> : Sticky<Guid, TObject>
        where TObject : class, IObject
    {
        public UniquelyIdentifiableSticky(ISession session)
            : base(session, M.UniquelyIdentifiable.UniqueId)
        {
        }
    }
}
