using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IAvailabilityServices;
using Application.Interfaces.IValidator;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.AvailabilityServices
{
    public class AvailabilityPutServices : IAvailabilityPutServices
    {
        private readonly IMapper _mapper;
        private readonly IAvailabilityCommand _command;
        private readonly IAvailabilityQuery _query;
        private readonly IValidatorHandler<AvailabilityRequest> _availabilityValidator;

        public AvailabilityPutServices(IMapper mapper, IAvailabilityCommand command, IAvailabilityQuery query, IValidatorHandler<AvailabilityRequest> availabilityValidator)
        {
            _mapper = mapper;
            _command = command;
            _query = query;
            _availabilityValidator = availabilityValidator;
        }


        public async Task<AvailabilityResponse> UpdateAvailability(int id, AvailabilityRequest request)
        {
            await _availabilityValidator.Validate(request);

            var availability = await _query.GetAvailabilityByID(id);

            if (availability == null)
            {
                throw new NotFoundException("Availability not found");
            }

            _mapper.Map(request, availability);

            await _command.UpdateAvailability(availability);

            var availabilityResponse = _mapper.Map<AvailabilityResponse>(availability);

            return availabilityResponse;

        }


    }
}
