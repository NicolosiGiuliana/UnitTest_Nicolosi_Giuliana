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
    public class FieldTypeQuery : IFieldTypeQuery
    {
        private readonly FieldMSContext _context;

        public FieldTypeQuery(FieldMSContext context)
        {
            _context = context;
        }

        public async Task<FieldType> GetFieldTypeById(int id)
        {
            var fieldtype = await _context.FieldTypes.FirstOrDefaultAsync(ft => ft.FieldTypeID == id);

            if (fieldtype == null)
            {
                throw new NotFoundException("Field Type not found");
            }

            return fieldtype;
        }

        public async Task<List<FieldType>> GetListFieldTypes()
        {

            var result = await _context.FieldTypes.ToListAsync();

            return result;
        }
    }
}
