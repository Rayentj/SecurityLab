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
    public class BillRepository : IBillRepository
    {
        private readonly DentalAppDbContext _context;

        public BillRepository(DentalAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bill>> GetAllAsync() =>
            await _context.Bills.Include(b => b.Patient).Include(b => b.Appointment).ToListAsync();

        public async Task<Bill> GetByIdAsync(int id) =>
            await _context.Bills.Include(b => b.Patient).Include(b => b.Appointment)
                .FirstOrDefaultAsync(b => b.BillId == id);

        public async Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId) =>
            await _context.Bills.Include(b => b.Appointment)
                .Where(b => b.PatientId == patientId).ToListAsync();

        public async Task AddAsync(Bill bill) => await _context.Bills.AddAsync(bill);

        public void Update(Bill bill) => _context.Bills.Update(bill);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
