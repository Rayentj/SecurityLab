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
    public class DentistService : IDentistService
    {
        private readonly IDentistRepository _repo;
        private readonly IMapper _mapper;

        public DentistService(IDentistRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<DentistResponseDto> CreateAsync(DentistRequestDto dto)
        {
            var dentist = _mapper.Map<Dentist>(dto);
            await _repo.AddAsync(dentist);
            await _repo.SaveChangesAsync();
            return _mapper.Map<DentistResponseDto>(dentist);
        }

        public async Task<IEnumerable<DentistResponseDto>> GetAllAsync()
        {
            var all = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<DentistResponseDto>>(all);
        }

        public async Task<DentistResponseDto> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Dentist with ID {id} not found");
            return _mapper.Map<DentistResponseDto>(entity);
        }

        public async Task<DentistResponseDto> UpdateAsync(int id, DentistRequestDto dto)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Dentist with ID {id} not found");
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<DentistResponseDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Dentist with ID {id} not found");
            _repo.Delete(entity);
            return await _repo.SaveChangesAsync();
        }
    }

}
