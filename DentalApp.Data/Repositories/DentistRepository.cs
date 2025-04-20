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
    public class DentistRepository : IDentistRepository
    {
        private readonly DentalAppDbContext _context;
        public DentistRepository(DentalAppDbContext context) => _context = context;

        public async Task<IEnumerable<Dentist>> GetAllAsync() => await _context.Dentists.ToListAsync();
        public async Task<Dentist> GetByIdAsync(int id) => await _context.Dentists.FindAsync(id);
        public async Task AddAsync(Dentist dentist) => await _context.Dentists.AddAsync(dentist);
        public void Update(Dentist dentist) => _context.Dentists.Update(dentist);
        public void Delete(Dentist dentist) => _context.Dentists.Remove(dentist);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
