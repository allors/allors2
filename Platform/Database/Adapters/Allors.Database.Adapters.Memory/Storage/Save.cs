// <copyright file="Save.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using Allors;
    using Allors.Meta;

    public class Save
    {
        public IStorage Storage { get; }

        public Dictionary<IClass, Dictionary<long, Strategy>> StrategyByIdByClass { get; }

        public Save(IStorage storage, Dictionary<long, Strategy> strategyById)
        {
            this.Storage = storage;

            this.StrategyByIdByClass = strategyById.Values
                .Where(v => !v.IsDeleted)
                .GroupBy(v => v.Class)
                .ToDictionary(v => v.Key, v => v.ToDictionary(v => v.ObjectId, v => v));
        }

        public void Execute()
        {
            var containersById = this.Storage.Containers.ToDictionary(v => v.Id, v => v);

            this.SyncContainers(containersById);

            foreach (var kvp in this.StrategyByIdByClass)
            {
                var @class = kvp.Key;
                var strategyById = kvp.Value;
                var container = containersById[@class.Id];

                this.SyncFiles(container, @class, strategyById);
            }
        }

        private void SyncContainers(IDictionary<Guid, IStorageContainer> containersById)
        {
            var classIds = new HashSet<Guid>(this.StrategyByIdByClass.Keys.Select(v => v.Id));

            foreach (var container in containersById.Values.ToArray())
            {
                if (!classIds.Contains(container.Id))
                {
                    container.Delete();
                    containersById.Remove(container.Id);
                }
            }

            foreach (var classId in classIds)
            {
                if (!containersById.ContainsKey(classId))
                {
                    var container = this.Storage.CreateContainer(classId);
                    containersById.Add(classId, container);
                }
            }
        }

        private void SyncFiles(IStorageContainer container, IClass @class, Dictionary<long, Strategy> strategyById)
        {
            var files = container.Files;
            var fileIds = new HashSet<long>();

            foreach (var file in files)
            {
                strategyById.TryGetValue(file.Id, out var strategy);
                if (strategy == null)
                {
                    file.Delete();
                }
                else
                {
                    fileIds.Add(file.Id);
                    if (strategy.ObjectVersion != file.Id)
                    {
                        this.SyncFile(file, strategy);
                    }
                }
            }

            foreach (var kvp in strategyById.Where(v => !fileIds.Contains(v.Key)))
            {
                var id = kvp.Key;
                var strategy = kvp.Value;
                var file = container.CreateFile(strategy.ObjectId, strategy.ObjectVersion, "json");

                this.SyncFile(file, strategy);
            }
        }

        private void SyncFile(IStorageFile file, Strategy strategy)
        {
            using (var stream = file.OpenWrite())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();

                    foreach (var roleType in strategy.Class.RoleTypes)
                    {
                        var relationType = roleType.RelationType;
                        if (strategy.ExistRole(relationType))
                        {
                            writer.WriteString(relationType.IdAsString, strategy.GetRole(relationType).ToString());
                        }
                    }

                    writer.WriteEndObject();
                }
            }
        }
    }
}
