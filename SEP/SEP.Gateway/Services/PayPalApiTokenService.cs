using Microsoft.IdentityModel.Tokens;
using SEP.Gateway.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SEP.Gateway.Services
{
    public class PayPalApiTokenService
    {
        public AuthToken GenerateToken(string authKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("some_big_key_value_here_secret"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expirationDate = DateTime.UtcNow.AddDays(2);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, authKey.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(audience: "audience",
                                              issuer: "issuer",
                                              claims: claims,
                                              expires: expirationDate,
                                              signingCredentials: credentials);

            var authToken = new AuthToken();
            authToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authToken.ExpirationDate = expirationDate;

            return authToken;
        }
    }
}
