using Application.DTOS.Request;
using Application.DTOS.Responses;
using Application.Interfaces.ICommand;
using Application.Interfaces.IServices.IAvailabilityServices;
using Application.Interfaces.IValidator;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AvailabilityServices
{
    public class AvailabilityPostServices : IAvailabilityPostServices
    {
        private readonly IMapper _mapper;
        private readonly IAvailabilityCommand _command;
        private readonly IValidatorHandler<AvailabilityRequest> _availabilityValidator;

        public AvailabilityPostServices(IMapper mapper, IAvailabilityCommand availabilityCommand, IValidatorHandler<AvailabilityRequest> availabilityValidator)
        {

            _mapper = mapper;
            _command = availabilityCommand;
            _availabilityValidator = availabilityValidator;

        }

        public async Task<AvailabilityResponse> CreateAvailability(Guid fieldId, AvailabilityRequest request)
        {
            await _availabilityValidator.Validate(request);

            var availability = _mapper.Map<Availability>(request);

            availability.FieldID = fieldId;

            await _command.InsertAvailability(availability);

            AvailabilityResponse availabilityResponse = _mapper.Map<AvailabilityResponse>(availability);

            return availabilityResponse;

        }
    }
}

