namespace Tests.Remote
{
    using System.Diagnostics;
    using System.Threading;
    using NUnit.Framework;

    using Process = System.Diagnostics.Process;

    [SetUpFixture]
    public class Fixture
    {
        public const string Port = "64739";

        private Process iisProcess;

        [SetUp]
        public void SetUp()
        {
            this.SetupIISExpress();
        }

        [TearDown]
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

            var programfiles = string.IsNullOrEmpty(startInfo.EnvironmentVariables["programfiles"])
                                ? startInfo.EnvironmentVariables["programfiles(x86)"]
                                : startInfo.EnvironmentVariables["programfiles"];

            startInfo.FileName = programfiles + "\\IIS Express\\iisexpress.exe";

            this.iisProcess = new Process { StartInfo = startInfo };

            var result = this.iisProcess.Start();
        }

        private void TearDownIISExpress()
        {
            try
            {
                this.iisProcess.CloseMainWindow();
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