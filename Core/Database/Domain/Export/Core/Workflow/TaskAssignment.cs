// <copyright file="TaskAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class TaskAssignment
    {
        public void CoreOnDerive(ObjectOnDerive method)
        {
            var singleton = this.Strategy.Session.GetSingleton();
            this.SecurityTokens = new[] { singleton.DefaultSecurityToken, this.User?.OwnerSecurityToken };

            this.Task?.ManageNotification(this);
        }

        public void CoreDelete(DeletableDelete method) => this.Notification?.Delete();
    }
}
