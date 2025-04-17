namespace API.KP.Controllers
{
    using API.KP.Common;
    using API.Models;
    using API.Services.Helpers;
    using API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class AuthenticationController : Controller
    {
        private readonly IAuthService authServices;

        public AuthenticationController(IAuthService authServices)
        {
            this.authServices = authServices;
        }

        [HttpPost("login")]
        public ApiResponse Login([FromBody] LoginModel model)
        {
            Result<TokenModel> result = authServices.GenerateToken(model);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }
    }
}
