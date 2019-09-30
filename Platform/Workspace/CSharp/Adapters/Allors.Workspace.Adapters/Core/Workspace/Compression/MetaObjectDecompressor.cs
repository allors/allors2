// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using Protocol;
    using Workspace;
    using Workspace.Meta;

    internal class MetaObjectDecompressor
    {
        private readonly Decompressor decompressor;
        private readonly IMetaPopulation metaPopulation;

        internal MetaObjectDecompressor(Decompressor decompressor, IWorkspace workspace)
        {
            this.decompressor = decompressor;
            this.metaPopulation = workspace.ObjectFactory.MetaPopulation;
        }

        public IMetaObject Read(string compressed)
        {
            var metaObjectIdAsString = this.decompressor.Read(compressed, out var first);
            if (metaObjectIdAsString == null)
            {
                return null;
            }

            var metaObjectId = Guid.Parse(metaObjectIdAsString);
            return this.metaPopulation.Find(metaObjectId);
        }
    }
}
