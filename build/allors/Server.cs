using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Serilog.Log;
using static Nuke.Common.Tooling.ProcessTasks;

partial class Server : IDisposable
{
    private IProcess Process { get; set; }

    public Server(AbsolutePath path)
    {
        var arguments = $@"{path}/Server.dll";
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
        if (!await Get("/allors/Test/Ready", TimeSpan.FromMinutes(5)))
        {
            throw new Exception("Server is not ready");
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
                    Information($"Server request: ${url}");
                    var response = await client.GetAsync($"http://localhost:5000{url}");
                    success = response.IsSuccessStatusCode;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (!success)
                    {
                        Error("Server response: Unsuccessful");
                        Error(result);
                    }
                    else
                    {
                        Information("Server response: Successful");
                        Information(result);
                    }
                }
            }
            catch
            {
                Error($"Server: Exception (run {++run})");
            }
        }

        return success;
    }
}
