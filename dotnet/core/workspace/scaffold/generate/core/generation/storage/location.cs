// <copyright file="Location.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Allors.Development.Repository.Storage
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
            Exception exception = null;

            for (var i = 0; i < RetryCount; i++)
            {
                try
                {
                    File.WriteAllText(fileInfo.FullName, fileContents);
                    fileInfo.CreationTime = DateTime.UtcNow;
                    return;
                }
                catch (Exception e)
                {
                    exception = e;
                    Thread.Sleep(100 * i);
                    fileInfo.Refresh();
                }
            }

            throw exception;
        }
    }
}
