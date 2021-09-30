// <copyright file="MetaInterface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Meta
{
    public abstract partial class MetaInterface
    {
        public abstract Interface Interface { get; }

        public Interface ObjectType => this.Interface;

        public static implicit operator Interface(MetaInterface metaInterface) => metaInterface.Interface;

        public static implicit operator Composite(MetaInterface metaInterface) => metaInterface.Interface;

        public static implicit operator ObjectType(MetaInterface metaInterface) => metaInterface.Interface;
    }
}
