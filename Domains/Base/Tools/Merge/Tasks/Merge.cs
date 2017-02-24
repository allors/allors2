// ------------------------------------------------------------------------------------------------- 
// <copyright file="Merge.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// -------------------------------------------------------------------------------------------------
namespace Allors.R1.Development.Resources.Tasks
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Framework;

    public class Merge : ITask
    {
        private ITaskItem[] inpuDirectories;
        private ITaskItem outputDirectory;

        public IBuildEngine BuildEngine { get; set; }

        public ITaskHost HostObject { get; set; }

        [Required]
        public ITaskItem[] InputDirectories
        {
            get
            {
                return this.inpuDirectories;
            }

            set
            {
                this.inpuDirectories = value;
            }
        }

        [Required]
        public ITaskItem OutputDirectory
        {
            get
            {
                return this.outputDirectory;
            }

            set
            {
                this.outputDirectory = value;
            }
        }

        public bool Execute()
        {
            try
            {
                var inputDirectories = this.InputDirectories.Select(taskItem => new DirectoryInfo(taskItem.ItemSpec)).ToArray();
                var outputDirectories = new DirectoryInfo(this.outputDirectory.ItemSpec);
                var resources = new Resources(inputDirectories, outputDirectories);
                resources.Merge();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
            }
        }
    }
}