// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Notification.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Services;

    public partial class Notification
    {
        public void BaseOnBuild(ObjectOnBuild method)
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

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
          
            derivation.AddDependency(this.NotificationListWhereNotification, this);
        }
        
        public void BaseOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistSecurityTokens)
            {
                if (this.ExistNotificationListWhereNotification && this.NotificationListWhereNotification.ExistUserWhereNotificationList)
                {
                    var singleton = this.strategy.Session.GetSingleton();
                    var user = this.NotificationListWhereNotification.UserWhereNotificationList;

                    this.SecurityTokens = new[] { user.OwnerSecurityToken, singleton.DefaultSecurityToken };
                }
            }
        }
    }
}
