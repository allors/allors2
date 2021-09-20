// <copyright file="Save.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory.IO
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Adapters.Memory;
    using Allors.Serialization;
    using Allors.Meta;

    public sealed class PopulationData : IPopulationData
    {
        private readonly Session session;
        private readonly Dictionary<IObjectType, List<Strategy>> sortedNonDeletedStrategiesByObjectType;

        public PopulationData(Session session, Dictionary<IObjectType, List<Strategy>> sortedNonDeletedStrategiesByObjectType)
        {
            this.session = session;
            this.sortedNonDeletedStrategiesByObjectType = sortedNonDeletedStrategiesByObjectType;
        }

        public IEnumerator<IClassData> GetEnumerator() => this.sortedNonDeletedStrategiesByObjectType
            .Select(kvp => new ClassData(kvp.Key, kvp.Value))
            .Cast<IClassData>()
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
