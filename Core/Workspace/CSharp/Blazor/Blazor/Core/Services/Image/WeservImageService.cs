namespace Allors.Services
{
    using System.Collections.Generic;
    using Allors.Workspace.Domain;
    using Microsoft.AspNetCore.Components;

    public partial class WeservImageService : IImageService
    {
        public WeservImageService(NavigationManager navigationManager) => this.NavigationManager = navigationManager;

        public NavigationManager NavigationManager { get; }

        public string BaseUri => this.NavigationManager.BaseUri;

        public string Source(Media media, int? width = null, int? quality = null, string type = null, string background = "FFF")
        {
            var parameters = new List<string>();
            if (width != null)
            {
                parameters.Add($"w={width}");
            }

            if (quality != null)
            {
                parameters.Add($"q={quality}");
            }

            if (type != null)
            {
                parameters.Add($"output={type}");
            }

            if ("png".Equals(type) || (string.IsNullOrWhiteSpace(type) && "image/png".Equals(media.Type)))
            {
                parameters.Add($"af");
            }

            if (!"png".Equals(type) && "image/png".Equals(media.Type))
            {
                parameters.Add($"bg={background}");
            }

            parameters.Add($"we");

            return $"//images.weserv.nl/?url={this.BaseUri}Image/{media.UniqueId}/{media.Revision}?{string.Join("&", parameters)}";
        }
    }
}
