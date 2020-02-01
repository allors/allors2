using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tooling.ProcessTasks;
using AbsolutePath = Nuke.Common.IO.PathConstruction.AbsolutePath;

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

    static void TaskKill()
    {
        void TaskKill(string imageName)
        {
            try
            {
                StartProcess("taskkill", $"/IM {imageName} /F /T /FI \"PID ge 0\"").WaitForExit();
            }
            catch
            {
            }
        }

        TaskKill("node.exe");
        TaskKill("chrome.exe");
        TaskKill("chromedriver.exe");
    }

    protected override void OnBuildInitialized()
    {
        base.OnBuildInitialized();
        TaskKill();
    }

    protected override void OnBuildFinished()
    {
        base.OnBuildFinished();
        TaskKill();
    }

    Target ResetDatabase => _ => _
        .DependsOn(AdaptersResetDatabase)
        .DependsOn(CoreResetDatabase)
        .DependsOn(BaseResetDatabase);

    Target Clean => _ => _
        .Executes(() =>
        {
            void Delete(DirectoryInfo directoryInfo)
            {
                directoryInfo.Refresh();
                if (directoryInfo.Exists)
                {
                    if (new[] { "node_modules", "packages", "out-tsc", "bin", "obj", "generated" }.Contains(directoryInfo.Name.ToLowerInvariant()))
                    {
                        DeleteDirectory(directoryInfo.FullName);
                        return;
                    }

                    if (!directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        foreach (var child in directoryInfo.GetDirectories())
                        {
                            Delete(child);
                        }
                    }
                }
            }

            foreach (var path in new AbsolutePath[] { Paths.Platform, Paths.Core, Paths.Base })
            {
                foreach (var child in new DirectoryInfo(path).GetDirectories().Where(v => !v.Name.Equals("build")))
                {
                    Delete(child);
                }
            }

            DeleteDirectory(Paths.Artifacts);
        });

    Target EnsureDirectories => _ => _
        .Executes(() =>
        {
            EnsureExistingDirectory(Paths.ArtifactsTests);
        });

    Target Default => _ => _
        .DependsOn(AdaptersGenerate)
        .DependsOn(CoreWorkspaceAutotest)
        .DependsOn(BaseWorkspaceAutotest);
}
