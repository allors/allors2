// <copyright file="AllorsFileInfo.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Allors.Development.Repository.Storage
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
            return relativePath == null ? null : Path.Combine(relativePath, this.fileInfo.Name);
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
