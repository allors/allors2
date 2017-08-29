//------------------------------------------------------------------------------------------------- 
// <copyright file="AutomatedAgent.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("3587d2e1-c3f6-4c55-a96c-016e0501d99c")]
    #endregion
    public partial class AutomatedAgent : User
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public bool UserEmailConfirmed { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string UserEmail { get; set; }

        public string UserPasswordHash { get; set; }

        public TaskList TaskList { get; set; }

        public NotificationList NotificationList { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Locale Locale { get; set; }

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
        [Required]
        [Size(256)]
        public string Description { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}