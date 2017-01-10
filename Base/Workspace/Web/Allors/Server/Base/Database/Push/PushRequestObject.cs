using System.Collections.Generic;

namespace Allors.Web.Database
{
    public class PushRequestObject
    {
        /// <summary>
        /// The id.
        /// </summary>
        public string I { get; set; }

        /// <summary>
        /// The version.
        /// </summary>
        public string V { get; set; }

        public IList<PushRequestRole> Roles { get; set; }
    }
}