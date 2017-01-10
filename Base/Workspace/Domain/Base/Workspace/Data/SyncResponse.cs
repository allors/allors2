namespace Allors.Workspace.Data
{
    public class SyncResponseObject
    {
        public string i { get; set; }

        public string v { get; set; }
        public string t { get; set; }

        public object[][] roles { get; set; }
        public string[][] methods { get; set; }
    }

    public class SyncResponse {
        public string userSecurityHash { get; set; }

        public SyncResponseObject[] objects { get; set; }
    }
}