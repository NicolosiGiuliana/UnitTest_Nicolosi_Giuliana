using Application.DTOS.Responses;
using Application.Interfaces.IServices.IFieldTypeServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Application.Exceptions;

namespace FieldMicroservice.Controllers
{
    //[Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FieldTypeController : ControllerBase
    {
        private readonly IFieldTypeGetServices _fieldTypeGetServices;

        public FieldTypeController(IFieldTypeGetServices fieldTypeGetServices)
        {
            _fieldTypeGetServices = fieldTypeGetServices;
        }

        /// <summary>
        /// Retrieves a list of all fields types.        
        /// </summary>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(FieldTypeResponse[]), 200)]
        public async Task<IActionResult> GetListFieldTypes()
        {
            var result = await _fieldTypeGetServices.GetAll();
            return new JsonResult(result) { StatusCode = 200 };
        }

        /// <summary>
        /// Retrieves detailed information about a specific field type by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the field.</param>
        /// <response code="200">Success</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FieldResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> GetFieldTypeById([FromRoute] int id)
        {
            try
            {
                var result = await _fieldTypeGetServices.GetFieldTypeById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }

    }
}
