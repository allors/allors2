using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Logger;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tooling.ProcessTasks;

partial class Api : IDisposable
{
    private IProcess Process { get; set; }

    public Api(AbsolutePath path)
    {
        var arguments = $@"{path}/Api.dll";
        var workingDirectory = path;

        Process = StartProcess(DotNetTasks.DotNetPath, arguments, workingDirectory);
    }

    public void Dispose()
    {
        Process?.Kill();
        Process?.Dispose();
        Process = null;
    }

    public async Task Ready()
    {
        if (!await Get("/Test/Ready", TimeSpan.FromMinutes(5)))
        {
            throw new Exception("Api is not ready");
        }
    }

    public async Task<bool> Get(string url, TimeSpan wait)
    {
        var stop = DateTime.Now.Add(wait);
        var run = 0;

        var success = false;
        while (!success && (DateTime.Now < stop))
        {
            await Task.Delay(1000);

            try
            {
                using (var client = new HttpClient())
                {
                    Normal($"Api request: ${url}");
                    var response = await client.GetAsync($"http://localhost:5000{url}");
                    success = response.IsSuccessStatusCode;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (!success)
                    {
                        Warn("Api response: Unsuccessful");
                        Warn(result);
                    }
                    else
                    {
                        Normal("Api response: Successful");
                        Normal(result);
                    }
                }
            }
            catch (Exception e)
            {
                Warn($"Api: Exception (run {++run})");
            }
        }

        return success;
    }
}
