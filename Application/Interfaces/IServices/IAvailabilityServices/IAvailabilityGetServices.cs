using Application.DTOS.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IAvailabilityServices
{
    public interface IAvailabilityGetServices
    {
        Task<Availability> GetAvailabilityById(int id);
    }
}
