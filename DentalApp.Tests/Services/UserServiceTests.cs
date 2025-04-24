using AutoMapper;
using DentalApp.Api.Mapper;
using DentalApp.Application.Services;
using DentalApp.Data.Repositories.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.Model;
using DentalApp.Infra.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repoMock = new Mock<IUserRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _service = new UserService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfUsers()
        {
            var users = new List<User>
            {
                new User { UserId = 1, FirstName = "Alice", LastName = "Smith", Role = new Role { Name = "Admin" } },
                new User { UserId = 2, FirstName = "Bob", LastName = "Johnson", Role = new Role { Name = "User" } }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(u => u.FullName == "Alice Smith" && u.RoleName == "Admin");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            var id = 1;
            var user = new User
            {
                UserId = id,
                FirstName = "Charlie",
                LastName = "Brown",
                Role = new Role { Name = "Support" }
            };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.FullName.Should().Be("Charlie Brown");
            result.RoleName.Should().Be("Support");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenEmailExists()
        {
            var request = new CreateUserRequestDto { Email = "duplicate@example.com" };
            _repoMock.Setup(r => r.FindByEmailAsync(request.Email)).ReturnsAsync(new User());

            var act = async () => await _service.CreateAsync(request);

            await act.Should().ThrowAsync<DuplicateEmailException>()
                .WithMessage("A user with email 'duplicate@example.com' already exists.");
        }


        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedUser()
        {
            var request = new CreateUserRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "secret",
                RoleId = 1
            };

            _repoMock.Setup(r => r.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateAsync(request);

            result.Should().NotBeNull();
            result.FullName.Should().Be("John Doe");
            result.Email.Should().Be("john@example.com");
        }
    }
}
