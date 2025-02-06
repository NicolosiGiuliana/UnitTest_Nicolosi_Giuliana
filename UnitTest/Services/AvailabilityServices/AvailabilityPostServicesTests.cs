using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Application.Interfaces.ICommand;
using Application.Interfaces.IValidator;
using Application.Services.AvailabilityServices;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using AutoMapper;
using Domain.Entities;
using FluentValidation;

namespace UnitTest.Services.AvailabilityServices
{
    public class AvailabilityPostServicesTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAvailabilityCommand> _availabilityCommandMock;
        private readonly Mock<IValidatorHandler<AvailabilityRequest>> _availabilityValidatorMock;
        private readonly AvailabilityPostServices _availabilityPostServices;

        public AvailabilityPostServicesTests()
        {
            _mapperMock = new Mock<IMapper>();
            _availabilityCommandMock = new Mock<IAvailabilityCommand>();
            _availabilityValidatorMock = new Mock<IValidatorHandler<AvailabilityRequest>>();
            _availabilityPostServices = new AvailabilityPostServices(_mapperMock.Object, _availabilityCommandMock.Object, _availabilityValidatorMock.Object);
        }

        [Fact]
        public async Task CreateAvailability_ShouldReturnAvailabilityResponse_WhenRequestIsValid()
        {
            // Arrange
            var fieldId = Guid.NewGuid();
            var request = new AvailabilityRequest
            {
                Day = "Monday",
                OpenHour = new TimeSpan(9, 0, 0),
                CloseHour = new TimeSpan(22, 0, 0)
            };

            var availability = new Availability
            {
                AvailabilityID = 1,
                FieldID = fieldId,
                DayName = request.Day,
                OpenHour = request.OpenHour,
                CloseHour = request.CloseHour
            };

            var availabilityResponse = new AvailabilityResponse
            {
                Id = availability.AvailabilityID,
                Day = availability.DayName,
                OpenHour = availability.OpenHour,
                CloseHour = availability.CloseHour
            };

            _availabilityValidatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<Availability>(request)).Returns(availability);
            _availabilityCommandMock.Setup(command => command.InsertAvailability(availability)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<AvailabilityResponse>(availability)).Returns(availabilityResponse);

            // Act
            var result = await _availabilityPostServices.CreateAvailability(fieldId, request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilityResponse);
        }

        [Fact]
        public async Task CreateAvailability_ShouldThrowValidationException_WhenRequestIsInvalid()
        {
            // Arrange
            var fieldId = Guid.NewGuid();
            var request = new AvailabilityRequest
            {//ID
                Day = "Monday", 
                OpenHour = new TimeSpan(22, 0, 0),
                CloseHour = new TimeSpan(9, 0, 0)
            };

            _availabilityValidatorMock.Setup(validator => validator.Validate(request)).ThrowsAsync(new ValidationException("Invalid request"));

            // Act
            var act = async () => await _availabilityPostServices.CreateAvailability(fieldId, request);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Invalid request");
            _availabilityCommandMock.Verify(command => command.InsertAvailability(It.IsAny<Availability>()), Times.Never);
        }
    }
}
