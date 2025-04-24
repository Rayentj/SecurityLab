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
    public class SurgeryRepository : ISurgeryRepository
    {
        private readonly DentalAppDbContext _context;

        public SurgeryRepository(DentalAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Surgery>> GetAllAsync() =>
            await _context.Surgeries.ToListAsync();

        public async Task<Surgery> GetByIdAsync(int id) =>
            await _context.Surgeries.FindAsync(id);

        public async Task AddAsync(Surgery surgery) =>
            await _context.Surgeries.AddAsync(surgery);

        public void Update(Surgery surgery) =>
            _context.Surgeries.Update(surgery);

        public void Delete(Surgery surgery) =>
            _context.Surgeries.Remove(surgery);

        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;

        public async Task<(IEnumerable<Surgery>, int)> GetPagedAsync(PagingRequest request)
        {
            
            var query = _context.Surgeries.AsQueryable();

            var total = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToListAsync();

            return (items, total);
        
    }
    }
}