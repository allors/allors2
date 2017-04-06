namespace Allors.Server
{
    public class SyncResponse
    {
        public string UserSecurityHash { get; set; }

        public SyncResponseObject[] Objects { get; set; }
    }
}