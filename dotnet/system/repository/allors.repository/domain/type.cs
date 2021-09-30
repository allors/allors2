// <copyright file="Type.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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

        public bool IsInterface => this is Interface;

        public bool IsClass => this is Class;

        public bool IsComposite => !this.IsUnit;

        public bool IsUnit => this is Unit;

        public override string ToString() => this.SingularName;
    }
}
