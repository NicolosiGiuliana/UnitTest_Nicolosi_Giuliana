using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IAvailabilityServices
{
    public interface IAvailabilityDeleteService
    {
        Task DeleteAvailability(int availabilityId);
    }
}
