using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Logger;
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

        var success = false;
        while (!success && (DateTime.Now < stop))
        {
            await Task.Delay(1000);

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"http://localhost:4200{url}");
                    success = response.IsSuccessStatusCode;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (!success)
                    {
                        Warn("Angular: Unsuccessful request");
                        Warn(result);
                    }
                    else
                    {
                        Normal(result);
                    }

                }
            }
            catch (Exception e)
            {
                Warn("Angular: Exception");
                Warn(e);
            }
        }

        return success;
    }
}
