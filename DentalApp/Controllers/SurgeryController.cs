using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurgeryController : ControllerBase
    {
        private readonly ISurgeryService _service;

        public SurgeryController(ISurgeryService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SurgeryResponseDto>>> GetAll()
        {
            var surgeries = await _service.GetAllAsync();
            return Ok(surgeries);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SurgeryResponseDto>> GetById(int id)
        {
            var surgery = await _service.GetByIdAsync(id);
            return Ok(surgery);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<SurgeryResponseDto>> Create([FromBody] SurgeryRequestDto request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.SurgeryId }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<SurgeryResponseDto>> Update(int id, [FromBody] SurgeryRequestDto request)
        {
            var updated = await _service.UpdateAsync(id, request);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }


}
