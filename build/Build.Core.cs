using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build 
{
    Target CoreGenerate => _ => _
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.CoreRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.CoreDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Core)
                .SetProjectFile(Paths.CoreDatabaseGenerate));
        });

}
