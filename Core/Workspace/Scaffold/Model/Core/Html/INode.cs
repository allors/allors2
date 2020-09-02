// <copyright file="INode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Html
{
    using Autotest.Angular;

    public interface INode
    {
        Template Template { get; }

        INode Parent { get; }

        Scope InScope { get; }

        void BaseLoad();

        void SetInScope(Scope rootScope);
    }
}
