using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.API.Models;
using Template.Application.Common.Interfaces;

namespace Template.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PatientsController : ControllerBase
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
        [ProducesResponseType(typeof(ApiResponse<Template.Application.DTOs.PaginatedResult<PatientResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1)
                {
                    return BadRequest(new ApiResponse("Page number must be greater than 0"));
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest(new ApiResponse("Page size must be between 1 and 100"));
                }

                var paginatedPatients = await _patientService.GetPatientsPaginatedAsync(pageNumber, pageSize);

                var patientResponses = paginatedPatients.Items.Select(p => new PatientResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth
                });

                var result = new Template.Application.DTOs.PaginatedResult<PatientResponse>(
                    patientResponses,
                    paginatedPatients.TotalCount,
                    paginatedPatients.PageNumber,
                    paginatedPatients.PageSize);

                return Ok(new ApiResponse<Template.Application.DTOs.PaginatedResult<PatientResponse>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving patients");
                return StatusCode(500, new ApiResponse("An error occurred while retrieving patients"));
            }
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
            try
            {
                var patientDto = await _patientService.GetPatientByIdAsync(id);
                if (patientDto == null)
                {
                    return NotFound(new ApiResponse("Patient not found"));
                }

                var response = new PatientResponse
                {
                    Id = patientDto.Id,
                    Name = patientDto.Name,
                    DateOfBirth = patientDto.DateOfBirth
                };

                return Ok(new ApiResponse<PatientResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving patient with ID: {PatientId}", id);
                return StatusCode(500, new ApiResponse("An error occurred while retrieving patient"));
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponse(errors));
                }

                var createPatientDto = new Template.Application.DTOs.CreatePatientDto
                {
                    Name = model.Name,
                    DateOfBirth = model.DateOfBirth
                };

                var patientDto = await _patientService.CreatePatientAsync(createPatientDto);

                var response = new PatientResponse
                {
                    Id = patientDto.Id,
                    Name = patientDto.Name,
                    DateOfBirth = patientDto.DateOfBirth
                };

                _logger.LogInformation("Patient created successfully with ID: {PatientId}", patientDto.Id);
                return CreatedAtAction(nameof(GetPatient), new { id = patientDto.Id }, new ApiResponse<PatientResponse>(response, "Patient created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating patient");
                return StatusCode(500, new ApiResponse("An error occurred while creating patient"));
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponse(errors));
                }

                var updatePatientDto = new Template.Application.DTOs.UpdatePatientDto
                {
                    Name = model.Name,
                    DateOfBirth = model.DateOfBirth
                };

                var patientDto = await _patientService.UpdatePatientAsync(id, updatePatientDto);
                if (patientDto == null)
                {
                    return NotFound(new ApiResponse("Patient not found"));
                }

                var response = new PatientResponse
                {
                    Id = patientDto.Id,
                    Name = patientDto.Name,
                    DateOfBirth = patientDto.DateOfBirth
                };

                _logger.LogInformation("Patient updated successfully with ID: {PatientId}", patientDto.Id);
                return Ok(new ApiResponse<PatientResponse>(response, "Patient updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating patient with ID: {PatientId}", id);
                return StatusCode(500, new ApiResponse("An error occurred while updating patient"));
            }
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
            try
            {
                var result = await _patientService.DeletePatientAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse("Patient not found"));
                }

                _logger.LogInformation("Patient deleted successfully with ID: {PatientId}", id);
                return Ok(new ApiResponse("Patient deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting patient with ID: {PatientId}", id);
                return StatusCode(500, new ApiResponse("An error occurred while deleting patient"));
            }
        }
    }
}
