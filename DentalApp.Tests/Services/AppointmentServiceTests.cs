using AutoMapper;
using DentalApp.Api.Mapper;
using DentalApp.Application.Services;
using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.Model;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly Mock<IDentistRepository> _dentistRepoMock;
        private readonly Mock<IPatientRepository> _patientRepoMock;
        private readonly Mock<IBillRepository> _billRepoMock;
        private readonly IMapper _mapper;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _dentistRepoMock = new Mock<IDentistRepository>();
            _patientRepoMock = new Mock<IPatientRepository>();
            _billRepoMock = new Mock<IBillRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();

            _service = new AppointmentService(
                _repoMock.Object,
                _dentistRepoMock.Object,
                _patientRepoMock.Object,
                _billRepoMock.Object,
                _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenAppointmentExists()
        {
            var id = 1;
            var fakeAppointment = new Appointment
            {
                AppointmentId = id,
                DateTime = DateTime.UtcNow,
                Dentist = new Dentist { FirstName = "Alice", LastName = "Smith" },
                Patient = new Patient { FirstName = "John", LastName = "Doe" },
                Surgery = new Surgery { Name = "Central Clinic" }
            };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(fakeAppointment);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(id);
            result.DentistName.Should().Be("Alice Smith");
            result.PatientName.Should().Be("John Doe");
            result.SurgeryName.Should().Be("Central Clinic");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedAppointment()
        {
            var request = new AppointmentRequestDto
            {
                DateTime = DateTime.UtcNow,
                DentistId = 1,
                PatientId = 2,
                SurgeryId = 3
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateAsync(request);

            result.Should().NotBeNull();
            result.DentistName.Should().BeNull();
            result.PatientName.Should().BeNull();
            result.SurgeryName.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenAppointmentExists()
        {
            var id = 1;
            var request = new AppointmentRequestDto
            {
                DateTime = DateTime.UtcNow.AddDays(1),
                DentistId = 1,
                PatientId = 2,
                SurgeryId = 3
            };

            var appointment = new Appointment
            {
                AppointmentId = id,
                DateTime = DateTime.UtcNow,
                DentistId = 1,
                PatientId = 2,
                SurgeryId = 3
            };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, request);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenAppointmentExists()
        {
            var id = 1;
            var appointment = new Appointment { AppointmentId = id };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
        }
    }
}
