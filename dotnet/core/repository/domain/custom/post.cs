// <copyright file="Post.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4B66BA88-A452-4AE4-91E7-7FBC28E65B31")]
    #endregion
    public partial class Post : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("D8714378-149D-4E4B-8A18-0D8622BCD32D")]
        [AssociationId("65E038C6-FE61-4B34-9BF3-39F12FBA62FB")]
        [RoleId("5CA8FB0F-D756-4F3B-A85E-DA610F8CECD6")]
        #endregion
        [Required]
        public int Counter { get; set; }

        #region inherited methods
        public void OnBuild() => throw new NotImplementedException();

        public void OnPostBuild() => throw new NotImplementedException();

        public void OnInit() => throw new NotImplementedException();

        public void OnPreDerive() => throw new NotImplementedException();

        public void OnDerive() => throw new NotImplementedException();

        public void OnPostDerive() => throw new NotImplementedException();
        #endregion
    }
}
