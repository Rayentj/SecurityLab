using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        public RoleController(IRoleService service) => _service = service;
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponseDto>> GetById(int id)
        {
            var role = await _service.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<RoleResponseDto>> Create([FromBody] CreateRoleRequestDto dto)
        {
            var role = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = role.RoleId }, role);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleResponseDto>> Update(int id, [FromBody] CreateRoleRequestDto dto)
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
