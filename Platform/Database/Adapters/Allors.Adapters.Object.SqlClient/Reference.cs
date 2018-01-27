// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

    using Allors.Meta;

    public class Reference
    {
        internal const long InitialVersion = 0;
        private const long UnknownVersion = -1;

        [FlagsAttribute]
        public enum Flags: byte
        {
            MaskIsNew  = 1,
            MaskExists = 2,
            MaskExistsKnown = 4,
        }

        private long version;

        private Flags flags;

        private WeakReference<Strategy> weakReference;

        private bool FlagIsNew
        {
            get { return flags.HasFlag(Flags.MaskIsNew); }

            set { flags = value ? flags | Flags.MaskIsNew : flags & ~Flags.MaskIsNew; }
        }

        private bool FlagExists
        {
            get { return flags.HasFlag(Flags.MaskExists); }

            set { flags = value ? flags | Flags.MaskExists : flags & ~Flags.MaskExists; }
        }

        private bool FlagExistsKnown
        {
            get { return flags.HasFlag(Flags.MaskExistsKnown); }

            set { flags = value ? flags | Flags.MaskExistsKnown : flags & ~Flags.MaskExistsKnown; }
        }

        internal Reference(Session session, IClass @class, long objectId, bool isNew)
        {
            this.Session = session;
            this.Class = @class;
            this.ObjectId = objectId;

            this.FlagIsNew = isNew;
            if (isNew)
            {
                this.FlagExistsKnown = true;
                this.FlagExists = true;
            }
        }

        internal Reference(Session session, IClass @class, long objectId, long version)
            : this(session, @class, objectId, false)
        {
            this.version = version;
            this.FlagExistsKnown = true;
            this.FlagExists = true;
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

        internal long Version
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

        internal bool IsNew => this.FlagIsNew;

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
                var flagsExistsKnown = this.FlagExistsKnown;
                if (!flagsExistsKnown)
                {
                    this.Session.AddReferenceWithoutVersionOrExistsKnown(this);
                    this.Session.GetVersionAndExists();
                }

                return this.FlagExists;
            }

            set
            {
                this.FlagExistsKnown = true;
                this.FlagExists = value;
            }
        }
        
        internal bool ExistsKnown
        {
            get
            {
                var existsKnown = this.FlagExistsKnown;
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
            this.FlagExistsKnown = false;
            this.FlagIsNew = false;
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
            if (this.FlagIsNew)
            {
                this.FlagExistsKnown = true;
                this.FlagExists = false;
            }
            else
            {
                this.FlagExistsKnown = false;
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