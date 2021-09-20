// <copyright file="Reference.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections.Generic;

    using Allors.Database.Meta;

    public class Reference
    {
        private long version;

        private Flags flags;

        private WeakReference<Strategy> weakReference;

        internal Reference(Transaction transaction, IClass @class, long objectId, bool isNew)
        {
            this.Transaction = transaction;
            this.Class = @class;
            this.ObjectId = objectId;

            this.FlagIsNew = isNew;
            if (isNew)
            {
                this.FlagExistsKnown = true;
                this.FlagExists = true;
                this.version = Allors.Version.DatabaseInitial;
            }
        }

        internal Reference(Transaction transaction, IClass @class, long objectId, long version)
            : this(transaction, @class, objectId, false)
        {
            this.version = version;
            this.FlagExistsKnown = true;
            this.FlagExists = true;
        }

        [Flags]
        public enum Flags : byte
        {
            MaskIsNew = 1,
            MaskExists = 2,
            MaskExistsKnown = 4,
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

        internal Transaction Transaction { get; }

        internal IClass Class { get; }

        public long ObjectId { get; }

        internal long Version
        {
            get
            {
                if (!this.IsNew && this.version == (long)Allors.Version.Unknown)
                {
                    this.Transaction.AddReferenceWithoutVersionOrExistsKnown(this);
                    this.Transaction.GetVersionAndExists();
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
                return this.version == (long)Allors.Version.Unknown;
            }
        }

        internal bool Exists
        {
            get
            {
                var flagsExistsKnown = this.FlagExistsKnown;
                if (!flagsExistsKnown)
                {
                    this.Transaction.AddReferenceWithoutVersionOrExistsKnown(this);
                    this.Transaction.GetVersionAndExists();
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
                return this.FlagExistsKnown;
            }
        }

        private bool FlagIsNew
        {
            get { return this.flags.HasFlag(Flags.MaskIsNew); }

            set { this.flags = value ? this.flags | Flags.MaskIsNew : this.flags & ~Flags.MaskIsNew; }
        }

        private bool FlagExists
        {
            get { return this.flags.HasFlag(Flags.MaskExists); }

            set { this.flags = value ? this.flags | Flags.MaskExists : this.flags & ~Flags.MaskExists; }
        }

        private bool FlagExistsKnown
        {
            get { return this.flags.HasFlag(Flags.MaskExistsKnown); }

            set { this.flags = value ? this.flags | Flags.MaskExistsKnown : this.flags & ~Flags.MaskExistsKnown; }
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

        public override int GetHashCode() => this.ObjectId.GetHashCode();

        public override bool Equals(object obj)
        {
            var that = (Reference)obj;
            return that != null && that.ObjectId.Equals(this.ObjectId);
        }

        public override string ToString() => "[" + this.Class + ":" + this.ObjectId + "]";

        public virtual Strategy CreateStrategy() => new Strategy(this);

        internal virtual void Commit(HashSet<Reference> referencesWithStrategy)
        {
            this.FlagExistsKnown = false;
            this.FlagIsNew = false;
            this.version = Allors.Version.Unknown;

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

            this.version = Allors.Version.Unknown;

            var strategy = this.Target;
            if (strategy != null)
            {
                referencesWithStrategy.Add(this);
                strategy.Release();
            }
        }
    }
}
