using Application.Interfaces.IQuery;
using Infrastructure.Persistence;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Application.DTOS.Responses;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;

namespace Infrastructure.Query
{
    public class FieldQuery : IFieldQuery
    {
        private readonly FieldMSContext _context;

        public FieldQuery(FieldMSContext context)
        {
            _context = context;
        }

        public Task<Field> GetFieldById(Guid id)
        {
            var field = _context.Set<Field>()
                .Include(ft => ft.FieldTypeNavigator)
                .Include(a => a.Availabilities)
                .FirstOrDefaultAsync(f => f.FieldID == id);


            if (field == null)
            {
                throw new NotFoundException("FieldResponse not found");
            }

            return field;
        }
        public async Task<Field> GetFieldByName(string name)
        {
            var field = await _context.Fields.FirstOrDefaultAsync(p => p.Name == name);

            return field;
        }
        public async Task<IEnumerable<Field>> GetFields(string? name, string? sizeoffield, int? type, int? availability, int? offset, int? size)
        {
            if (size.HasValue && size.Value == 0)
            {
                return new List<Field>();
            }

            var query = _context.Fields.Where(f => f.IsActive == true)
                .Include(ft => ft.FieldTypeNavigator)
                .Include(a => a.Availabilities)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(f => f.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(sizeoffield))
            {
                query = query.Where(f => f.Size.Contains(sizeoffield));
            }

            if (type.HasValue)
            {
                query = query.Where(p => p.FieldTypeID == type.Value);
            }


            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (size.HasValue)
            {
                query = query.Take(size.Value);
            }

            return await query.ToListAsync();
        }

    }
}
