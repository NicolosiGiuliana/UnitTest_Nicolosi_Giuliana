using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IAvailabilityServices;
using Application.Interfaces.IValidator;
using Application.Services.FieldServices;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using AutoMapper;
using Domain.Entities;
using Application.Exceptions;

namespace UnitTest.Services.FieldServices
{
    public class FieldPutServicesTests
    {
        private readonly Mock<IFieldCommand> _fieldCommandMock;
        private readonly Mock<IFieldQuery> _fieldQueryMock;
        private readonly Mock<IFieldTypeQuery> _fieldTypeQueryMock;
        private readonly Mock<IAvailabilityPostServices> _availabilityPostServicesMock;
        private readonly Mock<IAvailabilityGetServices> _availabilityGetServicesMock;
        private readonly Mock<IAvailabilityPutServices> _availabilityPutServicesMock;
        private readonly Mock<IAvailabilityDeleteService> _availabilityDeleteServiceMock;
        private readonly Mock<IValidatorHandler<FieldRequest>> _fieldValidatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FieldPutServices _fieldPutServices;

        public FieldPutServicesTests()
        {
            _fieldCommandMock = new Mock<IFieldCommand>();
            _fieldQueryMock = new Mock<IFieldQuery>();
            _fieldTypeQueryMock = new Mock<IFieldTypeQuery>();
            _availabilityPostServicesMock = new Mock<IAvailabilityPostServices>();
            _availabilityGetServicesMock = new Mock<IAvailabilityGetServices>();
            _availabilityPutServicesMock = new Mock<IAvailabilityPutServices>();
            _availabilityDeleteServiceMock = new Mock<IAvailabilityDeleteService>();
            _fieldValidatorMock = new Mock<IValidatorHandler<FieldRequest>>();
            _mapperMock = new Mock<IMapper>();
            _fieldPutServices = new FieldPutServices(_fieldCommandMock.Object, _fieldQueryMock.Object, _fieldTypeQueryMock.Object, _availabilityPostServicesMock.Object, _availabilityGetServicesMock.Object, _availabilityPutServicesMock.Object, _availabilityDeleteServiceMock.Object, _fieldValidatorMock.Object, _mapperMock.Object);
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

            var field = new Field
            {
                FieldID = fieldId,
                Name = "Campo 1",
                Availabilities = new List<Availability>()
            };

            var newAvailabilityResponse = new AvailabilityResponse
            {
                Id = 1,
                Day = request.Day,
                OpenHour = request.OpenHour,
                CloseHour = request.CloseHour
            };

            _fieldQueryMock.Setup(query => query.GetFieldById(fieldId)).ReturnsAsync(field);
            _availabilityPostServicesMock.Setup(service => service.CreateAvailability(fieldId, request)).ReturnsAsync(newAvailabilityResponse);
            _fieldCommandMock.Setup(command => command.UpdateField(field)).Returns(Task.CompletedTask);

            // Act
            var result = await _fieldPutServices.CreateAvailability(fieldId, request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(newAvailabilityResponse);
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

            var existingAvailability = new Availability
            {
                AvailabilityID = availabilityId,
                FieldID = Guid.NewGuid(),
                DayName = "Monday",
                OpenHour = new TimeSpan(8, 0, 0),
                CloseHour = new TimeSpan(16, 0, 0)
            };

            var field = new Field
            {
                FieldID = existingAvailability.FieldID,
                Name = "Campo 1",
                Availabilities = new List<Availability> { existingAvailability }
            };

            var updatedAvailabilityResponse = new AvailabilityResponse
            {
                Id = availabilityId,
                Day = request.Day,
                OpenHour = request.OpenHour,
                CloseHour = request.CloseHour
            };

            _availabilityGetServicesMock.Setup(service => service.GetAvailabilityById(availabilityId)).ReturnsAsync(existingAvailability);
            _fieldQueryMock.Setup(query => query.GetFieldById(existingAvailability.FieldID)).ReturnsAsync(field);
            _availabilityPutServicesMock.Setup(service => service.UpdateAvailability(availabilityId, request)).ReturnsAsync(updatedAvailabilityResponse);
            _fieldCommandMock.Setup(command => command.UpdateField(field)).Returns(Task.CompletedTask);

            // Act
            var result = await _fieldPutServices.UpdateAvailability(availabilityId, request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedAvailabilityResponse);
        }

        [Fact]
        public async Task DeleteAvailability_ShouldCallDeleteAvailabilityService_WhenAvailabilityExists()
        {
            // Arrange
            var availabilityId = 1;

            _availabilityDeleteServiceMock.Setup(service => service.DeleteAvailability(availabilityId)).Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _fieldPutServices.DeleteAvailability(availabilityId);

            // Assert
            await act.Should().NotThrowAsync();
            _availabilityDeleteServiceMock.Verify(service => service.DeleteAvailability(availabilityId), Times.Once);
        }

        [Fact]
        public async Task UpdateField_ShouldReturnFieldResponse_WhenFieldExists()
        {
            // Arrange
            var fieldId = Guid.NewGuid();
            var request = new FieldRequest
            {
                Name = "Campo 2",
                Size = "7",
                FieldType = 1
            };

            var fieldType = new FieldType
            {
                FieldTypeID = request.FieldType,
                Description = "pasto"
            };

            var existingField = new Field
            {
                FieldID = fieldId,
                Name = "campo2",
                Size = "7",
                FieldTypeID = request.FieldType
            };

            var updatedFieldResponse = new FieldResponse
            {
                Id = fieldId,
                Name = request.Name,
                Size = request.Size,
                FieldType = new FieldTypeResponse { Id = fieldType.FieldTypeID, Description = fieldType.Description }
            };

            _fieldValidatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _fieldQueryMock.Setup(query => query.GetFieldById(fieldId)).ReturnsAsync(existingField);
            _fieldTypeQueryMock.Setup(query => query.GetFieldTypeById(request.FieldType)).ReturnsAsync(fieldType);
            _mapperMock.Setup(mapper => mapper.Map(request, existingField));
            _fieldCommandMock.Setup(command => command.UpdateField(existingField)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<FieldResponse>(existingField)).Returns(updatedFieldResponse);

            // Act
            var result = await _fieldPutServices.UpdateField(fieldId, request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedFieldResponse);
        }

        [Fact]
        public async Task DeteleField_ShouldSetFieldAsInactive_WhenFieldExists() //error de tipeo del service
        {
            // Arrange
            var fieldId = Guid.NewGuid();
            var existingField = new Field
            {
                FieldID = fieldId,
                Name = "Campo 1",
                IsActive = true,
                Availabilities = new List<Availability> { new Availability { AvailabilityID = 1 } }
            };

            _fieldQueryMock.Setup(query => query.GetFieldById(fieldId)).ReturnsAsync(existingField);
            _availabilityDeleteServiceMock.Setup(service => service.DeleteAvailability(It.IsAny<int>())).Returns(Task.CompletedTask);
            _fieldCommandMock.Setup(command => command.UpdateField(existingField)).Returns(Task.CompletedTask);

            // Act
            await _fieldPutServices.DeteleField(fieldId);

            // Assert
            existingField.IsActive.Should().BeFalse();
            _availabilityDeleteServiceMock.Verify(service => service.DeleteAvailability(It.IsAny<int>()), Times.Exactly(existingField.Availabilities.Count));
            _fieldCommandMock.Verify(command => command.UpdateField(existingField), Times.Once);
        }
    }
}
