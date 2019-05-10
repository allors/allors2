//------------------------------------------------------------------------------------------------- 
// <copyright file="Permission.cs" company="Allors bvba">
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
    [Id("7fded183-3337-4196-afb0-3266377944bc")]
    #endregion
    public partial class Permission : Deletable, AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("097bb620-f068-440e-8d02-ef9d8be1d0f0")]
        [AssociationId("3442728c-164a-477c-87be-19a789229585")]
        [RoleId("3fd81194-2f6f-43e7-9c6b-91f5e3e118ac")]
        [Indexed]
        #endregion
        [Required]
        public Guid OperandTypePointer { get; set; }

        #region Allors
        [Id("29b80857-e51b-4dec-b859-887ed74b1626")]
        [AssociationId("8ffed1eb-b64e-4341-bbb6-348ed7f06e83")]
        [RoleId("cadaca05-55ba-4a13-8758-786ff29c8e46")]
        [Indexed]
        #endregion
        [Required]
        public Guid ConcreteClassPointer { get; set; }
        
        #region Allors
        [Id("9d73d437-4918-4f20-b9a7-3ce23a04bd7b")]
        [AssociationId("891734d6-4855-4b33-8b3b-f46fd6103149")]
        [RoleId("d29ce0ed-fba8-409d-8675-dc95e1566cfb")]
        [Indexed]
        #endregion
        [Required]
        public int OperationEnum { get; set; }

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