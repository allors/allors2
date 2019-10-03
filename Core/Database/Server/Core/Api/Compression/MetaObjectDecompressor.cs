// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using Meta;
    using Protocol;

    internal class MetaObjectDecompressor
    {
        private readonly Decompressor decompressor;
        private readonly IMetaPopulation metaPopulation;

        internal MetaObjectDecompressor(Decompressor decompressor, IMetaPopulation metaPopulation)
        {
            this.decompressor = decompressor;
            this.metaPopulation = metaPopulation;
        }

        public IMetaObject Read(string compressed)
        {
            var value = this.decompressor.Read(compressed, out var first);
            var id = Guid.Parse(value);
            return this.metaPopulation.Find(id);
        }
    }
}
