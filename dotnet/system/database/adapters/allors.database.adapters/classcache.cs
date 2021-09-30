// <copyright file="ClassCache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System.Collections.Generic;
    using Meta;

    public class ClassCache : IClassCache
    {
        private Dictionary<long, IClass> classByObject;

        public ClassCache() => this.classByObject = new Dictionary<long, IClass>();

        public bool TryGet(long objectId, out IClass @class) => this.classByObject.TryGetValue(objectId, out @class);

        public void Set(long objectId, IClass @class) => this.classByObject[objectId] = @class;

        public void Invalidate() => this.classByObject = new Dictionary<long, IClass>();

        public void Invalidate(long[] objectsToInvalidate)
        {
            foreach (var objectToInvalidate in objectsToInvalidate)
            {
                this.classByObject.Remove(objectToInvalidate);
            }
        }
    }
}
