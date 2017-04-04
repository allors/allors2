namespace Tests.Remote
{
    using System.Diagnostics;
    using System.Threading;
    using Xunit;

    using Process = System.Diagnostics.Process;
    
    public class Fixture
    {
        public const string Port = "64739";

        private Process iisProcess;

        public void SetUp()
        {
            this.SetupIISExpress();
        }

        public void TearDown()
        {
            this.TearDownIISExpress();
        }

        private void SetupIISExpress()
        {
            this.KillIISExpress();

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                Arguments = $"/path:\"{Test.AppLocation.FullName}\" /port:{Port}"
            };

            var programfiles = string.IsNullOrEmpty(startInfo.Environment["programfiles"])
                                ? startInfo.Environment["programfiles(x86)"]
                                : startInfo.Environment["programfiles"];

            startInfo.FileName = programfiles + "\\IIS Express\\iisexpress.exe";

            this.iisProcess = new Process { StartInfo = startInfo };

            this.iisProcess.Start();
        }

        private void TearDownIISExpress()
        {
            try
            {
                this.iisProcess.Kill();
            }
            finally
            {
                try
                {
                    this.iisProcess.Dispose();
                }
                finally
                {
                    this.KillIISExpress();
                }
            }

        }

        private void KillIISExpress()
        {
            Process.Start("taskkill", "/F /IM iisexpress.exe");

            // Make sure process is killed
            Thread.Sleep(1000);
        }
    }
}