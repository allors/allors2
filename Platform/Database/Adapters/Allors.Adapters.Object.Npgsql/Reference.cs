// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using Allors.Adapters.Database.Npgsql;
    using Allors.Meta;

    public class Reference
    {
        public const int InitialVersion = 0;
        public const int UnknownVersion = -1;

        private static readonly int MaskIsNew = BitVector32.CreateMask();
        private static readonly int MaskExists = BitVector32.CreateMask(MaskIsNew);
        private static readonly int MaskExistsKnown = BitVector32.CreateMask(MaskExists);

        private readonly DatabaseSession session;
        private readonly IClass objectType;
        private readonly long objectId;
        private long version;

        private BitVector32 flags;

        private WeakReference weakReference;

        public Reference(DatabaseSession session, IClass objectType, long objectId, bool isNew)
        {
            this.session = session;
            this.objectType = objectType;
            this.objectId = objectId;

            this.flags[MaskIsNew] = isNew;
            if (isNew)
            {
                this.flags[MaskExistsKnown] = true;
                this.flags[MaskExists] = true;
            }
        }

        public Reference(DatabaseSession session, IClass objectType, long objectId, long version)
            : this(session, objectType, objectId, false)
        {
            this.version = version;
            this.flags[MaskExistsKnown] = true;
            this.flags[MaskExists] = true;
        }

        public virtual Strategy Strategy
        {
            get
            {
                var strategy = this.Target;

                if (strategy == null)
                {
                    strategy = this.CreateStrategy();
                    this.weakReference = new WeakReference(strategy);
                }

                return strategy;
            }
        }

        public DatabaseSession Session
        {
            get
            {
                return this.session;
            }
        }

        public IClass ObjectType
        {
            get
            {
                return this.objectType;
            }
        }

        public long ObjectId
        {
            get
            {
                return this.objectId;
            }
        }

        public long Version
        {
            get
            {
                if (!this.IsNew && this.version == UnknownVersion)
                {
                    this.Session.GetCacheIdsAndExists();
                }

                return this.version;
            }

            set
            {
                this.version = value;
            }
        }

        public bool IsNew
        {
            get
            {
                return this.flags[MaskIsNew];
            }
        }

        public bool Exists
        {
            get
            {
                var flagsExistsKnown = this.flags[MaskExistsKnown];
                if (!flagsExistsKnown)
                {
                    this.Session.AddReferenceWithoutCacheId(this);
                    this.Session.GetCacheIdsAndExists();
                }

                return this.flags[MaskExists];
            }

            set
            {
                this.flags[MaskExistsKnown] = true;
                this.flags[MaskExists] = value;
            }
        }

        public Strategy Target
        {
            get
            {
                return (this.weakReference == null) ? null : (Strategy)this.weakReference.Target;
            }
        }

        public override int GetHashCode()
        {
            return this.objectId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var that = (Reference)obj;
            return that != null && that.objectId.Equals(this.objectId);
        }

        public override string ToString()
        {
            return "[" + this.objectType + ":" + this.ObjectId + "]";
        }

        public virtual void Commit(HashSet<Reference> referencesWithStrategy)
        {
            this.flags[MaskExistsKnown] = false;
            this.flags[MaskIsNew] = false;
            this.version = UnknownVersion;

            var strategy = this.Target;
            if (strategy != null)
            {
                referencesWithStrategy.Add(this);
                strategy.Release();
            }
        }

        public virtual void Rollback(HashSet<Reference> referencesWithStrategy)
        {
            if (this.flags[MaskIsNew])
            {
                this.flags[MaskExistsKnown] = true;
                this.flags[MaskExists] = false;
            }
            else
            {
                this.flags[MaskExistsKnown] = false;
            }

            this.version = UnknownVersion;

            var strategy = this.Target;
            if (strategy != null)
            {
                referencesWithStrategy.Add(this);
                strategy.Release();
            }
        }
      
        protected virtual Strategy CreateStrategy()
        {
            return new Strategy(this);
        }
    }
}