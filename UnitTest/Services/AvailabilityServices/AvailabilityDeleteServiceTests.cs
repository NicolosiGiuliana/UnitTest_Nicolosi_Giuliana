using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Application.Interfaces.ICommand;
using Application.Interfaces.IServices.IAvailabilityServices;
using Application.Services.AvailabilityServices;
using Application.Exceptions;
using Domain.Entities;

namespace UnitTest.Services.AvailabilityServices
{
    public class AvailabilityDeleteServiceTests
    {
        private readonly Mock<IAvailabilityCommand> _availabilityCommandMock;
        private readonly Mock<IAvailabilityGetServices> _availabilityGetServicesMock;
        private readonly AvailabilityDeleteService _availabilityDeleteService;

        public AvailabilityDeleteServiceTests()
        {
            _availabilityCommandMock = new Mock<IAvailabilityCommand>();
            _availabilityGetServicesMock = new Mock<IAvailabilityGetServices>();
            _availabilityDeleteService = new AvailabilityDeleteService(_availabilityCommandMock.Object, _availabilityGetServicesMock.Object);
        }

        [Fact]
        public async Task DeleteAvailability_ShouldDeleteAvailability_WhenAvailabilityExists()
        {
            // Arrange, creo obj 
            var availabilityId = 1;
            var availability = new Availability { AvailabilityID = availabilityId, DayName = "Monday", OpenHour = new TimeSpan(9, 0, 0), CloseHour = new TimeSpan(22, 0, 0) };

            //Adjunto services 
            _availabilityGetServicesMock.Setup(service => service.GetAvailabilityById(availabilityId)).ReturnsAsync(availability);            
            _availabilityCommandMock.Setup(command => command.DeleteAvailability(availability)).Returns(Task.CompletedTask);

            // Act            
            var act = async () => await _availabilityDeleteService.DeleteAvailability(availabilityId);

            // Assert
            await act.Should().NotThrowAsync(); //act no lance excep

            _availabilityCommandMock.Verify(command => command.DeleteAvailability(availability), Times.Once);
        }

        [Fact]
        public async Task DeleteAvailability_ShouldThrowNotFoundException_WhenAvailabilityDoesNotExist()
        {
            // Arrange
            var availabilityId = 1;

            _availabilityGetServicesMock.Setup(service => service.GetAvailabilityById(availabilityId)).ThrowsAsync(new NotFoundException("Availability not found"));

            // Act
            var act = async () => await _availabilityDeleteService.DeleteAvailability(availabilityId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Availability not found");
            _availabilityCommandMock.Verify(command => command.DeleteAvailability(It.IsAny<Availability>()), Times.Never);
        }
    }
}
