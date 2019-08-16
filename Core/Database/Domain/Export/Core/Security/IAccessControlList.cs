// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAccessControlList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Meta;

    public interface IAccessControlList
    {
        User User { get; }

        bool CanRead(IPropertyType propertyType);

        bool CanRead(ConcreteRoleType propertyType);

        bool CanWrite(IRoleType roleType);

        bool CanWrite(ConcreteRoleType propertyType);

        bool CanExecute(IMethodType methodType);

        bool IsPermitted(IOperandType operandType, Operations operation);

    }
}
