using Application.Interfaces.ICommand;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Command
{
    public class FieldCommand : IFieldCommand
    {
        private readonly FieldMSContext _context;

        public FieldCommand(FieldMSContext context)
        {
            _context = context;
        }

        public async Task InsertField(Field field)
        {
            _context.Add(field);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateField(Field field)
        {
            _context.Update(field);
            await _context.SaveChangesAsync();
        }
    }
}
