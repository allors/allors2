// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsDirectoryInfo.cs" company="Allors bvba">
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

namespace Allors.Tools.Repository.Storage
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class AllorsDirectoryInfo
    {
        private readonly DirectoryInfo directoryInfo;
        private readonly AllorsDirectoryInfo parent;

        public AllorsDirectoryInfo(DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
            if (directoryInfo.Parent != null)
            {
                this.parent = new AllorsDirectoryInfo(directoryInfo.Parent);
            }
        }

        internal DirectoryInfo DirectoryInfo
        {
            get { return this.directoryInfo; }
        }

        internal AllorsDirectoryInfo Parent
        {
            get { return this.parent; }
        }

        public string GetRelativeName(DirectoryInfo baseDirectoryInfo)
        {
            return this.GetRelativeName(new AllorsDirectoryInfo(baseDirectoryInfo));
        }

        public string GetRelativeOrFullName(DirectoryInfo baseDirectoryInfo)
        {
            string relativeName = this.GetRelativeName(baseDirectoryInfo);
            if (relativeName == null)
            {
                return this.directoryInfo.FullName;
            }
            return relativeName;
        }

        public override string ToString()
        {
            return this.directoryInfo.FullName;
        }

        private void BuildAncestors(AllorsDirectoryInfo root, List<AllorsDirectoryInfo> ancestors)
        {
            if (!this.directoryInfo.FullName.Equals(this.directoryInfo.Root.FullName))
            {
                if (!this.directoryInfo.FullName.Equals(root.directoryInfo.FullName))
                {
                    ancestors.Add(this);
                    this.parent.BuildAncestors(root, ancestors);
                }
            }
        }

        private AllorsDirectoryInfo GetCommonAncestor(AllorsDirectoryInfo destination)
        {
            if (destination.IsAncestor(this))
            {
                return this;
            }

            return this.parent.GetCommonAncestor(destination);
        }

        private bool IsAncestor(AllorsDirectoryInfo ancestor)
        {
            if (this.directoryInfo.FullName.Equals(ancestor.directoryInfo.FullName))
            {
                return true;
            }

            if (this.parent != null)
            {
                return this.parent.IsAncestor(ancestor);
            }

            return false;
        }

        internal string GetRelativeName(AllorsDirectoryInfo baseDirectory)
        {
            if (this.directoryInfo.Root.FullName.Equals(baseDirectory.directoryInfo.Root.FullName))
            {
                AllorsDirectoryInfo commonAncestor = this.GetCommonAncestor(baseDirectory);

                var ancestors = new List<AllorsDirectoryInfo>();
                this.BuildAncestors(commonAncestor, ancestors);

                var baseAncestors = new List<AllorsDirectoryInfo>();
                baseDirectory.BuildAncestors(commonAncestor, baseAncestors);

                var relativePath = new StringBuilder();
                foreach (AllorsDirectoryInfo baseAncestor in baseAncestors)
                {
                    if (relativePath.Length > 0)
                    {
                        relativePath.Append(Path.DirectorySeparatorChar);
                    }

                    relativePath.Append("..");
                }

                for (int i = ancestors.Count - 1; i >= 0; --i)
                {
                    AllorsDirectoryInfo ancestor = ancestors[i];
                    if (relativePath.Length > 0)
                    {
                        relativePath.Append(Path.DirectorySeparatorChar);
                    }

                    relativePath.Append(ancestor.directoryInfo.Name);
                }

                return relativePath.ToString();
            }

            return null;
        }
    }
}