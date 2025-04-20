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
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _service;

        public DentistController(IDentistService service)
        {
            _service = service;
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
    }

}
