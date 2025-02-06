using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ICommand
{
    public interface IAvailabilityCommand
    {
        Task UpdateAvailability(Availability availability);
        Task InsertAvailability(Availability availability);
        Task DeleteAvailability(Availability availability);
    }
}
