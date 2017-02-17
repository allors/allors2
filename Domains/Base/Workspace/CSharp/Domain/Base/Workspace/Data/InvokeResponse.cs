namespace Allors.Workspace.Data
{
    public class InvokeResponse : ErrorResponse
    {
        public bool hasErrors { get; set; }

        public string errorMessage { get; set; }

        public string[] versionErrors { get; set; }

        public string[] accessErrors { get; set; }

        public string[] missingErrors { get; set; }

        public PullResponseDerivationError[] derivationErrors { get; set; }
    }
}