using AssignmentSpraxa.API.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssignmentSpraxa.API.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<(string Token, DateTime Expires)> GenerateTokenAsync(User user, IEnumerable<string> roles)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key not configured");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expiresMinutes = jwtSection.GetValue<int?>("ExpireMinutes") ?? 60;

            var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("Id", user.Id.ToString()),
                new Claim("FullName", user.FullName.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles
            foreach (var userRole in user.UserRoles)
            {
                if (!string.IsNullOrEmpty(userRole.Role?.Name))
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                }
            }

            var expandedRoles = new List<string>();

            foreach (var role in roles)
            {
                expandedRoles.Add(role);

                if (role == "GlobalAdmin")
                {
                    expandedRoles.AddRange(new[] { "Admin", "User" });
                }
                else if (role == "Admin")
                {
                    expandedRoles.Add("User");
                }
            }

            var roleClaims = expandedRoles.Distinct().Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult((tokenString, expires));
        }
    }
}
