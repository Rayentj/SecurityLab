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
    public class SurgeryControllerTests
    {
        private readonly Mock<ISurgeryService> _serviceMock;
        private readonly SurgeryController _controller;

        public SurgeryControllerTests()
        {
            _serviceMock = new Mock<ISurgeryService>();
            _controller = new SurgeryController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithSurgeries()
        {
            var surgeries = new List<SurgeryResponseDto>
            {
                new SurgeryResponseDto { SurgeryId = 1, Name = "Dental Plus" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(surgeries);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<SurgeryResponseDto>>(ok.Value);
            value.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenFound()
        {
            var surgery = new SurgeryResponseDto { SurgeryId = 1, Name = "Smile Dental" };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(surgery);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<SurgeryResponseDto>(ok.Value);
            value.SurgeryId.Should().Be(1);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            var request = new SurgeryRequestDto
            {
                Name = "Healthy Teeth",
                PhoneNumber = "1234567890",
                AddressId = 1
            };

            var response = new SurgeryResponseDto
            {
                SurgeryId = 2,
                Name = "Healthy Teeth"
            };

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            var result = await _controller.Create(request);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var value = Assert.IsType<SurgeryResponseDto>(created.Value);
            value.SurgeryId.Should().Be(2);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenSuccessful()
        {
            var request = new SurgeryRequestDto
            {
                Name = "Updated Clinic",
                PhoneNumber = "9876543210",
                AddressId = 2
            };

            var response = new SurgeryResponseDto
            {
                SurgeryId = 1,
                Name = "Updated Clinic"
            };

            _serviceMock.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(response);

            var result = await _controller.Update(1, request);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<SurgeryResponseDto>(ok.Value);
            value.Name.Should().Be("Updated Clinic");
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenFail()
        {
            _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
