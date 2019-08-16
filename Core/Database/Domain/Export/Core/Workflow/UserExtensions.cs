// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserExtensions.cs" company="Allors bvba">
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
    public static partial class UserExtensions
    {
        public static void CoreDelete(this User @this, DeletableDelete method)
        {
            if (@this.ExistNotificationList)
            {
                @this.NotificationList.Delete();
            }
        }

        public static void CoreOnBuild(this User @this, ObjectOnBuild method)
        {
            if (!@this.ExistNotificationList)
            {
                @this.NotificationList = new NotificationListBuilder(@this.Strategy.Session).Build();
            }
        }
    }
}
