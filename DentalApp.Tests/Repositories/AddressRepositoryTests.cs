using DentalApp.Data.Repositories;
using DentalApp.Domain.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Tests.Repositories
{
    public class AddressRepositoryTests
    {
        private async Task<DentalAppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DentalAppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid().ToString())
                .Options;
            var context = new DentalAppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAddresses()
        {
            using var context = await GetInMemoryDbContext();
            var address = new Address { Street = "Main", City = "City", State = "ST", Zip = "00000" };
            await context.Addresses.AddAsync(address);
            await context.SaveChangesAsync();

            var repo = new AddressRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectAddress()
        {
            using var context = await GetInMemoryDbContext();
            var address = new Address { Street = "Main", City = "City", State = "ST", Zip = "00000" };
            await context.Addresses.AddAsync(address);
            await context.SaveChangesAsync();

            var repo = new AddressRepository(context);
            var result = await repo.GetByIdAsync(address.AddressId);

            result.Should().NotBeNull();
            result.Street.Should().Be("Main");
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewAddress()
        {
            using var context = await GetInMemoryDbContext();
            var repo = new AddressRepository(context);

            var address = new Address { Street = "Elm", City = "Another", State = "XY", Zip = "11111" };
            await repo.AddAsync(address);
            var saved = await repo.SaveChangesAsync();

            saved.Should().BeTrue();
            context.Addresses.Should().ContainSingle(a => a.Street == "Elm");
        }

        [Fact]
        public async Task Update_ShouldModifyExistingAddress()
        {
            using var context = await GetInMemoryDbContext();
            var address = new Address { Street = "Old", City = "Old", State = "OL", Zip = "12345" };
            context.Addresses.Add(address);
            await context.SaveChangesAsync();

            var repo = new AddressRepository(context);
            address.City = "NewCity";
            repo.Update(address);
            await repo.SaveChangesAsync();

            var updated = await context.Addresses.FindAsync(address.AddressId);
            updated.City.Should().Be("NewCity");
        }

        [Fact]
        public async Task Delete_ShouldRemoveAddress()
        {
            using var context = await GetInMemoryDbContext();
            var address = new Address { Street = "ToDelete", City = "Delete", State = "DL", Zip = "99999" };
            context.Addresses.Add(address);
            await context.SaveChangesAsync();

            var repo = new AddressRepository(context);
            repo.Delete(address);
            await repo.SaveChangesAsync();

            var deleted = await context.Addresses.FindAsync(address.AddressId);
            deleted.Should().BeNull();
        }
    }
}
