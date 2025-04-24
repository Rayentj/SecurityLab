using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DentalApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientApiController : ControllerBase
    {
        private readonly IPatientService _service;
        private readonly IAppointmentService _appointmentService;
        public PatientApiController(IPatientService service, IAppointmentService appointmentService)
        {
            _service = service;
            _appointmentService = appointmentService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _service.GetAllAsync();
            return Ok(patients);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientRequestDto request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.PatientId }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientRequestDto request)
        {
            var success = await _service.UpdateAsync(id, request);
            if (!success) return NotFound();
            return NoContent();
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var request = new PagingRequest { Page = page, Size = size };
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }


        [Authorize(Roles = "Patient,Admin")]
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointments()
        {
            // Extract Patient ID from JWT claims
            var patientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (patientIdClaim == null)
            {
                return Unauthorized("Patient ID not found in token.");
            }

            if (!int.TryParse(patientIdClaim.Value, out int patientId))
            {
                return BadRequest("Invalid Patient ID in token.");
            }

            var appointments = await _appointmentService.GetByPatientIdAsync(patientId);
            return Ok(appointments);
        }


        [Authorize(Roles = "Patient,Admin")]

        [HttpPost("{id}/appointments/request")]
        public async Task<IActionResult> RequestAppointment(int id, [FromBody] AppointmentRequestDto dto)
        {
            dto.PatientId = id;
            var created = await _appointmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAppointments), new { id = id }, created);
        }

        [Authorize(Roles = "Patient,Admin")]
        [HttpPut("/appointments/{id}/reschedule")]
        public async Task<IActionResult> Reschedule(int id, [FromBody] AppointmentRequestDto dto)
        {
            var success = await _appointmentService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Patient,Admin")]
        [HttpDelete("/appointments/{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var success = await _appointmentService.CancelAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
