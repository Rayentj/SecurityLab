using AutoMapper;
using DentalApp.Application.Exceptions;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services
{
    public class SurgeryService : ISurgeryService
    {
        private readonly ISurgeryRepository _repository;
        private readonly IMapper _mapper;

        public SurgeryService(ISurgeryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SurgeryResponseDto>> GetAllAsync()
        {
            var surgeries = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SurgeryResponseDto>>(surgeries);
        }

        public async Task<SurgeryResponseDto> GetByIdAsync(int id)
        {
            var surgery = await _repository.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Surgery ID {id} not found");

            return _mapper.Map<SurgeryResponseDto>(surgery);
        }

        public async Task<SurgeryResponseDto> CreateAsync(SurgeryRequestDto request)
        {
            var surgery = _mapper.Map<Surgery>(request);
            await _repository.AddAsync(surgery);
            await _repository.SaveChangesAsync();
            return _mapper.Map<SurgeryResponseDto>(surgery);
        }

        public async Task<SurgeryResponseDto> UpdateAsync(int id, SurgeryRequestDto request)
        {
            var entity = await _repository.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Surgery ID {id} not found");

            _mapper.Map(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            return _mapper.Map<SurgeryResponseDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Surgery ID {id} not found");

            _repository.Delete(entity);
            return await _repository.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<SurgeryResponseDto>> GetPagedAsync(PagingRequest request)
        {
             var (entities, total) = await _repository.GetPagedAsync(request);
            var items = _mapper.Map<IEnumerable<SurgeryResponseDto>>(entities);

            return new PaginatedResponse<SurgeryResponseDto>
            {
                Page = request.Page,
                Size = request.Size,
                TotalCount = total,
                Items = items
            };
        }
    }

}
