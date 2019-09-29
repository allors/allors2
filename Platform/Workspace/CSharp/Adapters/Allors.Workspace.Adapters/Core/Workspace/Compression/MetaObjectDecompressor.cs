// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol.Remote.Sync;
    using Workspace;
    using Workspace.Meta;

    internal class MetaObjectDecompressor
    {
        private readonly Dictionary<string, IMetaObject> metaObjectByKey;
        private readonly IMetaPopulation metaPopulation;

        internal MetaObjectDecompressor(IWorkspace workspace)
        {
            this.metaPopulation = workspace.ObjectFactory.MetaPopulation;
            this.metaObjectByKey = new Dictionary<string, IMetaObject>();
        }

        public IMetaObject Read(string compressed)
        {
            if (compressed[0] == Compression.IndexMarker)
            {
                var secondIndexMarkerIndex = compressed.IndexOf(Compression.IndexMarker, 1);
                var key = compressed.Substring(1, secondIndexMarkerIndex - 1);
                var classIdAsString = compressed.Substring(secondIndexMarkerIndex + 1);
                var classId = Guid.Parse(classIdAsString);
                var @class = (IClass)this.metaPopulation.Find(classId);
                this.metaObjectByKey.Add(key, @class);
                return @class;
            }

            return this.metaObjectByKey[compressed];
        }
    }
}
