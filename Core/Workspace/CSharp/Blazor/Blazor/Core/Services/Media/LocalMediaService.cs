namespace Allors.Services
{
    using Allors.Workspace.Domain;

    public partial class LocalMediaService : IMediaService
    {
        public string Source(Media media, int? width) => $"/Image/{media.UniqueId}/{media.Revision}{(width != null ? $"?w={width}" : string.Empty)}";
    }
}
