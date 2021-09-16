// <copyright file="ProductQuoteApproval.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("C547102C-F665-45FF-81B0-F610CF4690F2")]
    #endregion
    public partial class ProductQuoteApproval : ApproveTask
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public WorkItem WorkItem { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime DateClosed { get; set; }

        public User[] Participants { get; set; }

        public User Performer { get; set; }

        public string Comment { get; set; }

        public Notification ApprovalNotification { get; set; }

        public Notification RejectionNotification { get; set; }

        #endregion

        #region Allors
        [Id("F0CF2F10-695D-47F0-9E0D-F90921F5FD36")]
        [AssociationId("5778D472-761A-47C8-A298-3F0EAA166A2F")]
        [RoleId("07B3D10C-B480-4103-9DD8-C8D7BD272984")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        public ProductQuote ProductQuote { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        public void Approve() { }

        public void Reject() { }

        public void DeriveParticipants() { }

        #endregion
    }
}
