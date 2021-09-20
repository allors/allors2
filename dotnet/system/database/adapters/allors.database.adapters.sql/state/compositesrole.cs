// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System.Collections.Generic;
    using Meta;

    internal class CompositesRole
    {
        private readonly HashSet<long> baseline;
        private HashSet<long> added;
        private HashSet<long> removed;

        internal CompositesRole(IEnumerable<long> compositeRoles) => this.baseline = new HashSet<long>(compositeRoles);

        internal HashSet<long> ObjectIds
        {
            get
            {
                if ((this.removed == null || this.removed.Count == 0) && (this.added == null || this.added.Count == 0))
                {
                    return this.baseline;
                }

                var merged = new HashSet<long>(this.baseline);
                if (this.removed != null && this.removed.Count > 0)
                {
                    merged.ExceptWith(this.removed);
                }

                if (this.added != null && this.added.Count > 0)
                {
                    merged.UnionWith(this.added);
                }

                return merged;
            }
        }

        internal int Count
        {
            get
            {
                var addedCount = this.added?.Count ?? 0;
                var removedCount = this.removed?.Count ?? 0;

                return this.baseline.Count + addedCount - removedCount;
            }
        }

        internal bool Contains(long objectId)
        {
            if (this.removed != null && this.removed.Contains(objectId))
            {
                return false;
            }

            return this.baseline.Contains(objectId) || this.added?.Contains(objectId) == true;
        }

        internal void Add(long objectId)
        {
            if (this.removed != null && this.removed.Contains(objectId))
            {
                this.removed.Remove(objectId);
                return;
            }

            if (!this.baseline.Contains(objectId))
            {
                if (this.added == null)
                {
                    this.added = new HashSet<long>();
                }

                this.added.Add(objectId);
            }
        }

        internal void Remove(long objectId)
        {
            if (this.added != null && this.added.Contains(objectId))
            {
                this.added.Remove(objectId);
                return;
            }

            if (this.baseline.Contains(objectId))
            {
                if (this.removed == null)
                {
                    this.removed = new HashSet<long>();
                }

                this.removed.Add(objectId);
            }
        }

        internal void Flush(Flush flush, Strategy strategy, IRoleType roleType)
        {
            if (this.Count == 0)
            {
                flush.ClearCompositeAndCompositesRole(strategy.Reference, roleType);
            }
            else
            {
                if (this.added != null && this.added.Count > 0)
                {
                    flush.AddCompositeRole(strategy.Reference, roleType, this.added);
                }

                if (this.removed != null && this.removed.Count > 0)
                {
                    flush.RemoveCompositeRole(strategy.Reference, roleType, this.removed);
                }
            }

            if (this.added != null)
            {
                this.baseline.UnionWith(this.added);
            }

            if (this.removed != null)
            {
                this.baseline.ExceptWith(this.removed);
            }

            this.added = null;
            this.removed = null;
        }
    }
}
