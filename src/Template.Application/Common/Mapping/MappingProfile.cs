using AutoMapper;
using Template.Application.Common.DTOs;
using Template.Application.Features.Doctors.DTOs;
using Template.Application.Features.Patients.DTOs;
using Template.Domain.Entities;
using Template.Domain.ValueObjects;

namespace Template.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient mappings
        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src.UserProfile));
        CreateMap<CreatePatientDto, Patient>()
            .ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src.UserProfile));
        CreateMap<UpdatePatientDto, Patient>()
            .ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src.UserProfile));

        // Doctor mappings
        CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src.UserProfile));
        CreateMap<CreateDoctorDto, Doctor>()
            .ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src.UserProfile));
        CreateMap<UpdateDoctorDto, Doctor>()
            .ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src.UserProfile));

        // UserProfile mappings
        CreateMap<UserProfile, UserProfileDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email != null ? src.Email.Value : string.Empty))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber.Value : string.Empty))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        CreateMap<CreateUserProfileDto, UserProfile>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => new PhoneNumber(src.PhoneNumber)))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ?
                new Address(src.Address.Street, src.Address.City, src.Address.State, src.Address.PostalCode, src.Address.Country) : null));

        // Address mappings
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}
