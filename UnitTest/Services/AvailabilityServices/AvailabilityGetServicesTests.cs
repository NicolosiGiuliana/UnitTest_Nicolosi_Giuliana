using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Application.Interfaces.IQuery;
using Application.Services.AvailabilityServices;
using Application.Exceptions;
using Domain.Entities;

namespace UnitTest.Services.AvailabilityServices
{
    public class AvailabilityGetServicesTests
    {
        private readonly Mock<IAvailabilityQuery> _availabilityQueryMock;
        private readonly AvailabilityGetServices _availabilityGetServices;

        public AvailabilityGetServicesTests()
        {
            _availabilityQueryMock = new Mock<IAvailabilityQuery>();
            _availabilityGetServices = new AvailabilityGetServices(_availabilityQueryMock.Object, null);
        }

        [Fact]
        public async Task GetAvailabilityById_ShouldReturnAvailability_WhenAvailabilityExists()
        {
            // Arrange
            var availabilityId = 1;
            var availability = new Availability { AvailabilityID = availabilityId, DayName = "Monday", OpenHour = new TimeSpan(9, 0, 0), CloseHour = new TimeSpan(22, 0, 0) };

            _availabilityQueryMock.Setup(query => query.GetAvailabilityByID(availabilityId)).ReturnsAsync(availability);

            // Act
            var result = await _availabilityGetServices.GetAvailabilityById(availabilityId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availability);
        }

        [Fact]
        public async Task GetAvailabilityById_ShouldThrowNotFoundException_WhenAvailabilityDoesNotExist()
        {
            // Arrange
            var availabilityId = 1;

            _availabilityQueryMock.Setup(query => query.GetAvailabilityByID(availabilityId)).ReturnsAsync((Availability)null);

            // Act
            var act = async () => await _availabilityGetServices.GetAvailabilityById(availabilityId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Availability not found");
        }
    }
}