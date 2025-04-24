using DentalApp.Data.Repositories;
using DentalApp.Domain.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        private DentalAppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DentalAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new DentalAppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private async Task SeedDependencies(DentalAppDbContext context)
        {
            context.Dentists.Add(new Dentist
            {
                DentistId = 1,
                FirstName = "Andre",
                LastName = "Ross",
                Email = "andre@clinic.com",
                PhoneNumber = "1234567890",
                Specialization = "Orthodontist",
                Dob = DateTime.UtcNow.AddYears(-35)
            });

            context.Patients.Add(new Patient
            {
                PatientId = 2,
                FirstName = "Rayen",
                LastName = "Tajo",
                Email = "rayen@example.com",
                PhoneNumber = "9876543210",
                Dob = DateTime.UtcNow.AddYears(-25),
                AddressId = 1
            });

            //context.Patients.Add(new Patient { PatientId = 2, FirstName = "John", LastName = "Doe" });
            context.Surgeries.Add(new Surgery
            {
                SurgeryId = 1,
                Name = "DallasSurgery",
                PhoneNumber = "555-8888",
                AddressId = 1
            });

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddAndGetAppointment_ShouldWorkCorrectly()
        {
            var context = GetDbContext();
            await SeedDependencies(context);
            var repo = new AppointmentRepository(context);

            var appointment = new Appointment
            {
                DateTime = DateTime.UtcNow,
                DentistId = 1,
                PatientId = 2,
                SurgeryId = 1
            };

            await repo.AddAsync(appointment);
            await repo.SaveChangesAsync();

            var result = await repo.GetAllAsync();
            result.Should().ContainSingle();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectAppointment()
        {
            var context = GetDbContext();
            var repo = new AppointmentRepository(context);

            // Seed dependencies
            var dentist = new Dentist
            {
                DentistId = 1,
                FirstName = "Andre",
                LastName = "Ross",
                Email = "andre@miu.edu",
                PhoneNumber = "1234567890",
                Specialization = "Ortho"
            };

            var patient = new Patient
            {
                PatientId = 1,
                FirstName = "Rayen",
                LastName = "Tajo",
                Email = "rayen@example.com",
                PhoneNumber = "9876543210",
                Dob = DateTime.UtcNow.AddYears(-25),
                AddressId = 1
            };

            var address = new Address
            {
                AddressId = 1,
                Street = "Main",
                City = "Fairfield",
                State = "IA",
                Zip = "52557"
            };

            var surgery = new Surgery
            {
                SurgeryId = 1,
                Name = "DallasSurgery",
                PhoneNumber = "555-1234",
                AddressId = 1
            };

            context.Addresses.Add(address);
            context.Dentists.Add(dentist);
            context.Patients.Add(patient);
            context.Surgeries.Add(surgery);
            await context.SaveChangesAsync();

            // Create the appointment
            var appointment = new Appointment
            {
                DateTime = DateTime.UtcNow,
                DentistId = dentist.DentistId,
                PatientId = patient.PatientId,
                SurgeryId = surgery.SurgeryId
            };

            await repo.AddAsync(appointment);
            await repo.SaveChangesAsync();

            var fetched = await repo.GetByIdAsync(appointment.AppointmentId);
            fetched.Should().NotBeNull();
            fetched.AppointmentId.Should().Be(appointment.AppointmentId);
        }


        [Fact]
        public async Task Delete_ShouldRemoveAppointment()
        {
            var context = GetDbContext();
            await SeedDependencies(context);
            var repo = new AppointmentRepository(context);

            var appointment = new Appointment
            {
                DateTime = DateTime.UtcNow,
                DentistId = 1,
                PatientId = 1,
                SurgeryId = 1
            };

            await repo.AddAsync(appointment);
            await repo.SaveChangesAsync();

            repo.Delete(appointment);
            await repo.SaveChangesAsync();

            var result = await repo.GetByIdAsync(appointment.AppointmentId);
            result.Should().BeNull();
        }
    }
}
