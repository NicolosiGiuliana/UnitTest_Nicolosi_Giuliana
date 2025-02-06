using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Interfaces;
using Application.Interfaces.ICommand;
using Infrastructure.Persistence;

namespace Infrastructure.Command
{
    public class AvailabilityCommand : IAvailabilityCommand
    {
        private readonly FieldMSContext _context;

        public AvailabilityCommand(FieldMSContext context)
        {
            _context = context;
        }

        public async Task UpdateAvailability(Availability availability)
        {
            _context.Update(availability);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAvailability(Availability availability)
        {
            _context.Add(availability);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAvailability(Availability availability)
        {
            _context.Remove(availability);
            await _context.SaveChangesAsync();
        }
    }
}
