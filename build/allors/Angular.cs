using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Logger;

internal class Angular : IDisposable
{
    public Angular(AbsolutePath path, string command)
    {
        var npmRunSetting = new NpmRunSettings()
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(path)
            .SetCommand(command);

        Process = ProcessTasks.StartProcess(npmRunSetting);
    }

    private IProcess Process { get; set; }

    public void Dispose()
    {
        Process?.Kill();
        Process?.Dispose();
        Process = null;
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
        var run = 0;

        var success = false;
        while (!success && DateTime.Now < stop)
        {
            await Task.Delay(1000);

            try
            {
                using (var client = new HttpClient())
                {
                    Normal($"Angular request: ${url}");
                    var response = await client.GetAsync($"http://localhost:4200{url}");
                    success = response.IsSuccessStatusCode;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (!success)
                    {
                        Warn("Angular response: Unsuccessful");
                        Warn(result);
                    }
                    else
                    {
                        Normal("Angular response: Successful");
                        Normal(result);
                    }
                }
            }
            catch
            {
                Warn($"Angular: Exception (run {++run})");
            }
        }

        return success;
    }
}
