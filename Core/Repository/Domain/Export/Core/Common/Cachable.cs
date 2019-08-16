//------------------------------------------------------------------------------------------------- 
// <copyright file="Cachable.cs" company="Allors bvba">
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
    [Id("B17AFC19-9E91-4631-B6D8-43B32A65E0A0")]
    #endregion
    public partial interface Cachable : Object
    {
        #region Allors
        [Id("EF6F1F4C-5B62-49DC-9D05-0F02973ACCB3")]
        [AssociationId("1137FDD3-07E6-432E-8C42-273EF24863D5")]
        [RoleId("D6A473F7-4EFF-4D3D-BDB2-59F5EE8B0E52")]
        #endregion
        [Required]
        Guid CacheId { get; set; }
    }
}