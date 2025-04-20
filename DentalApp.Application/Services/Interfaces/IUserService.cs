using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto> GetByIdAsync(int id);
        Task<UserResponseDto> CreateAsync(CreateUserRequestDto request);
        Task<UserResponseDto> UpdateAsync(int id, CreateUserRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
