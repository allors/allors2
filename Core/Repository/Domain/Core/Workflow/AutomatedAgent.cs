// <copyright file="AutomatedAgent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("3587d2e1-c3f6-4c55-a96c-016e0501d99c")]
    #endregion
    public partial class AutomatedAgent : User
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string UserPasswordHash { get; set; }

        public string UserEmail { get; set; }

        public string NormalizedUserEmail { get; set; }

        public bool UserEmailConfirmed { get; set; }

        public string UserSecurityStamp { get; set; }

        public string UserPhoneNumber { get; set; }

        public bool UserPhoneNumberConfirmed { get; set; }

        public bool UserTwoFactorEnabled { get; set; }

        public DateTime UserLockoutEnd { get; set; }

        public bool UserLockoutEnabled { get; set; }

        public int UserAccessFailedCount { get; set; }

        public IdentityClaim[] IdentityClaims { get; set; }

        public Login[] Logins { get; set; }

        public NotificationList NotificationList { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("4e158d75-d0b5-4cb7-ad41-e8ed3002d175")]
        [AssociationId("6f2a83eb-17e9-408e-b18b-9bb2b9a3e812")]
        [RoleId("4fac2dd3-8711-4115-96b9-a38f62e2d093")]
        #endregion
        [Indexed]
        [Size(256)]
        public string Name { get; set; }

        #region Allors
        [Id("58870c93-b066-47b7-95f7-5411a46dbc7e")]
        [AssociationId("31925ed6-e66c-4718-963f-c8a71d566fe8")]
        [RoleId("eee42775-b172-4fde-9042-a0f9b2224ec3")]
        #endregion
        [Size(-1)]
        public string Description { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
