namespace Web.Services.Interfaces
{
    using System.Threading.Tasks;
    using Web.Models;
    using Web.Services.Helpers;

    public interface IAccountSerivce
    {
        Task<Result<TokenModel>> GetToken(LoginModel model);
    }
}
