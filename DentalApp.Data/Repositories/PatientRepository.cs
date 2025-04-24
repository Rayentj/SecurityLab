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
    public class PatientRepository : IPatientRepository
    {
        private readonly DentalAppDbContext _context;
        public PatientRepository(DentalAppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
        }

        public void Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.Include(p => p.Address).ToListAsync();
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients
        .Include(p => p.Address) // Optional, if you want address info too
        .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }


        /* public async Task<IEnumerable<Patient>> GetPagedAsync(int page, int size)
         {
             return await _context.Patients
                 .Include(p => p.Address)
                 .Skip((page - 1) * size)
                 .Take(size)
                 .ToListAsync();
         }
        */
        public async Task<(IEnumerable<Patient>, int)> GetPagedAsync(PagingRequest request)
        {
            var query = _context.Patients.AsQueryable();

            var total = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToListAsync();

            return (items, total);
        }

    }
}
