// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAccessControlListFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors;

    /// <summary>
    /// A factory for AccessControlLists.
    /// </summary>
    public interface IAccessControlListFactory
    {
        IAccessControlList Create(IObject allorsObject);
    }
}
