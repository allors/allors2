//------------------------------------------------------------------------------------------------- 
// <copyright file="Role.cs" company="Allors bvba">
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
    using System;

    using Attributes;

    #region Allors
    [Id("af6fe5f4-e5bc-4099-bcd1-97528af6505d")]
    #endregion
    public partial class Role : AccessControlledObject, UniquelyIdentifiable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("51e56ae1-72dc-443f-a2a3-f5aa3650f8d2")]
        [AssociationId("47af1a0f-497d-4a19-887b-79e5fb77c8bd")]
        [RoleId("7e6a71b0-2194-47f8-b562-cb4a15e335b6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        public Permission[] Permissions { get; set; }

        #region Allors
        [Id("934bcbbe-5286-445c-a1bd-e2fcc786c448")]
        [AssociationId("05785884-ca83-43de-a6f3-86d3fa7ec82a")]
        [RoleId("8d87c74f-53ed-4e1d-a2ea-12190ac233d2")]
        #endregion
        [Required]
        [Unique]
        [Size(256)]
        public string Name { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}