// <copyright file="Sticky.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public class Merger<T>
        where T : class, UniquelyIdentifiable
    {
        private readonly UniquelyIdentifiableSticky<T> sticky;
        private readonly ISession session;
        private readonly IClass @class;

        public Merger(UniquelyIdentifiableSticky<T> sticky)
        {
            this.sticky = sticky;
            this.session = sticky.Session;
            this.@class = (IClass)this.session.Database.ObjectFactory.GetObjectTypeForType(typeof(T));
        }

        public Action<Guid, Action<T>> Action() =>
            (uniqueId, action) =>
            {
                if (this.sticky[uniqueId] == null)
                {

                    var @object = (T)Allors.ObjectBuilder.Build(this.session, this.@class);
                    @object.UniqueId = uniqueId;
                    action(@object);
                }
            };
    }
}
