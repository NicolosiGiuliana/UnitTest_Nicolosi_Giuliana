using Application.DTOS.Request;
using Application.DTOS.Responses;
using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IFieldServices;
using Application.Interfaces.IValidator;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.FieldServices
{
    public class FieldGetServices : IFieldGetServices
    {
        private readonly IFieldQuery _fieldQuery;
        private readonly IValidatorHandler<GetFieldsRequest> _validator;
        private readonly IMapper _mapper;

        public FieldGetServices(IFieldQuery query, IValidatorHandler<GetFieldsRequest> validator, IMapper mapper)
        {
            _fieldQuery = query;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<FieldResponse> GetFieldById(Guid id)
        {
            var field = await _fieldQuery.GetFieldById(id);

            if (field == null)
            {
                throw new NotFoundException("Field not found");
            }

            var responseFields = _mapper.Map<FieldResponse>(field);

            return responseFields;
        }

        public async Task<List<FieldResponse>> GetAllFields(string? name, string? sizeoffield, int? type, int? availability, int? offset, int? size)
        {
            var request = new GetFieldsRequest
            {
                Name = name,
                Sizeoffield = sizeoffield,
                Type = type,
                Availability = availability,
                Offset = offset,
                Size = size
            };

            await _validator.Validate(request);

            var fields = await _fieldQuery.GetFields(name, sizeoffield, type, availability, offset, size);
            if (fields == null || !fields.Any())
            {
                return new List<FieldResponse>();
            }

            // Map the list of fields entity to a list of response fields
            var responseFields = _mapper.Map<List<FieldResponse>>(fields);
            return responseFields;
        }
    }
}
