namespace Allors.Services
{
    using Allors.Workspace.Domain;
    using Microsoft.AspNetCore.Components;

    public partial class WeservMediaService : IMediaService
    {
        public WeservMediaService(NavigationManager navigationManager) => this.NavigationManager = navigationManager;

        public NavigationManager NavigationManager { get; }

        public string BaseUri => this.NavigationManager.BaseUri;

        public string Source(Media media, int? width) => $"//images.weserv.nl/?url={this.BaseUri}Image/{media.UniqueId}/{media.Revision}{(width != null ? $"?w={width}" : string.Empty)}";
    }
}
