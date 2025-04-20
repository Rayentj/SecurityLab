using DentalApp.Application.Services;
using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly TokenService _tokenService;

        public AuthController(IUserRepository userRepo, TokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var user = await _userRepo.FindByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateJwtToken(user);
            return Ok(new { token });
        }
    }

}
