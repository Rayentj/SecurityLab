using DentalApp.Data.Repositories;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Context
{
    public class DentalAppDbContextFactory : IDesignTimeDbContextFactory<DentalAppDbContext>
    {
        public DentalAppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json") // make sure this file exists in your root
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DentalAppDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new DentalAppDbContext(optionsBuilder.Options);
        }
    }
}
