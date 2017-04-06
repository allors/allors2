namespace Allors.Server
{
    public class SyncResponseObject
    {
        /// <summary>
        /// The id.
        /// </summary>
        public string I { get; set; }

        /// <summary>
        /// The version.
        /// </summary>
        public string V { get; set; }

        /// <summary>
        /// The object type.
        /// </summary>
        public string T { get; set; }

        public object[][] Roles { get; set; }

        public object[][] Methods { get; set; }

        public override string ToString()
        {
            return $"{this.T} [{this.I}:{this.V}]";
        }
    }
}