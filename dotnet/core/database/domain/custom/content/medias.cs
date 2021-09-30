namespace Allors.Domain
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public partial class Medias
    {
        public static readonly Guid AvatarId = new Guid("E9B790FB-B35E-441C-A25F-904D0674B32C");
        public static readonly Guid MadeliefjeId = new Guid("AE0D2BAA-9E07-4DD2-8AAD-98E57010CE98");
        public static readonly Guid AboutId = new Guid("F5922C1B-A0DA-4A77-98BD-21F037C0E3E6");

        private UniquelyIdentifiableSticky<Media> cache;

        public Sticky<Guid, Media> Cache => this.cache ??= new UniquelyIdentifiableSticky<Media>(this.Session);

        public Media Avatar => this.Cache[AvatarId];

        public Media Madeliefje => this.Cache[MadeliefjeId];

        public Media About => this.Cache[AboutId];

        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(AvatarId, v =>
            {
                v.InData = this.GetResourceBytes("avatar.png");
                v.InFileName = "avatar.png";
            });

            merge(AboutId, v =>
            {
                v.InData = this.GetResourceBytes("about.md");
                v.InFileName = "about.md";
            });

            merge(MadeliefjeId, v =>
            {
                v.InData = this.GetResourceBytes("madeliefje.jpg");
                v.InFileName = "madeliefje.jpg";
            });
        }

        private byte[] GetResourceBytes(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var manifestResourceName = assembly.GetManifestResourceNames().First(v => v.Contains(name));
            var resource = assembly.GetManifestResourceStream(manifestResourceName);
            if (resource != null)
            {
                using (var ms = new MemoryStream())
                {
                    resource.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }
    }
}
