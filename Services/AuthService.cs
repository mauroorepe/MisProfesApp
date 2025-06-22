using Microsoft.IdentityModel.Tokens;
using MisProfesApp.Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MisProfesApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration Configuration;

        public AuthService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<AuthResponse> Login(AuthRequest req)
        {
            var userToken = GenerateToken(req);

            var res = new AuthResponse()
            {
                Id = req.Id,
                Token = userToken,
            };

            return res;
        }

        private string GenerateToken(AuthRequest req)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new Exception("The environment variable JWT_SECRET is not set.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, req.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(90),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string userToken = tokenHandler.WriteToken(token);

            return userToken;
        }
    }
}
