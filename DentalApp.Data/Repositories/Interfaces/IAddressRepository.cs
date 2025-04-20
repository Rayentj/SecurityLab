using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(int id);
        Task AddAsync(Address address);
        void Update(Address address);
        void Delete(Address address);
        Task<bool> SaveChangesAsync();
    }
}
