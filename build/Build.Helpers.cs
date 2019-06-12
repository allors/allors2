using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Logger;

partial class Build
{
    public async Task<bool> ServerGet(string url, TimeSpan wait)
    {
        var stop = DateTime.Now.Add(wait);

        using (var client = new HttpClient())
        {
            var success = false;
            while (!success && DateTime.Now < stop)
            {
                try
                {
                    var response = await client.GetAsync(url);
                    success = response.IsSuccessStatusCode;
                }
                catch { }

                await Task.Delay(100);
            }

            return success;
        }
    }


    async Task<IProcess> BaseInitServer()
    {
        IProcess process = null;
        try
        {
            var dotNetRunSettings = new DotNetRunSettings()
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseDatabaseServer);

            process = ProcessTasks.StartProcess(dotNetRunSettings);
            if (!await ServerGet("http://localhost:5000/Test/Init", TimeSpan.FromMinutes(5)))
            {
                throw new Exception("Could not initialize server");
            }

            return process;
        }
        catch
        {
            process?.Kill();
            throw;
        }
    }
}
