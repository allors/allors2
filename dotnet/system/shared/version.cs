// <copyright file="Multiplicity.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;

    public readonly struct Version : IEquatable<Version>
    {
        public static readonly Version Unknown = new Version(0);

        public static readonly Version WorkspaceInitial = new Version(1);

        public static readonly Version DatabaseInitial = new Version(2);

        private Version(long value) => this.Value = value;

        public long Value { get; }

        public static bool operator ==(Version @this, Version other) => @this.Equals(other);

        public static bool operator !=(Version @this, Version other) => !@this.Equals(other);

        public static implicit operator long(Version version) => version.Value;

        public static implicit operator Version(long value) => new Version(value);

        public bool Equals(Version other) => this.Value == other.Value;

        public override bool Equals(object obj) => obj is Version other && this.Equals(other);

        public override int GetHashCode() => this.Value.GetHashCode();

        public Version Next() => new Version(this.Value + 1);
    }
}
