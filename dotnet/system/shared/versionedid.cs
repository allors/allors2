// <copyright file="Multiplicity.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;

    public class VersionedId : IEquatable<VersionedId>
    {
        public VersionedId(long id, long version)
        {
            this.Id = id;
            this.Version = version;
        }

        public long Id { get; }

        public long Version { get; }

        public bool Equals(VersionedId other) => this.Id == other?.Id && this.Version == other.Version;

        public override bool Equals(object other) => other is VersionedId otherVersionedId && this.Equals(otherVersionedId);

        public override int GetHashCode()
        {
            unchecked
            {
                return this.Id.GetHashCode() * 397 ^ this.Version.GetHashCode();
            }
        }
    }
}
