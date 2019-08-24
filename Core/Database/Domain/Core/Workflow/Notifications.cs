// <copyright file="Notifications.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Notifications
    {
        protected override void CoreSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantOwner(this.ObjectType, full);
            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
