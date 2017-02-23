namespace Allors.Web.Database
{
    using System.Collections.Generic;

    public class PullResponse
    {
        public string UserSecurityHash { get; set; }

        public string[][] Objects { get; set; }

        public Dictionary<string, string> NamedObjects { get; set; }

        public Dictionary<string, string[]> NamedCollections { get; set; }

        public Dictionary<string, object> NamedValues { get; set; }
    }
}