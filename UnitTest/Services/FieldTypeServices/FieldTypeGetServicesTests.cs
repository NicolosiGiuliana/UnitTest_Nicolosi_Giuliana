using System;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.IQuery;
using Application.Services.FieldTypeServices;
using Application.DTOS.Responses;
using AutoMapper;
using Application.Exceptions;
using Domain.Entities;

namespace UnitTest.Services.FieldTypeServices
{
    public class FieldTypeGetServicesTests
    {

        private readonly Mock<IFieldTypeQuery> _fieldTypeQueryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FieldTypeGetServices _fieldTypeGetServices;

        public FieldTypeGetServicesTests()
        {
            _fieldTypeQueryMock = new Mock<IFieldTypeQuery>();
            _mapperMock = new Mock<IMapper>();
            _fieldTypeGetServices = new FieldTypeGetServices(_fieldTypeQueryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfFieldTypeResponse_WhenFieldTypesExist()
        {
            // Arrange
            var fieldTypes = new List<FieldType>
            {
                new FieldType { FieldTypeID = 1, Description = "pasto" },
                new FieldType { FieldTypeID = 2, Description = "sintetico" },
                new FieldType { FieldTypeID = 3, Description = "cemento" }
            };

            _fieldTypeQueryMock.Setup(query => query.GetListFieldTypes()).ReturnsAsync(fieldTypes);

            var fieldTypeResponses = new List<FieldTypeResponse>
            {
                new FieldTypeResponse { Id = 1, Description = "pasto" },
                new FieldTypeResponse { Id = 2, Description = "sintetico" },
                new FieldTypeResponse { Id = 3, Description = "cemento" }
            };

            _mapperMock.Setup(mapper => mapper.Map<List<FieldTypeResponse>>(fieldTypes)).Returns(fieldTypeResponses);

            // Act
            var result = await _fieldTypeGetServices.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(fieldTypeResponses);
        }

        [Fact]
        public async Task GetFieldTypeById_ShouldReturnFieldTypeResponse_WhenFieldTypeExists()
        {
            // Arrange
            var fieldTypeId = 1;
            var fieldType = new FieldType { FieldTypeID = fieldTypeId, Description = "pasto" };

            _fieldTypeQueryMock.Setup(query => query.GetFieldTypeById(fieldTypeId)).ReturnsAsync(fieldType);

            var fieldTypeResponse = new FieldTypeResponse { Id = fieldTypeId, Description = "pasto" };

            _mapperMock.Setup(mapper => mapper.Map<FieldTypeResponse>(fieldType)).Returns(fieldTypeResponse);

            // Act
            var result = await _fieldTypeGetServices.GetFieldTypeById(fieldTypeId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fieldTypeResponse);
        }

        [Fact]
        public async Task GetFieldTypeById_ShouldThrowNotFoundException_WhenFieldTypeDoesNotExist()
        {
            // Arrange
            var fieldTypeId = 1;

            _fieldTypeQueryMock.Setup(query => query.GetFieldTypeById(fieldTypeId)).ThrowsAsync(new NotFoundException("Field Type not found"));

            // Act
            var act = async () => await _fieldTypeGetServices.GetFieldTypeById(fieldTypeId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Field Type not found");
        }

    }
}
