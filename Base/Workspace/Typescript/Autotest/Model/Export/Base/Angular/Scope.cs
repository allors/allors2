// <copyright file="Scope.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using System.Collections.Generic;
    using Autotest.Html;

    public partial class Scope
    {
        public string Name { get; set; }

        public HashSet<INode> Nodes = new HashSet<INode>();
    }
}