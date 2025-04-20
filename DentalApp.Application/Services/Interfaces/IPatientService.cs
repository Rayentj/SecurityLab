using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDto>> GetAllAsync();
        Task<PatientResponseDto> GetByIdAsync(int id);
        Task<PatientResponseDto> CreateAsync(PatientRequestDto request);
        Task<bool> UpdateAsync(int id, PatientRequestDto request);
        Task<bool> DeleteAsync(int id);
    }
}
