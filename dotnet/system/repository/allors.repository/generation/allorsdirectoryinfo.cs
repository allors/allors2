// <copyright file="AllorsDirectoryInfo.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository.Generation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class AllorsDirectoryInfo
    {
        public AllorsDirectoryInfo(DirectoryInfo directoryInfo)
        {
            this.DirectoryInfo = directoryInfo;
            if (directoryInfo.Parent != null)
            {
                this.Parent = new AllorsDirectoryInfo(directoryInfo.Parent);
            }
        }

        internal DirectoryInfo DirectoryInfo { get; }

        internal AllorsDirectoryInfo Parent { get; }

        public string GetRelativeName(DirectoryInfo baseDirectoryInfo) => this.GetRelativeName(new AllorsDirectoryInfo(baseDirectoryInfo));

        public string GetRelativeOrFullName(DirectoryInfo baseDirectoryInfo)
        {
            var relativeName = this.GetRelativeName(baseDirectoryInfo);
            if (relativeName == null)
            {
                return this.DirectoryInfo.FullName;
            }

            return relativeName;
        }

        public override string ToString() => this.DirectoryInfo.FullName;

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

                for (var i = ancestors.Count - 1; i >= 0; --i)
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

        private void BuildAncestors(AllorsDirectoryInfo root, List<AllorsDirectoryInfo> ancestors)
        {
            if (!this.DirectoryInfo.FullName.Equals(this.DirectoryInfo.Root.FullName) && !this.DirectoryInfo.FullName.Equals(root.DirectoryInfo.FullName))
            {
                ancestors.Add(this);
                this.Parent.BuildAncestors(root, ancestors);
            }
        }

        private AllorsDirectoryInfo GetCommonAncestor(AllorsDirectoryInfo destination)
        {
            if (destination.IsAncestor(this))
            {
                return this;
            }

            return this.Parent.GetCommonAncestor(destination);
        }

        private bool IsAncestor(AllorsDirectoryInfo ancestor)
        {
            if (this.DirectoryInfo.FullName.Equals(ancestor.DirectoryInfo.FullName))
            {
                return true;
            }

            if (this.Parent != null)
            {
                return this.Parent.IsAncestor(ancestor);
            }

            return false;
        }
    }
}
