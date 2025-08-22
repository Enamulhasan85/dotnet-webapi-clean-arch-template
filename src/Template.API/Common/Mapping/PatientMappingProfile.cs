using AutoMapper;
using Template.API.Models.Patients;
using Template.Application.Features.Patients.DTOs;

namespace Template.API.Common.Mapping
{
    /// <summary>
    /// AutoMapper profile for Patient-related mappings
    /// </summary>
    public class PatientMappingProfile : Profile
    {
        public PatientMappingProfile()
        {
            // Request to DTO mappings
            CreateMap<CreatePatientRequest, PatientDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdatePatientRequest, PatientDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // DTO to Response mappings
            CreateMap<PatientDto, PatientResponse>();

            // Reverse mappings if needed
            CreateMap<PatientResponse, PatientDto>();
        }
    }
}
