// <copyright file="PermissionBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
