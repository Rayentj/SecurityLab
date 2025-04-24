using DentalApp.Application.Services;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DentalApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _service;
        private readonly IAppointmentService _appointmentService;

        public DentistController(IDentistService service, IAppointmentService appointmentService)
        {
            _service = service;
            _appointmentService = appointmentService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DentistResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DentistResponseDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result); // No need for null check; exception will handle it
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DentistResponseDto>> Create([FromBody] DentistRequestDto dentist)
        {
            var created = await _service.CreateAsync(dentist);
            return CreatedAtAction(nameof(GetById), new { id = created.DentistId }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<DentistResponseDto>> Update(int id, [FromBody] DentistRequestDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }



        
        [Authorize(Roles = "Dentist,Admin")]
        [HttpGet("appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetMyAppointments()
        {
            // Extract Dentist ID from JWT claims
            var dentistIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (dentistIdClaim == null)
            {
                return Unauthorized("Dentist ID not found in token.");
            }

            if (!int.TryParse(dentistIdClaim.Value, out int dentistId))
            {
                return BadRequest("Invalid Dentist ID in token.");
            }

            var appointments = await _appointmentService.GetByDentistIdAsync(dentistId);
            return Ok(appointments);
        }
    }

}
