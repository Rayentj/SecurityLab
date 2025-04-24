using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories.Interfaces
{
    public interface IBillRepository
    {
        Task<IEnumerable<Bill>> GetAllAsync();
        Task<Bill> GetByIdAsync(int id);
        Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId);
        Task AddAsync(Bill bill);
        void Update(Bill bill);
        Task<bool> SaveChangesAsync();
    }
}
