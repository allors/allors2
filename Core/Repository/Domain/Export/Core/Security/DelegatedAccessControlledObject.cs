// <copyright file="DelegatedAccessControlledObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("842FA7B5-2668-43E9-BFEF-21B6F5B20E8B")]
    #endregion
    public partial interface DelegatedAccessControlledObject : Object
    {
        [Id("C56B5BC5-35BD-4762-B237-54EA3BFC7E7A")]
        void DelegateAccess();
    }
}
