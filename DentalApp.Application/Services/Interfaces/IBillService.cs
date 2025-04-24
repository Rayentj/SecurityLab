using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services.Interfaces
{
    public interface IBillService
    {
        Task<IEnumerable<BillResponseDto>> GetAllAsync();
        Task<BillResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<BillResponseDto>> GetByPatientIdAsync(int patientId);
        Task<BillResponseDto> CreateAsync(int patientId, BillRequestDto dto);
        Task<bool> UpdatePaymentStatusAsync(int id, bool isPaid);
    }
}
