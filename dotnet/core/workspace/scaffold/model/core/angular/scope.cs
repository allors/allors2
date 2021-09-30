// <copyright file="Scope.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Angular
{
    using System.Collections.Generic;
    using Autotest.Html;

    public partial class Scope
    {
        public HashSet<INode> Nodes = new HashSet<INode>();

        public string Name { get; set; }
    }
}
