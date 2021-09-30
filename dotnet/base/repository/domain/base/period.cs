// <copyright file="Period.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("80adbbfd-952e-46f3-a744-78e0ce42bc80")]
    #endregion
    public partial interface Period : Object
    {
        #region Allors
        [Id("5aeb31c7-03d4-4314-bbb2-fca5704b1eab")]
        [AssociationId("8cf0bd14-753d-4f34-99b3-7a6b0d90c3d4")]
        [RoleId("0da8ef4e-53b7-4152-b219-7e0cebbca268")]
        #endregion
        [Required]
        [Workspace]
        DateTime FromDate { get; set; }

        #region Allors
        [Id("d7576ce2-da27-487a-86aa-b0912f745bc0")]
        [AssociationId("cb2fa6c1-f826-45f0-a03f-00e6cb268ebb")]
        [RoleId("4e021875-5bae-4f01-8deb-641016cd2f8d")]
        #endregion
        [Workspace]
        DateTime ThroughDate { get; set; }
    }
}
