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
    public class PatientControllerTests
    {
        private readonly Mock<IPatientService> _serviceMock;
        private readonly PatientApiController _controller;

        public PatientControllerTests()
        {
            _serviceMock = new Mock<IPatientService>();
            var appointmentMock = new Mock<IAppointmentService>();
            _controller = new PatientApiController(_serviceMock.Object, appointmentMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenPatientExists()
        {
            // Arrange
            var id = 1;
            var expected = new PatientResponseDto
            {
                PatientId = id,
                FullName = "Rayen Tajouri",
                Email = "rayen@miu.edu",
                PhoneNumber = "1234567890"
            };

            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(expected);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returned = okResult.Value.Should().BeAssignableTo<PatientResponseDto>().Subject;
            returned.FullName.Should().Be("Rayen Tajouri");
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenPatientMissing()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((PatientResponseDto)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            // Arrange
            var request = new PatientRequestDto
            {
                FirstName = "Rayen",
                LastName = "Tajouri",
                PhoneNumber = "1234567890",
                Email = "rayen@miu.edu",
                Dob = DateTime.UtcNow.AddYears(-25),
                AddressId = 1
            };

            var response = new PatientResponseDto
            {
                PatientId = 1,
                FullName = "Rayen Tajouri",
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var id = 1;
            var request = new PatientRequestDto
            {
                FirstName = "Updated",
                LastName = "Name",
                Email = "updated@mail.com",
                PhoneNumber = "22222222",
                Dob = DateTime.UtcNow.AddYears(-30),
                AddressId = 2
            };

            _serviceMock.Setup(s => s.UpdateAsync(id, request)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(id, request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<PatientRequestDto>()))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(999, new PatientRequestDto());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var id = 1;
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenPatientMissing()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
