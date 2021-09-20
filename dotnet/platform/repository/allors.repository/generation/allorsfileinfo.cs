// <copyright file="AllorsFileInfo.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository.Generation
{
    using System.IO;

    public class AllorsFileInfo
    {
        private readonly FileInfo fileInfo;

        public AllorsFileInfo(FileInfo fileInfo) => this.fileInfo = fileInfo;

        public string GetRelativeName(DirectoryInfo baseDirectoryInfo)
        {
            var baseDirectory = new AllorsDirectoryInfo(baseDirectoryInfo);
            var directory = new AllorsDirectoryInfo(this.fileInfo.Directory);

            var relativePath = directory.GetRelativeName(baseDirectory);
            if (relativePath == null)
            {
                return null;
            }

            return Path.Combine(relativePath, this.fileInfo.Name);
        }

        public string GetRelativeOrFullName(DirectoryInfo baseDirectoryInfo)
        {
            var relativeName = this.GetRelativeName(baseDirectoryInfo);
            if (relativeName == null)
            {
                return this.fileInfo.FullName;
            }

            return relativeName;
        }
    }
}
