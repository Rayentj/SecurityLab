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
    public class UserRepository : IUserRepository
    {
        private readonly DentalAppDbContext _context;
        public UserRepository(DentalAppDbContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.Include(u => u.Role).ToListAsync();
        public async Task<User> GetByIdAsync(int id) => await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
        public async Task AddAsync(User user) => await _context.Users.AddAsync(user);
        public void Update(User user) => _context.Users.Update(user);
        public void Delete(User user) => _context.Users.Remove(user);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role)
                                       .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
