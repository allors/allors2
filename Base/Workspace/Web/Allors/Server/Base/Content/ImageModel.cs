namespace Allors.Web.Content
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class ImageModel
    {
        public string UniqueId { get; set; }

        public string FileName { get; set; }
        
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}