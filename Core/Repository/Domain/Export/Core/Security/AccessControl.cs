//------------------------------------------------------------------------------------------------- 
// <copyright file="AccessControl.cs" company="Allors bvba">
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
    [Id("c4d93d5e-34c3-4731-9d37-47a8e801d9a8")]
    #endregion
    public partial class AccessControl : Cachable, Deletable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid CacheId { get; set; }
        #endregion

        #region Allors
        [Id("0dbbff5c-3dca-4257-b2da-442d263dcd86")]
        [AssociationId("92e8d639-9205-422b-b4ff-c7e8c2980abf")]
        [RoleId("2d9b7157-5409-40d3-bd3e-6911df9aface")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Repository]
        public UserGroup[] SubjectGroups { get; set; }

        #region Allors
        [Id("37dd1e27-ba75-404c-9410-c6399d28317c")]
        [AssociationId("3d74101d-97bc-46fb-9548-96cb7aa01b39")]
        [RoleId("e0303a17-bf7a-4a7f-bb4b-0a447c56aaaf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public User[] Subjects { get; set; }
        
        #region Allors
        [Id("69a9dae8-678d-4c1c-a464-2e5aa5caf39e")]
        [AssociationId("ec79e57d-be81-430a-b12f-08ffd1e94af3")]
        [RoleId("370d3d12-3164-4753-8a72-1c604bda1b64")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Role Role { get; set; }
        
        #region Allors
        [Id("5e218f37-3b07-4002-87a4-7581a53f01ba")]
        [AssociationId("be94d5f0-df53-4118-987a-11bce8593a1b")]
        [RoleId("46d65185-9e0d-4347-a38f-6afa2431c241")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Derived]
        public Permission[] EffectivePermissions { get; set; }
        
        #region Allors
        [Id("50ecae85-e5a9-467e-99a3-78703d954b2f")]
        [AssociationId("01590aea-d75c-45be-af4b-bf56545a4008")]
        [RoleId("bac6c53c-e103-42cb-b36d-2faa00ebf574")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Derived]
        public User[] EffectiveUsers { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete(){}

        #endregion
    }
}