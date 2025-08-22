using AutoMapper;
using Template.API.Models.Doctors;
using Template.Application.Features.Doctors.DTOs;

namespace Template.API.Common.Mapping
{
    /// <summary>
    /// AutoMapper profile for Doctor-related mappings
    /// </summary>
    public class DoctorMappingProfile : Profile
    {
        public DoctorMappingProfile()
        {
            // Request to DTO mappings
            CreateMap<CreateDoctorRequest, DoctorDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateDoctorRequest, DoctorDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // DTO to Response mappings
            CreateMap<DoctorDto, DoctorResponse>();

            // Reverse mappings if needed
            CreateMap<DoctorResponse, DoctorDto>();
        }
    }
}
