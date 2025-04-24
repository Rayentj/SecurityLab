using AutoMapper;
using DentalApp.Application.Exceptions;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IDentistRepository _dentistRepo;
        private readonly IPatientRepository _patientRepo;
        private readonly IBillRepository _billRepo;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository repo,
            IDentistRepository dentistRepo,
            IPatientRepository patientRepo,
            IBillRepository billRepo,
            IMapper mapper)
        {
            _repo = repo;
            _dentistRepo = dentistRepo;
            _patientRepo = patientRepo;
            _billRepo = billRepo;
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
            var dentist = await _dentistRepo.GetByIdAsync(dto.DentistId)
                         ?? throw new EntityNotFoundException($"Dentist with ID {dto.DentistId} not found");

            var patient = await _patientRepo.GetByIdAsync(dto.PatientId)
                         ?? throw new EntityNotFoundException($"Patient with ID {dto.PatientId} not found");

            // 1️⃣ Check weekly appointment count
            // Use dto.DateTime instead of DateTime.UtcNow
            var startOfWeek = dto.DateTime.Date.AddDays(-(int)dto.DateTime.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var weeklyAppointments = await _repo.GetAppointmentsForDentistInRange(dto.DentistId, startOfWeek, endOfWeek);
            if (weeklyAppointments.Count() >= 5)
                throw new ValidationException("Dentist cannot have more than 5 appointments in a week.");


            // 2️⃣ Check for unpaid bills
            var unpaidBills = await _billRepo.GetByPatientIdAsync(dto.PatientId);
            if (unpaidBills.Any(b => !b.IsPaid))
                throw new ValidationException("Patient cannot book new appointment with unpaid bills.");

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = "Confirmed";
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

        public async Task<IEnumerable<AppointmentResponseDto>> GetByDentistIdAsync(int dentistId)
        {
            var appointments = await _repo.GetByDentistIdAsync(dentistId);
            return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(int patientId)
        {
            var appointments = await _repo.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
        }

        public async Task<bool> CancelAsync(int id)
        {
            var appointment = await _repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException($"Appointment with ID {id} not found");

            appointment.Status = "Cancelled";
            _repo.Update(appointment);
            return await _repo.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<AppointmentResponseDto>> GetPagedAsync(PagingRequest request)
        {
            var (entities, total) = await _repo.GetPagedAsync(request);
            var items = _mapper.Map<IEnumerable<AppointmentResponseDto>>(entities);

            return new PaginatedResponse<AppointmentResponseDto>
            {
                Page = request.Page,
                Size = request.Size,
                TotalCount = total,
                Items = items
            };
        }
    }

}
