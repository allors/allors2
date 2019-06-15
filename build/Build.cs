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

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Default);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    public readonly Paths Paths = new Paths(RootDirectory);

    Target Clean => _ => _
        .Executes(() =>
        {
            void Delete(DirectoryInfo v)
            {
                if (new[] { "node_modules", "packages", "bin", "obj", "generated" }.Contains(v.Name.ToLowerInvariant()))
                {
                    DeleteDirectory(v.FullName);
                    return;
                }

                foreach (var child in v.GetDirectories())
                {
                    Delete(child);
                }
            }

            foreach (var child in new DirectoryInfo(Paths.Platform).GetDirectories().Where(v => !v.Name.Equals("build")))
            {
                Delete(child);
            }

            EnsureCleanDirectory(Paths.Artifacts);
        });

    Target EnsureDirectories => _ => _
        .Executes(() =>
        {
            EnsureExistingDirectory(Paths.ArtifactsTests);
        });
    
    Target Default => _ => _
        .DependsOn(BaseWorkspaceAutotest)
        .DependsOn(AppsWorkspaceAutotest);
}
