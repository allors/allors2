namespace Allors.Workspace.Data
{
    using System.Collections.Generic;

    public class PullResponse
    {
        public string userSecurityHash { get; set; }

        public string[][] objects { get; set; }

        public Dictionary<string, string> namedObjects { get; set; }

        public Dictionary<string, string[]> namedCollections { get; set; }

        public Dictionary<string,object> namedValues { get; set; }
    }
}