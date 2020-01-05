namespace Allors.Services
{
    using Allors.Workspace.Domain;
    using Microsoft.AspNetCore.Components;

    public partial class WeservMediaService : IMediaService
    {
        public WeservMediaService(NavigationManager navigationManager) => this.NavigationManager = navigationManager;

        public NavigationManager NavigationManager { get; }

        public string Source(Media media, MediaOptions options) => $"//images.weserv.nl/?url={this.NavigationManager.BaseUri}Media/DownloadWithRevision/{media.UniqueId}?revision={media.Revision}";
    }
}
