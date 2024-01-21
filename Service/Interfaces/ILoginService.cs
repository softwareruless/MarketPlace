using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Data.Model;
using MarketPlace.Data.Entities;

namespace MarketPlace.Service
{
    public interface ILoginService
    {
        Task<UserTokenResponseModel> Authenticate(UserLoginModel model);

        Task<ResponseModel> LogOut();

        Task<UserTokenResponseModel> TokenRefresh(string refreshToken);

        Task<UserTokenResponseModel> AdminLogin(UserLoginModel model);
        Task<ResponseModel> CreateRoleAsync(User user);


    }
}
