using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponseDto>> GetAllAsync();
        Task<RoleResponseDto> GetByIdAsync(int id);
        Task<RoleResponseDto> CreateAsync(CreateRoleRequestDto request);
        Task<RoleResponseDto> UpdateAsync(int id, CreateRoleRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
