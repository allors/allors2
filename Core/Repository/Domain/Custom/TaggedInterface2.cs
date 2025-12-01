// <copyright file="AccessClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("9DEB64AF-809A-400C-8C9C-C08A8D4702B9")]
    #endregion
    public partial interface TaggedInterface2 : Object
    {
        #region Allors
        [Id("730E348E-22B7-483B-9D7F-BB7A7A518E9F")]
        [AssociationId("C20F6C75-7DBF-4298-8056-33F1CC426850")]
        [RoleId("85436819-055E-498F-A7B0-631CA75D6076")]
        #endregion
        [Tags("TagI2")]
        public bool TagInterface2 { get; set; }
    }
}
