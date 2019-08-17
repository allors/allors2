// <copyright file="AllorsDirectoryInfo.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Development.Repository.Storage
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class AllorsDirectoryInfo
    {
        private readonly AllorsDirectoryInfo parent;

        public AllorsDirectoryInfo(DirectoryInfo directoryInfo)
        {
            this.DirectoryInfo = directoryInfo;
            if (directoryInfo.Parent != null)
            {
                this.parent = new AllorsDirectoryInfo(directoryInfo.Parent);
            }
        }

        internal DirectoryInfo DirectoryInfo { get; private set; }

        internal AllorsDirectoryInfo Parent => this.parent;

        public string GetRelativeName(DirectoryInfo baseDirectoryInfo) => this.GetRelativeName(new AllorsDirectoryInfo(baseDirectoryInfo));

        public string GetRelativeOrFullName(DirectoryInfo baseDirectoryInfo)
        {
            var relativeName = this.GetRelativeName(baseDirectoryInfo);
            return relativeName ?? this.DirectoryInfo.FullName;
        }

        public override string ToString() => this.DirectoryInfo.FullName;

        private void BuildAncestors(AllorsDirectoryInfo root, List<AllorsDirectoryInfo> ancestors)
        {
            if (!this.DirectoryInfo.FullName.Equals(this.DirectoryInfo.Root.FullName))
            {
                if (!this.DirectoryInfo.FullName.Equals(root.DirectoryInfo.FullName))
                {
                    ancestors.Add(this);
                    this.parent.BuildAncestors(root, ancestors);
                }
            }
        }

        private AllorsDirectoryInfo GetCommonAncestor(AllorsDirectoryInfo destination) => destination.IsAncestor(this) ? this : this.parent.GetCommonAncestor(destination);

        private bool IsAncestor(AllorsDirectoryInfo ancestor)
        {
            if (this.DirectoryInfo.FullName.Equals(ancestor.DirectoryInfo.FullName))
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
            if (this.DirectoryInfo.Root.FullName.Equals(baseDirectory.DirectoryInfo.Root.FullName))
            {
                var commonAncestor = this.GetCommonAncestor(baseDirectory);

                var ancestors = new List<AllorsDirectoryInfo>();
                this.BuildAncestors(commonAncestor, ancestors);

                var baseAncestors = new List<AllorsDirectoryInfo>();
                baseDirectory.BuildAncestors(commonAncestor, baseAncestors);

                var relativePath = new StringBuilder();
                foreach (var baseAncestor in baseAncestors)
                {
                    if (relativePath.Length > 0)
                    {
                        relativePath.Append(Path.DirectorySeparatorChar);
                    }

                    relativePath.Append("..");
                }

                for (int i = ancestors.Count - 1; i >= 0; --i)
                {
                    var ancestor = ancestors[i];
                    if (relativePath.Length > 0)
                    {
                        relativePath.Append(Path.DirectorySeparatorChar);
                    }

                    relativePath.Append(ancestor.DirectoryInfo.Name);
                }

                return relativePath.ToString();
            }

            return null;
        }
    }
}
