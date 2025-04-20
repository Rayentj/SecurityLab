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
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
        {
            var appointments = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
        }

        public async Task<AppointmentResponseDto> GetByIdAsync(int id)
        {
            var appointment = await _repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException($"Appointment with ID {id} not found");

            return _mapper.Map<AppointmentResponseDto>(appointment);
        }

        public async Task<AppointmentResponseDto> CreateAsync(AppointmentRequestDto dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            await _repo.AddAsync(appointment);
            await _repo.SaveChangesAsync();
            return _mapper.Map<AppointmentResponseDto>(appointment);
        }

        public async Task<bool> UpdateAsync(int id, AppointmentRequestDto dto)
        {
            var appointment = await _repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException($"Appointment with ID {id} not found");

            _mapper.Map(dto, appointment);
            _repo.Update(appointment);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException($"Appointment with ID {id} not found");

            _repo.Delete(appointment);
            return await _repo.SaveChangesAsync();
        }
    }

}
