// <copyright file="User.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("0d6bc154-112b-4a58-aa96-3b2a96f82523")]
    #endregion
    public partial class User : System.Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("1ffa3cb7-41f0-406a-a3a5-2f3a4c5ad59c")]
        [AssociationId("5b87b0d4-3bad-499d-96f1-9d39ab58d1e8")]
        [RoleId("939e2772-0bf6-4867-ae7d-3331ab395ba7")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        public User[] Selects { get; set; }
        #region Allors
        [Id("bc6b71a8-2a66-4b57-9c86-ecf521b973ba")]
        [AssociationId("36058495-3b0d-416b-b2fb-cfe06e88035c")]
        [RoleId("4ed76e62-3de2-415f-896e-c90d1f32e129")]
        [Size(256)]
        #endregion
        public string From { get; set; }

        #region inherited methods
        #endregion
    }
}
