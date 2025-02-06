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
    public class FieldPostServices : IFieldPostServices
    {
        private readonly IFieldCommand _command;
        private readonly IFieldQuery _fieldQuery;
        private readonly IFieldTypeQuery _fieldTypeQuery;
        private readonly IAvailabilityQuery _availabilityQuery;
        private readonly IValidatorHandler<FieldRequest> _fieldValidator;
        private readonly IMapper _mapper;

        public FieldPostServices(IFieldCommand command, IFieldQuery fieldQuery, IFieldTypeQuery fieldTypeQuery, IAvailabilityQuery availabilityQuery, IValidatorHandler<FieldRequest> fieldValidator, IMapper mapper)
        {
            _command = command;
            _fieldQuery = fieldQuery;
            _fieldTypeQuery = fieldTypeQuery;
            _availabilityQuery = availabilityQuery;
            _fieldValidator = fieldValidator;
            _mapper = mapper;
        }

        public async Task<FieldResponse> CreateField(FieldRequest request)
        {
            await _fieldValidator.Validate(request);
       

            var existingFieldType = await _fieldTypeQuery.GetFieldTypeById(request.FieldType);

            if (existingFieldType == null)
            {
                throw new NotFoundException("FieldTypeNavigator not found");
            }


            var newField = _mapper.Map<Field>(request);

            newField.IsActive = true;

            newField.FieldTypeNavigator = existingFieldType;

            await _command.InsertField(newField);

            FieldResponse response = _mapper.Map<FieldResponse>(newField);

            return response;
        }

    }
}
