// -------------------------------------------------------------------------------------------------
// <copyright file="Generate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// -------------------------------------------------------------------------------------------------
namespace Allors.Development.Repository.Tasks
{
    using System.IO;

    using Allors.Development.Repository.Generation;
    using Allors.Meta;

    public static class Generate
    {
        public static Log Execute(string template, string output)
        {
            var log = new GenerateLog();

            var templateFileInfo = new FileInfo(template);
            var stringTemplate = new StringTemplate(templateFileInfo);
            var outputDirectoryInfo = new DirectoryInfo(output);

            stringTemplate.Generate(MetaPopulation.Instance, outputDirectoryInfo, log);

            return log;
        }
    }
}
