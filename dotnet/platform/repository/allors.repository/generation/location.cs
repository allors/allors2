// <copyright file="Location.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository.Generation
{
    using System;
    using System.IO;
    using System.Threading;

    public class Location
    {
        private const int RetryCount = 10;

        public Location(DirectoryInfo directoryInfo) => this.DirectoryInfo = directoryInfo;

        public DirectoryInfo DirectoryInfo { get; }

        public void Save(string fileName, string fileContents)
        {
            if (!this.DirectoryInfo.Exists)
            {
                this.DirectoryInfo.Create();
            }

            var fileInfo = new FileInfo(this.DirectoryInfo.FullName + Path.DirectorySeparatorChar + fileName.Replace('\\', Path.DirectorySeparatorChar));
            Save(fileInfo, fileContents);
        }

        private static void Save(FileInfo fileInfo, string fileContents)
        {
            for (var i = 0; i < RetryCount; i++)
            {
                try
                {
                    File.WriteAllText(fileInfo.FullName, fileContents);
                    fileInfo.CreationTime = DateTime.UtcNow;
                    return;
                }
                catch
                {
                    Thread.Sleep(100 * i);
                    fileInfo.Refresh();
                }
            }

            throw new Exception("Could not save " + fileInfo.FullName);
        }
    }
}
