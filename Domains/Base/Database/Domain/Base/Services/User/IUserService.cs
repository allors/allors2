namespace Allors.Services
{
    using Allors.Domain;

    public interface IUserService : IService
    {
        string GetUser();
    }
}
