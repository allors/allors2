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
using static Nuke.Common.Tools.DotNet.DotNetTasks;

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

    public async Task Init()
    {
        if (!await Get("/Test/Setup", TimeSpan.FromMinutes(10)))
        {
            throw new Exception("Could not initialize server");
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
                    Normal($"Server request: ${url}");
                    var response = await client.GetAsync($"http://localhost:5000{url}");
                    success = response.IsSuccessStatusCode;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (!success)
                    {
                        Warn("Server response: Unsuccessful");
                        Warn(result);
                    }
                    else
                    {
                        Warn("Server response: Successful");
                        Normal(result);
                    }
                }
            }
            catch (Exception e)
            {
                Warn($"Server: Exception (run {++run})");
            }
        }

        return success;
    }
}
