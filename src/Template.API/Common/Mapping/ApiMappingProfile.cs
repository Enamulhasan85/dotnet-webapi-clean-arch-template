using AutoMapper;
using Template.API.Models;
using Template.Application.DTOs;

namespace Template.API.Common.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            // Patient mappings
            CreateMap<CreatePatientRequest, CreatePatientDto>().ReverseMap();
            CreateMap<UpdatePatientRequest, UpdatePatientDto>().ReverseMap();
            CreateMap<PatientDto, PatientResponse>().ReverseMap();

            // Doctor mappings
            CreateMap<CreateDoctorRequest, CreateDoctorDto>().ReverseMap();
            CreateMap<UpdateDoctorRequest, UpdateDoctorDto>().ReverseMap();
            CreateMap<DoctorDto, DoctorResponse>().ReverseMap();
        }
    }
}
