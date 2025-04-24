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
    public class AddressControllerTests
    {
        private readonly Mock<IAddressService> _serviceMock;
        private readonly AddressController _controller;

        public AddressControllerTests()
        {
            _serviceMock = new Mock<IAddressService>();
            _controller = new AddressController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithAddresses()
        {
            var addresses = new List<AddressResponseDto>
            {
                new AddressResponseDto { AddressId = 1, FullAddress = "123 Main St" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(addresses);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<AddressResponseDto>>(ok.Value);
            value.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenFound()
        {
            var address = new AddressResponseDto { AddressId = 1, FullAddress = "123 Main St" };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(address);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<AddressResponseDto>(ok.Value);
            value.AddressId.Should().Be(1);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            var request = new AddressRequestDto
            {
                Street = "456 Elm",
                City = "Fairfield",
                State = "IA",
                Zip = "52557"
            };

            var response = new AddressResponseDto
            {
                AddressId = 2,
                FullAddress = "456 Elm, Fairfield, IA 52557"
            };

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            var result = await _controller.Create(request);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<AddressResponseDto>(created.Value);
            returned.AddressId.Should().Be(2);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            var request = new AddressRequestDto
            {
                Street = "789 Oak",
                City = "Ottumwa",
                State = "IA",
                Zip = "52501"
            };

            _serviceMock.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenFail()
        {
            var request = new AddressRequestDto
            {
                Street = "789 Oak",
                City = "Ottumwa",
                State = "IA",
                Zip = "52501"
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
        public async Task Delete_ShouldReturnNotFound_WhenFail()
        {
            _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
