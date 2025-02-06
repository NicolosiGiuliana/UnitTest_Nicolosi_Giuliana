using Application.DTOS.Request;
using Application.DTOS.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IFieldServices
{
    public interface IFieldPutServices
    {
        Task<FieldResponse> UpdateField(Guid id, FieldRequest request);

        Task<AvailabilityResponse> CreateAvailability(Guid fieldId, AvailabilityRequest request);

        Task<AvailabilityResponse> UpdateAvailability(int availabilityId, AvailabilityRequest request);

        Task DeleteAvailability(int availabilityId);

        Task DeteleField(Guid fieldId);
    }
}
