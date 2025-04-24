using DentalApp.Api.Controllers;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.DTOs.Request;
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
    public class RoleControllerTests
    {
        private readonly Mock<IRoleService> _serviceMock;
        private readonly RoleController _controller;

        public RoleControllerTests()
        {
            _serviceMock = new Mock<IRoleService>();
            _controller = new RoleController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenFound()
        {
            var response = new RoleResponseDto { RoleId = 1, Name = "User" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<RoleResponseDto>(okResult.Value);
            returned.Name.Should().Be("User");
        }


        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((RoleResponseDto)null);

            var result = await _controller.GetById(99);

            var notFound = Assert.IsType<NotFoundResult>(result.Result); // 👈 unwrap the ActionResult<T>
        }



        [Fact]
        
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            var request = new CreateRoleRequestDto { Name = "Admin" };

            var response = new RoleResponseDto { RoleId = 1, Name = "Admin" };

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            var result = await _controller.Create(request);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<RoleResponseDto>(created.Value);
            returned.RoleId.Should().Be(1);
        }


        [Fact]
        public async Task Update_ShouldReturnOk_WhenSuccess()
        {
            var request = new CreateRoleRequestDto
            {
                Name = "Admin"
            };

            var response = new RoleResponseDto
            {
                RoleId = 1,
                Name = "Admin"
            };

            _serviceMock.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(response);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result); // unwrap ActionResult<RoleResponseDto>
            var role = Assert.IsType<RoleResponseDto>(okResult.Value);
            role.RoleId.Should().Be(1);
            role.Name.Should().Be("Admin");
        }


        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenNotFound()
        {
            _serviceMock.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);
            var result = await _controller.Delete(999);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
