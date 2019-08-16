namespace Allors
{
    using System;
    using System.IO;

    public class Upgrade
    {
        private readonly ISession session;

        private readonly DirectoryInfo dataPath;

        public Upgrade(ISession session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.dataPath = dataPath;
        }

        public void Execute()
        {
        }
    }
}
