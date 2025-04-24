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
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly RoleService _service;

        public RoleServiceTests()
        {
            _repoMock = new Mock<IRoleRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _service = new RoleService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfRoles()
        {
            var roles = new List<Role>
            {
                new Role { RoleId = 1, Name = "Admin" },
                new Role { RoleId = 2, Name = "User" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(roles);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(r => r.Name == "Admin");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRole_WhenExists()
        {
            var id = 1;
            var role = new Role { RoleId = id, Name = "Manager" };
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(role);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.Name.Should().Be("Manager");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenRoleNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Role)null);

            var act = async () => await _service.GetByIdAsync(99);

            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Role with ID 99 not found");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedRole()
        {
            var request = new CreateRoleRequestDto { Name = "Support" };
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateAsync(request);

            result.Should().NotBeNull();
            result.Name.Should().Be("Support");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedRole_WhenExists()
        {
            var id = 3;
            var dto = new CreateRoleRequestDto { Name = "Coordinator" };
            var existing = new Role { RoleId = id, Name = "OldRole" };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, dto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Coordinator");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRoleExists()
        {
            var id = 4;
            var role = new Role { RoleId = id };
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(role);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
        }
    }
}
