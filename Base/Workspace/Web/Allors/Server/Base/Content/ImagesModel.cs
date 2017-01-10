namespace Allors.Web.Content
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class ImagesModel
    {
        public ImagesItemModel[] Items { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}