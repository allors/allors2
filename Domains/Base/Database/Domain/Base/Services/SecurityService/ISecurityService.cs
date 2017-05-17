namespace Allors.Services
{
    public interface ISecurityService : IService
    {
        string HashPassword(string user, string password);

        bool VerifyHashedPassword(string user, string hashedPassword, string providedPassword);
    }
}
