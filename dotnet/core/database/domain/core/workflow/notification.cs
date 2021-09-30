// <copyright file="Notification.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Notification
    {
        public void CoreOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistDateCreated)
            {
                this.DateCreated = this.Strategy.Session.Now();
            }

            if (!this.ExistConfirmed)
            {
                this.Confirmed = false;
            }
        }

        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (changeSet.HasChangedRole(this, this.Meta.Confirmed))
            {
                iteration.AddDependency(this.NotificationListWhereNotification, this);
                iteration.Mark(this.NotificationListWhereNotification);
            }
        }

        public void BaseConfirm(NotificationConfirm method) => this.Confirmed = true;

        public void CoreOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistSecurityTokens)
            {
                if (this.ExistNotificationListWhereNotification && this.NotificationListWhereNotification.ExistUserWhereNotificationList)
                {
                    var user = this.NotificationListWhereNotification.UserWhereNotificationList;
                    var defaultSecurityToken = new SecurityTokens(this.Session()).DefaultSecurityToken;

                    this.SecurityTokens = new[] { user.OwnerSecurityToken, defaultSecurityToken };
                }
            }
        }
    }
}
