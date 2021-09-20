// <copyright file="MethodClassProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MethodClass type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;

    public abstract partial class ObjectTypeProps : MetaObjectProps, IMetaIdentifiableObjectProps
    {
        public Guid Id => this.AsObjectType.Id;

        public string Tag => this.AsObjectType.Tag;

        public bool IsUnit => this.AsObjectType.IsUnit;

        public bool IsComposite => this.AsObjectType.IsComposite;

        public bool IsInterface => this.AsObjectType.IsInterface;

        public bool IsClass => this.AsObjectType.IsClass;

        public string SingularName => this.AsObjectType.SingularName;

        public string PluralName => this.AsObjectType.PluralName;

        public string Name => this.AsObjectType.Name;

        public IEnumerable<string> WorkspaceNames => this.AsObjectType.WorkspaceNames;

        public Type ClrType => this.AsObjectType.ClrType;
        
        #region As
        protected abstract IObjectTypeBase AsObjectType { get; }
        #endregion
    }
}
