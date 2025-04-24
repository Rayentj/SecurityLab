using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories.Interfaces
{
    public interface ISurgeryRepository
    {
        Task<IEnumerable<Surgery>> GetAllAsync();
        Task<Surgery> GetByIdAsync(int id);
        Task AddAsync(Surgery surgery);
        void Update(Surgery surgery);
        void Delete(Surgery surgery);
        Task<bool> SaveChangesAsync();
        Task<(IEnumerable<Surgery>, int)> GetPagedAsync(PagingRequest request);
    }

}
