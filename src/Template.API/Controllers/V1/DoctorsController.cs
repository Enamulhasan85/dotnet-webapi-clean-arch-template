using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.API.Common.Attributes;
using Template.API.Common.Extensions;
using Template.API.Controllers.Common;
using Template.API.Models;
using Template.API.Models.Common;
using Template.API.Models.Doctors;
using Template.Application.Common.DTOs;
using Template.Application.Common.Models;
using Template.Application.Features.Doctors.DTOs;
using Template.Application.Features.Doctors.Services;
using Template.Domain.Enums;

namespace Template.API.Controllers.V1
{
    /// <summary>
    /// V1 Doctors Controller - Core functionality
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class DoctorsController : BaseController
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<DoctorsController> _logger;

        public DoctorsController(IDoctorService doctorService, ILogger<DoctorsController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of doctors
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10, max: 100)</param>
        /// <returns>Paginated list of doctors</returns>
        [HttpGet]
        [Cache]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<DoctorResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedDoctors = await _doctorService.GetDoctorsPaginatedAsync(pageNumber, pageSize);

            var doctorResponses = paginatedDoctors.Items.Select(d => new DoctorResponse
            {
                Id = d.Id,
                Name = d.UserProfile.FullName,
                Specialty = d.Specialty.ToString()
            });

            var result = new PaginatedResult<DoctorResponse>(
                doctorResponses,
                paginatedDoctors.TotalCount,
                paginatedDoctors.PageNumber,
                paginatedDoctors.PageSize);

            return SuccessResponse(result);
        }

        /// <summary>
        /// Get a doctor by ID
        /// </summary>
        /// <param name="id">Doctor ID</param>
        /// <returns>Doctor details</returns>
        [HttpGet("{id}")]
        [Cache(600)] // Cache for 10 minutes using default settings from IOptions
        [ProducesResponseType(typeof(ApiResponse<DoctorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctorDto = await _doctorService.GetDoctorByIdAsync(id);
            if (doctorDto == null)
            {
                return HandleEntityNotFound("Doctor", id);
            }

            var response = new DoctorResponse
            {
                Id = doctorDto.Id,
                Name = doctorDto.UserProfile.FullName,
                Specialty = doctorDto.Specialty.ToString()
            };

            return HandleEntityFound(response, "Doctor");
        }

        /// <summary>
        /// Create a new doctor
        /// </summary>
        /// <param name="model">Doctor creation details</param>
        /// <returns>Created doctor</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DoctorResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest model)
        {
            if (ModelState.HasValidationErrors())
            {
                return BadRequestResponse(ModelState.GetErrorMessages());
            }

            // Parse name into first and last name
            var nameParts = model.Name.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

            var createDoctorDto = new CreateDoctorDto
            {
                UserProfile = new CreateUserProfileDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = "", // TODO: Add email field to CreateDoctorRequest
                    PhoneNumber = "" // TODO: Add phone field to CreateDoctorRequest
                },
                Specialty = Enum.Parse<DoctorSpecialty>(model.Specialty, true)
            };

            var doctorDto = await _doctorService.CreateDoctorAsync(createDoctorDto);

            var response = new DoctorResponse
            {
                Id = doctorDto.Id,
                Name = doctorDto.UserProfile.FullName,
                Specialty = doctorDto.Specialty.ToString()
            };

            _logger.LogInformation("Doctor created successfully with ID: {DoctorId}", doctorDto.Id);
            return HandleEntityCreated(response, "Doctor");
        }

        /// <summary>
        /// Update an existing doctor
        /// </summary>
        /// <param name="id">Doctor ID</param>
        /// <param name="model">Doctor update details</param>
        /// <returns>Updated doctor</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<DoctorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorRequest model)
        {
            if (ModelState.HasValidationErrors())
            {
                return BadRequestResponse(ModelState.GetErrorMessages());
            }

            // Parse name into first and last name
            var nameParts = model.Name.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

            var updateDoctorDto = new UpdateDoctorDto
            {
                Id = id,
                UserProfile = new UserProfileDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = "", // TODO: Get existing email or add to request
                    PhoneNumber = "" // TODO: Get existing phone or add to request
                },
                Specialty = Enum.Parse<DoctorSpecialty>(model.Specialty, true)
            };

            var doctorDto = await _doctorService.UpdateDoctorAsync(id, updateDoctorDto);
            if (doctorDto == null)
            {
                return HandleEntityNotFound("Doctor", id);
            }

            var response = new DoctorResponse
            {
                Id = doctorDto.Id,
                Name = doctorDto.UserProfile.FullName,
                Specialty = doctorDto.Specialty.ToString()
            };

            return HandleEntityUpdated(response, "Doctor");
        }

        /// <summary>
        /// Delete a doctor
        /// </summary>
        /// <param name="id">Doctor ID</param>
        /// <returns>Deletion confirmation</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);
            if (!result)
            {
                return HandleEntityNotFound("Doctor", id);
            }

            return HandleEntityDeleted("Doctor");
        }
    }
}
