namespace Allors.Services
{
    using Allors.Workspace.Domain;

    public partial interface IMediaService
    {
        string Source(Media media, int? width);
    }
}
