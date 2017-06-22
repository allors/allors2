namespace Allors.Server
{
    using Allors.Domain;

    public interface IAllorsContext
    {
        ISession Session { get; }

        User User { get; }
    }
}