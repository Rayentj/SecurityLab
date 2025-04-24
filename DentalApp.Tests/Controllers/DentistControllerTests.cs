using DentalApp.Api.Controllers;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Language.Flow; // sometimes needed

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Tests.Controllers
{
    public class DentistControllerTests
    {
        private readonly Mock<IDentistService> _serviceMock;
        private readonly DentistController _controller;

        public DentistControllerTests()
        {
            _serviceMock = new Mock<IDentistService>();
            var appointmentMock = new Mock<IAppointmentService>();
            _controller = new DentistController(_serviceMock.Object, appointmentMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenDentistExists()
        {
            var id = 1;
            var expected = new DentistResponseDto { DentistId = id, FullName = "Rayen Dentist" };
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(expected);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<DentistResponseDto>(okResult.Value);
            returned.DentistId.Should().Be(id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DentistResponseDto)null);

            var result = await _controller.GetById(999);
            var notFound = Assert.IsType<NotFoundResult>(result.Result);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            var request = new DentistRequestDto
            {
                FirstName = "Rayen",
                LastName = "Tajouri",
                Email = "rayen@miu.edu",
                PhoneNumber = "1234567890",
                Dob = DateTime.UtcNow.AddYears(-25),
                Specialization = "Orthodontics"
            };

            var response = new DentistResponseDto
            {
                DentistId = 1,
                FullName = "Rayen Tajouri",
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            var result = await _controller.Create(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdValue = Assert.IsType<DentistResponseDto>(createdResult.Value);
            createdValue.DentistId.Should().Be(1);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenSuccessful()
        {
            var request = new DentistRequestDto
            {
                FirstName = "Update",
                LastName = "Dentist",
                Email = "update@miu.edu",
                PhoneNumber = "9876543210",
                Dob = DateTime.UtcNow.AddYears(-30),
                Specialization = "Surgery"
            };

            var response = new DentistResponseDto
            {
                DentistId = 1,
                FullName = "Updated Dentist",
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            _serviceMock.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(response);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result); // <- fix here
            var updated = Assert.IsType<DentistResponseDto>(okResult.Value);
            updated.DentistId.Should().Be(1);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
