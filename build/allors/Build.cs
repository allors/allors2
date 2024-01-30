using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.Tooling.ProcessTasks;
using static Nuke.Common.IO.FileSystemTasks;

public partial class Build
{
    [Parameter("DotNet Verbosity")]
    private readonly DotNetVerbosity DotNetVerbosity = DotNetVerbosity.quiet;

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
