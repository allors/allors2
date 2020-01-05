namespace Allors.Services
{
    using Allors.Workspace.Domain;
    using Microsoft.AspNetCore.Components;

    public partial class WeservMediaService : IMediaService
    {
        public WeservMediaService(NavigationManager navigationManager) => this.NavigationManager = navigationManager;

        public NavigationManager NavigationManager { get; }

        public string BaseUri => this.NavigationManager.BaseUri;

        //public string BaseUri => "https://aviaco-internet.inxin.net/";

        public string Source(Media media, MediaOptions options)
        {
            return $"//images.weserv.nl/?url={this.BaseUri}Media/DownloadWithRevision/{media.UniqueId}?revision={media.Revision}";
        }
    }
}
