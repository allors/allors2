namespace Allors.Server
{
    public class PullRequest
    {
        /// <summary>
        /// The queries.
        /// </summary>
        public PullRequestQuery[] Q { get; set; }

        /// <summary>
        /// The instantiations.
        /// </summary>
        public PullRequestFetch[] F { get; set; }
    }
}