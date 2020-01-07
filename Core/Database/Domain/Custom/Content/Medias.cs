namespace Allors.Domain
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public partial class Medias
    {
        protected override void CustomSetup(Setup setup) => new MediaBuilder(this.Session).WithInData(this.GetResourceBytes("madeliefje.jpg")).WithFileName("madeliefje.jpg").Build();

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
