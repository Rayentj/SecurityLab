using AutoMapper;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using DentalApp.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateAsync(CreateUserRequestDto dto)
        {
            var existing = await _repository.FindByEmailAsync(dto.Email);
            if (existing != null) throw new DuplicateEmailException(dto.Email);

            var user = _mapper.Map<User>(dto);
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();
            return _mapper.Map<UserResponseDto>(user);
        }

        public Task<UserResponseDto> UpdateAsync(int id, CreateUserRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
