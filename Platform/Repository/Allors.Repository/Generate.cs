// -------------------------------------------------------------------------------------------------
// <copyright file="Generate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Allors.Repository.Roslyn
{
    using System.IO;

    using Allors.Repository;
    using Allors.Repository.Domain;
    using Allors.Repository.Generation;

    using Microsoft.Build.Locator;
    using Microsoft.CodeAnalysis.MSBuild;

    public class Generate
    {
        public static void Execute(string projectPath, string template, string output)
        {
            if (!MSBuildLocator.IsRegistered)
            {
                MSBuildLocator.RegisterDefaults();
            }

            using var workspace = MSBuildWorkspace.Create();
            var project = workspace.OpenProjectAsync(projectPath).GetAwaiter().GetResult();
            var repository = new Repository(project);

            if (repository.HasErrors)
            {
                throw new RepositoryException("Repository has errors.");
            }

            var templateFileInfo = new FileInfo(template);
            var stringTemplate = new StringTemplate(templateFileInfo);
            var outputDirectoryInfo = new DirectoryInfo(output);

            stringTemplate.Generate(repository, outputDirectoryInfo);
        }
    }
}
