// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectOnBuild.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class ObjectOnBuild
    {
        public IObjectBuilder Builder { get; private set; }

        public ObjectOnBuild WithBuilder(IObjectBuilder builder)
        {
            this.Builder = builder;
            return this;
        }
    }
}
