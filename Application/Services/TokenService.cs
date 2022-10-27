using Application.Data.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenService
    {
        private readonly JwtOptions _jwtOptions;
        public TokenService(
            IOptions<JwtOptions> jwt
        )
        {
            _jwtOptions = jwt.Value;
        }
        public async Task<string> GenerateToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.DurationInMinutes),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public static async Task<IEnumerable<Claim>> GenerateUserClaims(User user)
        {

            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("name", $"{user.Name}"),
                new Claim("roles", $"{user.Role}")
            };

            return claims;
        }
    }
}
