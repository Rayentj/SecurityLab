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
    public class SurgeryServiceTests
    {
        private readonly Mock<ISurgeryRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly SurgeryService _service;

        public SurgeryServiceTests()
        {
            _repoMock = new Mock<ISurgeryRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _service = new SurgeryService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSurgeries()
        {
            var surgeries = new List<Surgery>
            {
                new Surgery { SurgeryId = 1, Name = "Dental A", Address = new Address { Street = "123 A", City = "Fairfield", State = "IA", Zip = "52557" } },
                new Surgery { SurgeryId = 2, Name = "Dental B", Address = new Address { Street = "456 B", City = "Iowa City", State = "IA", Zip = "52240" } }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(surgeries);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(s => s.AddressSummary.Contains("Fairfield"));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSurgery_WhenExists()
        {
            var id = 1;
            var surgery = new Surgery
            {
                SurgeryId = id,
                Name = "Oral Clinic",
                Address = new Address { Street = "789 C", City = "Des Moines", State = "IA", Zip = "50309" }
            };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(surgery);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.Name.Should().Be("Oral Clinic");
            result.AddressSummary.Should().Be("789 C, Des Moines, IA 50309");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedSurgery()
        {
            var request = new SurgeryRequestDto
            {
                Name = "Pediatric Dentistry",
                PhoneNumber = "1234567890",
                AddressId = 1
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Surgery>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateAsync(request);

            result.Should().NotBeNull();
            result.Name.Should().Be("Pediatric Dentistry");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedSurgery_WhenExists()
        {
            var id = 3;
            var request = new SurgeryRequestDto
            {
                Name = "Implant Center",
                PhoneNumber = "0987654321",
                AddressId = 2
            };

            var entity = new Surgery { SurgeryId = id, Address = new Address() };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, request);

            result.Should().NotBeNull();
            result.Name.Should().Be("Implant Center");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenSurgeryExists()
        {
            var id = 4;
            var entity = new Surgery { SurgeryId = id };
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Surgery)null);

            var act = async () => await _service.GetByIdAsync(999);

            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Surgery ID 999 not found");
        }
    }
}
