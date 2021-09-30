// <copyright file="MetaClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Meta
{
    public abstract partial class MetaClass
    {
        public abstract Class Class { get; }

        public Class ObjectType => this.Class;

        public static implicit operator Class(MetaClass metaClass) => metaClass.Class;

        public static implicit operator Composite(MetaClass metaClass) => metaClass.Class;

        public static implicit operator ObjectType(MetaClass metaClass) => metaClass.Class;
    }
}
