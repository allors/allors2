namespace Allors.Server
{
    public class QueryResponse
    {
        public string UserSecurityHash { get; set; }

        public QueryResponseObject[] Objects { get; set; }
    }
}