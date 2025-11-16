using AssignmentSpraxa.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSpraxa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AssignmentSpraxContext _context;

        public UserController(AssignmentSpraxContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            bool isAdmin = User.IsInRole("Admin");

            IQueryable<User> query = _context.Users;

            if (!isAdmin)
            {
                query = query.Where(u =>
                    u.UserRoles.Any(ur => ur.Role.Name == "User")
                );
            }

            var users = await query
                .Select(u => new
                {
                    u.FullName,
                    u.Id,
                    u.Username,
                    u.Email,
                    u.PhoneNumber,
                    Roles = u.UserRoles
                        .Select(ur => ur.Role.Name)
                        .ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.FullName,
                    u.Id,
                    u.Username,
                    u.Email,
                    u.PhoneNumber,
                    Roles = u.UserRoles
                        .Select(ur => ur.Role.Name)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

        [Authorize(Roles = "User")]
        [HttpGet("Dashboard/{id}")]
        public async Task<IActionResult> GetDashboardById(Guid id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.FullName,
                    u.Id,
                    u.Username,
                    u.Email,
                    u.PhoneNumber,
                    Roles = u.UserRoles
                        .Select(ur => ur.Role.Name)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            var loginHistory = await _context.LoginHistories
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.LoginTime)
                .ToListAsync();

            return Ok(new
            {
                User = user,
                LoginHistory = loginHistory
            });
        }

        [Authorize(Roles = "User")]
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string src)
        {
            IQueryable<User> query = _context.Users;

            // If search term provided → filter
            if (!string.IsNullOrWhiteSpace(src))
            {
                src = src.Trim().ToLower();

                query = query.Where(u =>
                    u.FullName.ToLower().Contains(src) ||
                    u.Username.ToLower().Contains(src) ||
                    u.Email.ToLower().Contains(src) ||
                    u.Id.ToString().ToLower().Contains(src)
                );
            }

            var users = await query
                .Select(u => new
                {
                    u.FullName,
                    u.Id,
                    u.Username,
                    u.Email,
                    u.PhoneNumber,
                    Roles = u.UserRoles
                        .Select(ur => ur.Role.Name)
                        .ToList()
                })
                .ToListAsync();

            // Return empty message if no users matched
            if (users == null || users.Count == 0)
                return NotFound("Users not found");

            return Ok(users);
        }
    }
}
