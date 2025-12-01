// <copyright file="AccessClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("D0A9EB9A-6877-49D6-86E6-2D510964371A")]
    #endregion
    public partial interface TaggedInterface : Object
    {
        #region Allors
        [Id("09E6E596-821F-4BE8-B7BD-42D27FFE21B2")]
        [AssociationId("C05DEA76-62BA-4FE3-B7D0-783FAC1961C1")]
        [RoleId("333BD86B-B60A-412F-8E40-B184F7C528BD")]
        #endregion
        [Tags("TagI")]
        public bool TagInterface { get; set; }

    }
}
