// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
//
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
//
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Development.Repository.Storage
{
    using System;
    using System.IO;
    using System.Threading;

    public class Location
    {
        private const int RetryCount = 10;

        public Location(DirectoryInfo directoryInfo)
        {
            this.DirectoryInfo = directoryInfo;
        }

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