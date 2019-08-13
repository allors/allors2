using System.IO;

namespace Allors.Domain
{
    public static class FileReader
    {
        public static Media CreateMedia(ISession session, string fileName)
        {
            if (File.Exists(fileName))
            {
                var fileInfo = new FileInfo(fileName);

                var name = Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                return new MediaBuilder(session).WithFileName(name).WithInData(content).Build();
            }

            return null;
        }
    }
}
