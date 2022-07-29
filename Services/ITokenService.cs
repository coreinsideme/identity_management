using IdentityManagement.Entities;

namespace IdentityManagement.Services
{
    public interface ITokenService
    {
        string GetToken(User user);
        bool ValidateToken(string token);
    }
}
