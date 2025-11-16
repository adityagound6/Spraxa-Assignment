using AssignmentSpraxa.Portal.DTO;
using AssignmentSpraxa.Portal.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AssignmentSpraxa.Portal.Controllers
{
    [Authorize]
    public class MedicineController : Controller
    {
        private readonly IApiService _api;

        public MedicineController(IApiService api)
        {
            _api = api;
        }

        // GET: Medicine
        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            bool isAdmin = jwt.Claims
                              .Any(c => c.Type == "role" || c.Type == ClaimTypes.Role &&
                                        c.Value == "Admin");

            ViewBag.IsAdmin = isAdmin;

            var medicines = await _api.GetAsync<List<MedicineDto>>("/api/Medicine");
            return View(medicines);
        }

        // GET: Medicine
        public async Task<IActionResult> Search(string term)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            bool isAdmin = jwt.Claims
                              .Any(c => c.Type == "role" || c.Type == ClaimTypes.Role &&
                                        c.Value == "Admin");

            ViewBag.IsAdmin = isAdmin;

            var medicines = await _api.GetAsync<List<MedicineDto>>($"/api/Medicine/Search/{term}");
            return View(medicines);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MedicineCreateDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                string token = HttpContext.Session.GetString("jwt");

                if (!string.IsNullOrEmpty(token))
                    _api.SetBearerToken(token);

                await _api.PostAsync<MedicineDto>("/api/Medicine", model);
                TempData["Success"] = "Medicine created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message; // send readable API message
                return View(model);
            }
        }

        // GET: Edit
        public async Task<IActionResult> Edit(Guid id)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);
            var med = await _api.GetAsync<MedicineDto>($"/api/Medicine/{id}");
            return View(med);
        }

        public async Task<IActionResult> View(Guid id)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);
            var med = await _api.GetAsync<MedicineDto>($"/api/Medicine/{id}");
            return View(med);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(MedicineDto model)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);
            await _api.PutAsync<MedicineDto>($"/api/Medicine/{model.MedicineId}", model);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);

            await _api.DeleteAsync($"/api/Medicine/{id}");
            return RedirectToAction("Index");
        }
    }
}
