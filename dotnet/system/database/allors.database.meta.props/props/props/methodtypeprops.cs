// <copyright file="MethodClassProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MethodClass type.</summary>

namespace Allors.Database.Meta
{
    using System;

    public sealed partial class MethodTypeProps : OperandTypeProps, IMetaIdentifiableObjectProps
    {
        private readonly IMethodTypeBase methodType;

        internal MethodTypeProps(IMethodTypeBase methodType) => this.methodType = methodType;

        public IComposite ObjectType => ((IMethodType)this.methodType).ObjectType;

        public Guid Id => this.methodType.Id;

        public string Tag => this.methodType.Tag;

        public string Name => this.methodType.Name;

        public string FullName => this.methodType.FullName;

        public string[] WorkspaceNames => this.methodType.WorkspaceNames;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.methodType;

        protected override IOperandTypeBase AsOperandType => this.methodType;
        #endregion
    }
}
