using Application.Exceptions;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Domain.Entities;
using Application.Interfaces.IServices.IFieldServices;
using Azure.Core;
using FluentValidation;

namespace FieldMicroservice.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class FieldController : ControllerBase
    {
        private readonly IFieldGetServices _fieldGetServices;
        private readonly IFieldPostServices _fieldPostServices;
        private readonly IFieldPutServices _fieldPutServices;

        public FieldController(IFieldGetServices fieldGetServices, IFieldPostServices fieldPostServices, IFieldPutServices fieldPutServices)
        {
            _fieldGetServices = fieldGetServices;
            _fieldPostServices = fieldPostServices;
            _fieldPutServices = fieldPutServices;
        }


        /// <summary>
        /// Retrieves a list of fields based on the provided filters such as 
        /// project id, name, size, type, availability, with optional pagination parameters.
        /// </summary>
        /// <param name="name">Optional. Filter by name of field.</param>
        /// <param name="sizeoffield">Optional. Filter by size of field.</param>
        /// <param name="type">Optional. Filter by type of field.</param>
        /// <param name="availability">Optional. Filter by availability ID.</param>

        /// <param name="offset">Optional. Skip the specified number of records (used for pagination).</param>
        /// <param name="size">Optional. Limit the number of records returned (used for pagination).</param>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<FieldResponse>), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<ActionResult> GetAllFields(
            [FromQuery] string? name,
            [FromQuery] string? sizeoffield,
            [FromQuery] int? type,
            [FromQuery] int? availability,
            [FromQuery] int? offset,
            [FromQuery] int? size)
        {
            try
            {
                var result = await _fieldGetServices.GetAllFields(name, sizeoffield, type, availability, offset, size);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }


        /// <summary>
        /// Retrieves detailed information about a specific field by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the field.</param>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FieldResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> GetFieldById([FromRoute] Guid id)
        {
            try
            {
                var result = await _fieldGetServices.GetFieldById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }


        /// <summary>
        /// Creates a new Field with the specified details.
        /// </summary> 
        /// <param name="request">The details of the Field to be created.</param>
        /// <response code="201">Field created successfully.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(FieldResponse), 201)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> CreateField([FromBody] FieldRequest request)
        {
            try
            {
                var result = await _fieldPostServices.CreateField(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Update an existing Field with the specified details.
        /// </summary>
        /// <param name="id">The unique identifier of the field to be updated.</param>
        /// <param name="request">The updated details of the field.</param>
        /// <response code="200">Success</response> 
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FieldResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> UpdateField([FromRoute] Guid id, [FromBody] FieldRequest request)
        {
            try
            {
                var result = await _fieldPutServices.UpdateField(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }


        /// <summary>
        /// Adds a new Availability to an existing field.
        /// </summary>
        /// <param name="Id">The unique identifier of the field.</param>
        /// <param name="request">The details of the availability to be added.</param>
        /// <response code="201">Success</response>
        /// <returns>The field availabilities.</returns>
        [Authorize]
        [HttpPatch("{Id}/Availability")]
        public async Task<IActionResult> CreateAvailability(Guid Id, AvailabilityRequest request)
        {
            try
            {
                var result = await _fieldPutServices.CreateAvailability(Id, request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Update an existing Availability with the specified details.
        /// </summary>
        /// <param name="Id">The unique identifier of the availability to be updated.</param>
        /// <param name="request">The updated details of the availability.</param>
        /// <response code="200">Success</response> 
        [Authorize]
        [HttpPut("/api/v1/Availability/{Id}")]
        [ProducesResponseType(typeof(AvailabilityResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> UpdateAvailability(int Id, AvailabilityRequest request)
        {
            try
            {
                var result = await _fieldPutServices.UpdateAvailability(Id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        /// <summary>
        /// Delete an existing Availability.
        /// </summary>
        /// <param name="Id">The unique identifier of the availability to be deleted.</param>        
        /// <response code="200">Success</response>
        [Authorize]
        [HttpDelete("/api/v1/Availability/{Id}")]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> DeleteAvailability(int Id)
        {
            try
            {
                await _fieldPutServices.DeleteAvailability(Id);
                return Ok(new ApiResponse
                {
                    Message = "Availability successfully deleted",
                    StatusCode = 200
                });
               
            }
            catch (NotFoundException ex)
            {
                var apiError = new ApiError { Message = ex.Message };
                return BadRequest(apiError);
            }
        }


        /// <summary>
        /// Delete an existing Field.
        /// </summary>
        /// <param name="Id">The unique identifier of the field to be deleted.</param>        
        /// <response code="200">Success</response>
        [Authorize]
        [HttpDelete("/api/v1/Field/{Id}")]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> DeleteField(Guid Id)
        {
            try
            {
                await _fieldPutServices.DeteleField(Id);
                return Ok(new ApiResponse
                {
                    Message = "Field successfully deleted",
                    StatusCode = 200
                });
            }
            catch(NotFoundException ex)
            {
                var apiError = new ApiResponse { Message = ex.Message };
                return BadRequest(apiError);
            }
        }

    }
}