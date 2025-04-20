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
    public class AddressRepository : IAddressRepository
    {
        private readonly DentalAppDbContext _context;
        public AddressRepository(DentalAppDbContext context) => _context = context;

        // ✅ Include Patients and Surgeries for full DTO mapping
        public async Task<IEnumerable<Address>> GetAllAsync() =>
            await _context.Addresses
                          .Include(a => a.Patients)
                          .Include(a => a.Surgeries)
                          .ToListAsync();

        public async Task<Address> GetByIdAsync(int id) =>
            await _context.Addresses
                          .Include(a => a.Patients)
                          .Include(a => a.Surgeries)
                          .FirstOrDefaultAsync(a => a.AddressId == id);

        public async Task AddAsync(Address address) => await _context.Addresses.AddAsync(address);
        public void Update(Address address) => _context.Addresses.Update(address);
        public void Delete(Address address) => _context.Addresses.Remove(address);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }

}
