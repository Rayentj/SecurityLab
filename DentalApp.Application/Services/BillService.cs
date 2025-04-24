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
    public class BillService : IBillService
    {
        private readonly IBillRepository _repo;
        private readonly IPatientRepository _patientRepo;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IMapper _mapper;

        public BillService(IBillRepository repo, IPatientRepository patientRepo, IAppointmentRepository appointmentRepo, IMapper mapper)
        {
            _repo = repo;
            _patientRepo = patientRepo;
            _appointmentRepo = appointmentRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BillResponseDto>> GetAllAsync()
        {
            var bills = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<BillResponseDto>>(bills);
        }

        public async Task<BillResponseDto> GetByIdAsync(int id)
        {
            var bill = await _repo.GetByIdAsync(id) ?? throw new EntityNotFoundException($"Bill with ID {id} not found");
            return _mapper.Map<BillResponseDto>(bill);
        }

        public async Task<IEnumerable<BillResponseDto>> GetByPatientIdAsync(int patientId)
        {
            var bills = await _repo.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<BillResponseDto>>(bills);
        }

        public async Task<BillResponseDto> CreateAsync(int patientId, BillRequestDto dto)
        {
            var patient = await _patientRepo.GetByIdAsync(patientId)
                          ?? throw new EntityNotFoundException($"Patient with ID {patientId} not found");

            var appointment = await _appointmentRepo.GetByIdAsync(dto.AppointmentId)
                              ?? throw new EntityNotFoundException($"Appointment with ID {dto.AppointmentId} not found");

            var bill = new Bill
            {
                PatientId = patientId,
                AppointmentId = dto.AppointmentId,
                Amount = dto.Amount,
                IsPaid = dto.IsPaid
            };

            await _repo.AddAsync(bill);
            await _repo.SaveChangesAsync();
            return _mapper.Map<BillResponseDto>(bill);
        }

        public async Task<bool> UpdatePaymentStatusAsync(int id, bool isPaid)
        {
            var bill = await _repo.GetByIdAsync(id)
                       ?? throw new EntityNotFoundException($"Bill with ID {id} not found");

            bill.IsPaid = isPaid;
            _repo.Update(bill);
            return await _repo.SaveChangesAsync();
        }
    }
}
