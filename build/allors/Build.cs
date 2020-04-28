using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.Tooling.ProcessTasks;
using static Nuke.Common.IO.FileSystemTasks;

public partial class Build
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("DotNet Verbosity")]
    private readonly DotNetVerbosity DotNetVerbosity = DotNetVerbosity.Quiet;

    [Solution] private readonly Solution Solution;
    [GitRepository] private readonly GitRepository GitRepository;
    [GitVersion] private readonly GitVersion GitVersion;

    private readonly Paths Paths = new Paths(RootDirectory);

    public static int Main() => Execute<Build>(x => x.Default);

    protected override void OnBuildInitialized()
    {
        base.OnBuildInitialized();
        this.TaskKill();
    }

    protected override void OnBuildFinished()
    {
        base.OnBuildFinished();
        this.TaskKill();
    }
       
    public void TaskKill()
    {
        static void TaskKill(string imageName)
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

    public Target EnsureDirectories => _ => _
       .Executes(() => EnsureExistingDirectory(this.Paths.ArtifactsTests));
}
