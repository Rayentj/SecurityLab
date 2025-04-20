using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.Api.Controllers
{
    public class PatientController : Controller  // ⬅️ Changed from ControllerBase to Controller
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        // GET: /Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _service.GetAllAsync();
            return View(patients); // Looks for Views/Patient/Index.cshtml
        }

        // GET: /Patient/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient); // Views/Patient/Details.cshtml
        }

        // GET: /Patient/Create
        public IActionResult Create()
        {
            return View(); // Renders empty form
        }

        // POST: /Patient/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientRequestDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var created = await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Patient/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null) return NotFound();

            var dto = new PatientRequestDto
            {
                FirstName = patient.FullName?.Split(' ')[0],
                LastName = patient.FullName?.Split(' ').Last(),
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                Dob = DateTime.Now, // You can map from full DTO if needed
                AddressId = 1       // Placeholder
            };

            return View(dto); // Views/Patient/Edit.cshtml
        }

        // POST: /Patient/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientRequestDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Patient/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient); // Views/Patient/Delete.cshtml
        }

        // POST: /Patient/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int PatientId)
        {
            var deleted = await _service.DeleteAsync(PatientId);
            if (!deleted) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
