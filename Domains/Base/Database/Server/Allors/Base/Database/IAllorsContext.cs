using Allors;

namespace Allors.Server
{
    public interface IAllorsContext
    {
        ISession Session { get; }
    }
}