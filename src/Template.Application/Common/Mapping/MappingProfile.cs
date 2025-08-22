using AutoMapper;
using Template.Application.DTOs;
using Template.Domain.Entities;

namespace Template.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Patient, CreatePatientDto>().ReverseMap();
            CreateMap<Patient, UpdatePatientDto>().ReverseMap();

            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<Doctor, CreateDoctorDto>().ReverseMap();
            CreateMap<Doctor, UpdateDoctorDto>().ReverseMap();
        }
    }
}
