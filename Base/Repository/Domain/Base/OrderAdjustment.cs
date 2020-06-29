// <copyright file="OrderAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("c5578565-c07a-4dc1-8381-41955db364e2")]
    #endregion
    public partial interface OrderAdjustment : Deletable, Versioned
    {
        #region Allors
        [Id("4e7cbdda-9f19-44dd-bbef-6cab5d92a8a3")]
        [AssociationId("5ccd492c-cf29-468b-b99d-126a9573e573")]
        [RoleId("7388d1a3-f24a-4c41-b57c-938160b3d1a6")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal Amount { get; set; }

        #region Allors
        [Id("bc1ad594-88b6-4176-994c-a52be672f06d")]
        [AssociationId("ebc960bf-dd8c-4854-afec-185b260315e9")]
        [RoleId("9d2f66e2-0bbd-46ab-b65b-43e6b38383b9")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal Percentage { get; set; }
    }
}
