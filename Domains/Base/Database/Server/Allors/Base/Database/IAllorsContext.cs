namespace Allors.Web.Database
{
    public interface IAllorsContext
    {
        ISession Session { get; }
    }
}