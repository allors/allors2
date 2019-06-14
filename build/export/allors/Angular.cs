using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.PathConstruction;

partial class Angular : IDisposable
{
    private IProcess Process { get; set; }

    public Angular(AbsolutePath path)
    {
        var npmRunSetting = new NpmRunSettings()
            .SetWorkingDirectory(path)
            .SetCommand("start");

        Process = ProcessTasks.StartProcess((ToolSettings)npmRunSetting);
    }

    public void Dispose()
    {
       Process?.Kill();
       Process?.Dispose();
       Process = null;

       // TODO: Only stop child processes
       System.Diagnostics.Process.Start("taskkill", "/F /IM node.exe").WaitForExit();
    }

    public async Task Init()
    {
        if (!await Get("/", TimeSpan.FromMinutes(10)))
        {
            throw new Exception("Could not initialize angular");
        }
    }

    public async Task<bool> Get(string url, TimeSpan wait)
    {
        var stop = DateTime.Now.Add(wait);

        using (var client = new HttpClient())
        {
            var success = false;
            while (!success && (DateTime.Now < stop))
            {
                try
                {
                    var response = await client.GetAsync($"http://localhost:4200{url}");
                    success = response.IsSuccessStatusCode;
                }
                catch { }

                await Task.Delay(100);
            }

            return success;
        }
    }
}
