using System.Collections.Generic;

namespace Allors.Web.Database
{
    public class PushRequestNewObject
    {
        /// <summary>
        /// The new id.
        /// </summary>
        public string NI { get; set; }

        /// <summary>
        /// The object type.
        /// </summary>
        public string T { get; set; }

        public IList<PushRequestRole> Roles { get; set; }
    }
}