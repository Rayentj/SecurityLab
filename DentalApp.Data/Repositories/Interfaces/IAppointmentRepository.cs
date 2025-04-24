using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task AddAsync(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Appointment appointment);
        Task<bool> SaveChangesAsync();
        public  Task<IEnumerable<Appointment>> GetByDentistIdAsync(int dentistId);
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsForDentistInRange(int dentistId, DateTime start, DateTime end);


        Task<(IEnumerable<Appointment>, int)> GetPagedAsync(PagingRequest request);

    }
}
