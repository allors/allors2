// <copyright file="Type.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;

    public abstract class Type
    {
        protected Type(Guid id, string name)
        {
            this.Id = id;
            this.SingularName = name;
        }

        public Guid Id { get; }

        public string SingularName { get; }

        public override string ToString() => this.SingularName;
    }
}
