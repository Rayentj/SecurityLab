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
    public interface ISurgeryService
    {
        Task<IEnumerable<SurgeryResponseDto>> GetAllAsync();
        Task<SurgeryResponseDto> GetByIdAsync(int id);
        Task<SurgeryResponseDto> CreateAsync(SurgeryRequestDto request);
        Task<SurgeryResponseDto> UpdateAsync(int id, SurgeryRequestDto request);
        Task<bool> DeleteAsync(int id);
        Task<PaginatedResponse<SurgeryResponseDto>> GetPagedAsync(PagingRequest request);
    }
}
