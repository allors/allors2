//------------------------------------------------------------------------------------------------- 
// <copyright file="Currency.cs" company="Allors bvba">
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
    using Attributes;

    #region Allors
    [Id("fd397adf-40b4-4ef8-b449-dd5a24273df3")]
    #endregion
    [Plural("Currencies")]
    public partial class Currency : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("294a4bdc-f03a-47a2-a649-419e6b9021a3")]
        [AssociationId("f9eec7c6-c4cd-4d8c-a5f7-44855328cd7e")]
        [RoleId("09d74027-4457-4788-803c-24b857245565")]
        #endregion
        [Required]
        [Unique]
        [Size(256)]
        [Workspace]
        public string IsoCode { get; set; }

        #region Allors
        [Id("74c8308b-1b76-4218-9532-f01c9d1e146b")]
        [AssociationId("2cb43671-c648-4bd4-ac08-7302c29246e7")]
        [RoleId("e7c93764-d634-4187-97ed-9248ea56bab2")]
        #endregion
        [Indexed]
        [Required]
        [Unique]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }
        
        #region Allors
        [Id("e9fc0472-cf7a-4e02-b061-cb42b6f5c273")]
        [AssociationId("06b8f2b2-91f0-4b89-ae19-b47de4524556")]
        [RoleId("e1301b8f-25cc-4ace-884e-79af1d303f53")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}