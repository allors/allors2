namespace Allors
{
    using System;
    using System.IO;
    using System.Linq;

    public class FileStorageContainer : IStorageContainer
    {
        public FileStorageContainer(FileStorage fileStorage, DirectoryInfo directoryInfo)
        {
            this.FileStorage = fileStorage;
            this.DirectoryInfo = directoryInfo;

            this.Id = Guid.Parse(Path.GetFileNameWithoutExtension(this.DirectoryInfo.Name));
        }

        public FileStorage FileStorage { get; }

        public DirectoryInfo DirectoryInfo { get; }

        public Guid Id { get; }

        public IStorageFile[] Files
        {
            get
            {
                this.DirectoryInfo.Refresh();

                return this.DirectoryInfo.GetFiles().Select(v => new FileStorageFile(this, v)).Cast<IStorageFile>().ToArray();
            }
        }

        public void Delete() => this.DirectoryInfo.Delete(true);

        public IStorageFile CreateFile(long id, long version, string extension)
        {
            var fileName = Path.Combine(this.DirectoryInfo.FullName, $"{id}_{version}.{extension}");
            return new FileStorageFile(this, new FileInfo(fileName));
        }
    }
}
