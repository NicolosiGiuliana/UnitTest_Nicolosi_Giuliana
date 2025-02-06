using Application.Exceptions;
using Application.Interfaces.IQuery;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class AvailabilityQuery : IAvailabilityQuery
    {
        private readonly FieldMSContext _context;

        public AvailabilityQuery(FieldMSContext context)
        {
            _context = context;
        }

        public async Task<Availability> GetAvailabilityByID(int id)
        {
            var availability = await _context.Set<Availability>()
                .FirstOrDefaultAsync(f => f.AvailabilityID == id);

            return availability;
        }

        public async Task<bool> AvailabilityExists(int availabilityID)
        {
            return await _context.Availabilities.AnyAsync(a => a.AvailabilityID == availabilityID);
        }
    }
}