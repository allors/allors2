// <copyright file="AssociationTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta
{
    public abstract partial class OperandTypeProps : MetaObjectProps
    {
        public string DisplayName => this.AsOperandType.DisplayName;

        #region As
        protected abstract IOperandTypeBase AsOperandType { get; }
        #endregion
    }
}
