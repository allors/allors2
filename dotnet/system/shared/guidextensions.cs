namespace Allors
{
    using System;

    public static class GuidExtensions
    {
        /// <summary>
        /// Converts to a url friendly base64 encoded string
        /// </summary>
        /// <param name="this"></param>
        /// <returns>tag</returns>
        public static string Tag(this Guid @this) =>
            Convert.ToBase64String(@this.ToByteArray())
                .Substring(0, 22)
                .Replace("/", "_")
                .Replace("+", "-");
    }
}
