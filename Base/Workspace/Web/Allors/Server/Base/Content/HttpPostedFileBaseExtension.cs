namespace Allors.Web.Content
{
    using System.IO;
    using System.Web;

    using Allors.Domain;

    public static class HttpPostedFileBaseExtension
    {
        public static byte[] GetContent(this HttpPostedFileBase @this)
        {
            using (var binaryReader = new BinaryReader(@this.InputStream))
            {
                return binaryReader.ReadBytes(@this.ContentLength);
            }
        }
    }
}