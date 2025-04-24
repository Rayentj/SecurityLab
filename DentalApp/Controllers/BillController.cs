using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _service;

        public BillController(IBillService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [Authorize(Roles = "Admin")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId) => Ok(await _service.GetByPatientIdAsync(patientId));

        [Authorize(Roles = "Admin")]
        [HttpPost("patient/{patientId}")]
        public async Task<IActionResult> Create(int patientId, [FromBody] BillRequestDto dto)
        {
            var result = await _service.CreateAsync(patientId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.BillId }, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/pay")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromQuery] bool isPaid)
        {
            var result = await _service.UpdatePaymentStatusAsync(id, isPaid);
            return result ? NoContent() : NotFound();
        }
    }
}
