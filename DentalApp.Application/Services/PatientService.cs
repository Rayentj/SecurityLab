using AutoMapper;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientResponseDto>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientResponseDto>>(patients);
        }
        public async Task<PatientResponseDto> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return null;
            return _mapper.Map<PatientResponseDto>(patient);
        }

        public async Task<PatientResponseDto> CreateAsync(PatientRequestDto request)
        {
            var patient = _mapper.Map<Patient>(request);
            await _repository.AddAsync(patient);
            await _repository.SaveChangesAsync();
            return _mapper.Map<PatientResponseDto>(patient);
        }
        public async Task<bool> UpdateAsync(int id, PatientRequestDto request)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            _mapper.Map(request, patient);
            _repository.Update(patient);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            _repository.Delete(patient);
            return await _repository.SaveChangesAsync();
        }



        public async Task<PaginatedResponse<PatientResponseDto>> GetPagedAsync(PagingRequest request)
        {
            var (entities, total) = await _repository.GetPagedAsync(request);
            var items = _mapper.Map<IEnumerable<PatientResponseDto>>(entities);

            return new PaginatedResponse<PatientResponseDto>
            {
                Page = request.Page,
                Size = request.Size,
                TotalCount = total,
                Items = items
            };
        }

       
    }
}
