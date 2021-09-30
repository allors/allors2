// <copyright file="Configuration.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    public abstract class Configuration
    {
        public IObjectFactory ObjectFactory { get; set; }

        public IRoleCache RoleCache { get; set; }

        public IClassCache ClassCache { get; set; }
    }
}
