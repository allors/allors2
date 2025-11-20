// <copyright file="PermissionBuilder.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the PermissionBuilder type.</summary>

namespace Allors.Domain
{
    public partial class PermissionBuilder
    {
        public PermissionBuilder WithOperation(Operations operation)
        {
            this.OperationEnum = (int)operation;
            return this;
        }
    }
}
