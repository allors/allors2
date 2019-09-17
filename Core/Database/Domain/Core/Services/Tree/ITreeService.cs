// <copyright file="ITreeService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using Allors.Data;
    using Allors.Meta;

    public partial interface ITreeService : IStateful
    {
        TreeNode[] Get(IComposite composite);

        void Set(IComposite composite, TreeNode[] tree);
    }
}
