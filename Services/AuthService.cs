namespace API.Services
{
    using API.Models;
    using API.Services.Constants;
    using API.Services.Helpers;
    using API.Services.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class AuthService: IAuthService
    {
        private readonly IConfiguration config;

        public AuthService(IConfiguration config)
        {
            this.config = config;
        }

        public Result<TokenModel> GenerateToken(LoginModel model)
        {
            try
            {
                Claim[] claims = new[]
                {
                new Claim(ClaimTypes.Name, model.UserName),
                };

                SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(config[AppSettingKeyConstants.AuthKey]));
                SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new(
                    issuer: config[AppSettingKeyConstants.AuthIssuer],
                    audience: config[AppSettingKeyConstants.AuthAudience],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                JwtSecurityTokenHandler tokenHandler = new();
                string tokenString = tokenHandler.WriteToken(token);

                return new TokenModel
                {
                    Token = $"{tokenString}",
                };
            }
            catch (Exception)
            {
                return new Result<TokenModel>(new ResultError($"Failed to generate a token"));
            }

        }
    }
}
