//-------------------------------------------------------------------------------------------------
// <copyright file="WorkItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
        [Size(-1)]
        #endregion
        [Workspace]
        string WorkItemDescription { get; set; }
    }
}
