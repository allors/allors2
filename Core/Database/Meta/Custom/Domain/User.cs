// <copyright file="User.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    public partial class MetaUser
    {
        internal override void CustomExtend()
        {
            this.UserEmail.RelationType.Workspace = true;

            this.UserName.RelationType.Workspace = true;
        }
    }
}
