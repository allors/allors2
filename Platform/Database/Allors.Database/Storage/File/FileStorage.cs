// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.IO;
    using System.Linq;

    public class FileStorage : IStorage
    {
        public DirectoryInfo Root { get; }

        public FileStorage(DirectoryInfo root) => this.Root = root;

        public IStorageContainer[] Containers
        {
            get
            {
                this.Root.Refresh();

                return this.Root.GetDirectories()
                    .Where(v => Guid.TryParse(Path.GetFileNameWithoutExtension(v.Name), out var guid))
                    .Select(v => new FileStorageContainer(this, v))
                    .Cast<IStorageContainer>()
                    .ToArray();
            }
        }

        public IStorageContainer CreateContainer(Guid id)
        {
            this.Root.Refresh();

            return new FileStorageContainer(this, this.Root.CreateSubdirectory(id.ToString("N")));
        }
    }
}
