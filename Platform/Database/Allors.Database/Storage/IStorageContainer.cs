namespace Allors
{
    using System;

    public interface IStorageContainer
    {
        Guid Id { get; }

        void Delete();

        IStorageFile[] Files { get; }

        IStorageFile CreateFile(long id, long version, string extension);
    }
}
