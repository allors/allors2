// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using Allors.Meta;

    public class Reference
    {
        internal const long InitialVersion = 0;
        private const long UnknownVersion = -1;

        private static readonly int MaskIsNew = BitVector32.CreateMask();
        private static readonly int MaskExists = BitVector32.CreateMask(MaskIsNew);
        private static readonly int MaskExistsKnown = BitVector32.CreateMask(MaskExists);

        private long version;

        private BitVector32 flags;

        private WeakReference<Strategy> weakReference;

        internal Reference(Session session, IClass @class, long objectId, bool isNew)
        {
            this.Session = session;
            this.Class = @class;
            this.ObjectId = objectId;

            this.flags[MaskIsNew] = isNew;
            if (isNew)
            {
                this.flags[MaskExistsKnown] = true;
                this.flags[MaskExists] = true;
            }
        }

        internal Reference(Session session, IClass @class, long objectId, long version)
            : this(session, @class, objectId, false)
        {
            this.version = version;
            this.flags[MaskExistsKnown] = true;
            this.flags[MaskExists] = true;
        }

        internal virtual Strategy Strategy
        {
            get
            {
                var strategy = this.Target;

                if (strategy == null)
                {
                    strategy = this.CreateStrategy();
                    this.weakReference = new WeakReference<Strategy>(strategy);
                }

                return strategy;
            }
        }

        internal Session Session { get; }

        internal IClass Class { get; }

        internal long ObjectId { get; }

        internal long VersionId
        {
            get
            {
                if (!this.IsNew && this.version == UnknownVersion)
                {
                    this.Session.AddReferenceWithoutVersionOrExistsKnown(this);
                    this.Session.GetVersionAndExists();
                }

                return this.version;
            }

            set
            {
                this.version = value;
            }
        }

        internal bool IsNew => this.flags[MaskIsNew];

        internal bool IsUnknownVersion
        {
            get
            {
                var isUnknown = this.version == UnknownVersion; 
                return isUnknown;
            }
        }

        internal bool Exists
        {
            get
            {
                var flagsExistsKnown = this.flags[MaskExistsKnown];
                if (!flagsExistsKnown)
                {
                    this.Session.AddReferenceWithoutVersionOrExistsKnown(this);
                    this.Session.GetVersionAndExists();
                }

                return this.flags[MaskExists];
            }

            set
            {
                this.flags[MaskExistsKnown] = true;
                this.flags[MaskExists] = value;
            }
        }
        
        internal bool ExistsKnown
        {
            get
            {
                var existsKnown = this.flags[MaskExistsKnown];
                return existsKnown;
            }
        }

        private Strategy Target
        {
            get
            {
                Strategy strategy = null;
                this.weakReference?.TryGetTarget(out strategy);
                return strategy;
            }
        }

        public override int GetHashCode()
        {
            return this.ObjectId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var that = (Reference)obj;
            return that != null && that.ObjectId.Equals(this.ObjectId);
        }

        public override string ToString()
        {
            return "[" + this.Class + ":" + this.ObjectId + "]";
        }

        internal virtual void Commit(HashSet<Reference> referencesWithStrategy)
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

        internal virtual void Rollback(HashSet<Reference> referencesWithStrategy)
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
      
        public virtual Strategy CreateStrategy()
        {
            return new Strategy(this);
        }
    }
}