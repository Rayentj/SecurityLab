using DentalApp.Data.Repositories.Interfaces;
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
    }
}
