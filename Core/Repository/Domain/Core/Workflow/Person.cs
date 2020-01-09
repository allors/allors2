// <copyright file="Person.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c799ca62-a554-467d-9aa2-1663293bb37f")]
    #endregion
    public partial class Person : User
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string InUserPassword { get; set; }

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
        #endregion

        #region Allors
        [Id("ed4b710a-fe24-4143-bb96-ed1bd9beae1a")]
        [AssociationId("1ea9eca4-eed0-4f61-8903-c99feae961ad")]
        [RoleId("f10ea049-6d24-4ca2-8efa-032fcf3000b3")]
        [Size(256)]
        #endregion
        [Workspace]
        public string FirstName { get; set; }

        #region Allors
        [Id("eb18bb28-da9c-47b4-a091-2f8f2303dcb6")]
        [AssociationId("e3a4d7b2-c5f1-4101-9aab-a0135d37a5a5")]
        [RoleId("a86fc7a6-dedd-4da9-a250-75c9ec730d22")]
        [Size(256)]
        #endregion
        [Workspace]
        public string MiddleName { get; set; }

        #region Allors
        [Id("8a3e4664-bb40-4208-8e90-a1b5be323f27")]
        [AssociationId("9b48ff56-afef-4501-ac97-6173731ff2c9")]
        [RoleId("ace04ad8-bf64-4fc3-8216-14a720d3105d")]
        [Size(256)]
        #endregion
        [Workspace]
        public string LastName { get; set; }

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
