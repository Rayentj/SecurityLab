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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressResponseDto>> GetAllAsync()
        {
            var addresses = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AddressResponseDto>>(addresses);
        }

        public async Task<AddressResponseDto> GetByIdAsync(int id)
        {
            var address = await _repository.GetByIdAsync(id);
            return _mapper.Map<AddressResponseDto>(address);
        }

        public async Task<AddressResponseDto> CreateAsync(AddressRequestDto dto)
        {
            var address = _mapper.Map<Address>(dto);
            await _repository.AddAsync(address);
            await _repository.SaveChangesAsync();
            return _mapper.Map<AddressResponseDto>(address);
        }


        public async Task<bool> UpdateAsync(int id, AddressRequestDto dto)
        {
            var entity = await _repository.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Address ID {id} not found");

            _mapper.Map(dto, entity);
            _repository.Update(entity);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id)
                         ?? throw new EntityNotFoundException($"Address ID {id} not found");

            _repository.Delete(entity);
            return await _repository.SaveChangesAsync();
        }
    }
}
