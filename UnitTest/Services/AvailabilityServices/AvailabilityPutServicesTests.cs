using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IValidator;
using Application.Services.AvailabilityServices;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using AutoMapper;
using Domain.Entities;
using Application.Exceptions;

namespace UnitTest.Services.AvailabilityServices
{
    public class AvailabilityPutServicesTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAvailabilityCommand> _availabilityCommandMock;
        private readonly Mock<IAvailabilityQuery> _availabilityQueryMock;
        private readonly Mock<IValidatorHandler<AvailabilityRequest>> _availabilityValidatorMock;
        private readonly AvailabilityPutServices _availabilityPutServices;

        public AvailabilityPutServicesTests()
        {
            _mapperMock = new Mock<IMapper>();
            _availabilityCommandMock = new Mock<IAvailabilityCommand>();
            _availabilityQueryMock = new Mock<IAvailabilityQuery>();
            _availabilityValidatorMock = new Mock<IValidatorHandler<AvailabilityRequest>>();
            _availabilityPutServices = new AvailabilityPutServices(_mapperMock.Object, _availabilityCommandMock.Object, _availabilityQueryMock.Object, _availabilityValidatorMock.Object);
        }

        [Fact]
        public async Task UpdateAvailability_ShouldReturnAvailabilityResponse_WhenAvailabilityExists()
        {
            // Arrange
            var availabilityId = 1;
            var request = new AvailabilityRequest
            {
                Day = "Monday",
                OpenHour = new TimeSpan(9, 0, 0),
                CloseHour = new TimeSpan(22, 0, 0)
            };

            var availability = new Availability
            {
                AvailabilityID = availabilityId,
                DayName = "Monday",
                OpenHour = new TimeSpan(8, 0, 0),
                CloseHour = new TimeSpan(16, 0, 0)
            };

            _availabilityValidatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _availabilityQueryMock.Setup(query => query.GetAvailabilityByID(availabilityId)).ReturnsAsync(availability);
            _mapperMock.Setup(mapper => mapper.Map(request, availability));
            _availabilityCommandMock.Setup(command => command.UpdateAvailability(availability)).Returns(Task.CompletedTask);

            var availabilityResponse = new AvailabilityResponse
            {
                Id = availability.AvailabilityID,
                Day = request.Day,
                OpenHour = request.OpenHour,
                CloseHour = request.CloseHour
            };

            _mapperMock.Setup(mapper => mapper.Map<AvailabilityResponse>(availability)).Returns(availabilityResponse);

            // Act
            var result = await _availabilityPutServices.UpdateAvailability(availabilityId, request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilityResponse);
        }

        [Fact]
        public async Task UpdateAvailability_ShouldThrowNotFoundException_WhenAvailabilityDoesNotExist()
        {
            // Arrange
            var availabilityId = 1;
            var request = new AvailabilityRequest
            {
                Day = "Monday",
                OpenHour = new TimeSpan(9, 0, 0),
                CloseHour = new TimeSpan(20, 0, 0)
            };

            _availabilityValidatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _availabilityQueryMock.Setup(query => query.GetAvailabilityByID(availabilityId)).ReturnsAsync((Availability)null); //id 1, devuelve null

            // Act
            var act = async () => await _availabilityPutServices.UpdateAvailability(availabilityId, request);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Availability not found");
            _availabilityCommandMock.Verify(command => command.UpdateAvailability(It.IsAny<Availability>()), Times.Never);
        }
    }
}
