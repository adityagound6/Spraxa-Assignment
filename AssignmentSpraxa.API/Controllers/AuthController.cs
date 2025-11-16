using AssignmentSpraxa.API.Dto;
using AssignmentSpraxa.API.Dto.PostDto;
using AssignmentSpraxa.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AssignmentSpraxa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AssignmentSpraxContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(AssignmentSpraxContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestPostDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isExternalProvider = !string.IsNullOrEmpty(model.Provider) && model.Provider.ToLower() != "local";

            if(await _context.Users.AnyAsync(u => u.Username == model.UserName || u.Email == model.Email || u.PhoneNumber == model.PhoneNumber))
            {
                return Conflict("Username is already taken.");
            }

            var user = new User
            {
                ExternalId = model.ExternalId,
                FullName = model.FullName,
                Username = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Provider = isExternalProvider ? model.Provider : "Local",
                PasswordHash = isExternalProvider ? null : HashPassword(model.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var requestedRoles = model.Roles?.Any() == true ? model.Roles : new[] { "User" };

            foreach (var roleName in requestedRoles)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role == null)
                {
                    role = new Role { Name = roleName };
                    await _context.Roles.AddAsync(role);
                    await _context.SaveChangesAsync();
                }

                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };

                await _context.UserRoles.AddAsync(userRole);
            }

            await _context.SaveChangesAsync();

            var (token, expires) = await _jwtTokenService.GenerateTokenAsync(user, requestedRoles);

            return Created(string.Empty, new AuthResponse
            {
                Token = token,
                Expires = expires,
                UserId = user.Id.ToString(),
                Roles = requestedRoles
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestPostDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.UserNameOrEmail || u.Email == model.UserNameOrEmail);

            if (user == null)
            {
                await LogLogin(null, false);
                return Unauthorized("Invalid username or password.");
            }

            if (user.Provider.ToLower() != "local")
            {
                if (string.Equals(user.Provider, model.Provider, StringComparison.OrdinalIgnoreCase))
                {
                    var roles = await GetUserRolesAsync(user.Id);
                    var (token, expires) = await _jwtTokenService.GenerateTokenAsync(user, roles);
                    await LogLogin(user.Id, true);

                    return Ok(new AuthResponse
                    {
                        Token = token,
                        Expires = expires,
                        UserId = user.Id.ToString(),
                        Roles = roles
                    });
                }
                else
                {
                    await LogLogin(user.Id, false);
                    return Unauthorized("Invalid provider for this account.");
                }
            }

            var hashedInputPassword = HashPassword(model.Password);
            if (user.PasswordHash != hashedInputPassword)
            {
                await LogLogin(user.Id, false);
                return Unauthorized("Invalid username or password.");
            }

            var userRoles = await GetUserRolesAsync(user.Id);
            var (localToken, localExpires) = await _jwtTokenService.GenerateTokenAsync(user, userRoles);
            await LogLogin(user.Id, true);

            return Ok(new AuthResponse
            {
                Token = localToken,
                Expires = localExpires,
                UserId = user.Id.ToString(),
                Roles = userRoles
            });
        }

        private async Task LogLogin(Guid? userId, bool status)
        {
            var loginHistory = new LoginHistory
            {
                UserId = userId,
                Status = status,
                LoginTime = DateTime.UtcNow
            };
            await _context.LoginHistories.AddAsync(loginHistory);
            await _context.SaveChangesAsync();
        }

        private async Task<string[]> GetUserRolesAsync(Guid userId)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role.Name)
                .ToArrayAsync();

            return roles.Length > 0 ? roles : new[] { "User" };
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
