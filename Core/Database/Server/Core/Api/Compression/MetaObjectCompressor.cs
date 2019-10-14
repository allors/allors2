// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using Meta;
    using Protocol;

    public class MetaObjectCompressor
    {
        private readonly Compressor compressor;

        internal MetaObjectCompressor(Compressor compressor) => this.compressor = compressor;

        public string Write(IMetaObject metaObject)
        {
            var value = metaObject.Id.ToString("D");
            return this.compressor.Write(value);
        }
    }
}
