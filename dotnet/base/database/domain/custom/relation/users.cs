// <copyright file="Users.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Users
    {
        protected override void CustomPrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
            setup.AddDependency(this.ObjectType, M.Singleton.ObjectType);
            setup.AddDependency(this.ObjectType, M.ContactMechanismPurpose.ObjectType);
        }
    }
}
