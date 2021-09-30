// <copyright file="ApproveTask.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    /// <summary>
    /// A <see cref="Task"/> that can be approved or rejected.
    /// </summary>
    #region Allors
    [Id("b86d8407-c411-49e4-aae3-64192457c701")]
    #endregion
    public partial interface ApproveTask : Task
    {
        /// <summary>
        /// A text the user can enter when approving or rejecting a task.
        /// </summary>
        #region Allors
        [Id("a280bf60-2eb7-488a-abf7-f03c9d9197b5")]
        [AssociationId("33be2d23-16d7-4739-8ef2-42391b0f4bd1")]
        [RoleId("9f88a8cf-84c1-42cc-be52-1d08597e56fa")]
        [Size(-1)]
        #endregion
        [Workspace]
        string Comment { get; set; }

        /// <summary>
        /// The <see cref="Notification"/> that is created when this task is rejected.
        /// </summary>
        #region Allors
        [Id("a7c646a2-7aaa-44ae-9240-77b3b6f2e8fa")]
        [AssociationId("2a95997a-ba81-42c0-842d-d3b9221249fe")]
        [RoleId("205fa9b1-8418-4953-a220-3ee8ac6b6ad7")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        [Derived]
        Notification RejectionNotification { get; set; }

        /// <summary>
        /// The <see cref="Notification"/> that is created when this task is approved.
        /// </summary>
        #region Allors
        [Id("4AF7D84E-393F-402F-8E76-044A75F77543")]
        [AssociationId("4DD209A2-1F0D-4476-8417-6E54781E2227")]
        [RoleId("6912971D-1754-46B5-A162-A0CE70001710")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        [Derived]
        Notification ApprovalNotification { get; set; }

        /// <summary>
        /// Approve this task.
        /// </summary>
        #region Allors
        [Id("0158D8F3-3E9F-48B3-AD25-51BD7EABC27C")]
        #endregion
        [Workspace]
        void Approve();

        /// <summary>
        /// Reject this task.
        /// </summary>
        #region Allors
        [Id("F68B3D21-0108-40EC-9455-98764EB74874")]
        #endregion
        [Workspace]
        void Reject();
    }
}
