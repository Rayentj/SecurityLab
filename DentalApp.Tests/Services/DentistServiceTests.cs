using AutoMapper;
using DentalApp.Api.Mapper;
using DentalApp.Application.Exceptions;
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
    public class DentistServiceTests
    {
        private readonly Mock<IDentistRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly DentistService _service;

        public DentistServiceTests()
        {
            _repoMock = new Mock<IDentistRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _service = new DentistService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenExists()
        {
            var id = 1;
            var dentist = new Dentist { DentistId = id, FirstName = "Alice", LastName = "Smith" };
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(dentist);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.FullName.Should().Be("Alice Smith");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedDentistDto()
        {
            var request = new DentistRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Dob = DateTime.UtcNow.AddYears(-30),
                Specialization = "Orthodontics"
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Dentist>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateAsync(request);

            result.Should().NotBeNull();
            result.FullName.Should().Be("John Doe");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedDentistDto()
        {
            var id = 2;
            var request = new DentistRequestDto
            {
                FirstName = "Updated",
                LastName = "Dentist",
                Email = "updated@example.com",
                PhoneNumber = "9876543210",
                Dob = DateTime.UtcNow.AddYears(-35),
                Specialization = "Surgery"
            };

            var entity = new Dentist { DentistId = id };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, request);

            result.Should().NotBeNull();
            result.FullName.Should().Be("Updated Dentist");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDentistExists()
        {
            var id = 5;
            var entity = new Dentist { DentistId = id };
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenDentistNotFound()
        {
            var id = 99;
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Dentist)null);

            var act = async () => await _service.GetByIdAsync(id);
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Dentist with ID 99 not found");
        }
    }
}

