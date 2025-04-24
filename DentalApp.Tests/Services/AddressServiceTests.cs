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
    public class AddressServiceTests
    {
        private readonly Mock<IAddressRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly AddressService _service;

        public AddressServiceTests()
        {
            _repoMock = new Mock<IAddressRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _service = new AddressService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenExists()
        {
            var address = new Address
            {
                AddressId = 1,
                Street = "123 Main St",
                City = "Fairfield",
                State = "IA",
                Zip = "52557",
                Patients = new List<Patient> {
                    new Patient { FirstName = "John", LastName = "Doe" }
                },
                Surgeries = new List<Surgery> {
                    new Surgery { Name = "Surgery A" }
                }
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(address);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.FullAddress.Should().Be("123 Main St, Fairfield, IA 52557");
            result.PatientNames.Should().ContainSingle().And.Contain("John Doe");
            result.SurgeryNames.Should().Contain("Surgery A");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedDto()
        {
            var request = new AddressRequestDto
            {
                Street = "456 Elm St",
                City = "Iowa City",
                State = "IA",
                Zip = "52240"
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Address>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateAsync(request);

            result.Should().NotBeNull();
            result.FullAddress.Should().Be("456 Elm St, Iowa City, IA 52240");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenAddressExists()
        {
            var id = 2;
            var entity = new Address { AddressId = id };
            var request = new AddressRequestDto
            {
                Street = "789 Pine St",
                City = "Des Moines",
                State = "IA",
                Zip = "50309"
            };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, request);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenAddressExists()
        {
            var id = 3;
            var address = new Address { AddressId = id };
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(address);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenAddressNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Address)null);

            var act = async () => await _service.UpdateAsync(99, new AddressRequestDto());

            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Address ID 99 not found");
        }
    }
}
