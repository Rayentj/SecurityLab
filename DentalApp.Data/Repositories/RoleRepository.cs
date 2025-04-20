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
    public class RoleRepository : IRoleRepository
    {
        private readonly DentalAppDbContext _context;
        public RoleRepository(DentalAppDbContext context) => _context = context;

        public async Task<IEnumerable<Role>> GetAllAsync() => await _context.Roles.ToListAsync();

        public async Task<Role> GetByIdAsync(int id) => await _context.Roles.FindAsync(id);

        public async Task AddAsync(Role role) => await _context.Roles.AddAsync(role);

        public void Update(Role role) => _context.Roles.Update(role);

        public void Delete(Role role) => _context.Roles.Remove(role);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
