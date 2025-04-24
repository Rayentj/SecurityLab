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
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _serviceMock;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _serviceMock = new Mock<IAppointmentService>();
            _controller = new AppointmentController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithAppointments()
        {
            var list = new List<AppointmentResponseDto>
            {
                new AppointmentResponseDto { AppointmentId = 1, PatientName = "John", DentistName = "Dr. Rayen" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<AppointmentResponseDto>>(ok.Value);
            value.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenFound()
        {
            var dto = new AppointmentResponseDto { AppointmentId = 1, PatientName = "John" };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<AppointmentResponseDto>(ok.Value);
            value.AppointmentId.Should().Be(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((AppointmentResponseDto)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            var request = new AppointmentRequestDto
            {
                PatientId = 1,
                DentistId = 2,
                SurgeryId = 3,
                DateTime = DateTime.Now
            };

            var response = new AppointmentResponseDto
            {
                AppointmentId = 10,
                PatientName = "John",
                DentistName = "Dr. Rayen"
            };

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            var result = await _controller.Create(request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var value = Assert.IsType<AppointmentResponseDto>(created.Value);
            value.AppointmentId.Should().Be(10);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            var request = new AppointmentRequestDto
            {
                PatientId = 1,
                DentistId = 2,
                SurgeryId = 3,
                DateTime = DateTime.Now
            };

            _serviceMock.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenNotExists()
        {
            var request = new AppointmentRequestDto
            {
                PatientId = 1,
                DentistId = 2,
                SurgeryId = 3,
                DateTime = DateTime.Now
            };

            _serviceMock.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(false);

            var result = await _controller.Update(1, request);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
