//------------------------------------------------------------------------------------------------- 
// <copyright file="ApproveTask.cs" company="Allors bvba">
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
    [Id("b86d8407-c411-49e4-aae3-64192457c701")]
    #endregion
    public partial interface ApproveTask : Task 
    {
        #region Allors
        [Id("a7c646a2-7aaa-44ae-9240-77b3b6f2e8fa")]
        [AssociationId("2a95997a-ba81-42c0-842d-d3b9221249fe")]
        [RoleId("205fa9b1-8418-4953-a220-3ee8ac6b6ad7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        Notification RejectionNotification { get; set; }

        #region Allors
        [Id("0158D8F3-3E9F-48B3-AD25-51BD7EABC27C")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("F68B3D21-0108-40EC-9455-98764EB74874")]
        #endregion
        [Workspace]
        void Reject();
    }
}