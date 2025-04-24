using AutoMapper;
using DentalApp.Api.Mapper;
using DentalApp.Application.Services;
using DentalApp.Data.Repositories.Interfaces;
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
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IPatientRepository>();

            // Set up AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); // your actual profile
            });
            _mapper = config.CreateMapper();

            _service = new PatientService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatientResponseDto_WhenPatientExists()
        {
            // Arrange
            var id = 1;
            var fakePatient = new Patient { PatientId = id, FirstName = "John", LastName = "Doe" };
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(fakePatient);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.FullName.Should().Be("John Doe");
        }
    }

}
