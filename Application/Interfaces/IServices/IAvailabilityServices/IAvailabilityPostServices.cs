using Application.DTOS.Request;
using Application.DTOS.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IAvailabilityServices
{
    public interface IAvailabilityPostServices
    {
        public Task<AvailabilityResponse> CreateAvailability(Guid fieldId, AvailabilityRequest request);
    }
}
