namespace Allors
{
    using System.IO;
    using System.Linq;

    using Allors.R1.Development.Resources;

    class Program
    {
        static int Main(string[] args)
        {
            var directoryInfos = args.Select(v => new DirectoryInfo(v)).ToArray();

            var inputDirectories = directoryInfos.Take(directoryInfos.Length-1).ToArray();
            var outputDirectory = directoryInfos.Last();
            var resources = new Resources(inputDirectories, outputDirectory);
            resources.Merge();
        }
    }
}
