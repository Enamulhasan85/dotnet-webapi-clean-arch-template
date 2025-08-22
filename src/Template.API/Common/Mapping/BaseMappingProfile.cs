using AutoMapper;
using Template.API.Models.Common;
using Template.Application.DTOs;

namespace Template.API.Common.Mapping
{
    /// <summary>
    /// Base AutoMapper profile for common mappings
    /// </summary>
    public class BaseMappingProfile : Profile
    {
        public BaseMappingProfile()
        {
            // Generic paginated result mappings
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResponse<>));

            // Common API response mappings can be added here
        }
    }
}
