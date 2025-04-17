namespace API.Services.Interfaces
{
    using API.Models;
    using API.Services.Helpers;

    public interface IAuthService
    {
        Result<TokenModel> GenerateToken(LoginModel model);
    }
}
