using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tooling.ProcessTasks;
using static Nuke.Common.IO.FileSystemTasks;

public partial class Build
{
    [Parameter("DotNet Verbosity")] private readonly DotNetVerbosity DotNetVerbosity = DotNetVerbosity.Quiet;

    //[Solution] private readonly Solution Solution;
    //[GitRepository] private readonly GitRepository GitRepository;
    //[GitVersion] private readonly GitVersion GitVersion;

    private readonly Paths Paths = new Paths(RootDirectory);

    public Target EnsureDirectories => _ => _
        .Executes(() => EnsureExistingDirectory(Paths.ArtifactsTests));

    public static int Main() => Execute<Build>(x => x.Default);

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
}
