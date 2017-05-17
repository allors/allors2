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
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static partial class UserExtensions
    {
        public static string SecurityHash(this User @this)
        {
            var accessControls = @this.AccessControlsWhereEffectiveUser;

            // TODO: Append a Salt 
            var idsWithVersion = string.Join(":", accessControls.OrderBy(v => v.Id).Select(v => v.Id + v.Strategy.ObjectVersion));

            var crypt = SHA256.Create();
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(idsWithVersion), 0, Encoding.UTF8.GetByteCount(idsWithVersion));
            foreach (var theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        public static void BaseDelete(this User @this, DeletableDelete method)
        {
            if (@this.ExistTaskList)
            {
                @this.TaskList.Delete();
            }

            if (@this.ExistNotificationList)
            {
                @this.NotificationList.Delete();
            }
        }

        public static void BaseOnBuild(this User @this, ObjectOnBuild method)
        {
            if (!@this.ExistTaskList)
            {
                @this.TaskList = new TaskListBuilder(@this.Strategy.Session).Build();
            }

            if (!@this.ExistNotificationList)
            {
                @this.NotificationList = new NotificationListBuilder(@this.Strategy.Session).Build();
            }
        }
    }
}