
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
            var derivation = method.Derivation;

            derivation.AddDependency(this.NotificationListWhereNotification, this);
        }

        public void CoreOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistSecurityTokens)
            {
                if (this.ExistNotificationListWhereNotification && this.NotificationListWhereNotification.ExistUserWhereNotificationList)
                {
                    var singleton = this.Strategy.Session.GetSingleton();
                    var user = this.NotificationListWhereNotification.UserWhereNotificationList;

                    this.SecurityTokens = new[] { user.OwnerSecurityToken, singleton.DefaultSecurityToken };
                }
            }
        }
    }
}
