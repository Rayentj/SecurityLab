using DentalApp.Application.Services;
using DentalApp.Application.Services.Interfaces;
using DentalApp.Data.Repositories;
using DentalApp.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Infra
{
    public class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, PatientService>();

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAddressService, AddressService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<TokenService>();

            services.AddScoped<IDentistRepository, DentistRepository>();
            services.AddScoped<IDentistService, DentistService>();


            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();


            services.AddScoped<ISurgeryRepository, SurgeryRepository>();
            services.AddScoped<ISurgeryService, SurgeryService>();

            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBillService, BillService>();




        }
    }
}
