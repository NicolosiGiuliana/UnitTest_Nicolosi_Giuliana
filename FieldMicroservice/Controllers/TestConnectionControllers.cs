using Application.Exceptions;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Domain.Entities;
using Application.Interfaces.IServices.IFieldServices;
using Azure.Core;

namespace FieldMicroservice.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TestConnectionController : ControllerBase
    {
        private readonly IFieldConnectionService _fieldConnectionServices;

        public TestConnectionController(IFieldConnectionService fieldConnectionService)
        {
            _fieldConnectionServices = fieldConnectionService;
        }

        /// <summary>
        /// Retrieves detailed information about a specific field by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the field.</param>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(typeof(FieldResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> TestConnectionHttp()
        {

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                var result = await _fieldConnectionServices.LlamarOtroMicroservicioAsync(token);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }
    }
}