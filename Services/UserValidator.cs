using Microsoft.Extensions.Options;
using IdentityManagement.Entities;

namespace IdentityManagement.Services
{
    public class UserValidator: IUserValidator
    {
        private readonly IOptions<UserInfo> _userInfo;

        public UserValidator(IOptions<UserInfo> userInfo)
        {
            _userInfo = userInfo;
        }

        public User ValidateUser(AuthorizationRequestModel model)
        {
            foreach (var item in _userInfo.Value.Users)
            {
                if (item.Name == model.Name && item.Password == model.Password)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
