// <copyright file="NotificationList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class NotificationList
    {
        public void CoreOnDerive(ObjectOnDerive method)
        {
            this.UnconfirmedNotifications = this.Notifications.Where(notification => !notification.Confirmed).ToArray();
            this.ConfirmedNotifications = this.Notifications.Where(notification => notification.Confirmed).ToArray();

            if (!this.ExistSecurityTokens)
            {
                if (this.ExistUserWhereNotificationList)
                {
                    var defaultSecurityToken = new SecurityTokens(this.Session()).DefaultSecurityToken;

                    this.SecurityTokens = new[] { this.UserWhereNotificationList.OwnerSecurityToken, defaultSecurityToken };
                }
            }
        }
    }
}
