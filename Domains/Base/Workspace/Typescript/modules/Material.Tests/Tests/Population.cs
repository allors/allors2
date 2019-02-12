namespace Tests
{
    using System.IO;

    using Allors;

    public class Population
    {
        private readonly ISession session;

        private DirectoryInfo dataPath;

        public Population(ISession session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.dataPath = dataPath;
        }

        public void Execute()
        {
            this.session.Derive();
        }
    }
}