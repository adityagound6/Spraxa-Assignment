using AssignmentSpraxa.Portal.DTO;
using AssignmentSpraxa.Portal.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentSpraxa.Portal.Controllers
{
    public class UserController : Controller
    {
        private readonly IApiService _api;
        public UserController(IApiService api)
        {
            _api = api;
        }

        // GET: UserController1
        public async Task<ActionResult> IndexAsync()
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);

            var medicines = await _api.GetAsync<List<UserDto>>("/api/User");
            return View(medicines);
        }

        public async Task<ActionResult> Search(string term)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);

            var medicines = await _api.GetAsync<List<UserDto>>($"/api/User/Search/{term}");
            return View(medicines);
        }

        // GET: UserController1/Details/5
        public async Task<ActionResult> DetailsAsync(Guid id)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);
            var med = await _api.GetAsync<UserDto>($"/api/User/{id}");
            return View(med);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(Guid id)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);
            var med = await _api.GetAsync<UserDto>($"/api/User/{id}");
            return View(med);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(UserDto model)
        {
            string token = HttpContext.Session.GetString("jwt");

            if (!string.IsNullOrEmpty(token))
                _api.SetBearerToken(token);
            await _api.PutAsync<MedicineDto>($"/api/User/{model.Id}", model);
            return RedirectToAction("Index");
        }

        // GET: UserController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
