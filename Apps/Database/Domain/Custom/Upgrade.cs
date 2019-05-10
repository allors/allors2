namespace Allors
{
    using System;
    using System.IO;

    public class Upgrade
    {
        private readonly ISession session;

        private DirectoryInfo DataPath;

        public Upgrade(ISession session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.DataPath = dataPath;
        }

        public void Execute()
        {
        }
    }
}