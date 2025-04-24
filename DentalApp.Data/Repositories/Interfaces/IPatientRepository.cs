using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
        Task<bool> SaveChangesAsync();
        //Task<IEnumerable<Patient>> GetPagedAsync(int page, int size);
        Task<(IEnumerable<Patient>, int)> GetPagedAsync(PagingRequest request);


    }
}
