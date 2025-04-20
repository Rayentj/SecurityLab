using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories.Interfaces
{
    public interface IDentistRepository
    {
        Task<IEnumerable<Dentist>> GetAllAsync();
        Task<Dentist> GetByIdAsync(int id);
        Task AddAsync(Dentist dentist);
        void Update(Dentist dentist);
        void Delete(Dentist dentist);
        Task<bool> SaveChangesAsync();
    }
}
