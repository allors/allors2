namespace Allors.Database.Adapters.Sql
{
    public interface IConnection
    {
        ICommand CreateCommand();

        void Commit();

        void Rollback();
    }
}
