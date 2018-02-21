namespace Allors.Excel
{
    using Allors.Workspace;
    using Allors.Workspace.Client;

    public partial class Client
    {
        public Client(Database database, Workspace workspace)
        {
            this.Database = database;
            this.Workspace = workspace;
        }

        public Database Database { get; }

        public Workspace Workspace { get; }
    }
}
