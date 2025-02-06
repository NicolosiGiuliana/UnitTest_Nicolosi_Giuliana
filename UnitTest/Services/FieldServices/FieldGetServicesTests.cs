using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.IQuery;
using Application.Services.FieldServices;
using Application.DTOS.Request;
using Application.DTOS.Responses;
using Application.Interfaces.IValidator;
using AutoMapper;
using Application.Exceptions;
using Domain.Entities;

namespace UnitTest.Services.FieldServices
{    
    public class FieldGetServicesTests
    {
        private readonly Mock<IFieldQuery> _fieldQueryMock;
        private readonly Mock<IValidatorHandler<GetFieldsRequest>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FieldGetServices _fieldGetServices;

        public FieldGetServicesTests()
        {
            _fieldQueryMock = new Mock<IFieldQuery>();
            _validatorMock = new Mock<IValidatorHandler<GetFieldsRequest>>();
            _mapperMock = new Mock<IMapper>();
            _fieldGetServices = new FieldGetServices(_fieldQueryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetFieldById_ShouldReturnFieldResponse_WhenFieldExists()
        {
            // Arrange
            var fieldId = Guid.NewGuid();
            var fieldType = new FieldType { FieldTypeID = 1, Description = "pasto" };
            
            var field = new Field
            {
                FieldID = fieldId,
                Name = "Campo 1",
                Size = "5",
                FieldTypeID = fieldType.FieldTypeID                
            };

            _fieldQueryMock.Setup(query => query.GetFieldById(fieldId)).ReturnsAsync(field);

            var fieldResponse = new FieldResponse
            {
                Id = field.FieldID,
                Name = field.Name,
                Size = field.Size,
                FieldType = new FieldTypeResponse { Id = fieldType.FieldTypeID, Description = fieldType.Description },
                Availabilities = new List<AvailabilityResponse>
                {
                    new AvailabilityResponse { Id = 1, Day = "Monday", OpenHour = new TimeSpan(9, 0, 0), CloseHour = new TimeSpan(17, 0, 0) }
                }
            };

            _mapperMock.Setup(mapper => mapper.Map<FieldResponse>(field)).Returns(fieldResponse);
            

            // Act
            var result = await _fieldGetServices.GetFieldById(fieldId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fieldResponse);
        }

        [Fact]
        public async Task GetFieldById_ShouldThrowNotFoundException_WhenFieldDoesNotExist()
        {
            // Arrange
            var fieldId = Guid.NewGuid();

            _fieldQueryMock.Setup(query => query.GetFieldById(fieldId)).ThrowsAsync(new NotFoundException("Field not found"));

            // Act
            var act = async () => await _fieldGetServices.GetFieldById(fieldId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Field not found");
        }

        [Fact]
        public async Task GetAllFields_ShouldReturnListOfFieldResponse_WhenFieldsExist()
        {
            // Arrange
            var fieldType = new FieldType { FieldTypeID = 1, Description = "pasto" };
            var availabilities = new List<Availability>
            {
                new Availability { AvailabilityID = 1, DayName = "Monday", OpenHour = new TimeSpan(9, 0, 0), CloseHour = new TimeSpan(17, 0, 0) }
            };
            var fields = new List<Field>
            {
                new Field { FieldID = Guid.NewGuid(), Name = "Campo 1", Size = "5", FieldTypeID = fieldType.FieldTypeID },
                new Field { FieldID = Guid.NewGuid(), Name = "Campo 2", Size = "7", FieldTypeID = fieldType.FieldTypeID }
            };

            var request = new GetFieldsRequest
            {
                Name = "Campo",
                Sizeoffield = "5",
                Type = 1,
                Availability = 1,
                Offset = 0,
                Size = 10
            };

            _validatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _fieldQueryMock.Setup(query => query.GetFields(request.Name, request.Sizeoffield, request.Type, request.Availability, request.Offset, request.Size)).ReturnsAsync(fields);

            var fieldResponses = new List<FieldResponse>
            {
                new FieldResponse
                {
                    Id = fields[0].FieldID,
                    Name = fields[0].Name,
                    Size = fields[0].Size,
                    FieldType = new FieldTypeResponse { Id = fieldType.FieldTypeID, Description = fieldType.Description },
                    Availabilities = new List<AvailabilityResponse>
                    {
                        new AvailabilityResponse 
                        { 
                            Id = availabilities[0].AvailabilityID, 
                            Day = availabilities[0].DayName, 
                            OpenHour = availabilities[0].OpenHour, 
                            CloseHour = availabilities[0].CloseHour 
                        }
                    }
                },
                new FieldResponse
                {
                    Id = fields[1].FieldID,
                    Name = fields[1].Name,
                    Size = fields[1].Size,
                    FieldType = new FieldTypeResponse { Id = fieldType.FieldTypeID, Description = fieldType.Description },
                    Availabilities = new List<AvailabilityResponse>
                    {
                        new AvailabilityResponse
                        {
                            Id = availabilities[0].AvailabilityID,
                            Day = availabilities[0].DayName,
                            OpenHour = availabilities[0].OpenHour,
                            CloseHour = availabilities[0].CloseHour
                        }
                    }
                }
            };

            _mapperMock.Setup(mapper => mapper.Map<List<FieldResponse>>(fields)).Returns(fieldResponses);

            // Act
            var result = await _fieldGetServices.GetAllFields(request.Name, request.Sizeoffield, request.Type, request.Availability, request.Offset, request.Size);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(fieldResponses);
        }

        [Fact]
        public async Task GetAllFields_ShouldReturnEmptyList_WhenNoFieldsExist()
        {
            // Arrange
            var request = new GetFieldsRequest
            {
                Name = "",
                Sizeoffield = "5",
                Type = 1,
                Availability = 1,
                Offset = 0,
                Size = 10
            };

            _validatorMock.Setup(validator => validator.Validate(request)).Returns(Task.CompletedTask);
            _fieldQueryMock.Setup(query => query.GetFields(request.Name, request.Sizeoffield, request.Type, request.Availability, request.Offset, request.Size)).ReturnsAsync(new List<Field>());

            // Act
            var result = await _fieldGetServices.GetAllFields(request.Name, request.Sizeoffield, request.Type, request.Availability, request.Offset, request.Size);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
