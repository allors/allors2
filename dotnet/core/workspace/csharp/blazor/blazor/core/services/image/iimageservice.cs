namespace Allors.Services
{
    using Allors.Workspace.Domain;

    public partial interface IImageService
    {
        string Source(Media media, int? width = null, int? quality = null, string type = null, string background = null);
    }
}
