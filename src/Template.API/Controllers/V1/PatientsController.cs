using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.API.Common.Attributes;
using Template.API.Common.Extensions;
using Template.API.Controllers.Common;
using Template.API.Models;
using Template.API.Models.Common;
using Template.API.Models.Patients;
using Template.Application.Common.DTOs;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Patients.DTOs;
using Template.Application.Features.Patients.Services;
using Template.Domain.Enums;

namespace Template.API.Controllers.V1
{
    /// <summary>
    /// V1 Patients Controller - Core functionality
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class PatientsController : BaseController
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IPatientService patientService, IMapper mapper, ILogger<PatientsController> logger)
        {
            _patientService = patientService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of patients
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10, max: 100)</param>
        /// <returns>Paginated list of patients</returns>
        [HttpGet]
        [Cache]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<PatientResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedPatients = await _patientService.GetPatientsPaginatedAsync(pageNumber, pageSize);

            var patientResponses = paginatedPatients.Items.Select(p => new PatientResponse
            {
                Id = p.Id,
                Name = p.UserProfile.FullName,
                DateOfBirth = p.UserProfile.DateOfBirth!.Value
            });

            var result = new PaginatedResult<PatientResponse>(
                patientResponses,
                paginatedPatients.TotalCount,
                paginatedPatients.PageNumber,
                paginatedPatients.PageSize);

            return SuccessResponse(result);
        }

        /// <summary>
        /// Get a patient by ID
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns>Patient details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PatientResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patientDto = await _patientService.GetPatientByIdAsync(id);
            if (patientDto == null)
            {
                return HandleEntityNotFound("Patient", id);
            }

            var response = new PatientResponse
            {
                Id = patientDto.Id,
                Name = patientDto.UserProfile.FullName,
                DateOfBirth = patientDto.UserProfile.DateOfBirth!.Value
            };

            return HandleEntityFound(response, "Patient");
        }

        /// <summary>
        /// Create a new patient
        /// </summary>
        /// <param name="model">Patient creation details</param>
        /// <returns>Created patient</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PatientResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequest model)
        {
            if (ModelState.HasValidationErrors())
            {
                return BadRequestResponse(ModelState.GetErrorMessages());
            }

            // Parse name into first and last name
            var nameParts = model.Name.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

            var createPatientDto = new CreatePatientDto
            {
                UserProfile = new CreateUserProfileDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = "", // TODO: Add email field to CreatePatientRequest
                    PhoneNumber = "", // TODO: Add phone field to CreatePatientRequest
                    DateOfBirth = model.DateOfBirth
                }
            };

            var patientDto = await _patientService.CreatePatientAsync(createPatientDto);

            var response = new PatientResponse
            {
                Id = patientDto.Id,
                Name = patientDto.UserProfile.FullName,
                DateOfBirth = patientDto.UserProfile.DateOfBirth!.Value
            };

            return HandleEntityCreated(response, "Patient");
        }

        /// <summary>
        /// Update an existing patient
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="model">Patient update details</param>
        /// <returns>Updated patient</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PatientResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest model)
        {
            if (ModelState.HasValidationErrors())
            {
                return BadRequestResponse(ModelState.GetErrorMessages());
            }

            // Parse name into first and last name
            var nameParts = model.Name.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

            var updatePatientDto = new UpdatePatientDto
            {
                Id = id,
                UserProfile = new UserProfileDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = "", // TODO: Get existing email or add to request
                    PhoneNumber = "", // TODO: Get existing phone or add to request
                    DateOfBirth = model.DateOfBirth
                }
            };

            var patientDto = await _patientService.UpdatePatientAsync(id, updatePatientDto);
            if (patientDto == null)
            {
                return HandleEntityNotFound("Patient", id);
            }

            var response = new PatientResponse
            {
                Id = patientDto.Id,
                Name = patientDto.UserProfile.FullName,
                DateOfBirth = patientDto.UserProfile.DateOfBirth!.Value
            };

            return HandleEntityUpdated(response, "Patient");
        }

        /// <summary>
        /// Delete a patient
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns>Deletion confirmation</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeletePatientAsync(id);
            if (!result)
            {
                return HandleEntityNotFound("Patient", id);
            }

            return HandleEntityDeleted("Patient");
        }
    }
}
