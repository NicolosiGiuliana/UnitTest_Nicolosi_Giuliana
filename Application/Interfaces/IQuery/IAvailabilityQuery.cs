using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IQuery
{
    public interface IAvailabilityQuery
    {
        public Task<Availability> GetAvailabilityByID(int id);
        Task<bool> AvailabilityExists(int availabilityID);
    }
}
