// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cache.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    public class Sticky<TKey, TObject>
        where TObject : class, IObject
    {
        private readonly ISession session;
        private readonly RoleType roleType;

        private IDictionary<TKey, long> cache;

        public Sticky(ISession session, RoleType roleType)
        {
            if (!roleType.ObjectType.IsUnit)
            {
                throw new ArgumentException("ObjectType of RoleType should be a Unit");
            }

            this.roleType = roleType;
            this.session = session;
        }

        public TObject this[TKey key]
        {
            get
            {
                if (this.cache == null)
                {
                    this.cache = this.session.GetSticky<TKey>(typeof(TObject), this.roleType);
                }

                if (!this.cache.TryGetValue(key, out var objectId))
                {
                    var extent = this.session.Extent<TObject>();
                    extent.Filter.AddEquals(this.roleType, key);

                    var @object = extent.First;
                    if (@object != null)
                    {
                        objectId = @object.Id;
                        if (!@object.Strategy.IsNewInSession)
                        {
                            this.cache[key] = @object.Id;
                        }
                    }
                }

                return (TObject)this.session.Instantiate(objectId);
            }
        }
    }
}