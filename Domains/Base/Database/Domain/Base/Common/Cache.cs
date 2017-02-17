// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cache.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
    using Allors;
    using Allors.Meta;
    using System;
    using System.Collections.Concurrent;

    public class Cache<TKey, TObject>
        where TObject : class, IObject
    {
        private readonly string cacheKey = "Allors.Cache." + typeof(TObject);

        private readonly ISession databaseSession;
        private readonly IDatabase database;
        private readonly RoleType roleType;

        private ConcurrentDictionary<TKey, long> databaseSessionCache;
        private ConcurrentDictionary<TKey, long> databaseCache;

        public Cache(ISession session, RoleType roleType)
        {
            if (!roleType.ObjectType.IsUnit)
            {
                throw new ArgumentException("ObjectType of RoleType should be a Unit");
            }

            this.roleType = roleType;

            this.databaseSession = session;
            this.database = this.databaseSession.Database;
        }

        public TObject this[TKey key] => this.Get(key);

        public TObject Get(TKey key)
        {
            this.LazyLoadDatabaseSessionCache();

            long cachedObjectId;
            if (!this.databaseSessionCache.TryGetValue(key, out cachedObjectId))
            {
                this.LazyLoadDatabaseCache();

                if (!this.databaseCache.TryGetValue(key, out cachedObjectId))
                {
                    var extent = this.databaseSession.Extent<TObject>();
                    extent.Filter.AddEquals(this.roleType, key);

                    var databaseObject = extent.First;

                    if (databaseObject != null)
                    {
                        cachedObjectId = databaseObject.Id;

                        this.databaseSessionCache[key] = databaseObject.Id;
                        if (!databaseObject.Strategy.IsNewInSession)
                        {
                            this.databaseCache[key] = databaseObject.Id;
                        }
                    }
                }
            }

            return (TObject)this.databaseSession.Instantiate(cachedObjectId);
        }

        public void Add(TObject cachedObject)
        {
            if (cachedObject != null)
            {
                if (cachedObject.Strategy.ExistUnitRole(this.roleType.RelationType))
                {
                    var key = (TKey)cachedObject.Strategy.GetUnitRole(this.roleType.RelationType);

                    this.LazyLoadDatabaseSessionCache();

                    this.databaseSessionCache[key] = cachedObject.Id;

                    if (!cachedObject.Strategy.IsNewInSession)
                    {
                        this.LazyLoadDatabaseCache();
                        this.databaseCache[key] = cachedObject.Id;
                    }
                }
            }
        }

        private void LazyLoadDatabaseSessionCache()
        {
            if (this.databaseSessionCache == null)
            {
                this.databaseSessionCache = (ConcurrentDictionary<TKey, long>)this.databaseSession[this.cacheKey];
                if (this.databaseSessionCache == null)
                {
                    this.databaseSession[this.cacheKey] = new ConcurrentDictionary<TKey, long>();
                    this.databaseSessionCache = (ConcurrentDictionary<TKey, long>)this.databaseSession[this.cacheKey];
                }
            }
        }

        private void LazyLoadDatabaseCache()
        {
            if (this.databaseCache == null)
            {
                this.databaseCache = (ConcurrentDictionary<TKey, long>)this.database[this.cacheKey];
                if (this.databaseCache == null)
                {
                    this.database[this.cacheKey] = new ConcurrentDictionary<TKey, long>();
                    this.databaseCache = (ConcurrentDictionary<TKey, long>)this.database[this.cacheKey];
                }
            }
        }
    }
}