namespace Allors.Server
{
    using Allors.Domain.Query;

    public class PullRequestPage
    {
        /// <summary>
        /// The RoleType.
        /// </summary>
        public int? S { get; set; }

        /// <summary>
        /// The TreeNodes
        /// </summary>
        public int? T { get; set; }

        public Page Parse()
        {
            if (this.S.HasValue && this.T.HasValue)
            {
                return new Page
                           {
                               Skip = this.S.Value,
                               Take = this.T.Value
                           };
            }

            return null;
        }
    }
}