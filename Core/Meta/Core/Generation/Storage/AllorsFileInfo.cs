// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsFileInfo.cs" company="Allors bvba">
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
    using System.IO;

    public class AllorsFileInfo
    {
        private readonly FileInfo fileInfo;

        public AllorsFileInfo(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public string GetRelativeName(DirectoryInfo baseDirectoryInfo)
        {
            var baseDirectory = new AllorsDirectoryInfo(baseDirectoryInfo);
            var directory = new AllorsDirectoryInfo(fileInfo.Directory);

            string relativePath = directory.GetRelativeName(baseDirectory);
            if (relativePath == null)
            {
                return null;
            }

            return Path.Combine(relativePath, this.fileInfo.Name);
        }

        public string GetRelativeOrFullName(DirectoryInfo baseDirectoryInfo)
        {
            string relativeName = GetRelativeName(baseDirectoryInfo);
            if (relativeName == null)
            {
                return fileInfo.FullName;
            }
            return relativeName;
        }
    }
}