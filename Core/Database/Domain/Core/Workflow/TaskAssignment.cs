// <copyright file="TaskAssignment.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class TaskAssignment
    {
        public void CoreOnDerive(ObjectOnDerive method)
        {
            var defaultSecurityToken = new SecurityTokens(this.Session()).DefaultSecurityToken;
            this.SecurityTokens = new[] { defaultSecurityToken, this.User?.OwnerSecurityToken };

            //this.Task?.ManageNotification(this);
        }

        public void CoreDelete(DeletableDelete method) => this.Notification?.Delete();
    }
}
