// <copyright file="TreeNode.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class Node
    {
        /// <summary>
        /// PropertyType
        /// </summary>
        public Guid? propertyType { get; set; }

        /// <summary>
        /// Nodes
        /// </summary>
        public Node[] nodes { get; set; }
    }
}
