// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.IO;
    using System.Linq;

    using Allors.R1.Development.Resources;

    class Program
    {
        static int Main(string[] args)
        {
            var directoryInfos = args.Select(v => new DirectoryInfo(v)).ToArray();

            var inputDirectories = directoryInfos.Take(directoryInfos.Length - 1).ToArray();
            var outputDirectory = directoryInfos.Last();

            try
            {
                var merger = new Merger();
                merger.Input(inputDirectories);
                merger.Output(outputDirectory);
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }
    }
}
