// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using Domain;
    using Meta;

    internal class MetaObjectCompressor
    {
        private readonly Dictionary<IMetaObject, string> indexByMetaObject;

        private int counter;

        internal MetaObjectCompressor()
        {
            this.indexByMetaObject = new Dictionary<IMetaObject, string>();
            this.counter = 0;
        }

        public string Write(IMetaObject metaObject)
        {
            if (this.indexByMetaObject.TryGetValue(metaObject, out var index))
            {
                return index;
            }

            index = (++this.counter).ToString();
            this.indexByMetaObject.Add(metaObject, index);
            return $"{Compression.IndexMarker}{index}{Compression.IndexMarker}{metaObject.Id.ToString("D").ToLower()}";
        }
    }
}
