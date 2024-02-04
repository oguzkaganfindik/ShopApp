using ShopApp.Business.Dtos;
using ShopApp.Business.Types;

namespace ShopApp.Business.Services
{
    public interface IUserService
    {
        ServiceMessage AddUser(UserAddDto userAddDto);
        UserInfoDto LoginUser(UserLoginDto userLoginDto);

    }
}
