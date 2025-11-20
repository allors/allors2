// <copyright file="ITreeService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using Allors.Data;
    using Allors.Meta;

    public partial interface ITreeService : IStateful
    {
        Node[] Get(IComposite composite);

        void Set(IComposite composite, Node[] tree);
    }
}
