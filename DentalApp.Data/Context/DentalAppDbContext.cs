using DentalApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Data.Repositories
{
    public class DentalAppDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Dentist> Dentists { get; set; }
        public DbSet<Surgery> Surgeries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DentalAppDbContext(DbContextOptions<DentalAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Unique constraint for Address
            modelBuilder.Entity<Address>()
                .HasIndex(a => new { a.Street, a.City, a.State, a.Zip })
                .IsUnique();

            // Relationships
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Address)
                .WithMany(a => a.Patients)
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dentist>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Dentist)
                .HasForeignKey(a => a.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Surgery>()
                .HasMany(s => s.Appointments)
                .WithOne(a => a.Surgery)
                .HasForeignKey(a => a.SurgeryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Surgery>()
                .HasOne(s => s.Address)
                .WithMany(a => a.Surgeries)
                .HasForeignKey(s => s.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
