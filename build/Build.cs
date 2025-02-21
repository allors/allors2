using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tooling.ProcessTasks;

[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Install => _ => _
        .DependsOn(CoreInstall);

    Target ResetDatabase => _ => _
        .DependsOn(AdaptersResetDatabase)
        .DependsOn(CoreResetDatabase);

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
                        ((AbsolutePath)directoryInfo.FullName).DeleteDirectory();
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

            foreach (var path in new AbsolutePath[] { Paths.Platform, Paths.Core })
            {
                foreach (var child in new DirectoryInfo(path).GetDirectories().Where(v => !v.Name.Equals("build")))
                {
                    Delete(child);
                }
            }

            Paths.Artifacts.DeleteDirectory();
        });

    Target Generate => _ => _
        .DependsOn(this.AdaptersGenerate)
        .DependsOn(this.CoreGenerate);

    Target Default => _ => _
        .DependsOn(this.Generate);

    Target All => _ => _
        .DependsOn(this.Install)
        .DependsOn(this.Generate);
}
