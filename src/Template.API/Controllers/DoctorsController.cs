using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.API.Models;
using Template.Application.Interfaces;

namespace Template.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DoctorsController : ControllerBase
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
        [ProducesResponseType(typeof(ApiResponse<Template.Application.DTOs.PaginatedResult<DoctorResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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

                var paginatedDoctors = await _doctorService.GetDoctorsPaginatedAsync(pageNumber, pageSize);

                var doctorResponses = paginatedDoctors.Items.Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialty = d.Specialty
                });

                var result = new Template.Application.DTOs.PaginatedResult<DoctorResponse>(
                    doctorResponses,
                    paginatedDoctors.TotalCount,
                    paginatedDoctors.PageNumber,
                    paginatedDoctors.PageSize);

                return Ok(new ApiResponse<Template.Application.DTOs.PaginatedResult<DoctorResponse>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving doctors");
                return StatusCode(500, new ApiResponse("An error occurred while retrieving doctors"));
            }
        }

        /// <summary>
        /// Get a doctor by ID
        /// </summary>
        /// <param name="id">Doctor ID</param>
        /// <returns>Doctor details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<DoctorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctor(int id)
        {
            try
            {
                var doctorDto = await _doctorService.GetDoctorByIdAsync(id);
                if (doctorDto == null)
                {
                    return NotFound(new ApiResponse("Doctor not found"));
                }

                var response = new DoctorResponse
                {
                    Id = doctorDto.Id,
                    Name = doctorDto.Name,
                    Specialty = doctorDto.Specialty
                };

                return Ok(new ApiResponse<DoctorResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving doctor with ID: {DoctorId}", id);
                return StatusCode(500, new ApiResponse("An error occurred while retrieving doctor"));
            }
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

                var createDoctorDto = new Template.Application.DTOs.CreateDoctorDto
                {
                    Name = model.Name,
                    Specialty = model.Specialty
                };

                var doctorDto = await _doctorService.CreateDoctorAsync(createDoctorDto);

                var response = new DoctorResponse
                {
                    Id = doctorDto.Id,
                    Name = doctorDto.Name,
                    Specialty = doctorDto.Specialty
                };

                _logger.LogInformation("Doctor created successfully with ID: {DoctorId}", doctorDto.Id);
                return CreatedAtAction(nameof(GetDoctor), new { id = doctorDto.Id }, new ApiResponse<DoctorResponse>(response, "Doctor created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating doctor");
                return StatusCode(500, new ApiResponse("An error occurred while creating doctor"));
            }
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

                var updateDoctorDto = new Template.Application.DTOs.UpdateDoctorDto
                {
                    Name = model.Name,
                    Specialty = model.Specialty
                };

                var doctorDto = await _doctorService.UpdateDoctorAsync(id, updateDoctorDto);
                if (doctorDto == null)
                {
                    return NotFound(new ApiResponse("Doctor not found"));
                }

                var response = new DoctorResponse
                {
                    Id = doctorDto.Id,
                    Name = doctorDto.Name,
                    Specialty = doctorDto.Specialty
                };

                _logger.LogInformation("Doctor updated successfully with ID: {DoctorId}", doctorDto.Id);
                return Ok(new ApiResponse<DoctorResponse>(response, "Doctor updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating doctor with ID: {DoctorId}", id);
                return StatusCode(500, new ApiResponse("An error occurred while updating doctor"));
            }
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
            try
            {
                var result = await _doctorService.DeleteDoctorAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse("Doctor not found"));
                }

                _logger.LogInformation("Doctor deleted successfully with ID: {DoctorId}", id);
                return Ok(new ApiResponse("Doctor deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting doctor with ID: {DoctorId}", id);
                return StatusCode(500, new ApiResponse("An error occurred while deleting doctor"));
            }
        }
    }
}
