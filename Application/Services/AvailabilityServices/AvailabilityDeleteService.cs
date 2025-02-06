using Application.Interfaces.ICommand;
using Application.Interfaces.IServices.IAvailabilityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AvailabilityServices
{
    public class AvailabilityDeleteService : IAvailabilityDeleteService
    {
        private readonly IAvailabilityCommand _command;
        private readonly IAvailabilityGetServices _availabilityGetServices;

        public AvailabilityDeleteService(IAvailabilityCommand command, IAvailabilityGetServices availabilityGetServices)
        {
            _command = command;
            _availabilityGetServices = availabilityGetServices;
        }

        public async Task DeleteAvailability(int availabilityId)
        {
            var availability = await _availabilityGetServices.GetAvailabilityById(availabilityId);

            await _command.DeleteAvailability(availability);
        }
    }
}
