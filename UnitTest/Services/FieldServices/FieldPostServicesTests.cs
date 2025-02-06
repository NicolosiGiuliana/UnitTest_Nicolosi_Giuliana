using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IValidator;
using Application.Services.FieldServices;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using AutoMapper;
using Domain.Entities;
using Application.Exceptions;
using FluentValidation;

namespace UnitTest.Services.FieldServices
{
    public class FieldPostServicesTests
    {
        private readonly Mock<IFieldCommand> _fieldCommandMock;
        private readonly Mock<IFieldQuery> _fieldQueryMock;
        private readonly Mock<IFieldTypeQuery> _fieldTypeQueryMock;
        private readonly Mock<IValidatorHandler<FieldRequest>> _fieldValidatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FieldPostServices _fieldPostServices;

        public FieldPostServicesTests()
        {
            _fieldCommandMock = new Mock<IFieldCommand>();
            _fieldQueryMock = new Mock<IFieldQuery>();
            _fieldTypeQueryMock = new Mock<IFieldTypeQuery>();
            _fieldValidatorMock = new Mock<IValidatorHandler<FieldRequest>>();
            _mapperMock = new Mock<IMapper>();
            _fieldPostServices = new FieldPostServices(_fieldCommandMock.Object, _fieldQueryMock.Object, _fieldTypeQueryMock.Object, null, _fieldValidatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateField_ShouldReturnFieldResponse_WhenRequestIsValid()
        {
            // Arrange
            var request = new FieldRequest
            {
                Name = "Campo 1",
                Size = "5",
                FieldType = 1
            };

            var fieldType = new FieldType
            {
                FieldTypeID = request.FieldType,
                Description = "pasto"
            };

            var newField = new Field
            {
                FieldID = Guid.NewGuid(),
                Name = request.Name,
                Size = request.Size,
                FieldTypeID = request.FieldType,
                IsActive = true
                
            };

            var fieldResponse = new FieldResponse
            {
                Id = newField.FieldID,
                Name = newField.Name,
                Size = newField.Size,
                FieldType = new FieldTypeResponse { Id = fieldType.FieldTypeID, Description = fieldType.Description }
            };

            _fieldValidatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _fieldTypeQueryMock.Setup(query => query.GetFieldTypeById(request.FieldType)).ReturnsAsync(fieldType);
            _mapperMock.Setup(mapper => mapper.Map<Field>(request)).Returns(newField);
            _fieldCommandMock.Setup(command => command.InsertField(newField)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<FieldResponse>(newField)).Returns(fieldResponse);

            // Act
            var result = await _fieldPostServices.CreateField(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fieldResponse);
        }

        [Fact]
        public async Task CreateField_ShouldThrowValidationException_WhenRequestIsInvalid()
        {
            // Arrange
            var request = new FieldRequest
            {
                Name = "Campo 1",
                Size = "15",
                FieldType = 1
            };

            _fieldValidatorMock.Setup(validator => validator.Validate(request)).ThrowsAsync(new ValidationException("Invalid request"));

            // Act
            var act = async () => await _fieldPostServices.CreateField(request);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Invalid request");
            _fieldCommandMock.Verify(command => command.InsertField(It.IsAny<Field>()), Times.Never);
        }

        [Fact]
        public async Task CreateField_ShouldThrowNotFoundException_WhenFieldTypeDoesNotExist()
        {
            // Arrange
            var request = new FieldRequest
            {
                Name = "Campo 1",
                Size = "5",
                FieldType = 8
            };

            _fieldValidatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _fieldTypeQueryMock.Setup(query => query.GetFieldTypeById(request.FieldType)).ReturnsAsync((FieldType)null);

            // Act
            var act = async () => await _fieldPostServices.CreateField(request);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("FieldTypeNavigator not found");
            _fieldCommandMock.Verify(command => command.InsertField(It.IsAny<Field>()), Times.Never);
        }
    }
}
