using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DentalAppDbContext _context;

        public AppointmentRepository(DentalAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Surgery)
                .ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Surgery)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public void Delete(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }




        public async Task<IEnumerable<Appointment>> GetByDentistIdAsync(int dentistId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Surgery)
                .Where(a => a.DentistId == dentistId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Dentist)
                .Include(a => a.Surgery)
                .Include(a=> a.Patient)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }


        public async Task<IEnumerable<Appointment>> GetAppointmentsForDentistInRange(int dentistId, DateTime start, DateTime end)
        {
            return await _context.Appointments
                .Where(a => a.DentistId == dentistId && a.DateTime >= start && a.DateTime < end)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Appointment>, int)> GetPagedAsync(PagingRequest request)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Surgery)
                .AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToListAsync();

            return (items, total);
        }

    }

}
