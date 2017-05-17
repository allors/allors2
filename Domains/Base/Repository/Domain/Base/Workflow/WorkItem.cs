//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkItem.cs" company="Allors bvba">
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
    [Id("fbea29c6-6109-4163-a088-9f0b4deac896")]
    #endregion
    public partial interface WorkItem : Object 
    {
        #region Allors
        [Id("7e6d392b-00e7-4095-8525-d9f4ef8cfaa3")]
        [AssociationId("b20f1b54-87a4-4fc2-91db-8848d6d40ad1")]
        [RoleId("cf456f4d-8c76-4bfe-9996-89b660c9b153")]
        [Derived]
        [Indexed]
        [Size(256)]
        [Workspace]
        #endregion
        string WorkItemDescription { get; set; }
    }
}