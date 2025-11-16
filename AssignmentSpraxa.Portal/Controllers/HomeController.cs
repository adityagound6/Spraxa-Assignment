using AssignmentSpraxa.Portal.DTO;
using AssignmentSpraxa.Portal.Interface;
using AssignmentSpraxaContext.Portal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AssignmentSpraxaContext.Portal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiService _api;
        public HomeController(ILogger<HomeController> logger, IApiService api)
        {
            _logger = logger;
            _api = api;
        }

        public async Task<IActionResult> IndexAsync()
        {
            string token = HttpContext.Session.GetString("jwt");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            // extract user id from token
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userId = jwt.Claims.FirstOrDefault(c =>
                                c.Type == ClaimTypes.NameIdentifier ||
                                c.Type == JwtRegisteredClaimNames.Sub
                            )?.Value;

            if (userId == null)
                return Unauthorized("User ID not found in token.");

            _api.SetBearerToken(token);
            var med = await _api.GetAsync<DashboardDto>($"/api/User/Dashboard/{userId}");
            return View(med);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
