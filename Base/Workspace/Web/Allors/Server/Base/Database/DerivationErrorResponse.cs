namespace Allors.Web.Database
{
    public class DerivationErrorResponse
    {
        /// <summary>
        /// The error message.
        /// </summary>
        public string M { get; set; }

        /// <summary>
        /// The roles.
        /// </summary>
        public string[][] R { get; set; }
    }
}