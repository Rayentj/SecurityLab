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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
        {
            var roles = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
        }

        public async Task<RoleResponseDto> GetByIdAsync(int id)
        {
            var role = await _repository.GetByIdAsync(id)
                       ?? throw new EntityNotFoundException($"Role with ID {id} not found");
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> CreateAsync(CreateRoleRequestDto request)
        {
            var role = _mapper.Map<Role>(request);
            await _repository.AddAsync(role);
            await _repository.SaveChangesAsync();
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> UpdateAsync(int id, CreateRoleRequestDto dto)
        {
            var role = await _repository.GetByIdAsync(id)
                       ?? throw new EntityNotFoundException($"Role with ID {id} not found");

            _mapper.Map(dto, role);
            _repository.Update(role);
            await _repository.SaveChangesAsync();

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _repository.GetByIdAsync(id)
                       ?? throw new EntityNotFoundException($"Role with ID {id} not found");

            _repository.Delete(role);
            return await _repository.SaveChangesAsync();
        }
    }
}
