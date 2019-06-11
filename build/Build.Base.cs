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
    Target BaseRestore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));

            foreach (var path in Paths.BaseWorkspaceTypescript)
            {
                NpmTasks.NpmInstall(s => s
                    .SetWorkingDirectory(path));
            }
        });

    Target BaseGenerate => _ => _
        .DependsOn(BaseRestore)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.BaseRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.BaseDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseDatabaseGenerate));

            foreach (var path in new[] { Paths.BaseWorkspaceTypescriptMaterial, Paths.BaseWorkspaceTypescriptAutotestAngular })
            {
                NpmTasks.NpmRun(s => s
                    .SetWorkingDirectory(path)
                    .SetCommand("autotest"));
            }

            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseWorkspaceTypescriptAutotestGenerateGenerate));
        });
}
