//------------------------------------------------------------------------------------------------- 
// <copyright file="Version.cs" company="Allors bvba">
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using System;

    using Attributes;

    public partial interface Version : AccessControlledObject
    {
        #region Allors
        [Id("561C7A91-5232-453F-BA26-9B84D871ECC9")]
        [AssociationId("DD8E700B-1E1D-4A69-8262-F38C971B730D")]
        [RoleId("F19CFCEA-5D55-4F22-9F46-820C2F63A9B4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User LastModifiedBy { get; set; }
    }
}