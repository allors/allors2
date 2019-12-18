namespace Allors
{
    using System.Buffers;
    using System.IO;

    public interface IStorageFile
    {
        long Id { get; }

        long Version { get; }

        Stream OpenWrite();

        void Delete();
    }
}
