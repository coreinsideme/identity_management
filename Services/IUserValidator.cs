using IdentityManagement.Entities;

namespace IdentityManagement.Services
{
    public interface IUserValidator
    {
        User ValidateUser(AuthorizationRequestModel model);
    }
}
