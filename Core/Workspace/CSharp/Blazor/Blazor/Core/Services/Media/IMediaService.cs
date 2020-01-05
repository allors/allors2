using Allors.Workspace.Domain;

namespace Allors.Services
{
    public partial interface IMediaService
    {
        string Source(Media media, MediaOptions options);
    }
}
