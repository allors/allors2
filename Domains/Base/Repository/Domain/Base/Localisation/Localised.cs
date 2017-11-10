//------------------------------------------------------------------------------------------------- 
// <copyright file="Localised.cs" company="Allors bvba">
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
    [Id("7979a17c-0829-46df-a0d4-1b01775cfaac")]
    #endregion
    public partial interface Localised : Object 
    {
        #region Allors
        [Id("8c005a4e-5ffe-45fd-b279-778e274f4d83")]
        [AssociationId("6684d98b-cd43-4612-bf9d-afefe02a0d43")]
        [RoleId("d43b92ac-9e6f-4238-9625-1e889be054cf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Workspace]
        Locale Locale { get; set; }
    }
}