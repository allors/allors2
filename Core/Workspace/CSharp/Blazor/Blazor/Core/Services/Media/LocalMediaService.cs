namespace Allors.Services
{
    using Allors.Workspace.Domain;

    public partial class LocalMediaService : IMediaService
    {
        public string Source(Media media, MediaOptions options) => "/Media/DownloadWithRevision/" + media.UniqueId + "?revision=" + media.Revision;
    }
}
