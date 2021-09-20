// <copyright file="MetaPerson.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    public partial class MetaPerson
    {
        internal override void BaseExtend() => this.Delete.Workspace = true;
    }
}
