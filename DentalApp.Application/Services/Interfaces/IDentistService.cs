using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services.Interfaces
{
    public interface IDentistService
    {
        Task<DentistResponseDto> CreateAsync(DentistRequestDto dto);
        Task<IEnumerable<DentistResponseDto>> GetAllAsync();
        Task<DentistResponseDto> GetByIdAsync(int id);
        Task<DentistResponseDto> UpdateAsync(int id, DentistRequestDto dto);
        Task<bool> DeleteAsync(int id);

    }
}
