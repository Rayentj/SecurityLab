using AutoMapper;
using DentalApp.Domain.DTOs.Request;
using DentalApp.Domain.DTOs.Response;
using DentalApp.Domain.Model;
using Org.BouncyCastle.Crypto.Generators;

namespace DentalApp.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            // Patient
            CreateMap<Patient, PatientResponseDto>()
    .ForMember(dest => dest.FullName,
               opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
    .ForMember(dest => dest.AddressSummary,
               opt => opt.MapFrom(src =>
                   $"{src.Address.Street}, {src.Address.City}, {src.Address.State} {src.Address.Zip}"));
    CreateMap<PatientRequestDto, Patient>();
            // Dentist
            CreateMap<DentistRequestDto, Dentist>().ReverseMap();

            CreateMap<Dentist, DentistResponseDto>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            // Address
            CreateMap<AddressRequestDto, Address>().ReverseMap();
            CreateMap<Address, AddressResponseDto>()
                .ForMember(dest => dest.FullAddress,
                           opt => opt.MapFrom(src =>
                               $"{src.Street}, {src.City}, {src.State} {src.Zip}"))
                .ForMember(dest => dest.PatientNames,
                           opt => opt.MapFrom(src =>
                               src.Patients.Select(p => $"{p.FirstName} {p.LastName}").ToList()))
                .ForMember(dest => dest.SurgeryNames,
                           opt => opt.MapFrom(src =>
                               src.Surgeries.Select(s => s.Name).ToList()));

            // Surgery
            CreateMap<SurgeryRequestDto, Surgery>().ReverseMap();

            CreateMap<Surgery, SurgeryResponseDto>()
                .ForMember(dest => dest.AddressSummary,
                           opt => opt.MapFrom(src =>
                               $"{src.Address.Street}, {src.Address.City}, {src.Address.State} {src.Address.Zip}"));

            // Appointment
            CreateMap<AppointmentRequestDto, Appointment>().ReverseMap();

            CreateMap<Appointment, AppointmentResponseDto>()
                .ForMember(dest => dest.DentistName,
                           opt => opt.MapFrom(src => $"{src.Dentist.FirstName} {src.Dentist.LastName}"))
                .ForMember(dest => dest.PatientName,
                           opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.SurgeryName,
                           opt => opt.MapFrom(src => src.Surgery.Name));

            //role and user


            CreateMap<CreateUserRequestDto, User>()
                .ForMember(dest => dest.PasswordHash,
                           opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.RoleName,
                           opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<CreateRoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();



        }
    }
}
