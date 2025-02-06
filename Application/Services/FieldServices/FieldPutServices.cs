using Application.DTOS.Request;
using Application.DTOS.Responses;
using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IAvailabilityServices;
using Application.Interfaces.IServices.IFieldServices;
using Application.Interfaces.IValidator;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Services.FieldServices
{
    public class FieldPutServices : IFieldPutServices
    {
        private readonly IFieldCommand _command;
        private readonly IFieldQuery _fieldQuery;
        private readonly IFieldTypeQuery _fieldTypeQuery;
        private readonly IAvailabilityPostServices _availabilityPostServices;
        private readonly IAvailabilityGetServices _availabilityGetServices;
        private readonly IAvailabilityPutServices _availabilityPutServices;
        private readonly IAvailabilityDeleteService _availabilityDeleteService;
        private readonly IValidatorHandler<FieldRequest> _fieldValidator;
        private readonly IMapper _mapper;

        public FieldPutServices(IFieldCommand command, IFieldQuery fieldQuery, IFieldTypeQuery fieldTypeQuery, IAvailabilityPostServices availabilityPostServices, IAvailabilityGetServices availabilityGetServices, IAvailabilityPutServices availabilityPutServices, IAvailabilityDeleteService availabilityDeleteService, IValidatorHandler<FieldRequest> fieldValidator, IMapper mapper)
        {
            _command = command;
            _fieldQuery = fieldQuery;
            _fieldTypeQuery = fieldTypeQuery;
            _availabilityPostServices = availabilityPostServices;
            _availabilityGetServices = availabilityGetServices;
            _availabilityPutServices = availabilityPutServices;
            _availabilityDeleteService = availabilityDeleteService;
            _fieldValidator = fieldValidator;
            _mapper = mapper;
        }

        public async Task<AvailabilityResponse> CreateAvailability(Guid fieldId, AvailabilityRequest request)
        {
            var field = await _fieldQuery.GetFieldById(fieldId);

            var exisitingAvailabilities = field.Availabilities.Where(a => a.DayName == request.Day).ToList();

            ValidateAvailability(exisitingAvailabilities, request);

            var newAvailabilityResponse = await _availabilityPostServices.CreateAvailability(fieldId, request);

            await _command.UpdateField(field);

            return newAvailabilityResponse;
        }

        public async Task<AvailabilityResponse> UpdateAvailability(int availabilityId, AvailabilityRequest request)
        {

            var existingAvailabilty = await _availabilityGetServices.GetAvailabilityById(availabilityId);

            var field = await _fieldQuery.GetFieldById(existingAvailabilty.FieldID);

            var otherAvailabilities = field.Availabilities
                .Where(a => a.DayName == request.Day && a.AvailabilityID != availabilityId)
                .ToList();

            ValidateAvailability(otherAvailabilities, request);

            var availabilityResponse = await _availabilityPutServices.UpdateAvailability(availabilityId, request);

            await _command.UpdateField(field);

            return availabilityResponse;
        }

        public async Task DeleteAvailability(int availabilityId)
        {
            await _availabilityDeleteService.DeleteAvailability(availabilityId);
        }

        public async Task<FieldResponse> UpdateField(Guid id, FieldRequest request)
        {
            await _fieldValidator.Validate(request);

            var existingField = await _fieldQuery.GetFieldById(id);

            if (existingField == null)
            {
                throw new NotFoundException("Field not found");
            }

            var existingFieldType = await _fieldTypeQuery.GetFieldTypeById(request.FieldType);

            if (existingFieldType == null)
            {
                throw new NotFoundException("FieldTypeNavigator not found");
            }


            _mapper.Map(request, existingField);

            existingField.FieldTypeNavigator = existingFieldType;

            await _command.UpdateField(existingField);

            FieldResponse response = _mapper.Map<FieldResponse>(existingField);

            return response;
        }

       

        private void ValidateAvailability(List<Availability> availabilityList, AvailabilityRequest request)
        {
            if (availabilityList.Any(a => request.OpenHour < a.CloseHour && request.CloseHour > a.OpenHour))
            {
                throw new InvalidOperationException("La disponibilidad se solapa con otras");
            }
        }

        public async Task DeteleField(Guid fieldId)
        {
            var existingField = await _fieldQuery.GetFieldById(fieldId);

            if (existingField == null)
            {
                throw new NotFoundException("Field not found");
            }

            existingField.IsActive = false;

            await DeleteAvailabilitesFromField(existingField);

            await _command.UpdateField(existingField);
        }

        private async Task DeleteAvailabilitesFromField(Field field)
        {
            foreach(var availability in field.Availabilities)
            {
                await _availabilityDeleteService.DeleteAvailability(availability.AvailabilityID);
            }
        }
           
    }
}
