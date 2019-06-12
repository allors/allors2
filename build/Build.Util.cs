using System;
using System.Net.Http;
using System.Threading.Tasks;
using static Nuke.Common.Logger;

partial class Build
{
    public async Task<bool> WaitForServer(string url, TimeSpan wait)
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
                catch {}

                await Task.Delay(100);
            }

            return success;
        }
    }
}
