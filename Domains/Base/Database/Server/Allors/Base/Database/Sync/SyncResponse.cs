namespace Allors.Web.Database
{
    public class SyncResponse
    {
        public string UserSecurityHash { get; set; }

        public SyncResponseObject[] Objects { get; set; }
    }
}