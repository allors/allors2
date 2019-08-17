// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class TreeNode
    {
        public Guid? RoleType { get; set; }

        public TreeNode[] Nodes { get; set; }
    }
}
