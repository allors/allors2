namespace Allors
{
    using System;
    using System.IO;

    public class FileStorageFile : IStorageFile
    {
        public FileStorageFile(FileStorageContainer container, FileInfo fileInfo)
        {
            this.Container = container;
            this.FileInfo = fileInfo;

            var fileName = fileInfo.Name;

            var idAndVersion = fileName.Split('_');
            this.Id = long.Parse(idAndVersion[0]);
            this.Version = long.Parse(idAndVersion[1]);
            this.Extension = Path.GetExtension(fileName);
        }

        public FileStorageContainer Container { get; }

        public FileInfo FileInfo { get; }

        public long Id { get; }

        public long Version { get; }

        public string Extension { get; }

        public Stream OpenWrite() => this.FileInfo.OpenWrite();

        public void Delete() => this.FileInfo.Delete();
    }
}
