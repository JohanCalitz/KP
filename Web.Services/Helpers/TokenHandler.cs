namespace Web.Services.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using System.Threading.Tasks;

    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ISession? session = httpContextAccessor.HttpContext?.Session;

            if (session != null && session.TryGetValue("AccessToken", out byte[]? tokenBytes))
            {
                string token = Encoding.UTF8.GetString(tokenBytes);
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    JwtSecurityTokenHandler handler = new();
                    JwtSecurityToken jwtToken = handler.ReadJwtToken(token);


                }
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Handle expired token (401)
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                httpContextAccessor.HttpContext?.Session.Clear();
            }

            return response;
        }
    }
}
