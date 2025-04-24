using DentalApp.Api.Controllers;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _serviceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _serviceMock = new Mock<IUserService>();
            _controller = new UserController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithUserList()
        {
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { UserId = 1, FullName = "Rayen Tajouri", Email = "rayen@miu.edu" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(ok.Value);
            value.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenUserExists()
        {
            var user = new UserResponseDto { UserId = 1, FullName = "Rayen Tajouri" };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserResponseDto>(ok.Value);
            value.UserId.Should().Be(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenUserMissing()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((UserResponseDto)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenSuccessful()
        {
            var request = new CreateUserRequestDto
            {
                FirstName = "Rayen",
                LastName = "Tajouri",
                Email = "rayen@miu.edu",
                Password = "pass123"
            };

            var response = new UserResponseDto
            {
                UserId = 1,
                FullName = "Rayen Tajouri",
                Email = "rayen@miu.edu"
            };

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            var result = await _controller.Create(request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var value = Assert.IsType<UserResponseDto>(created.Value);
            value.UserId.Should().Be(1);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenSuccessful()
        {
            var request = new CreateUserRequestDto
            {
                FirstName = "Updated",
                LastName = "User",
                Email = "updated@miu.edu",
                Password = "updated123"
            };

            var updated = new UserResponseDto
            {
                UserId = 1,
                FullName = "Updated User"
            };

            _serviceMock.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(updated);

            var result = await _controller.Update(1, request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<UserResponseDto>(ok.Value);
            value.FullName.Should().Be("Updated User");
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenUserMissing()
        {
            _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
