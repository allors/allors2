// -------------------------------------------------------------------------------------------------
// <copyright file="Generate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Allors.Repository.Roslyn
{
    using System.IO;
    using System.Linq;
    using Buildalyzer;
    using Buildalyzer.Workspaces;
    using Domain;
    using Generation;

    public class Generate
    {
        public static void Execute(string projectPath, string template, string output)
        {
            var log = new StringWriter();
            var analyzerManager = new AnalyzerManager(
                new AnalyzerManagerOptions
                {
                    LogWriter = log,
                });

            var projectAnalyzer = analyzerManager.GetProject(projectPath);
            var workspace = projectAnalyzer.GetWorkspace();
            var solution = workspace.CurrentSolution;
            var project = solution.Projects.First();
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
