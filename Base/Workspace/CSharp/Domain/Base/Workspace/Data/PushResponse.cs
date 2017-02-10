namespace Allors.Workspace.Data
{
    public class PushResponse : ErrorResponse
    {
        public bool hasErrors { get; set; }

        public string errorMessage { get; set; }

        public string[] versionErrors { get; set; }

        public string[] accessErrors { get; set; }

        public string[] missingErrors { get; set; }

        public PullResponseDerivationError[] derivationErrors { get; set; }

        public PushResponseNewObject[] newObjects { get; set; }
    }
}