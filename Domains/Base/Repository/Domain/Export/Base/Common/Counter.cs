//------------------------------------------------------------------------------------------------- 
// <copyright file="Counter.cs" company="Allors bvba">
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
    [Id("0568354f-e3d9-439e-baac-b7dce31b956a")]
    #endregion
    public partial class Counter : UniquelyIdentifiable 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("309d07d9-8dea-4e99-a3b8-53c0d360bc54")]
        [AssociationId("0c807020-5397-4cdb-8380-52899b7af6b7")]
        [RoleId("ab60f6a7-d913-4377-ab47-97f0fb9d8f3b")]
        #endregion
        [Required]
        public int Value { get; set; }

        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}