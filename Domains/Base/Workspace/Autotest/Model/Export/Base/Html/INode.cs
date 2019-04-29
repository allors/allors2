// <copyright file="INode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using Autotest.Angular;

    public interface INode
    {
        Template Template { get; }

        INode Parent { get; }

        void BaseLoad();
    }
}